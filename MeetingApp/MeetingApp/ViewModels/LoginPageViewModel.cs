using MeetingApp.Models.Constants;
using MeetingApp.Models.Param;
using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;

namespace MeetingApp.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;

        private string _loginUserId;
        private string _loginPassword;
        private LoginParam _loginParam;


        public ICommand NavigationSignUpPageCommand { get; }
        public ICommand LoginCommand { get; }

        public string LoginUserId
        {
            get { return _loginUserId; }
            set { SetProperty(ref _loginUserId, value); }
        }

        public string LoginPassword
        {
            get { return _loginPassword; }
            set { SetProperty(ref _loginPassword, value); }
        }

        RestService _restService;

        public LoginPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            _loginParam = new LoginParam();
            _restService = new RestService();

            //新規登録ページへ遷移
            NavigationSignUpPageCommand = new DelegateCommand(() =>
           {
               _navigationService.NavigateAsync("SignUpPage");
           });

            //ログインボタンの動作
            LoginCommand = new DelegateCommand(async () =>
            {

                _loginParam = await _restService.LoginUserDataAsync(UserConstants.OpenUserLoginEndPoint, LoginUserId, LoginPassword);
                //新規登録にエラーが無く追加が実行されていればLoginPageに遷移する
                if (_loginParam.HasError == false)
                {
                    await _navigationService.NavigateAsync("MeetingDataTopPage");
                }
            });
        }


    }
}
