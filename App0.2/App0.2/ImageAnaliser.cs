using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Linq;

namespace App0._2
{

    static class ImageAnaliser
    {
        // Add your Computer Vision subscription key and endpoint
        static string subscriptionKey = "*";

        static string endpoint = "https://diana.cognitiveservices.azure.com/";


        public static ImageAnalysis AnalyzeImageUrl(ComputerVisionClient client, string imageUrl)
        {
            //ComputerVisionClient client = Authenticate(endpoint, subscriptionKey);

            // Creating a list that defines the features to be extracted from the image. 

            List<VisualFeatureTypes?> features = new List<VisualFeatureTypes?>(){
                VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces,
                VisualFeatureTypes.Tags, 
                VisualFeatureTypes.Color
            };
            // Analyze the URL image 
            return client.AnalyzeImageAsync(imageUrl, features).Result;
        }

        public static ComputerVisionClient Authenticate()
        {
            return Authenticate(endpoint, subscriptionKey);
        }
        /// <summary>
        /// Creates a Computer Vision client used by each example.
        /// </summary>
        private static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
              new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
              { Endpoint = endpoint };
            return client;
        }

        /// <summary>
        /// Возвращает список хэштегов.
        /// </summary>
        /// <param name="results">Результат анализа.</param>
        /// <returns>Лист строк - хэштегов.</returns>
        public static List<string> GetHeshtags(ImageAnalysis results)
        {
            List<string> ans = new List<string>();
            foreach (var i in results.Tags)
            {
                ans.Add("#" + i.Name);
            }
            return ans;


        }
    }
}
