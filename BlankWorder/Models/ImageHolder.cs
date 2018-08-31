using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Prism.Mvvm;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.FileProperties;

namespace BlankWorder.Models
{
    public class ImageHolder : BindableBase
    {
        public IStorageItem StorageItem { get; }
        public string Name => StorageItem.Name;
        public string Path { get; }
        public BitmapSource Image { get; } = new BitmapImage();

        private IThumbnail thumbnail;
        public IThumbnail Thumbnail
        {
            get => thumbnail;
            private set => SetProperty(ref thumbnail, value);
        }

        public async void BeginInit()
        {
            if (StorageItem.IsOfType(StorageItemTypes.File))
            {
                var thumb = await ((StorageFile)StorageItem).GetThumbnailAsync(ThumbnailMode.PicturesView);
                if (thumb != null)
                    Image.SetSource(thumb);
                //var thumb = ;
                //Thumbnail = new FileThumbnail(thumb);
                return;
            }
            //Thumbnail = new FolderThumbnail();
        }

        public ImageHolder(IStorageItem storageItem)
        {
            StorageItem = storageItem;

            Path = storageItem.Path;
        }

        public static ImageHolder FromItem(IStorageItem item)
        {
            return new ImageHolder(item);
        }
    }
}
