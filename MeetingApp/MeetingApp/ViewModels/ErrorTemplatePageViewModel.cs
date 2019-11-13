using MeetingApp.Utils;
using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;

namespace MeetingApp.ViewModels
{
    public class ErrorTemplatePageViewModel : ViewModelBase
    {

        //data
        private string _errorMessage;
        private string _errorMessageDescription;
        private string _errorMessageTitle;

        //publicData
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }
        public string ErrorMessageDespription
        {
            get { return _errorMessageDescription; }
            set { SetProperty(ref _errorMessageDescription, value); }
        }
        public string ErrorMessageTitle
        {
            get { return _errorMessageTitle; }
            set { SetProperty(ref _errorMessageTitle, value); }
        }

        public ICommand NavigateLoginPageCommand { get; }

        INavigationService _navigationService;
        ApplicationProperties _applicationProperties;


        public ErrorTemplatePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            _applicationProperties = new ApplicationProperties();


            NavigateLoginPageCommand = new DelegateCommand(() =>

            {
                //ログイン済情報の削除
                _applicationProperties.ClearPropertie("userId");
                _applicationProperties.ClearPropertie("token");

                //ログインページに遷移させる
                _navigationService.NavigateAsync("/NavigationPage/LoginPage");

            });


        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var errorPageType = parameters.GetValue<ErrorPageType>("ErrorPageType");

            switch (errorPageType)
            {
                case ErrorPageType.FinishedMeeting:
                    //会議が終了済みであるエラー
                    ErrorMessageTitle = "会議終了済みエラー";
                    ErrorMessage = "この会議は終了しています";
                    ErrorMessageDespription = "会議は既に終了しています。Top画面に戻ってください。";

                    break;
            }
        }
    }
}
