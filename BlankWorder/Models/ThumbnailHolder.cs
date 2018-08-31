using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.FileProperties;

namespace BlankWorder.Models
{
    public interface IThumbnail { }

    public class FileThumbnail: IThumbnail
    {
        public StorageItemThumbnail Thumbnail { get; }
        public FileThumbnail(StorageItemThumbnail thumb)
        {
            Thumbnail = thumb;
        }
    }

    public class FolderThumbnail : IThumbnail { }
}
