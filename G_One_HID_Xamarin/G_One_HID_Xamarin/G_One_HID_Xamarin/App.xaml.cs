using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace G_One_HID_Xamarin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            switch (Device.Idiom)
            {
                case TargetIdiom.Phone:
                    MainPage = new Page_Mobile();
                    break;
                case TargetIdiom.Desktop:
                    MainPage = new Page_Desktop();
                    break;
                case TargetIdiom.Unsupported:
                    break;
                case TargetIdiom.Tablet:
                    break;
                case TargetIdiom.TV:
                    break;
                case TargetIdiom.Watch:
                    break;
                default:
                    MainPage = new MainPage();
                    break;
            }
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
