using MeetingApp.Utils;
using Prism.Navigation;

namespace MeetingApp.ViewModels
{
    public class ErrorTemplatePageViewModel : ViewModelBase
    {

        //data
        private string _errorMessage;
        private string _errorMessageDescription;

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

        public ErrorTemplatePageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var errorPageType = parameters.GetValue<ErrorPageType>("ErrorPageType");

            switch (errorPageType)
            {
                case ErrorPageType.FinishedMeeting:
                    //会議が終了済みであるエラー
                    ErrorMessage = "会議終了済みエラー";
                    ErrorMessageDespription = "会議は既に終了しています。Top画面に戻ってください。";

                    break;
            }
        }
    }
}
