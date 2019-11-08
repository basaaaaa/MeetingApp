using MeetingApp.Constants;
using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MeetingApp.ViewModels
{
    public class MeetingFinishTopPageViewModel : ViewModelBase
    {
        //private Data
        private ObservableCollection<ParticipantData> _participants;
        //private Param
        private GetParticipantsParam _getParticipantsParam;
        //public Data
        public ObservableCollection<ParticipantData> Participants
        {
            get { return _participants; }
            set { SetProperty(ref _participants, value); }
        }
        //public Param
        public GetParticipantsParam GetParticipantsParam
        {
            get { return _getParticipantsParam; }
            set { SetProperty(ref _getParticipantsParam, value); }
        }
        //Command
        public ICommand NavigateMeetingFinishUserPage { get; }


        RestService _restService;
        INavigationService _navigationService;

        public MeetingFinishTopPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _restService = new RestService();
            _navigationService = navigationService;

            NavigateMeetingFinishUserPage = new DelegateCommand<object>((selectedParticipant) =>
            {
                //Viewで選択されたparticipantの情報を取得
                var participant = (ParticipantData)selectedParticipant;
                //遷移用のパラメータに変換
                var p = new NavigationParameters
                {
                    { "participant", participant},
                };
                //UserPageに遷移する
                _navigationService.NavigateAsync("MeetingFinishUserPage", p);

            });

        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            //会議idの取得
            var mid = (int)parameters["mid"];

            //退室済含むすべての会議参加者データの取得
            GetParticipantsParam = await _restService.GetParticipantsDataAsync(MeetingConstants.OPENMeetingParticipantEndPoint, mid);
            //View用のObservableCollection型リストを取得
            Participants = new ObservableCollection<ParticipantData>(GetParticipantsParam.Participants);


        }

    }
}
