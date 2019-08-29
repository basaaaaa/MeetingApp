using MeetingApp.Models.Constants;
using MeetingApp.Models.Param;
using MeetingApp.Utils;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MeetingApp.ViewModels
{

    public class SignUpPageViewModel : ViewModelBase
    {

        private string _signUpUserId;
        private string _signUpPassword;

        private SignUpParam _signUpParam;

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

        public SignUpParam SignUpParam
        {
            get { return _signUpParam; }
            set { SetProperty(ref _signUpParam, value); }
        }





        public ICommand SignUpCommand { get; }
        public ICommand GoBackCommand { get; }

        RestService _restService;
        INavigationService _navigationService;

        public SignUpPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _restService = new RestService();
            _navigationService = navigationService;
            _signUpParam = new SignUpParam();


            SignUpCommand = new DelegateCommand(async () =>
            {
                //SignUpParam�ɐV�K�o�^�̐������s��Ԃ�
                SignUpParam = await _restService.SignUpUserDataAsync(UserConstants.OpenUserEndPoint, SignUpUserId, SignUpPassword);

                //�V�K�o�^�ɃG���[�������ǉ������s����Ă����LoginPage�ɑJ�ڂ���
                if (SignUpParam.HasError == false)
                {
                    await Task.Delay(1000);
                    await _navigationService.NavigateAsync("LoginPage");
                }

            });

            GoBackCommand = new DelegateCommand(async () =>
            {
                _ = await _navigationService.GoBackAsync();
            });
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {

            base.OnNavigatingTo(parameters);

            //��c���S���擾API�̃R�[��
            SignUpParam.HasError = false;


        }


    }
}
