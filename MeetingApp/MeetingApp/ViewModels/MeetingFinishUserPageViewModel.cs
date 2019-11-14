using MeetingApp.Constants;
using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MeetingApp.ViewModels
{
    public class MeetingFinishUserPageViewModel : ViewModelBase
    {
        //private Data
        private ParticipantData _targetParticipantData;
        private ObservableCollection<MeetingLabelData> _targetMeetingLabels;
        private bool _loadingData;

        //private Param
        private GetMeetingLabelsParam _getMeetingLabelsParam;

        //public Data
        public ParticipantData TargetParticipantData
        {
            get { return _targetParticipantData; }
            set { SetProperty(ref _targetParticipantData, value); }
        }
        public ObservableCollection<MeetingLabelData> TargetMeetingLabels
        {
            get { return _targetMeetingLabels; }
            set { SetProperty(ref _targetMeetingLabels, value); }
        }
        public bool LoadingData
        {
            get { return _loadingData; }
            set { SetProperty(ref _loadingData, value); }
        }

        //public Param
        public GetMeetingLabelsParam GetMeetingLabelsParam
        {
            get { return _getMeetingLabelsParam; }
            set { SetProperty(ref _getMeetingLabelsParam, value); }
        }
        //Command
        public ICommand NavigateMeetingFinishUserPageCommand { get; }
        public ICommand GoBackCommand { get; }


        RestService _restService;
        INavigationService _navigationService;

        public MeetingFinishUserPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _restService = new RestService();
            _navigationService = navigationService;

            NavigateMeetingFinishUserPageCommand = new DelegateCommand<object>((selectedMeetingLabel) =>
            {
                var MeetingLabel = (MeetingLabelData)selectedMeetingLabel;

                var detailPageTitle = TargetParticipantData.UserId + " > " + MeetingLabel.LabelName;

                var p = new NavigationParameters
                            {
                                { "meetingLabelData", MeetingLabel},
                                { "detailPageTitle", detailPageTitle },
                                { "targetParticipantData",TargetParticipantData }
                            };

                _navigationService.NavigateAsync("MeetingFinishDetailPage", p);
            });

            GoBackCommand = new DelegateCommand(async () =>
            {
                var p = new NavigationParameters
                {
                    {"mid", TargetParticipantData.Mid }
                };
                await _navigationService.NavigateAsync("MeetingFinishTopPage", p);
            });
        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            LoadingData = true;

            //�J�ڃp�����[�^�����participantData�̎擾
            TargetParticipantData = (ParticipantData)parameters["participant"];

            //�Ώۉ�c�ɑ΂���Ώۃ��[�U�[�����J�[�h���x���̎擾
            GetMeetingLabelsParam = await _restService.GetMeetingLabelsDataAsync(MeetingConstants.OPENMeetingLabelEndPoint, TargetParticipantData.Mid, TargetParticipantData.Uid);
            //View�p��Labels�f�[�^���擾
            TargetMeetingLabels = new ObservableCollection<MeetingLabelData>(GetMeetingLabelsParam.MeetingLabelDatas);

            LoadingData = false;

        }
    }
}
