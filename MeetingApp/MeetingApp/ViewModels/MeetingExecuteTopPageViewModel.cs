using MeetingApp.Constants;
using MeetingApp.Data;
using MeetingApp.Models.Constants;
using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using MeetingApp.Utils;
using Prism.AppModel;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MeetingApp.ViewModels
{
    public class MeetingExecuteTopPageViewModel : ViewModelBase, IApplicationLifecycleAware
    {
        //private
        //data
        private MeetingData _targetMeetingData;
        private ObservableCollection<ParticipantData> _participants;
        private int _targetMeetingId;

        //param
        private GetMeetingParam _getMeetingParam;
        private CheckParticipantParam _checkParticipantParam;
        private GetUserParam _getUserParam;
        private GetParticipantsParam _getParticipantsParam;
        private DeleteParticipantParam _deleteParticipantParam;
        private CreateParticipateParam _createParticipateParam;

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
        public int TargetMeetingId
        {
            get { return _targetMeetingId; }
            set { SetProperty(ref _targetMeetingId, value); }
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
        public CreateParticipateParam CreateParticipateParam
        {
            get { return _createParticipateParam; }
            set { SetProperty(ref _createParticipateParam, value); }
        }


        //Command
        public ICommand MeetingExitCommand { get; }
        public ICommand MeetingEndCommand { get; }
        public ICommand NavigateMeetingExecuteUserPage { get; }
        public ICommand UpdateParticipantsCommand { get; }


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

            MeetingEndCommand = new DelegateCommand(async () =>
            {
                var deleteParticipantParam = new DeleteParticipantParam();

                while (deleteParticipantParam.HasError != true)
                {
                    deleteParticipantParam = await _restService.DeleteParticipantDataAsync(MeetingConstants.OPENMeetingParticipantEndPoint, GetUserParam.User.Id, TargetMeetingData.Id);
                }
                await _navigationService.NavigateAsync("MeetingDataTopPage");

            });

            NavigateMeetingExecuteUserPage = new DelegateCommand<object>((participant) =>
            {
                var targetParticipantData = (ParticipantData)participant;

                var p = new NavigationParameters
                {
                    { "participantData", targetParticipantData},
                };

                _navigationService.NavigateAsync("MeetingExecuteUserPage", p);

            });

            UpdateParticipantsCommand = new DelegateCommand(() =>
            {
                Reload();
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
            var mid = (int)parameters["mid"];
            CheckParticipantParam = await _restService.CheckParticipantDataAsync(MeetingConstants.OPENMeetingParticipantEndPoint, uid, mid);
            if (CheckParticipantParam.HasError == true) { return; }

            //対象の会議データ取得

            TargetMeetingId = (int)parameters["mid"];
            GetMeetingParam = await _restService.GetMeetingDataAsync(MeetingConstants.OpenMeetingEndPoint, TargetMeetingId);
            TargetMeetingData = GetMeetingParam.MeetingData;

            //participantsDBの全データ読み込み (midで指定して全件取得）
            GetParticipantsParam = await _restService.GetParticipantsDataAsync(MeetingConstants.OPENMeetingParticipantEndPoint, TargetMeetingId);
            Participants = new ObservableCollection<ParticipantData>(GetParticipantsParam.Participants);

        }

        //戻るボタンを押してMeetingAttendPageに遷移するときの処理
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {

            base.OnNavigatingTo(parameters);

            parameters.Add("mid", GetMeetingParam.MeetingData.Id);

        }

        public async void OnSleep()
        {
            _restService = new RestService();

            Console.WriteLine("test");
            //参加情報をparticipantDBから削除するAPIのコール
            DeleteParticipantParam = await _restService.DeleteParticipantDataAsync(MeetingConstants.OPENMeetingParticipantEndPoint, GetUserParam.User.Id, TargetMeetingData.Id);

            if (DeleteParticipantParam.IsSuccessed == true)
            {
                Console.WriteLine("Delete Successed");
            }
        }

        public void OnResume()
        {
            Reload();
        }

        //更新処理
        public async void Reload()
        {
            _restService = new RestService();

            //対象の会議データ取得
            GetMeetingParam = await _restService.GetMeetingDataAsync(MeetingConstants.OpenMeetingEndPoint, TargetMeetingId);
            TargetMeetingData = GetMeetingParam.MeetingData;

            //ParticipantDBに対する最終更新時刻とActive状態の更新
            //ParticipantDBに既にユーザーが居ないかチェック
            CheckParticipantParam = await _restService.CheckParticipantDataAsync(MeetingConstants.OPENMeetingParticipantEndPoint, GetUserParam.User.Id, TargetMeetingData.Id);

            //ユーザーが既にParticipantDBに存在していた場合
            if (CheckParticipantParam.IsSuccessed == true)
            {
                //会議参加済みかつAtciveの場合は最終更新時刻のみ更新する
                if (CheckParticipantParam.Participant.Active == true)
                {
                    CheckParticipantParam.Participant.LastUpdateTime = DateTime.Now;
                    var updateParticipant = CheckParticipantParam.Participant;
                    await _restService.UpdateParticipantDataAsync(MeetingConstants.OPENMeetingParticipantEndPoint, updateParticipant);

                }
                else
                //参加済みかつ非Activeの場合はActive/最終更新時刻を両方更新する
                {
                    CheckParticipantParam.Participant.Active = true;
                    CheckParticipantParam.Participant.LastUpdateTime = DateTime.Now;
                    var updateParticipant = CheckParticipantParam.Participant;

                    await _restService.UpdateParticipantDataAsync(MeetingConstants.OPENMeetingParticipantEndPoint, updateParticipant);
                }

            }
            //既にユーザーが退室済の場合は更新エラーを出す
            else
            {

            }


            var mid = TargetMeetingData.Id;
            //participantsDBの全データ読み込み (midで指定して全件取得）
            GetParticipantsParam = await _restService.GetParticipantsDataAsync(MeetingConstants.OPENMeetingParticipantEndPoint, mid);
            Participants = new ObservableCollection<ParticipantData>(GetParticipantsParam.Participants);
        }

    }
}
