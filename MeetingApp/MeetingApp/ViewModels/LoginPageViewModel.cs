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


            //�V�K�o�^�y�[�W�֑J��
            NavigationSignUpPageCommand = new DelegateCommand(() =>
           {
               _navigationService.NavigateAsync("SignUpPage");
           });

            //���O�C���{�^���̓���
            LoginCommand = new DelegateCommand(async () =>
            {
                //���[�f�B���O���̕\��
                LoadingLogin = true;

                //���O�C��API�̃R�[��
                LoginParam = await _restService.LoginUserDataAsync(UserConstants.OpenUserLoginEndPoint, LoginUserId, LoginPassword);

                //���O�C�������ɃG���[�������ǉ������s����Ă����
                if (LoginParam.HasError == false)
                {
                    //token����ێ�����
                    var tokenData = LoginParam.TokenData;
                    _applicationProperties.SaveToProperties<TokenData>("token", tokenData);
                    //���[�J����userId����ێ�����
                    _applicationProperties.SaveToProperties<string>("userId", LoginUserId);

                    //���[�f�B���O�����\���ɂ���
                    LoadingLogin = false;

                    //��c���g�b�v�y�[�W�ɑJ�ڂ���
                    await _navigationService.NavigateAsync("/NavigationPage/MeetingDataTopPage");
                }
                else
                {
                    //���[�f�B���O�����\���ɂ���
                    LoadingLogin = false;
                }
            });
        }


    }
}
