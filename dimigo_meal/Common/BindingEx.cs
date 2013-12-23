using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace dimigo_meal.Common
{
    /// <summary>
    /// Provides a way to declare a binding whose value is converted
    /// by methods on the element's DataContext object, instead of
    /// a standard IValueConverter object.
    /// </summary>
    public class BindingEx : MarkupExtension, IValueConverter
    {
        #region Constructor

        public BindingEx()
        {
            _methodMap = new Dictionary<string, MethodInfo>();
        }

        #endregion // Constructor

        #region Properties

        /// <summary>
        /// Gets/sets the Binding whose value is converted.
        /// </summary>
        public Binding Binding { get; set; }

        /// <summary>
        /// Gets/sets the name of the method on the DataContext object of the bound 
        /// element that is used to convert the bound value when moving from the
        /// binding source to the binding target.
        /// </summary>
        public string ConvertMethod { get; set; }

        /// <summary>
        /// Gets/sets the name of the method on the DataContext object of the bound 
        /// element that is used to convert the bound value when moving from the
        /// binding target to the binding source.
        /// </summary>
        public string ConvertBackMethod { get; set; }

        #endregion // Properties

        #region ProvideValue

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (this.Binding == null)
                return null;

            var provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (provideValueTarget == null)
                return null;

            _boundObject = provideValueTarget.TargetObject as DependencyObject;
            if (_boundObject == null)
                return null;

            _boundProperty = provideValueTarget.TargetProperty as DependencyProperty;
            if (_boundProperty == null)
                return null;

            this.MonitorDataContext();

            this.Binding.Converter = this;

            var bindingExpression = this.Binding.ProvideValue(serviceProvider);
            return bindingExpression;
        }

        void MonitorDataContext()
        {
            // DataContext changes are not observed if a data source is specified.
            bool attach =
                !String.IsNullOrEmpty(this.Binding.ElementName) ||
                this.Binding.RelativeSource != null ||
                this.Binding.Source != null;

            if (attach)
            {
                var fe = _boundObject as FrameworkElement;
                if (fe != null)
                {
                    fe.DataContextChanged += this.HandleDataContextChanged;
                }
                else
                {
                    var fce = _boundObject as FrameworkContentElement;
                    if (fce != null)
                        fce.DataContextChanged += this.HandleDataContextChanged;
                }
            }
        }

        void HandleDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // There might be a new type of DataContext object, so throw away the cached MethodInfos.
            _methodMap.Clear();

            var expression = BindingOperations.GetBindingExpression(_boundObject, _boundProperty);
            if (expression != null)
            {
                // Use try/catch blocks because the Binding's Mode might 
                // not allow for the source or target to be updated.
                try
                {
                    expression.UpdateSource();
                }
                catch { }

                try
                {
                    expression.UpdateTarget();
                }
                catch { }
            }
        }

        #endregion // ProvideValue

        #region Value Conversion

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.PerformConversion(value, this.ConvertMethod);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.PerformConversion(value, this.ConvertBackMethod);
        }

        object PerformConversion(object value, string methodName)
        {
            if (String.IsNullOrEmpty(methodName))
                return value;

            var dataContext = this.GetDataContext();
            if (dataContext == null)
                return value;

            var method = this.GetDataContextMethod(dataContext, methodName);
            if (method == null)
                return value;

            try
            {
                return method.Invoke(dataContext, new object[] { value });
            }
            catch
            {
                return Binding.DoNothing;
            }
        }

        object GetDataContext()
        {
            // FrameworkContentElement AddOwner's FE.DataContextProperty, so this logic works for both types.
            return _boundObject.GetValue(FrameworkElement.DataContextProperty);
        }

        MethodInfo GetDataContextMethod(object dataContext, string methodName)
        {
            if (dataContext == null)
                throw new ArgumentNullException("dataContext");

            if (String.IsNullOrEmpty(methodName))
                return null;

            MethodInfo method;
            if (!_methodMap.TryGetValue(methodName, out method))
            {
                method = dataContext.GetType().GetMethod(methodName,
                  BindingFlags.Instance |
                  BindingFlags.Static |
                  BindingFlags.Public |
                  BindingFlags.NonPublic |
                  BindingFlags.FlattenHierarchy);

                if (method == null)
                    throw new ArgumentException(methodName + " is not a method in " + dataContext.GetType().FullName);

                var parameters = method.GetParameters();
                if (parameters.Length != 1)
                    throw new ArgumentException(methodName + " does not take exactly one parameter.");

                _methodMap.Add(methodName, method);
            }
            return method;
        }

        #endregion // Value Conversion

        #region Fields

        DependencyObject _boundObject;
        DependencyProperty _boundProperty;
        readonly Dictionary<string, MethodInfo> _methodMap;

        #endregion // Fields
    }
}