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
        private bool _loadingData;

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
        public bool LoadingData
        {
            get { return _loadingData; }
            set { SetProperty(ref _loadingData, value); }
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

            LoadingData = true;

            DetailPageTitle = (string)parameters["detailPageTitle"];
            TargetMeetingLabelData = (MeetingLabelData)parameters["meetingLabelData"];

            MeetingLabelItemDatas = new ObservableCollection<MeetingLabelItemData>(TargetMeetingLabelData.MeetingLabelItemDatas);

            LoadingData = false;

        }
    }
}
