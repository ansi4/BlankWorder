using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Prism.Mvvm;

namespace BlankWorder.Models
{
    public class StorageFolderWrapper : BindableBase, IStorageFolder
    {
        public IStorageFolder Folder { get; }

        private ObservableCollection<StorageFolderWrapper> subFolders = new ObservableCollection<StorageFolderWrapper>();
        public ObservableCollection<StorageFolderWrapper> SubFolders
        {
            get => subFolders;
            set => SetProperty(ref subFolders, value);
        }

        private bool hasUnrealizedChildren = false;
        private bool parentIsLibraryAndHasSameNameSibling = false;

        public bool HasUnrealizedChildren
        {
            get => !hasUnrealizedChildren;
            set => SetProperty(ref hasUnrealizedChildren, !value);
        }

        public bool IsRoot { get; }

        public StorageFolderWrapper()
        {
        }

        public StorageFolderWrapper(IStorageFolder folder, bool isRoot = false)
        {
            Folder = folder;
            IsRoot = isRoot;
        }

        public static StorageFolderWrapper FromFoder(IStorageFolder folder)
        {
            return new StorageFolderWrapper(folder);
        }

        IAsyncOperation<StorageFile> IStorageFolder.CreateFileAsync(string desiredName)
            => Folder.CreateFileAsync(desiredName);

        public IAsyncOperation<StorageFile> CreateFileAsync(string desiredName, CreationCollisionOption options)
            => Folder.CreateFileAsync(desiredName, options);


        public IAsyncOperation<StorageFolder> CreateFolderAsync(string desiredName)
            => Folder.CreateFolderAsync(desiredName);

        public IAsyncOperation<StorageFolder> CreateFolderAsync(string desiredName, CreationCollisionOption options)
            => Folder.CreateFolderAsync(desiredName, options);

        public IAsyncOperation<StorageFile> GetFileAsync(string name)
            => Folder.GetFileAsync(name);

        public IAsyncOperation<StorageFolder> GetFolderAsync(string name)
            => Folder.GetFolderAsync(name);

        public IAsyncOperation<IStorageItem> GetItemAsync(string name)
            => Folder.GetItemAsync(name);

        public IAsyncOperation<IReadOnlyList<StorageFile>> GetFilesAsync()
            => Folder.GetFilesAsync();

        public IAsyncOperation<IReadOnlyList<StorageFolder>> GetFoldersAsync()
            => Folder.GetFoldersAsync();

        public IAsyncOperation<IReadOnlyList<IStorageItem>> GetItemsAsync()
            => Folder.GetItemsAsync();

        public IAsyncAction RenameAsync(string desiredName)
            => Folder.RenameAsync(desiredName);

        public IAsyncAction RenameAsync(string desiredName, NameCollisionOption option)
            => Folder.RenameAsync(desiredName, option);

        public IAsyncAction DeleteAsync()
            => Folder.DeleteAsync();

        public IAsyncAction DeleteAsync(StorageDeleteOption option)
            => Folder.DeleteAsync(option);

        public IAsyncOperation<BasicProperties> GetBasicPropertiesAsync()
            => Folder.GetBasicPropertiesAsync();

        public bool IsOfType(StorageItemTypes type)
            => Folder.IsOfType(type);

        public FileAttributes Attributes => Folder.Attributes;
        public DateTimeOffset DateCreated => Folder.DateCreated;
        public string Name => (IsRoot || ParentIsLibraryAndHasSameNameSibling) ? Path : Folder.Name;
        public string Path => IsLibrary ? Folder.Name : Folder.Path;

        public bool IsLibrary => (Folder as StorageFolder)?.DisplayType?.Equals("Library") ?? false;
        public bool ParentIsLibraryAndHasSameNameSibling
        {
            get => parentIsLibraryAndHasSameNameSibling;
            set
            {
                SetProperty(ref parentIsLibraryAndHasSameNameSibling, value);
                RaisePropertyChanged(nameof(Name));
            }
        }
    }
}
