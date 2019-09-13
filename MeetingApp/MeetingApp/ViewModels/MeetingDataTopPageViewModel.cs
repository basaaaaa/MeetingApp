using MeetingApp.Constants;
using MeetingApp.Data;
using MeetingApp.Models.Constants;
using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using MeetingApp.Utils;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Windows.Input;

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

        public ICommand DeleteMeetingCommand { get; }

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

        public ICommand NavigateMeetingCreatePage { get; }

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

            //myUserId�̎擾
            MyUserId = _applicationProperties.GetFromProperties<string>("userId");


            //��c�̍폜�{�^���������ꂽ�Ƃ��̃R�}���h
            DeleteMeetingCommand = new DelegateCommand<object>(async id =>
            {

                var mid = Convert.ToInt32(id);
                _restService.DeleteMeetingDataAsync(MeetingConstants.OpenMeetingEndPoint, mid);

                //��c���Ď擾
                //��c���S���擾API�̃R�[��
                Meetings = await _restService.GetMeetingsDataAsync(MeetingConstants.OpenMeetingEndPoint, MyUserId);

            });

            //��c�̍폜�{�^���������ꂽ�Ƃ��̃R�}���h
            NavigateMeetingCreatePage = new DelegateCommand(async () =>
            {
                //��c���g�b�v�y�[�W�ɑJ�ڂ���
                await _navigationService.NavigateAsync("MeetingDataCreatePage");

            });

        }


        public override async void OnNavigatingTo(INavigationParameters parameters)
        {

            base.OnNavigatingTo(parameters);

            //Local��token���Q��
            if (_applicationProperties.GetFromProperties<TokenData>("token") == null)
            {
                _tokenCheckParam.HasError = true;
                _tokenCheckParam.NoExistMyToken = true;

            }
            else
            {
                var tokenData = _applicationProperties.GetFromProperties<TokenData>("token");
                //DB��token�Əƍ�����API�̃R�[��
                _tokenCheckParam = await _restService.CheckTokenDataAsync(TokenConstants.OpenTokenEndPoint, tokenData);


            }

            if (_tokenCheckParam.HasError == true)
            {
                //token�ƍ��̍ۂɃG���[�����������ۂ̏���
                Console.WriteLine("���O�C���Ɏ��s���܂���");
                await _navigationService.NavigateAsync("LoginPage");
            }

            //myUserId�̎擾
            MyUserId = _applicationProperties.GetFromProperties<string>("userId");

            //��c���S���擾API�̃R�[��
            Meetings = await _restService.GetMeetingsDataAsync(MeetingConstants.OpenMeetingEndPoint, MyUserId);

        }
    }


}

