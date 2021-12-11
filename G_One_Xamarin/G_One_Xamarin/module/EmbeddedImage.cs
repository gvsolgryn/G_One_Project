using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace G_One_Xamarin.module
{
    /// <summary>
    /// 콘텐츠 프로퍼티 설정
    /// </summary>
    [ContentProperty("ResourceID")]
    public class EmbeddedImage : IMarkupExtension
    {
        /// <summary>
        /// 리소스 ID(이미지 ID) 설정 및 가져오기 설정
        /// </summary>
        public string ResourceId { get; set; }

        /// <summary>
        /// 데이터 바인딩에 사용되는 코드
        /// </summary>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (String.IsNullOrWhiteSpace(ResourceId))
                return null;

            return ImageSource.FromResource(ResourceId);
        }
    }
}
