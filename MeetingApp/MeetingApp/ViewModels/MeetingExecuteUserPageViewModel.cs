using MeetingApp.Constants;
using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Windows.Input;

namespace MeetingApp.ViewModels
{
    public class MeetingExecuteUserPageViewModel : ViewModelBase
    {
        //privateData
        private ParticipantData _participantData;
        private List<MeetingLabelData> _targetMeetingLabels;
        private bool _loadingData;

        //privateParam
        private GetMeetingLabelsParam _getMeetingLabelsParam;

        //publicData
        public ParticipantData ParticipantData
        {
            get { return _participantData; }
            set { SetProperty(ref _participantData, value); }
        }
        public List<MeetingLabelData> TargetMeetingLabels
        {
            get { return _targetMeetingLabels; }
            set { SetProperty(ref _targetMeetingLabels, value); }
        }

        public bool LoadingData
        {
            get { return _loadingData; }
            set { SetProperty(ref _loadingData, value); }
        }


        //publicParam
        public GetMeetingLabelsParam GetMeetingLabelsParam
        {
            get { return _getMeetingLabelsParam; }
            set { SetProperty(ref _getMeetingLabelsParam, value); }
        }


        //commands
        public ICommand NavigateMeetingExecuteUserPageCommand { get; }
        //others
        RestService _restService;
        INavigationService _navigationService;


        public MeetingExecuteUserPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;

            //ユーザーの指定ラベルの項目一覧ページに遷移するコマンド
            NavigateMeetingExecuteUserPageCommand = new DelegateCommand<object>((meetingLabelData) =>
            {
                var targetMeetingLabelData = (MeetingLabelData)meetingLabelData;

                var detailPageTitle = ParticipantData.UserId + " > " + targetMeetingLabelData.LabelName;

                var p = new NavigationParameters
                            {
                                { "meetingLabelData", targetMeetingLabelData},
                                { "detailPageTitle", detailPageTitle }
                            };

                _navigationService.NavigateAsync("MeetingExecuteDetailPage", p);
            });

        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            LoadingData = true;

            _restService = new RestService();

            //遷移パラメータからのparticipantDataの取得
            ParticipantData = (ParticipantData)parameters["participantData"];

            //会議midに基づくカードラベルの取得
            GetMeetingLabelsParam = await _restService.GetMeetingLabelsDataAsync(MeetingConstants.OPENMeetingLabelEndPoint, ParticipantData.Mid, ParticipantData.Uid);
            TargetMeetingLabels = GetMeetingLabelsParam.MeetingLabelDatas;

            LoadingData = false;

        }
    }
}
