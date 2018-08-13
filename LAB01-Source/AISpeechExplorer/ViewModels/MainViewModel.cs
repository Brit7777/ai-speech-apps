using AISpeechExplorer.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace AISpeechExplorer.ViewModels
{
    public class MainViewModel : ObservableBase
    {
        public MainViewModel()
        {
            CourseChangedCommand = new RelayCommand<SelectionChangedEventArgs>((args) => { CourseSelectionChanged(args); });
            RefreshServicesCommand = new RelayCommand(() => RefreshServices(true));
            BrowseAndRecognizeAudioClipCommand = new RelayCommand(async () => { await BrowseAndRecognizeAudioClipAsync(); });
            SynthesizeTextToSpeechCommand = new RelayCommand(async () => { await SynthesizeTextToSpeechAsync(); });
            
            RefreshServices();
        }
        
        public ICommand RefreshServicesCommand { get; private set; }      
        public ICommand CourseChangedCommand { get; private set; }
        public ICommand BrowseAndRecognizeAudioClipCommand { get; private set; }       
        public ICommand SynthesizeTextToSpeechCommand { get; private set; }
                 
        private ObservableCollection<BingSpeechLanguageResult> _availableBingSpeechLanguages = new ObservableCollection<BingSpeechLanguageResult>();
        public ObservableCollection<BingSpeechLanguageResult> AvailableBingSpeechLanguages
        {
            get { return _availableBingSpeechLanguages; }
            set { Set(ref _availableBingSpeechLanguages, value); }
        }

        private BingSpeechLanguageResult _selectedBingSpeechLanguage;
        public BingSpeechLanguageResult SelectedBingSpeechLanguage
        {
            get { return _selectedBingSpeechLanguage; }
            set { Set(ref _selectedBingSpeechLanguage, value); }
        }

        private string _currentTextContent;
        public string CurrentTextContent
        {
            get { return _currentTextContent; }
            set { Set(ref _currentTextContent, value); }
        }
        
        private AudioClipInformation _currentAudioClip;
        public AudioClipInformation CurrentAudioClip
        {
            get { return _currentAudioClip; }
            set { Set(ref _currentAudioClip, value); }
        }

        private CourseType _currentCourseType;
        public CourseType CurrentCourseType
        {
            get { return _currentCourseType; }
            set { Set(ref _currentCourseType, value); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(ref _isBusy, value); }
        }

        // browsing a local computer for audio clips
        //connecting the user experience to information returned from the Bing Speech API speech recognition method
        public async Task<bool> BrowseAndRecognizeAudioClipAsync()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.MusicLibrary;
            picker.FileTypeFilter.Add(".wav");

            var file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                this.IsBusy = true;

                var fileProperties = await file.Properties.GetImagePropertiesAsync();

                byte[] audioBytes = await file.AsByteArrayAsync();

                var clip = new AudioClipInformation()
                {
                    DisplayName = file.DisplayName,
                    FileName = file.Name,
                    FileBytes = audioBytes,
                    Url = file.Path,

                };

                SpeechToText.SpeechRecognitionResult recognition = null;

                recognition = await Helpers.BingSpeechHelper.RecognizeSpeechAync(audioBytes);

                clip.Duration = recognition.Duration;
                clip.RecognizedContent = recognition.NBest.First().Display;

                this.CurrentAudioClip = clip;

                this.IsBusy = false;
            }

            return file != null;
        }

        //sending text content to generate audio clips 
        private async Task<bool> SynthesizeTextToSpeechAsync()
        {
            this.IsBusy = true;

            var clip = await Helpers.BingSpeechHelper.SynthesizeTextToSpeechAsync(this.CurrentTextContent, this.SelectedBingSpeechLanguage);

            await Helpers.BingSpeechHelper.SpeakContentAsync(clip);

            this.IsBusy = false;

            return true;
        }

        // connects the view model to a list of supported and available Bing Speech service languages
        private async void RefreshAvailableBingSpeechLanguages()
        {
            this.AvailableBingSpeechLanguages.Clear();

            this.IsBusy = true;

            var languages = await Helpers.BingSpeechHelper.GetSupportedLanguagesAsync();

            foreach (var language in languages)
            {
                this.AvailableBingSpeechLanguages.Add(language);
            }

            this.SelectedBingSpeechLanguage = this.AvailableBingSpeechLanguages.FirstOrDefault(w => w.Locale.Equals("en-US"));

            this.IsBusy = false;
        }

        private void RefreshServices(bool deleteState = false)
        {
            RefreshAvailableBingSpeechLanguages();
        }


        private void CourseSelectionChanged(object args)
        {
            bool isCustomSpeechSelected = (this.CurrentCourseType == CourseType.CustomSpeech);

            this.CurrentCourseType = args.AsCourseType();

            switch (this.CurrentCourseType)
            {
                case CourseType.BingSpeech:
                    break;
                case CourseType.TranslatorSpeech:                    
                    break;
                case CourseType.SpeakerRecognition:                   
                    break;
                case CourseType.CustomSpeech:
                    break;
            }

        }

    }
}
