using MeetingApp.Constants;
using MeetingApp.Data;
using MeetingApp.Models.Constants;
using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using MeetingApp.Utils;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MeetingApp.ViewModels
{
    public class MeetingExecuteTopPageViewModel : ViewModelBase
    {
        //private
        //data
        private MeetingData _targetMeetingData;
        private ObservableCollection<ParticipantData> _participants;
        private int _targetMeetingId;
        private bool _isOwner;
        private bool _isGeneral;

        //param
        private GetMeetingParam _getMeetingParam;
        private CheckParticipantParam _checkParticipantParam;
        private GetUserParam _getUserParam;
        private GetParticipantsParam _getParticipantsParam;
        private DeleteParticipantParam _deleteParticipantParam;
        private CreateParticipateParam _createParticipateParam;
        private UpdateParticipantParam _updateParticipantParam;
        private UpdateMeetingParam _updateMeetingParam;

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
        public bool IsOwner
        {
            get { return _isOwner; }
            set { SetProperty(ref _isOwner, value); }
        }
        public bool IsGeneral
        {
            get { return _isGeneral; }
            set { SetProperty(ref _isGeneral, value); }
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
        public UpdateParticipantParam UpdateParticipantParam
        {
            get { return _updateParticipantParam; }
            set { SetProperty(ref _updateParticipantParam, value); }
        }
        public UpdateMeetingParam UpdateMeetingParam
        {
            get { return _updateMeetingParam; }
            set { SetProperty(ref _updateMeetingParam, value); }
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

            IsOwner = false;
            IsGeneral = false;


            //会議から退出するコマンド
            MeetingExitCommand = new DelegateCommand(async () =>
            {

                //参加情報をparticipantDBから削除するAPIのコール
                CheckParticipantParam.Participant.isDeleted = true;
                var updateParticipant = CheckParticipantParam.Participant;

                UpdateParticipantParam = await _restService.UpdateParticipantDataAsync(MeetingConstants.OPENMeetingParticipantEndPoint, updateParticipant);

                if (UpdateParticipantParam.IsSuccessed == true)
                {
                    await _navigationService.NavigateAsync("MeetingDataTopPage");
                }


            });

            MeetingEndCommand = new DelegateCommand(async () =>
            {
                //管理者が操作する会議終了処理
                var select = await Application.Current.MainPage.DisplayAlert("警告", "本当に会議を終了してもよろしいでしょうか？", "OK", "キャンセル");

                if (select)
                {
                    //会議情報を取得
                    GetMeetingParam getMeetingParam = new GetMeetingParam();
                    getMeetingParam = await _restService.GetMeetingDataAsync(MeetingConstants.OpenMeetingEndPoint, TargetMeetingId);
                    var updateMeetingData = getMeetingParam.MeetingData;
                    //会議の状態を終了状態に変更
                    updateMeetingData.IsVisible = false;
                    UpdateMeetingParam = await _restService.UpdateMeetingDataAsync(MeetingConstants.OpenMeetingEndPoint, updateMeetingData);

                    //退室済も含めすべてのParticipants情報を取得
                    var p = new NavigationParameters
                {
                    { "mid", TargetMeetingId}
                };
                    await _navigationService.NavigateAsync("/NavigationPage/MeetingFinishTopPage", p);
                }
                else
                {
                    return;
                }


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

            UpdateParticipantsCommand = new DelegateCommand(async () =>
            {
                //更新処理
                Reload();
                Participants = await GetParticipants();
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

            //会議が終了状態でないかどうか判定
            if (GetMeetingParam.MeetingData.IsVisible == false)
            {
                var p = new NavigationParameters
                {
                    {"ErrorPageType",ErrorPageType.FinishedMeeting }
                };
                //終了している会議なのでエラー画面に飛ばす
                await _navigationService.NavigateAsync("/ErrorTemplatePage", p);
            }

            //会議管理者かどうか取得
            if (TargetMeetingData.Owner == GetUserParam.User.Id)
            {
                IsOwner = true;
            }
            else
            {
                IsGeneral = true;
            }

            Reload();

            //Participants全件取得
            Participants = await GetParticipants();

        }

        //戻るボタンを押してMeetingAttendPageに遷移するときの処理
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {

            base.OnNavigatingTo(parameters);

            parameters.Add("mid", GetMeetingParam.MeetingData.Id);

        }


        //更新処理
        public async void Reload()
        {
            _restService = new RestService();

            //対象の会議データ取得
            GetMeetingParam = await _restService.GetMeetingDataAsync(MeetingConstants.OpenMeetingEndPoint, TargetMeetingId);
            TargetMeetingData = GetMeetingParam.MeetingData;

            //会議が終了状態でないかどうか判定
            if (GetMeetingParam.MeetingData.IsVisible == false)
            {
                var p = new NavigationParameters
                {
                    {"ErrorPageType",ErrorPageType.FinishedMeeting }
                };
                //終了している会議なのでエラー画面に飛ばす
                await _navigationService.NavigateAsync("/ErrorTemplatePage", p);
            }

            //ParticipantDBに対する最終更新時刻とIsDeleted状態の更新
            //ParticipantDBに既にユーザーが居ないかチェック
            CheckParticipantParam = await _restService.CheckParticipantDataAsync(MeetingConstants.OPENMeetingParticipantEndPoint, GetUserParam.User.Id, TargetMeetingData.Id);

            //ユーザーが既にParticipantDBに存在していた場合
            if (CheckParticipantParam.IsSuccessed == true)
            {
                //ParticipantsDBに存在するが、論理削除済の場合
                if (CheckParticipantParam.Participant.isDeleted == true)
                {
                    CheckParticipantParam.NoExistUser = true;

                    await _navigationService.NavigateAsync("MeetingDataTopPage");

                }
                else
                //ParticipantsDBに存在するが、論理削除がされていない場合
                {
                    var operateDateTime = new OperateDateTime();
                    CheckParticipantParam.Participant.LastUpdateTime = operateDateTime.CurrentDateTime;
                    var updateParticipant = CheckParticipantParam.Participant;

                    UpdateParticipantParam = await _restService.UpdateParticipantDataAsync(MeetingConstants.OPENMeetingParticipantEndPoint, updateParticipant);
                }

            }
            //既にユーザーが退室済の場合は更新エラーを出す
            else
            {
                CheckParticipantParam.NoExistUser = true;
                await _navigationService.NavigateAsync("MeetingDataTopPage");
            }


        }

        public async Task<ObservableCollection<ParticipantData>> GetParticipants()
        {
            var mid = TargetMeetingData.Id;
            //participantsDBの全データ読み込み (midで指定して全件取得）
            GetParticipantsParam = await _restService.GetParticipantsDataAsync(MeetingConstants.OPENMeetingParticipantEndPoint, mid);

            //退室済みのユーザーを表示させない
            GetParticipantsParam.Participants.RemoveAll(p => p.isDeleted == true);

            var getParticipants = new ObservableCollection<ParticipantData>();
            var operateDateTime = new OperateDateTime();

            foreach (ParticipantData p in GetParticipantsParam.Participants)
            {
                var diffTime = operateDateTime.CurrentDateTime - p.LastUpdateTime;
                if (p.isDeleted == false && diffTime.TotalMinutes < 3)
                {
                    getParticipants.Add(p);
                }
            }

            return getParticipants;

        }

    }
}