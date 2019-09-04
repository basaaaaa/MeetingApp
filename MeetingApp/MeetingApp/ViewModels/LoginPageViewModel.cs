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

            //�V�K�o�^�y�[�W�֑J��
            NavigationSignUpPageCommand = new DelegateCommand(() =>
           {
               _navigationService.NavigateAsync("SignUpPage");
           });

            //���O�C���{�^���̓���
            LoginCommand = new DelegateCommand(async () =>
            {

                LoginParam = await _restService.LoginUserDataAsync(UserConstants.OpenUserLoginEndPoint, LoginUserId, LoginPassword);
                //���O�C�������ɃG���[�������ǉ������s����Ă����
                if (LoginParam.HasError == false)
                {
                    //token����ێ�����
                    var tokenData = LoginParam.TokenData;
                    _applicationProperties.SaveToProperties<TokenData>("token", tokenData);
                    //���[�J����userId����ێ�����
                    _applicationProperties.SaveToProperties<string>("userId", LoginUserId);

                    //��c���g�b�v�y�[�W�ɑJ�ڂ���
                    await _navigationService.NavigateAsync("MeetingDataTopPage");
                }
            });
        }


    }
}
