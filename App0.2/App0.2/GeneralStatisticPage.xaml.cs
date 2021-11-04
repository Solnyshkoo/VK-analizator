using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microcharts;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using Entry = Microcharts.ChartEntry;
namespace App0._2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GeneralStatisticPage : ContentPage
    {
        public GeneralStatisticPage()
        {
            InitializeComponent();
            BuildChart(chartViewLikes, VkParser.PhotosLikes);
            BuildChart(chartViewComments, VkParser.PostComments);
            BuildChart(chartViewReposts, VkParser.PhotosReposts);
        }

        /// <summary>
        /// Построение диаграммы.
        /// </summary>
        /// <param name="chartView">Элемент на странице.</param>
        /// <param name="ImageDict">Словарь значений.</param>
        private void BuildChart(Microcharts.Forms.ChartView chartView,Dictionary<string,int> ImageDict)
        {
            var entries = new List<ChartEntry>();
            foreach (var image in StatisticPage.Photos)
            {
                if (ImageDict.ContainsKey(image))
                {
                    entries.Add(new ChartEntry(ImageDict[image])
                    {
                        Label = ImageDict[image].ToString(),
                        ValueLabel = ImageDict[image].ToString(),
                        Color = SKColor.Parse("#CC1B37")
                    });
                }
                else
                {
                    entries.Add(new ChartEntry(0)
                    {
                        Label = "0",
                        ValueLabel = "0",
                        Color = SKColor.Parse("#CC1B37"),
                        ValueLabelColor = SKColor.Parse("#CC1B37"),

                    });
                }
            }
            var chart = new LineChart()
            {
                Entries = entries,
                LineMode = LineMode.Straight,
                LineSize = 8,
                PointMode = PointMode.Square,
                PointSize = 18,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelOrientation = Orientation.Horizontal,
                LabelColor = SKColor.Parse("#CC1B37"),
                LabelTextSize = 30

            };
            chartView.Chart = chart;
        }
    }
}