using MeetingApp.Models.Constants;
using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;

namespace MeetingApp.ViewModels
{

    public class SignUpPageViewModel : ViewModelBase
    {

        private string _signUpUserId;
        private string _signUpPassword;
        private bool _signUpFlag;
        public string SignUpUserId
        {
            get { return _signUpUserId; }
            set { SetProperty(ref _signUpUserId, value); }
        }

        public string SignUpPassword
        {
            get { return _signUpPassword; }
            set { SetProperty(ref _signUpPassword, value); }
        }

        public bool SignUpFlag
        {
            get { return _signUpFlag; }
            set { SetProperty(ref _signUpFlag, value); }
        }
        public ICommand SignUpCommand { get; }
        public ICommand GoBackCommand { get; }
        RestService _restService;
        INavigationService _navigationService;

        public SignUpPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _restService = new RestService();
            _navigationService = navigationService;

            SignUpCommand = new DelegateCommand(async () =>
            {
                SignUpFlag = await _restService.SignUpUserDataAsync(UserConstants.OpenUserEndPoint, SignUpUserId, SignUpPassword);
                if (SignUpFlag == true)
                {
                    await _navigationService.NavigateAsync("LoginPage");
                }

            });

            GoBackCommand = new DelegateCommand(async () =>
            {
                _ = await _navigationService.GoBackAsync();
            });
        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {

            base.OnNavigatingTo(parameters);

            //‰ï‹cî•ñ‘SŒæ“¾API‚ÌƒR[ƒ‹
            SignUpFlag = false;


        }


    }
}
