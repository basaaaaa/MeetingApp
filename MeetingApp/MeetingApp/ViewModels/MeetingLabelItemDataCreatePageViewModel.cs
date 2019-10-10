using MeetingApp.Models.Data;
using Prism.Navigation;

namespace MeetingApp.ViewModels
{
    public class MeetingLabelItemDataCreatePageViewModel : ViewModelBase
    {

        private MeetingLabelData _targetMeetingLabel;
        public MeetingLabelData TargetMeetingLabel
        {
            get { return _targetMeetingLabel; }
            set { SetProperty(ref _targetMeetingLabel, value); }
        }

        public MeetingLabelItemDataCreatePageViewModel(INavigationService navigationService) : base(navigationService)
        {

            //var p = new NavigationParameters
            //    {
            //        { "mid", getMeetingParam.MeetingData.Id}
            //    };

            ////会議情報トップページに遷移する
            //await _navigationService.NavigateAsync("MeetingAttendPage", p);

        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            //MeetingAttendPageからのラベルのパラメータを取得
            TargetMeetingLabel = (MeetingLabelData)parameters["meetingLabelData"];






        }
    }
}
