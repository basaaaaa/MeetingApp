using MeetingApp.Models.Constants;
using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using MeetingApp.Utils;
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

        public LoginParam LoginParam
        {
            get { return _loginParam; }
            set { SetProperty(ref _loginParam, value); }
        }

        RestService _restService;
        ApplicationProperties _applicationProperties;

        public LoginPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            _restService = new RestService();
            _loginParam = new LoginParam();
            _applicationProperties = new ApplicationProperties();

            //新規登録ページへ遷移
            NavigationSignUpPageCommand = new DelegateCommand(() =>
           {
               _navigationService.NavigateAsync("SignUpPage");
           });

            //ログインボタンの動作
            LoginCommand = new DelegateCommand(async () =>
            {

                LoginParam = await _restService.LoginUserDataAsync(UserConstants.OpenUserLoginEndPoint, LoginUserId, LoginPassword);
                //ログイン処理にエラーが無く追加が実行されていれば
                if (LoginParam.HasError == false)
                {
                    //token情報を保持する
                    var tokenData = LoginParam.TokenData;
                    _applicationProperties.SaveToProperties<TokenData>("token", tokenData);
                    //ローカルにuserId情報を保持する
                    _applicationProperties.SaveToProperties<string>("userId", LoginUserId);

                    //会議情報トップページに遷移する
                    await _navigationService.NavigateAsync("MeetingDataTopPage");
                }
            });
        }


    }
}
