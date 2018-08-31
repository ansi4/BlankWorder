using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using BlankWorder.Models;

namespace BlankWorder.Converters
{
    public class ThumbnailToImageConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var res = new BitmapImage
            {
                UriSource = new Uri("ms-appx:///Assets/NoImage.png")
            };

            if ((value as IThumbnail) == null)
                return res;

            if (value is FileThumbnail && (value as FileThumbnail)?.Thumbnail != null)
                res.SetSource((value as FileThumbnail)?.Thumbnail);
            else if (value is FolderThumbnail)
                res.UriSource = new Uri("ms-appx:///Assets/NoImage.png");

            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
