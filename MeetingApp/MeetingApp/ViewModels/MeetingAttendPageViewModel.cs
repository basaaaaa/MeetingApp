using MeetingApp.Constants;
using MeetingApp.Data;
using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using Prism.Navigation;
using System.Collections.ObjectModel;

namespace MeetingApp.ViewModels
{
    public class MeetingAttendPageViewModel : ViewModelBase
    {
        private MeetingData _targetMeetingData;
        private ObservableCollection<MeetingLabelData> _targetMeetingLabels;

        public MeetingData TargetMeetingData
        {
            get { return _targetMeetingData; }
            set { SetProperty(ref _targetMeetingData, value); }
        }
        public ObservableCollection<MeetingLabelData> TargetMeetingLabels
        {
            get { return _targetMeetingLabels; }
            set { SetProperty(ref _targetMeetingLabels, value); }
        }

        RestService _restService;
        public MeetingAttendPageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            _restService = new RestService();
            //対象の会議データ取得
            GetMeetingParam getMeetingParam = new GetMeetingParam();
            getMeetingParam = await _restService.GetMeetingDataAsync(MeetingConstants.OpenMeetingEndPoint, (int)parameters["mid"]);
            TargetMeetingData = getMeetingParam.MeetingData;

            //会議のラベルを取得
            GetMeetingLabelsParam getMeetingLabelsParam = new GetMeetingLabelsParam();
            getMeetingLabelsParam = await _restService.GetMeetingLabelsDataAsync(MeetingConstants.OPENMeetingLabelEndPoint, (int)parameters["mid"]);
            TargetMeetingLabels = new ObservableCollection<MeetingLabelData>(getMeetingLabelsParam.MeetingLabelDatas);

        }
    }
}
