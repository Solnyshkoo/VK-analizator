using App0._2.Services;
using App0._2.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App0._2
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
