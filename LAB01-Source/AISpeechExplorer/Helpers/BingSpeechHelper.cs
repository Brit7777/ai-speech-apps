using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace AISpeechExplorer.Helpers
{
    public static class BingSpeechHelper
    {
        //play a newly synthesized audio clip
        public async static Task SpeakContentAsync(IInputStream file)
        {
            InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();

            await RandomAccessStream.CopyAsync(file, stream);
            stream.Seek(0);

            Windows.UI.Xaml.Controls.MediaElement element = new Windows.UI.Xaml.Controls.MediaElement();
            element.SetSource(stream, "audio/x-wav");
            element.Play();

            return;
        }

        //generate XML-formated SSML content
        private static string GenerateSsml(string language, string gender, string name, string text)
        {
            var ssmlDoc = new XDocument(
                              new XElement("speak",
                                  new XAttribute("version", "1.0"),
                                  new XAttribute(XNamespace.Xml + "lang", language),
                                  new XElement("voice",
                                      new XAttribute(XNamespace.Xml + "lang", language),
                                      new XAttribute(XNamespace.Xml + "gender", gender),
                                      new XAttribute("name", name),
                                      text)));
            return ssmlDoc.ToString();
        }

        //calls the Bing Speech API Synthesize method to perform speech synthesize
        public async static Task<IInputStream> SynthesizeTextToSpeechAsync(string textContent, BingSpeechLanguageResult language)
        {
            IInputStream fileResult = null;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new HttpCredentialsHeaderValue("Bearer", await GetAccessTokenAsync());
                client.DefaultRequestHeaders.Add("X-Microsoft-OutputFormat", Common.AppConstants.SynthesisOutputType);
                client.DefaultRequestHeaders.Add("X-Search-AppId", Common.AppConstants.AppId);
                client.DefaultRequestHeaders.Add("X-Search-ClientID", Common.AppConstants.ClientId);
                client.DefaultRequestHeaders.Add("User-Agent", Common.AppConstants.AppDisplayName);

                try
                {
                    var response = await client.PostAsync(new Uri(Common.BingSpeechConstants.SynthesisBaseUrl), new HttpStringContent(GenerateSsml(language.Locale, language.Gender, language.VoiceLabel, $"{textContent}")));

                    fileResult = await response.Content.ReadAsInputStreamAsync();
                }
                catch (Exception ex)
                {

                }

            }

            return fileResult;
        }

        //generate a list of supported and available Bing Speech languages
        public static async Task<List<BingSpeechLanguageResult>> GetSupportedLanguagesAsync()
        {
            List<BingSpeechLanguageResult> supportedLanguages = new List<BingSpeechLanguageResult>();

            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(new Uri(Common.BingSpeechConstants.LanguageLookupUrl));
                var content = await result.Content.ReadAsStringAsync();

                supportedLanguages = JsonConvert.DeserializeObject<List<BingSpeechLanguageResult>>(content);
            }

            return supportedLanguages;
        }

        //calls the Bing Speech API Recognize method to perform speech recognition
        public static async Task<SpeechToText.SpeechRecognitionResult> RecognizeSpeechAync(byte[] bytes, bool maskProfanity = true)
        {
            SpeechToText.SpeechRecognitionResult result = null;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new HttpCredentialsHeaderValue("Bearer", await GetAccessTokenAsync());
                client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue(@"application/json;text/xml"));

                var payload = new HttpBufferContent(bytes.AsBuffer());
                payload.Headers.ContentType = new HttpMediaTypeHeaderValue(Common.AppConstants.RecognitionContentType);

                string language = "en-US";
                string locale = "en-US";
                string format = (maskProfanity) ? "detailed" : "simple";
                string profanityFilter = (maskProfanity) ? "masked" : "raw";

                string url = $"{Common.BingSpeechConstants.RecognitionBaseUrl}?language={language}&locale={locale}&format=detailed&profanity={profanityFilter}";

                try
                {
                    var response = await client.PostAsync(new Uri(url), payload);

                    var content = await response.Content.ReadAsStringAsync();

                    result = JsonConvert.DeserializeObject<SpeechToText.SpeechRecognitionResult>(content);
                }
                catch (Exception ex)
                {

                }

            }

            return result;
        }

        //generate a temporary access token to pass to Bing Speech API calls
        private static async Task<string> GetAccessTokenAsync()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Common.BingSpeechConstants.ApiSubscriptionKey);

                var result = await client.PostAsync(new Uri(Common.BingSpeechConstants.TokenUrl), null);

                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}