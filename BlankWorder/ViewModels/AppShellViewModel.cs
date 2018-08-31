using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Windows.Navigation;
using Prism.Windows.AppModel;
using BlankWorder.Models;

namespace BlankWorder.ViewModels
{
    public class AppShellViewModel: BindableBase
    {
        private INavigationService NavigationService { get; }
        private IResourceLoader ResourceLoader { get; }

        public ObservableCollection<NavigationMenuItem> NavigationItems { get; }

        private NavigationMenuItem selectedItem;
        public NavigationMenuItem SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }

        private ICommand itemInvokedCommand;
        public ICommand ItemInvokedCommand
        {
            get => itemInvokedCommand;
            set => SetProperty(ref itemInvokedCommand, value);
        }


        public void SelectWithNavigateToProperty(string navTo)
        {
            var newlySelected = NavigationItems.FirstOrDefault(i => i.NavigateTo.Equals(navTo, StringComparison.InvariantCultureIgnoreCase));
            if (newlySelected != null)
                SelectedItem = newlySelected;
        }

        public AppShellViewModel(INavigationService navigationService, IResourceLoader resourceLoader)
        {
            ResourceLoader = resourceLoader;
            NavigationService = navigationService;

            ItemInvokedCommand = new DelegateCommand<NavigationMenuItem>(item =>
                NavigationService.Navigate(item.NavigateTo, null),
                item => !item.Equals(selectedItem)
            );

            Func<string, string> getRes = ResourceLoader.GetString;
            NavigationItems = new ObservableCollection<NavigationMenuItem>
            {
                new NavigationMenuItem(getRes("Shell_Photos"), PageTokens.MainPage, "\uEB9F"),
                new NavigationMenuItem(getRes("Shell_Explorer"), PageTokens.NavigationPage, "\uED25"),
            };
        }
    }
}
