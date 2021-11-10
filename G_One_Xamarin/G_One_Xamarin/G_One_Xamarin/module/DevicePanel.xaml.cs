using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace G_One_Xamarin.module
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevicePanel : ContentView
    {
        public readonly MainPage _parent;

        public DevicePanel(MainPage parent)
        {
            InitializeComponent();

            _parent = parent;

        }

        private void ButtonClick(object sender, EventArgs e)
        {
            Console.WriteLine("Test");
        }
    }
}