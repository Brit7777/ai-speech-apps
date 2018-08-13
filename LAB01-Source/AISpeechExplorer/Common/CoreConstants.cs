using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISpeechExplorer.Common
{
    public class AppConstants
    {
        public static string AppDisplayName = "AI Speech Explorer";
        public static string AppId { get { return Windows.ApplicationModel.Package.Current.Id.Name.ToString().Replace("-", "").ToUpper(); } }
        public static string ClientId { get { return Guid.NewGuid().ToString().Replace("-", "").ToUpper(); } }
        public static string RecognitionContentType = @"audio/wav; codec=""audio/pcm""; samplerate=16000";
        public static string SynthesisOutputType = "riff-16khz-16bit-mono-pcm";
    }

    public class BingSpeechConstants
    {
        //insert your api key
        public static string ApiSubscriptionKey = " c0f81630f12f4c60bfb63dabf8e650d2";
        public static string TokenUrl = "https://api.cognitive.microsoft.com/sts/v1.0/issueToken";
        public static string ServiceBaseUrl = "https://speech.platform.bing.com/";
        public static string SynthesisBaseUrl = $"{ServiceBaseUrl}synthesize";
        public static string RecognitionBaseUrl = $"{ServiceBaseUrl}/speech/recognition/interactive/cognitiveservices/v1";
        public static string LanguageLookupUrl = "http://traininglabservices.azurewebsites.net/api/SpeechHelper/Languages";
    }

    
}
