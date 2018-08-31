using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using BlankWorder.Models;

namespace BlankWorder.Services
{
    public class FileSystemService
    {
        private IStorageItemAccessList fileCacheProvider = StorageApplicationPermissions.FutureAccessList;

        public async Task<IList<StorageFolderWrapper>> GetAccessibleFoldersList()
        {
            var tokens = fileCacheProvider.Entries.Select(i => i.Token);
            var cachedFoldersList = new List<StorageFolderWrapper>(fileCacheProvider.Entries.Count);
            foreach (var getFolderTask in tokens.Select(fileCacheProvider.GetFolderAsync))
            {
                 cachedFoldersList.Add(new StorageFolderWrapper(await getFolderTask));
            }
            return cachedFoldersList;
        }
    }
}
