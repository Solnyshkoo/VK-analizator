using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App0._2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            EnterButton.Clicked += TextConmlited;
            UrlEnter.Background = Brush.White;
        }
        /// <summary>
        /// Обработчик введенной ссылки на профиль.
        /// </summary>
        private void TextConmlited(object sender, EventArgs e)
        {
            string text = UrlEnter.Text;
            if (IsUrlCorrect(text))
            {
                UrlEnter.Background = Brush.White;
                ErrorText.IsVisible = false;
                string vkId = VkParser.GetId(text);
                VkParser.ParseVKPage(vkId);
                Navigation.PushAsync(new StatisticPage());
                // Вызов следущей страницы от текст.
            }
            else
            {
                UrlEnter.Background = Brush.LightPink;
                ErrorText.IsVisible = true;
                UrlEnter.Text = String.Empty;
            }
        }

        /// <summary>
        /// Проверка корректности сслыки на профиль.
        /// </summary>
        /// <param name="url">Ссылка.</param>
        /// <returns>Коректна ли ссылка.</returns>
        private bool IsUrlCorrect(string url)
        {
            if (url is null) return false;
            url = url.Trim();
            if (!LinkExist(url)) return false;

            string[] list = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (list.Length != 3) return false;
            if (list[0] != "https:" || list[1] != "vk.com") return false;
            List<string> VkWords = new List<string>{"im","feed","notes","groups"
                ,"albums","audio","video","docs","io","vklive","gifts","cc","market",
            "photo","admin"};

            if (list[2].Contains('?') || VkWords.Contains(list[2])) return false;
            if (list[2].Length < 5 || list[2].Length > 32) return false;
            if (IsNumber(list[2][0]) && IsNumber(list[2][1]) && IsNumber(list[2][2]))
            {
                return false;
            }
            if (list[2][0] == '_' || list[2][list[2].Length - 1] == '_') return false;
            for (int i = 0; i < list[2].Length; i++)
            {
                if (list[2][i] == '.')
                {
                    if (list[2].Length - 1 - i < 4 && IsLetter(list[2][i + 1]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// провекрка существования ссылки.
        /// </summary>
        /// <param name="url">Ссылка.</param>
        /// <returns>Существует ли ссылка.</returns>
        private bool LinkExist(string url)
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    string HTMLSource = wc.DownloadString(url);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Проверяет является ли символ цифрой.
        /// </summary>
        /// <param name="str">Символ.</param>
        /// <returns>Является ли символ цифрой.</returns>
        private bool IsNumber(char str)
            => str <= '9' && str >= '0';

        /// <summary>
        /// Проверяет является ли символ буквой.
        /// </summary>
        /// <param name="str">Символ.</param>
        /// <returns>Является ли символ буквой.</returns>
        private bool IsLetter(char str)
            => 'a' <= str && str <= 'z' || 'A' <= str && str <= 'Z';
    }
}
