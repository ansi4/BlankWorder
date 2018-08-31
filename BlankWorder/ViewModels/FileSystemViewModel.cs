using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Extensions.FileProviders;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Prism.Mvvm;
using Prism.Commands;
using BlankWorder.Models;
using System.ComponentModel;

namespace BlankWorder.ViewModels
{
    public class FileSystemViewModel : BindableBase
    {
        private StorageFolderWrapper selectedFolder;

        public StorageFolderWrapper SelectedFolder
        {
            get => selectedFolder;
            set => SetProperty(ref selectedFolder, value);
        }
        private ObservableCollection<ImageHolder> images = new ObservableCollection<ImageHolder>();
        public ObservableCollection<ImageHolder> Images
        {
            get => images;
            set => SetProperty(ref images, value);
        }

        public FileSystemViewModel()
        {
            PropertyChanged += HandlePropertyChange;
            //IFileProvider provider = 
        }

        private void HandlePropertyChange(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(SelectedFolder):
                    SelectedFolderChanged(sender, args);
                    break;
            }
        }

        private async void SelectedFolderChanged(object sender, PropertyChangedEventArgs args)
        {
            if (SelectedFolder == null)
            {
                Images.Clear();
                return;
            }

            var folder = SelectedFolder;
            var images = (await folder.GetItemsAsync().AsTask().ContinueWith(t => t.Result.Select(ImageHolder.FromItem)));
            Images.Clear();
            foreach (var image in images)
            {
                image.BeginInit();
                Images.Add(image);
            }
        }
    }
}
