using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace AISpeechExplorer.Helpers
{
    //public static class TextToSpeechHelper
    //{
    //    public async static Task<IInputStream> GetTextTranslationForSpeechAsync(string content)
    //    {
    //        HttpClient client = new HttpClient();
            
    //        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Common.CoreConstants.TranslatorTextSubscriptionKey);

    //        var result = await client.GetAsync(new Uri($"https://api.microsofttranslator.com/V2/Http.svc/Speak?text={Uri.EscapeDataString(content)}&language=en-US&options=Male"));

    //        var fileResult = await result.Content.ReadAsInputStreamAsync();

    //        return fileResult;
    //    }
    //}
}
