using MeetingApp.ViewModels;
using MeetingApp.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MeetingApp
{
    public partial class App
    {

        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<MeetingDataTopPage, MeetingDataTopPageViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<MeetingDataCreatePage, MeetingDataCreatePageViewModel>();
            containerRegistry.RegisterForNavigation<MeetingAttendPage, MeetingAttendPageViewModel>();
            containerRegistry.RegisterForNavigation<MeetingExecuteTopPage, MeetingExecuteTopPageViewModel>();
            containerRegistry.RegisterForNavigation<MeetingLabelItemDataCreatePage, MeetingLabelItemDataCreatePageViewModel>();
            containerRegistry.RegisterForNavigation<MeetingExecuteUserPage, MeetingExecuteUserPageViewModel>();
        }
    }
}
