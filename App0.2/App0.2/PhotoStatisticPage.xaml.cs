using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App0._2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoStatisticPage : ContentPage
    {
        string Url;
        string PhotoId;
        public PhotoStatisticPage(string url)
        {
            InitializeComponent();
            Url = url;
            image.Source = new UriImageSource
            {
                CachingEnabled = true,
                CacheValidity = new System.TimeSpan(2, 0, 0, 0),
                Uri = new System.Uri(Url)
            };
            PhotoId = VkParser.PhotoUploadUrl.Where(x => x.Value == url).FirstOrDefault().Key;
            Likes.Text = VkParser.PhotosLikes[PhotoId].ToString();
            try
            {
                Comments.Text = VkParser.PostComments[PhotoId].ToString();
            }
            catch
            {
                Comments.Text = "0";
            }
            Reposts.Text = VkParser.PhotosReposts[PhotoId].ToString();
            ColorAnalise();
            GetHashTags();
            GetDescription();
            GetFaces();
        }

        /// <summary>
        /// Заполненение статиcтики по цветам изображения
        /// </summary>
        private void ColorAnalise()
        {
            try
            {
                var statistic = VkParser.AIAniliseByUrl[Url];
                
                ForegroundColor.Text = statistic.Color.DominantColorForeground;
                BackgroundColor.Text = statistic.Color.DominantColorBackground;
                string ans = "";
                foreach(var i in statistic.Color.DominantColors)
                {
                    if(ans.Length>0) ans += ", ";
                    ans += i;
                }
                DominateColors.Text = ans;
            }
            catch
            {
                return;
            }
        }
        /// <summary>
        /// Заполненение статиcтики по хэштегам изображения
        /// </summary>
        private void GetHashTags()
        {
            try
            {
                string ans = "";
                foreach(var i in ImageAnaliser.GetHeshtags(VkParser.AIAniliseByUrl[Url])){
                    string h = i;
                    h.Replace(' ', '-');
                    ans += h;
                    ans += " ";
                    
                }
                HashTags.Text = ans;
            }
            catch
            {
                return;
            }
        }
        /// <summary>
        /// Заполненение описания изображения
        /// </summary>
        private void GetDescription()
        {
            try
            {
                string ans = "";
                foreach (var i in VkParser.AIAniliseByUrl[Url].Description.Captions)
                {
                    if (ans.Length > 0) ans += ", ";
                    ans += i.Text;

                }
                Description.Text = ans;
            }
            catch
            {
                return;
            }
        }

        /// <summary>
        /// Заполненение описания лиц изображения
        /// </summary>
        private void GetFaces()
        {
            try
            {
                string ans = "";
                foreach (var i in VkParser.AIAniliseByUrl[Url].Faces)
                {
                    if (ans.Length > 0) ans += ", ";
                    if(i.Gender.HasValue){
                        if (i.Gender == Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models.Gender.Female) ans += "Женщина ";
                        else ans += "Мужщина ";
                    }
                    ans += i.Age.ToString()+" лет";
                }
                Faces.Text = ans;
            }
            catch
            {
                return;
            }
        }
    }
}