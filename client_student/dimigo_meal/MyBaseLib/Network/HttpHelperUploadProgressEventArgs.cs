using System;
using System.IO;

namespace MyBaseLib.Network
{
    public class HttpHelperUploadProgressEventArgs : EventArgs
    {
        public HttpHelperUploadProgressEventArgs()
        {
        }

        public HttpHelperUploadProgressEventArgs(FileInfo fileInfo, int fileCurrentPos)
        {
            this.FileInfoObj = fileInfo;
            this.FileName = fileInfo.Name;
            this.FileSize = (int)fileInfo.Length;
            this.FileCurrentPos = fileCurrentPos;
        }

        public HttpHelperUploadProgressEventArgs(string fileName, int fileSize, int fileCurrentPos)
        {
            this.FileName = fileName;
            this.FileSize = fileSize;
            this.FileCurrentPos = fileCurrentPos;
        }

        public int FileCurrentPos { get; private set; }

        public FileInfo FileInfoObj { get; private set; }

        public string FileName { get; private set; }

        public int FileSize { get; private set; }
    }
}

