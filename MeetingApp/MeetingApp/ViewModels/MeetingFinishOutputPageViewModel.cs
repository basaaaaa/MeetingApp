using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;

namespace MeetingApp.ViewModels
{
    public class MeetingFinishOutputPageViewModel : ViewModelBase
    {

        //private data
        private string _outputText;

        //public data
        public string OutputText
        {
            get { return _outputText; }
            set { SetProperty(ref _outputText, value); }
        }
        public ICommand CopyToClipboardCommand { get; }
        public ICommand NavigateMeetingDataTopPageCommand { get; }

        INavigationService _navigationService;

        public MeetingFinishOutputPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;


            CopyToClipboardCommand = new DelegateCommand(() => { });

            NavigateMeetingDataTopPageCommand = new DelegateCommand(() =>
            {
                _navigationService.NavigateAsync("MeetingDataTopPage");
            });
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            //出力テキスト情報の取得
            OutputText = (string)parameters["outputText"];


        }
    }
}
