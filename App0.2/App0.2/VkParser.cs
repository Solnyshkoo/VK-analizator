using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace App0._2
{
    static class VkParser
    {

        public static Dictionary<string, int> PhotosLikes { get; set; }

        public static Dictionary<string, int> PhotosReposts { get; set; }

        public static Dictionary<string, int> PostComments { get; set; }

        public static Dictionary<string, string> PhotoUploadUrl { get; set; }

        // Словарь ссылка-объект с результатом машинного изучения.
        public static Dictionary<string, ImageAnalysis> AIAniliseByUrl { get; set; }

        private static string token = "*";

        /// <summary>
        /// Получает ссылку на вк и вытаскивает id
        /// </summary>
        /// <param name="url">ссылка на вк</param>
        /// <returns>id</returns>
        public static string GetId(string url)
        {
            string id = "";
            if (url.Contains("id"))
            {
                //id = url[(url.IndexOf("id") + 2)..];
                id = url.Substring((url.IndexOf("id") + 2));
            }
            else
            {
                //string tag = url[(url.LastIndexOf("/") + 1)..];
                string tag = url.Substring((url.LastIndexOf("/") + 1));
                string userInfo = Get("https://api.vk.com/method/users.get?v=5.52&" + $"user_ids={tag}&access_token={token}",
                HttpStatusCode.OK);
                int length = userInfo.IndexOf("last_name") - userInfo.IndexOf("id") - 6;
                id = userInfo.Substring(userInfo.IndexOf("id") + 4, length);
            }
            return id;
        }



        /// <summary>
        /// Отправка get запросов.
        /// </summary>
        /// <param name="url">Ссылка запроса.</param>
        /// <param name="httpStatusCode">Статус корректного запроса.</param>
        /// <returns>Резкльтат завпроса в формате json.</returns>
        private static string Get(string url, HttpStatusCode httpStatusCode)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";
            request.Accept = "application/json";
            //request.UserAgent = "Mozilla/5.0 ....";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // 200 or 100
            //if (response.StatusCode == HttpStatusCode.OK || response.StatusCode = HttpStatusCode.Continue)
            if (response.StatusCode != httpStatusCode)
            {
                new Exception(response.StatusCode.ToString());
            }
            StreamReader reader = new StreamReader(response.GetResponseStream());
            StringBuilder output = new StringBuilder();
            output.Append(reader.ReadToEnd());
            response.Close();
            return output.ToString();
        }
        public static void ParseVKPage(string VKID, bool logs = false)
        {
            try
            {
                //string token = "";
                string appID = ""; // Here goes your VK app ID 
                // Выполняем запрос по адресу и получаем ответ в виде строки
                string photoInfo = Get("https://api.vk.com/method/photos.getAll?v=5.52&owner_" + $"id={VKID}&access_token={token}&extended=1&count=200",
                    HttpStatusCode.OK);
                //Console.WriteLine(photoInfo);
                string photoComments = Get("https://api.vk.com/method/photos.getAllComments?v=5.52&owner_" + $"id={VKID}&access_token={token}&extended=1&count=100",
                    HttpStatusCode.Continue);
                if (logs)
                {
                    Console.WriteLine($"Successfully got an api request for id {VKID}");
                }
                Dictionary<string, int> photosLikes = new Dictionary<string, int>();
                Dictionary<string, int> photosReposts = new Dictionary<string, int>();
                Dictionary<string, string> photoUploadUrl = new Dictionary<string, string>();
                Dictionary<string, ImageAnalysis> aIAniliseByUrl = new Dictionary<string, ImageAnalysis>();
                var parsedLIkesAndReposts = JObject.Parse(photoInfo);
                ComputerVisionClient AIclient = ImageAnaliser.Authenticate();
                bool flag = true;
                foreach (var photo in parsedLIkesAndReposts["response"]["items"])
                {

                    photosLikes[(string)photo["id"]] = (int)photo["likes"]["count"];
                    photosReposts[(string)photo["id"]] = (int)photo["reposts"]["count"];
                    photoUploadUrl[(string)photo["id"]] = (string)photo["photo_604"];
                    /*if (flag)
                    {
                        flag = false;*/
                        
                        //aIAniliseByUrl[(string)photo["photo_604"]] = ImageAnaliser.AnalyzeImageUrl(AIclient, (string)photo["photo_604"]); 
                    //}
                }

                Dictionary<string, int> postComments = new Dictionary<string, int>();
                var parsedComments = JObject.Parse(photoComments);
                foreach (var post in parsedComments["response"]["items"])
                {
                    //Console.WriteLine((string)post["pid"]);

                    if (postComments.ContainsKey((string)post["pid"]))
                    {
                        postComments[(string)post["pid"]]++;
                    }
                    else
                    {
                        postComments[(string)post["pid"]] = 1;
                    }
                    //Console.WriteLine(postComments[(string)post["pid"]]);
                }
                PhotosLikes = photosLikes;
                PhotosReposts = photosReposts;
                PostComments = postComments;
                PhotoUploadUrl = photoUploadUrl;
                AIAniliseByUrl = aIAniliseByUrl;
                Console.WriteLine("--------------");
                foreach (var item in PhotoUploadUrl)
                {

                    Console.WriteLine(item);
                }
                // TODO: Возвращаемое значение функции.
                // TODO: Проверить что работают посты. ( c фото вроде все ок , может  у меня нет постов)
                // TODO: Проверить тип id фото и тд, который записывается в словарь ( пока ключи везде string, но возможно должны быть int)
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (logs)
                {
                    Console.WriteLine($"Invalid request for id {VKID}. Exception code {e.Message}");
                }
            }
        }

    }
}
