using System;
using System.Threading.Tasks;
using System.Linq;
using Prism.Mvvm;
using Prism.Unity.Windows;
using Prism.Windows.Navigation;
using Prism.Windows.AppModel;
using Unity;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using BlankWorder.Views;
using BlankWorder.ViewModels;

namespace BlankWorder
{
    sealed partial class App: PrismUnityApplication
    {
        private static readonly string viewModelNamespace = typeof(AppShellViewModel).Namespace;
        private static readonly string viewModelAssembly = typeof(AppShellViewModel).Assembly.FullName;

        public App()
        {
            InitializeComponent();
        }

        public void SetNavigationFrame(Frame frame)
        {
            var sessionStateService = Container.Resolve<ISessionStateService>();
            CreateNavigationService(new FrameFacadeAdapter(frame), sessionStateService);
        }

        protected override UIElement CreateShell(Frame rootFrame)
        {
            var shell = Container.TryResolve<AppShell>();
            shell.SetContentFrame(rootFrame);
            SetNavigationFrame(rootFrame);
            return shell;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(t => {
                var removeEndings = new[] { "Page", "View" };
                var ending = removeEndings.FirstOrDefault(t.Name.EndsWith);
                var viewModelName = ending != null ? t.Name.Remove(t.Name.Length - ending.Length) : t.Name;
                return Type.GetType($"{viewModelNamespace}.{viewModelName}ViewModel, {viewModelAssembly}");
            });
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate(PageTokens.NavigationPage, null);
            return Task.CompletedTask;
        }
    }
}