using AISpeechExplorer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISpeechExplorer
{
    public class AudioClipInformation : ObservableBase
    {
        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set { Set(ref _displayName, value); }
        }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set { Set(ref _fileName, value); }
        }

        private byte[] _fileBytes;
        public byte[] FileBytes
        {
            get { return _fileBytes; }
            set { Set(ref _fileBytes, value); }
        }

        private string _recognizedContent;
        public string RecognizedContent
        {
            get { return _recognizedContent; }
            set { Set(ref _recognizedContent, value); }
        }

        private string _url;
        public string Url
        {
            get { return _url; }
            set { Set(ref _url, value); }
        }

        private int _duration;
        public int Duration
        {
            get { return _duration; }
            set { Set(ref _duration, value); }
        }
    }
}
