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
    
    public partial class StatisticPage : ContentPage
    {
        public static List<string> Photos = VkParser.PhotoUploadUrl.Keys.ToList<string>();
        //string url = "https://sun9-34.userapi.com/impf/c853624/v853624041/83de7/4q6hzLRhHIQ.jpg?size=1080x1080&quality=96&sign=3d5bbbf91532c0d6b5dee499430b2caf&type=album";
        public StatisticPage()
        {
            InitializeComponent();
            ActivityIndicator loading = new ActivityIndicator() {IsRunning = true, IsVisible = true };
            StatisticButton.Clicked += StatisticButton_Clicked;
            Photos = VkParser.PhotoUploadUrl.Keys.ToList<string>();
            AddImages();
            loading.IsRunning = false;
            loading.IsVisible = false;

        }

        /// <summary>
        /// Открытие страницы общей статистики.
        /// </summary>
        private void StatisticButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GeneralStatisticPage());
        }

        /// <summary>
        /// Добавление изображение в сетку.
        /// </summary>
        private void AddImages()
        {
            for (int i = 0; i< Photos.Count / 3 + Photos.Count % 3 % 2; i++)
            {
                grid.RowSpacing = 1;
                grid.RowDefinitions.Add(new RowDefinition() { Height = 150 });
                for (int j = 0; j < 3 && i*3+j<Photos.Count; j++)
                {
                    try
                    {

                        ImageButton image = new ImageButton()
                        {
                            Aspect = Aspect.AspectFill,
                            Source = new UriImageSource
                            {
                                AutomationId = VkParser.PhotoUploadUrl[Photos[i * 3 + j]],
                                CachingEnabled = true,
                                CacheValidity = new System.TimeSpan(2, 0, 0, 0),
                                Uri = new System.Uri(VkParser.PhotoUploadUrl[Photos[i*3 + j]])
                                
                            }

                        };
                        image.Clicked += Image_Clicked;
                        grid.Children.Add(image, j, i);
                    }
                    catch 
                    {
                        Photos.RemoveAt(j + 3*i);
                        j--;
                    }
                }

            }
        }

        /// <summary>
        ///  Обработка события нажатия на изображение - переход к статистики по фото.
        /// </summary>
        private void Image_Clicked(object sender, EventArgs e)
        {
           Navigation.PushAsync(new PhotoStatisticPage(((ImageButton)sender).Source.AutomationId));
        }
    }
}