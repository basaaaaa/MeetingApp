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
        public MeetingFinishOutputPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            CopyToClipboardCommand = new DelegateCommand(() => { });
        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            //会議idの取得
            OutputText = (string)parameters["outputText"];


        }
    }
}
