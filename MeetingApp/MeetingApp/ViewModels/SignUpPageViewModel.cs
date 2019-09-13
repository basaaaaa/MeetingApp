using MeetingApp.Models.Constants;
using MeetingApp.Models.Param;
using MeetingApp.Models.Validate;
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
        SignUpValidation _signUpValidation;

        public SignUpPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _restService = new RestService();
            _navigationService = navigationService;
            _signUpParam = new SignUpParam();
            _signUpValidation = new SignUpValidation();


            SignUpCommand = new DelegateCommand(async () =>
            {

                //V‹K“o˜^‚Ì“ü—ÍŒnƒoƒŠƒf[ƒVƒ‡ƒ“
                SignUpParam = _signUpValidation.InputValidate(SignUpUserId, SignUpPassword);
                if (SignUpParam.HasError == true) { return; }

                //SignUpParam‚ÉV‹K“o˜^‚Ì¬Œ÷¸”s‚ğ•Ô‚·
                SignUpParam = await _restService.SignUpUserDataAsync(UserConstants.OpenUserEndPoint, SignUpUserId, SignUpPassword);

                //V‹K“o˜^‚ÉƒGƒ‰[‚ª–³‚­’Ç‰Á‚ªÀs‚³‚ê‚Ä‚¢‚ê‚ÎLoginPage‚É‘JˆÚ‚·‚é
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

            //‰ï‹cî•ñ‘SŒæ“¾API‚ÌƒR[ƒ‹
            SignUpParam.HasError = false;


        }


    }
}
