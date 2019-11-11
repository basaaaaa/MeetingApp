using MeetingApp.Constants;
using MeetingApp.Data;
using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MeetingApp.ViewModels
{
    public class MeetingFinishTopPageViewModel : ViewModelBase
    {
        //private Data
        private ObservableCollection<ParticipantData> _participants;
        private MeetingData _targetMeetingData;
        private string _outputText;

        //private Param
        private GetParticipantsParam _getParticipantsParam;
        private GetMeetingParam _getMeetingParam;
        private GetMeetingLabelItemsParam _getMeetingLabelItemsParam;
        //public Data
        public ObservableCollection<ParticipantData> Participants
        {
            get { return _participants; }
            set { SetProperty(ref _participants, value); }
        }
        public MeetingData TargetMeetingData
        {
            get { return _targetMeetingData; }
            set { SetProperty(ref _targetMeetingData, value); }
        }

        public string OutputText
        {
            get { return _outputText; }
            set { SetProperty(ref _outputText, value); }
        }
        //public Param
        public GetParticipantsParam GetParticipantsParam
        {
            get { return _getParticipantsParam; }
            set { SetProperty(ref _getParticipantsParam, value); }
        }
        public GetMeetingParam GetMeetingParam
        {
            get { return _getMeetingParam; }
            set { SetProperty(ref _getMeetingParam, value); }
        }
        public GetMeetingLabelItemsParam GetMeetingLabelItemsParam
        {
            get { return _getMeetingLabelItemsParam; }
            set { SetProperty(ref _getMeetingLabelItemsParam, value); }
        }

        //Command
        public ICommand NavigateMeetingFinishUserPage { get; }
        public ICommand OutputParticipantsDataCommand { get; }
        public ICommand NavigateMeetingDataTopPageCommand { get; }


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

            //参加者情報をText形式にOutPutするコマンド
            OutputParticipantsDataCommand = new DelegateCommand(async () =>
            {
                OutputText = "[会議名]:" + TargetMeetingData.Title + Environment.NewLine;
                OutputText += "[日時]:" + TargetMeetingData.Date + TargetMeetingData.StartTime + Environment.NewLine;
                OutputText += "[Location]:" + TargetMeetingData.Location + Environment.NewLine;

                foreach (ParticipantData p in Participants)
                {
                    OutputText += "[ユーザー名]:" + p.UserId + Environment.NewLine;
                    await p.GetMyLabelItems();

                    foreach (MeetingLabelData l in p.LabelItems)
                    {

                        OutputText += "【" + l.LabelName + "】" + Environment.NewLine;
                        foreach (MeetingLabelItemData i in l.MeetingLabelItemDatas)
                        {
                            OutputText += "・" + i.ItemName + Environment.NewLine;
                        }
                    }
                }

                //何かしらのツールでOutput

            });

            NavigateMeetingDataTopPageCommand = new DelegateCommand(async () =>
            {
                await _navigationService.NavigateAsync("/NavigationPage/MeetingDataTopPage");
            });

        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            //会議idの取得
            var mid = (int)parameters["mid"];

            //対象の会議データ取得
            GetMeetingParam = await _restService.GetMeetingDataAsync(MeetingConstants.OpenMeetingEndPoint, mid);
            TargetMeetingData = GetMeetingParam.MeetingData;

            //退室済含むすべての会議参加者データの取得
            GetParticipantsParam = await _restService.GetParticipantsDataAsync(MeetingConstants.OPENMeetingParticipantEndPoint, mid);
            //View用のObservableCollection型リストを取得
            Participants = new ObservableCollection<ParticipantData>(GetParticipantsParam.Participants);


        }

    }
}
