using MeetingApp.Constants;
using MeetingApp.Data;
using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using MeetingApp.Models.Validate;
using MeetingApp.Utils;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace MeetingApp.ViewModels
{
    public class MeetingDataTopPageViewModel : ViewModelBase
    {

        #region private data

        private string _meetingTitle;
        private string _scheduledDate;
        private string _scheduledTime;
        private string _location;
        private string _myUserId;
        private List<MeetingData> _meetings;
        private Boolean _isOwner;
        private Boolean _loadingMeetingData;

        #endregion

        #region private param

        private GetMeetingsParam _getMeetingsParam;
        private TokenCheckParam _tokenCheckParam;

        #endregion

        #region public data

        public List<MeetingData> Meetings
        {
            get { return _meetings; }
            set { SetProperty(ref _meetings, value); }
        }
        public string MeetingTitle
        {
            get { return _meetingTitle; }
            set { SetProperty(ref _meetingTitle, value); }
        }
        public string ScheduledDate
        {
            get { return _scheduledDate; }
            set { SetProperty(ref _scheduledDate, value); }
        }
        public string ScheduledTime
        {
            get { return _scheduledTime; }
            set { SetProperty(ref _scheduledTime, value); }
        }
        public string Location
        {
            get { return _location; }
            set { SetProperty(ref _location, value); }
        }
        public string MyUserId
        {
            get { return _myUserId; }
            set { SetProperty(ref _myUserId, value); }
        }
        public Boolean IsOwner
        {
            get { return _isOwner; }
            set { SetProperty(ref _isOwner, value); }
        }
        public bool LoadingMeetingData
        {
            get { return _loadingMeetingData; }
            set { SetProperty(ref _loadingMeetingData, value); }
        }
        #endregion

        #region public param

        public TokenCheckParam TokenCheckParam
        {
            get { return _tokenCheckParam; }
            set { SetProperty(ref _tokenCheckParam, value); }
        }

        public GetMeetingsParam GetMeetingsParam
        {
            get { return _getMeetingsParam; }
            set { SetProperty(ref _getMeetingsParam, value); }
        }

        #endregion

        #region command

        public ICommand NavigateMeetingCreatePage { get; }
        public ICommand NavigateMeetingAttendPage { get; }
        public ICommand DeleteMeetingCommand { get; }

        #endregion

        #region others

        RestService _restService;
        ApplicationProperties _applicationProperties;
        INavigationService _navigationService;
        OperateDateTime _operateDateTime;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="navigationService"></param>
        public MeetingDataTopPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _restService = new RestService();
            _applicationProperties = new ApplicationProperties();
            _tokenCheckParam = new TokenCheckParam();
            _navigationService = navigationService;
            _operateDateTime = new OperateDateTime();


            //myUserIdの取得
            MyUserId = _applicationProperties.GetFromProperties<string>("userId");


            //会議の削除ボタンが押されたときのコマンド
            DeleteMeetingCommand = new DelegateCommand<object>(async id =>
            {

                //管理者が操作する会議終了処理
                var select = await Application.Current.MainPage.DisplayAlert("警告", "会議を削除してもよろしいでしょうか？", "OK", "キャンセル");

                if (select)
                {
                    var mid = Convert.ToInt32(id);

                    //論理削除

                    //対象となる会議情報を1件取得
                    GetMeetingParam getMeetingParam = new GetMeetingParam();
                    getMeetingParam = await _restService.GetMeetingDataAsync(MeetingConstants.OpenMeetingEndPoint, mid);
                    var updateMeetingData = getMeetingParam.MeetingData;
                    //フラグをfalseに変更
                    updateMeetingData.IsVisible = false;
                    await _restService.UpdateMeetingDataAsync(MeetingConstants.OpenMeetingEndPoint, updateMeetingData);

                    //会議情報再取得
                    //会議情報全件取得APIのコール
                    GetMeetingsParam = await _restService.GetMeetingsDataAsync(MeetingConstants.OpenMeetingEndPoint, MyUserId);
                    Meetings = GetMeetingsParam.Meetings;
                }
                else
                {
                    return;
                }

            });

            //会議新規作成ページに遷移するコマンド
            NavigateMeetingCreatePage = new DelegateCommand(async () =>
            {
                //会議情報トップページに遷移する
                await _navigationService.NavigateAsync("/NavigationPage/MeetingDataCreatePage");

            });

            //会議出席ページに遷移するコマンド
            NavigateMeetingAttendPage = new DelegateCommand<object>(async id =>
            {

                var mid = Convert.ToInt32(id);

                //指定の会議の情報をCommandParameterで受け取り、会議id(mid)を取得する
                GetMeetingParam getMeetingParam = new GetMeetingParam();
                getMeetingParam = await _restService.GetMeetingDataAsync(MeetingConstants.OpenMeetingEndPoint, mid);

                var p = new NavigationParameters
                {
                    { "mid", getMeetingParam.MeetingData.Id}
                };

                //会議情報トップページに遷移する
                await _navigationService.NavigateAsync("MeetingAttendPage", p);

            });

        }

        /// <summary>
        /// 画面遷移時の処理
        /// </summary>
        /// <param name="parameters">遷移中のパラメータ</param>
        public override async void OnNavigatingTo(INavigationParameters parameters)
        {

            base.OnNavigatingTo(parameters);

            LoadingMeetingData = true;

            TokenCheckValidation tokenCheckValidation = new TokenCheckValidation(_restService);
            TokenCheckParam = await tokenCheckValidation.Validate(_applicationProperties.GetFromProperties<TokenData>("token"));

            if (_tokenCheckParam.HasError == true)
            {
                //token照合の際にエラーが発生した際の処理
                Console.WriteLine("ログインに失敗しました");
                await _navigationService.NavigateAsync("LoginPage");
            }

            //myUserIdの取得
            MyUserId = _applicationProperties.GetFromProperties<string>("userId");

            //会議情報全件取得APIのコール
            GetMeetingsParam = await _restService.GetMeetingsDataAsync(MeetingConstants.OpenMeetingEndPoint, MyUserId);
            Meetings = GetMeetingsParam.Meetings;

            LoadingMeetingData = false;

        }
    }


}

