using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace TextAnalyticsSample
{
    /// <summary>
    /// This is a sample program  that shows how to use the Azure ML Text Analytics app (https://datamarket.azure.com/dataset/amla/text-analytics)
    /// </summary>
    public class Program
    {
        private const string ServiceBaseUri = "https://api.datamarket.azure.com/";
        public static SentimentResult sr;
       public string  GetSentiment(string args)
        {
            sr = new SentimentResult();
            return my(new string[] { args, "r0AOfcdEK9LNggwOdioFQqE//APp6imQr7G/llRoJQE" });
        }
        static string my(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Invalid arguments. This program requires 2 arguments -");
                Console.WriteLine("\t <input text>: string to be analyzed");
                Console.WriteLine("\t <Account key>: r0AOfcdEK9LNggwOdioFQqE//APp6imQr7G/llRoJQE");
                return "";
            }

            string inputText = args[0];
            string accountKey = args[1];

            using (var httpClient = new HttpClient())
            {
                string inputTextEncoded = HttpUtility.UrlEncode(inputText);

                httpClient.BaseAddress = new Uri(ServiceBaseUri);
                string creds = "AccountKey:" + "r0AOfcdEK9LNggwOdioFQqE//APp6imQr7G/llRoJQE";
                string authorizationHeader = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(creds));
                httpClient.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // get key phrases
                string keyPhrasesRequest = "data.ashx/amla/text-analytics/v1/GetKeyPhrases?Text=" + inputTextEncoded;
                Task<HttpResponseMessage> responseTask = httpClient.GetAsync(keyPhrasesRequest);
                responseTask.Wait();
                HttpResponseMessage response = responseTask.Result;
                Task<string> contentTask = response.Content.ReadAsStringAsync();
                contentTask.Wait();
                string content = contentTask.Result;
                if (!response.IsSuccessStatusCode)
                {
                    //Console.ReadKey();
                    throw new Exception("Call to get key phrases failed with HTTP status code: " +
                                        response.StatusCode + " and contents: " + content);
                }

                KeyPhraseResult keyPhraseResult = JsonConvert.DeserializeObject<KeyPhraseResult>(content);
                //Console.WriteLine("Key phrases: " + string.Join(",", keyPhraseResult.KeyPhrases));

                // get sentiment
                string sentimentRequest = "data.ashx/amla/text-analytics/v1/GetSentiment?Text=" + inputTextEncoded;
                responseTask = httpClient.GetAsync(sentimentRequest);
                responseTask.Wait();
                response = responseTask.Result;
                contentTask = response.Content.ReadAsStringAsync();
                contentTask.Wait();
                content = contentTask.Result;
                if (!response.IsSuccessStatusCode)
                {
                    Console.ReadKey();
                    throw new Exception("Call to get sentiment failed with HTTP status code: " +
                                        response.StatusCode + " and contents: " + content);
                }

                SentimentResult sentimentResult = JsonConvert.DeserializeObject<SentimentResult>(content);
                Console.WriteLine("Sentiment score: " + sentimentResult.Score);
                sr.Score = sentimentResult.Score;
                sr.compare();
                if (sr.Score >= 0.6)
                {
                     return "Sentiment Score: " +sentimentResult.Score+ "... User is happy :)";

                }
                else
                {
                    return "Sentiment Score: " + sentimentResult.Score + "... User is unhappy :(";
                }
            }
            //Console.ReadKey();
        }
    }

    /// <summary>
    /// Class to hold result of Key Phrases call
    /// </summary>
    public class KeyPhraseResult
    {
        public List<string> KeyPhrases { get; set; }
    }

    /// <summary>
    /// Class to hold result of Sentiment call
    /// </summary>
    public class SentimentResult
    {
        public SentimentResult() { }
        public double Score { get; set; }

        public void compare()
        {

           
        }


    }
}
