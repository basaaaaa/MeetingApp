using MeetingApp.Constants;
using MeetingApp.Data;
using MeetingApp.Models.Constants;
using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using MeetingApp.Utils;
using Prism.Navigation;
using System;
using System.Collections.Generic;

namespace MeetingApp.ViewModels
{
    public class MeetingDataTopPageViewModel : ViewModelBase
    {
        private string _test;
        private string _meetingTitle;
        private string _scheduledDate;
        private string _scheduledTime;
        private string _location;
        private string _myUserId;
        private List<MeetingData> _meetings;
        private Boolean _isOwner;

        public List<MeetingData> Meetings
        {
            get { return _meetings; }
            set { SetProperty(ref _meetings, value); }
        }
        public string Test
        {
            get { return _test; }
            set { SetProperty(ref _test, value); }
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


        RestService _restService;
        ApplicationProperties _applicationProperties;
        TokenCheckParam _tokenCheckParam;
        INavigationService _navigationService;


        public MeetingDataTopPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _restService = new RestService();
            _applicationProperties = new ApplicationProperties();
            _tokenCheckParam = new TokenCheckParam();
            _navigationService = navigationService;


        }


        public override async void OnNavigatingTo(INavigationParameters parameters)
        {

            base.OnNavigatingTo(parameters);

            //Localのtoken情報参照
            if (_applicationProperties.GetFromProperties<TokenData>("token") == null)
            {
                _tokenCheckParam.HasError = true;
                _tokenCheckParam.NoExistMyToken = true;

            }
            else
            {
                var tokenData = _applicationProperties.GetFromProperties<TokenData>("token");
                //DBのtokenと照合するAPIのコール
                _tokenCheckParam = await _restService.CheckTokenDataAsync(TokenConstants.OpenTokenEndPoint, tokenData);


            }

            if (_tokenCheckParam.HasError == true)
            {
                //token照合の際にエラーが発生した際の処理
                Console.WriteLine("ログインに失敗しました");
                await _navigationService.NavigateAsync("LoginPage");
            }

            //myUserIdの取得
            MyUserId = _applicationProperties.GetFromProperties<string>("userId");

            //会議情報全件取得APIのコール
            Meetings = await _restService.GetMeetingsDataAsync(MeetingConstants.OpenMeetingEndPoint);

            foreach (MeetingData meeting in Meetings)
            {
                if (meeting.Owner == MyUserId)
                {
                    meeting.IsOwner = true;
                }
                else
                {
                    meeting.IsOwner = false;
                }
            }

        }
    }


}

