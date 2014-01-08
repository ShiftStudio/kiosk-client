using System;
using System.IO;
using System.Windows.Controls;

namespace dimigo_meal.Common
{
    public class MyVideoPlayer : MediaElement
    {
        public MyVideoPlayer()
        {
            base.Volume = 0;
            base.MediaEnded += myVideoPlayer_MediaEnded;
        }

        private string[] _playable_list = { "mp4", "mov", "avi", "jpg", "png", "gif", "bmp", "mkv" };

        private bool IsPlayable(string path)
        {
            foreach (string pl in _playable_list)
            {
                if (path.EndsWith(pl))
                    return true;
            }
            return false;
        }

        public new void Play()
        {
            if (this.MovieList != null && this.MovieList.Length > 0)
            {
                if (_movieIndex >= this.MovieList.Length)
                {
                    _movieIndex = 0;
                }
                string path = MovieList[_movieIndex++];

                if (IsPlayable(path))
                    base.Source = new Uri(path);
                else
                    this.Play();
            }
        }

        public void Refresh()
        {
            try
            {
                if (Directory.Exists(this._movieDirectory))
                {
                    this.MovieList = Directory.GetFiles(this._movieDirectory);
                }
            }
            catch
            {
            }
        }

        private void myVideoPlayer_MediaEnded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Play();
        }

        private int _movieIndex;

        private string _movieDirectory;
        public string MovieDirectory
        {
            get
            {
                return _movieDirectory;
            }
            set
            {
                _movieDirectory = value;
                this.Refresh();
            }
        }

        private string[] _movieList;
        public string[] MovieList
        {
            get
            {
                return _movieList;
            }
            set
            {
                if (_movieList != value)
                {
                    _movieIndex = 0;
                    _movieList = value;
                }
            }
        }
    }
}
