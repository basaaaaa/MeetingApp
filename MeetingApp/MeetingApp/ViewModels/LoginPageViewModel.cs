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

        #region private data
        private string _loginUserId;
        private string _loginPassword;
        private bool _loadingLogin;
        #endregion

        #region private param
        private LoginParam _loginParam;
        #endregion

        #region public data
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
        public bool LoadingLogin
        {
            get { return _loadingLogin; }
            set { SetProperty(ref _loadingLogin, value); }
        }
        #endregion

        #region public param

        public LoginParam LoginParam
        {
            get { return _loginParam; }
            set { SetProperty(ref _loginParam, value); }
        }

        #endregion

        #region command

        public ICommand NavigationSignUpPageCommand { get; }
        public ICommand LoginCommand { get; }

        #endregion

        #region others
        RestService _restService;
        ApplicationProperties _applicationProperties;
        INavigationService _navigationService;
        #endregion

        public LoginPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;

            _restService = new RestService();
            _applicationProperties = new ApplicationProperties();

            LoginParam = new LoginParam();


            //新規登録ページへ遷移
            NavigationSignUpPageCommand = new DelegateCommand(() =>
           {
               _navigationService.NavigateAsync("SignUpPage");
           });

            //ログインボタンの動作
            LoginCommand = new DelegateCommand(async () =>
            {
                //ローディング情報の表示
                LoadingLogin = true;

                //ログインAPIのコール
                LoginParam = await _restService.LoginUserDataAsync(UserConstants.OpenUserLoginEndPoint, LoginUserId, LoginPassword);

                //ログイン処理にエラーが無く追加が実行されていれば
                if (LoginParam.HasError == false)
                {
                    //token情報を保持する
                    var tokenData = LoginParam.TokenData;
                    _applicationProperties.SaveToProperties<TokenData>("token", tokenData);
                    //ローカルにuserId情報を保持する
                    _applicationProperties.SaveToProperties<string>("userId", LoginUserId);

                    //ローディング情報を非表示にする
                    LoadingLogin = false;

                    //会議情報トップページに遷移する
                    await _navigationService.NavigateAsync("/NavigationPage/MeetingDataTopPage");
                }
                else
                {
                    //ローディング情報を非表示にする
                    LoadingLogin = false;
                }
            });
        }


    }
}
