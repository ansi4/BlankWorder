using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Windows.Input;
using Prism.Commands;
using BlankWorder.Models;
using BlankWorder.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BlankWorder.Views
{
    public sealed partial class DirectoryTreeView : UserControl
    {
        public DirectoryTreeView()
        {
            this.InitializeComponent();
            AddFolder = new DelegateCommand(AddDirectory);
            RemoveFolder = new DelegateCommand(RemoveDirectory, CanRemoveDir);
        }

        private static void SelectedFolder_PropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;
            var view = (o as DirectoryTreeView);
            if (view != null)
            {                
                view.SelectedNode = null;
                (view.RemoveFolder as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        public DirectoryTreeViewModel ViewModel => DataContext as DirectoryTreeViewModel;
        public ICommand AddFolder { get; }
        public ICommand RemoveFolder { get; }

        public StorageFolderWrapper SelectedFolder
        {
            get => (StorageFolderWrapper)GetValue(SelectedFolderProperty);
            set => SetValue(SelectedFolderProperty, value);
        }

        public static DependencyProperty SelectedFolderProperty = DependencyProperty.Register(
                nameof(SelectedFolder), typeof(StorageFolderWrapper),
                typeof(DirectoryTreeView), new PropertyMetadata(null, SelectedFolder_PropertyChanged));

        public async void AddDirectory()
        {
            var picker = new FolderPicker();
            picker.FileTypeFilter.Add("*");
            var picked = await picker.PickSingleFolderAsync();
            if (picked == null)
                return;
            var displayed = await ViewModel.OpenDirectory(picked);
            FolderView.RootNodes.Add(ToNode(displayed));
        }

        public void RemoveDirectory()
        {
            FolderView.RootNodes.Remove(SelectedNode);
            SelectedFolder = null;
        }

        public bool CanRemoveDir()
        {
            return SelectedNode?.Depth.Equals(0) ?? false;
        }

        private TreeViewNode selectedNode;
        private TreeViewNode SelectedNode
        {
            get => selectedNode ?? (selectedNode = FolderView.RootNodes.SingleOrDefault(n => n.Content == SelectedFolder));
            set => selectedNode = value;
        }

        private TreeViewNode ToNode(StorageFolderWrapper folder)
        {
            var node = new TreeViewNode
            {
                Content = folder,
                HasUnrealizedChildren = folder.HasUnrealizedChildren,
            };
            if (folder.HasUnrealizedChildren)
                return node;

            folder.SubFolders.Select(ToNode).ToList().ForEach(node.Children.Add);
            return node;
        }

        private async void FolderView_Loaded(object sender, RoutedEventArgs e)
        {
            FolderView.RootNodes.Add(ToNode(await ViewModel.OpenDirectory(KnownFolders.PicturesLibrary)));

            (await ViewModel.GetRecentlyAccessedAsync()).Select(ToNode).ToList().ForEach(FolderView.RootNodes.Add);
        }

        private async void FolderView_Expanding(TreeView sender, TreeViewExpandingEventArgs args)
        {
            var childNodes = args.Node.Children
                .Where(c => c.HasUnrealizedChildren)
                .Select(c => new { Parent = c, Task = ViewModel.GetChildFolders((StorageFolderWrapper)c.Content) });
            foreach (var t in childNodes)
            {
                (await t.Task).Select(ToNode).ToList().ForEach(t.Parent.Children.Add);
                t.Parent.HasUnrealizedChildren = false;
            }
        }

        private void FolderView_ItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
        {
            SelectedFolder = ((TreeViewNode)args.InvokedItem).Content as StorageFolderWrapper;
        }
    }
}
