using MeetingApp.Models.Data;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MeetingApp.ViewModels
{
    public class MeetingFinishDetailPageViewModel : ViewModelBase
    {

        //private data
        private string _finishdetailPageTitle;
        private MeetingLabelData _targetMeetingLabelData;
        private ObservableCollection<MeetingLabelItemData> _meetingLabelItemDatas;
        private ParticipantData _targetParticipantData;

        //public data
        public string FinishDetailPageTitle
        {
            get { return _finishdetailPageTitle; }
            set { SetProperty(ref _finishdetailPageTitle, value); }
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
        public ParticipantData TargetParticipantData
        {
            get { return _targetParticipantData; }
            set { SetProperty(ref _targetParticipantData, value); }
        }

        INavigationService _navigationService;

        public ICommand GoBackCommand { get; }

        public MeetingFinishDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;

            GoBackCommand = new DelegateCommand(() =>
            {
                var p = new NavigationParameters
                {
                    { "participant", TargetParticipantData},
                };
                _navigationService.NavigateAsync("MeetingFinishUserPage", p);
            });

        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            FinishDetailPageTitle = (string)parameters["detailPageTitle"];
            TargetMeetingLabelData = (MeetingLabelData)parameters["meetingLabelData"];
            TargetParticipantData = (ParticipantData)parameters["targetParticipantData"];

            MeetingLabelItemDatas = new ObservableCollection<MeetingLabelItemData>(TargetMeetingLabelData.MeetingLabelItemDatas);


        }
    }
}
