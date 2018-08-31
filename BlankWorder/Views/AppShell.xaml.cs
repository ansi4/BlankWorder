using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Windows.Input;
using Prism.Commands;
using System.Globalization;
using BlankWorder.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BlankWorder.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppShell: Page
    {
        private AppShellViewModel ViewModel => DataContext as AppShellViewModel;

        public AppShell()
        {
            this.InitializeComponent();

            ItemInvoked = new DelegateCommand<NavigationViewItemInvokedEventArgs>(
                args => ViewModel.ItemInvokedCommand.Execute(args.InvokedItem),
                args => ViewModel.ItemInvokedCommand.CanExecute(args.InvokedItem)
            );
        }

        public void SetContentFrame(Frame frame)
        {
            Navigation.Content = frame;
            frame.Navigated += ContentFrame_Navigated;
        }

        private ICommand ItemInvoked { get; }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
             
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            var tag = e.SourcePageType.Name.Replace("Page", "", StringComparison.InvariantCultureIgnoreCase);
            ViewModel.SelectWithNavigateToProperty(tag);
        }
    }
}
