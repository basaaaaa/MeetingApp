using MeetingApp.Models.Data;
using Prism.Navigation;
using System.Collections.ObjectModel;

namespace MeetingApp.ViewModels
{
    public class MeetingExecuteDetailPageViewModel : ViewModelBase
    {

        //privateData
        private string _detailPageTitle;
        private MeetingLabelData _targetMeetingLabelData;
        private ObservableCollection<MeetingLabelItemData> _meetingLabelItemDatas;

        //privateParam


        //publicData
        public string DetailPageTitle
        {
            get { return _detailPageTitle; }
            set { SetProperty(ref _detailPageTitle, value); }
        }
        public MeetingLabelData TargetMeetingLabelData
        {
            get { return _targetMeetingLabelData; }
            set { SetProperty(ref _targetMeetingLabelData, value); }
        }
        public ObservableCollection<MeetingLabelItemData> MeetingLabelItemDatas
        {
            get { return _meetingLabelItemDatas; }
            set { SetProperty(ref _meetingLabelItemDatas, value); }
        }


        //publicParam


        //commands
        //others
        public MeetingExecuteDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            DetailPageTitle = (string)parameters["detailPageTitle"];
            TargetMeetingLabelData = (MeetingLabelData)parameters["meetingLabelData"];

            MeetingLabelItemDatas = new ObservableCollection<MeetingLabelItemData>(TargetMeetingLabelData.MeetingLabelItemDatas);


        }
    }
}
