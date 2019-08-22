using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;

namespace MeetingApp.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public ICommand NavigationSignUpPageCommand { get; }
        INavigationService _navigationService;
        public LoginPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            NavigationSignUpPageCommand = new DelegateCommand(() =>
           {
               _navigationService.NavigateAsync("SignUpPage", useModalNavigation: true);
           });
        }


    }
}
