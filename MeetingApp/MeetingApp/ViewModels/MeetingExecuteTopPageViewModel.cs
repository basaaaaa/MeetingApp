using MeetingApp.Constants;
using MeetingApp.Data;
using MeetingApp.Models.Constants;
using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using MeetingApp.Utils;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MeetingApp.ViewModels
{
    public class MeetingExecuteTopPageViewModel : ViewModelBase
    {
        //private
        //data
        private MeetingData _targetMeetingData;
        private ObservableCollection<ParticipantData> _participants;

        //param
        private GetMeetingParam _getMeetingParam;
        private CheckParticipantParam _checkParticipantParam;
        private GetUserParam _getUserParam;
        private GetParticipantsParam _getParticipantsParam;
        private DeleteParticipantParam _deleteParticipantParam;

        //public
        //data
        public MeetingData TargetMeetingData
        {
            get { return _targetMeetingData; }
            set { SetProperty(ref _targetMeetingData, value); }
        }
        public ObservableCollection<ParticipantData> Participants
        {
            get { return _participants; }
            set { SetProperty(ref _participants, value); }
        }

        //param
        public GetMeetingParam GetMeetingParam
        {
            get { return _getMeetingParam; }
            set { SetProperty(ref _getMeetingParam, value); }
        }
        public CheckParticipantParam CheckParticipantParam
        {
            get { return _checkParticipantParam; }
            set { SetProperty(ref _checkParticipantParam, value); }
        }
        public GetUserParam GetUserParam
        {
            get { return _getUserParam; }
            set { SetProperty(ref _getUserParam, value); }
        }
        public GetParticipantsParam GetParticipantsParam
        {
            get { return _getParticipantsParam; }
            set { SetProperty(ref _getParticipantsParam, value); }
        }
        public DeleteParticipantParam DeleteParticipantParam
        {
            get { return _deleteParticipantParam; }
            set { SetProperty(ref _deleteParticipantParam, value); }
        }


        //Command
        public ICommand MeetingExitCommand { get; }


        RestService _restService;
        ApplicationProperties _applicationProperties;
        INavigationService _navigationService;


        public MeetingExecuteTopPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            _restService = new RestService();


            //会議から退出するコマンド
            MeetingExitCommand = new DelegateCommand(async () =>
            {

                //参加情報をparticipantDBから削除するAPIのコール
                DeleteParticipantParam = await _restService.DeleteParticipantDataAsync(MeetingConstants.OPENMeetingParticipantEndPoint, GetUserParam.User.Id, TargetMeetingData.Id);

                if (DeleteParticipantParam.IsSuccessed == true)
                {
                    await _navigationService.NavigateAsync("MeetingDataTopPage");
                }
            });

        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            _restService = new RestService();
            _applicationProperties = new ApplicationProperties();

            //participantsDBにuidが存在するかどうか読み込み
            GetUserParam = await _restService.GetUserDataAsync(UserConstants.OpenUserEndPoint, _applicationProperties.GetFromProperties<string>("userId"));
            var uid = GetUserParam.User.Id;
            CheckParticipantParam = await _restService.CheckParticipantDataAsync(MeetingConstants.OPENMeetingParticipantEndPoint, uid);
            if (CheckParticipantParam.HasError == true) { return; }

            //対象の会議データ取得
            GetMeetingParam = await _restService.GetMeetingDataAsync(MeetingConstants.OpenMeetingEndPoint, (int)parameters["mid"]);
            TargetMeetingData = GetMeetingParam.MeetingData;

            var mid = TargetMeetingData.Id;
            //participantsDBの全データ読み込み (midで指定して全件取得）
            GetParticipantsParam = await _restService.GetParticipantsDataAsync(MeetingConstants.OPENMeetingParticipantEndPoint, mid);
            Participants = new ObservableCollection<ParticipantData>(GetParticipantsParam.Participants);





        }
    }
}
