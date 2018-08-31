using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Windows.Storage;
using Windows.Storage.AccessCache;

using BlankWorder.Models;

namespace BlankWorder.ViewModels
{
    public class DirectoryTreeViewModel: BindableBase
    {
        public ObservableCollection<StorageFolderWrapper> Directories { get; }
            = new ObservableCollection<StorageFolderWrapper>();

        public DirectoryTreeViewModel()
        {
        }

        public async Task<IList<StorageFolderWrapper>> GetRecentlyAccessedAsync()
        {
            var entries = StorageApplicationPermissions.MostRecentlyUsedList.Entries;
            var folders = new List<StorageFolderWrapper>(entries.Count);
            foreach (var entry in entries)
            {
                var folder = await StorageApplicationPermissions.MostRecentlyUsedList.GetFolderAsync(entry.Token);
                var dir = await OpenDirectory(folder);
                folders.Add(dir);
            }
            return folders;
        }

        public async Task<StorageFolderWrapper> OpenDirectory(IStorageFolder folder)
        {
            if (folder == null)
                throw new NullReferenceException("Folder cannot be absent");
            StorageApplicationPermissions.MostRecentlyUsedList.Add(folder);
            var wrapper = new StorageFolderWrapper(folder, isRoot: true);
            Directories.Add(wrapper);
            await GetChildFolders(wrapper);
            wrapper.HasUnrealizedChildren = false;
            return wrapper;
        }

        public async Task<ICollection<StorageFolderWrapper>> GetChildFolders(StorageFolderWrapper wrapper)
        {
            var subDirs = (await wrapper.GetFoldersAsync()).Select(StorageFolderWrapper.FromFoder).ToList();
            if (wrapper.IsLibrary)
            {
                subDirs
                    .GroupBy(s => s.Name)
                    .Where(g => g.Count() > 1)
                    .SelectMany(g => g).ToList()
                    .ForEach(d => d.ParentIsLibraryAndHasSameNameSibling = true);
            }
            subDirs.ForEach(wrapper.SubFolders.Add);
            return subDirs;
        }
    }
}
