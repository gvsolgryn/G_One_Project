using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace G_One_Xamarin.module
{
    [ContentProperty("ResourceID")]
    public class EmbeddedImage : IMarkupExtension
    {
        public string ResourceID { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (String.IsNullOrWhiteSpace(ResourceID))
                return null;

            return ImageSource.FromResource(ResourceID);
        }
    }
}
