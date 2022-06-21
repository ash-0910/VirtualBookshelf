using System;
using VirtualBookshelf.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VirtualBookshelf
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

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
