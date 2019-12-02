using MeetingApp.Models.Constants;
using MeetingApp.Models.Param;
using MeetingApp.Models.Validate;
using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;

namespace MeetingApp.ViewModels
{

    public class SignUpPageViewModel : ViewModelBase
    {

        #region private data
        private string _signUpUserId;
        private string _signUpPassword;
        private bool _signUpLoading;
        #endregion

        #region param

        private SignUpParam _signUpParam;
        #endregion

        #region public data
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
        public bool SignUpLoading
        {
            get { return _signUpLoading; }
            set { SetProperty(ref _signUpLoading, value); }
        }
        #endregion

        #region public param
        public SignUpParam SignUpParam
        {
            get { return _signUpParam; }
            set { SetProperty(ref _signUpParam, value); }
        }

        #endregion

        #region command
        public ICommand SignUpCommand { get; }
        public ICommand GoBackCommand { get; }
        #endregion

        #region others

        RestService _restService;
        INavigationService _navigationService;
        SignUpValidation _signUpValidation;

        #endregion

        public SignUpPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            _restService = new RestService();

            _signUpParam = new SignUpParam();

            _signUpValidation = new SignUpValidation();


            SignUpCommand = new DelegateCommand(async () =>
            {
                SignUpLoading = true;

                //V‹K“o˜^‚Ì“ü—ÍŒnƒoƒŠƒf[ƒVƒ‡ƒ“
                SignUpParam = _signUpValidation.InputValidate(SignUpUserId, SignUpPassword);
                if (SignUpParam.HasError == true) { SignUpLoading = false; return; }

                //V‹K“o˜^API‚ÌƒR[ƒ‹
                SignUpParam = await _restService.SignUpUserDataAsync(UserConstants.OpenUserEndPoint, SignUpUserId, SignUpPassword);

                //V‹K“o˜^‚ÉƒGƒ‰[‚ª–³‚­’Ç‰Á‚ªÀs‚³‚ê‚Ä‚¢‚ê‚ÎLoginPage‚É‘JˆÚ‚·‚é
                if (SignUpParam.HasError == false)
                {
                    SignUpLoading = false;
                    await _navigationService.NavigateAsync("/NavigationPage/LoginPage");
                }
                else
                {
                    SignUpLoading = false;
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
