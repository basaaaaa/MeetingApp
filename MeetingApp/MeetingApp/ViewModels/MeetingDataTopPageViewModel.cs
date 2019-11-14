using MeetingApp.Constants;
using MeetingApp.Data;
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
        private string _test;
        private string _meetingTitle;
        private string _scheduledDate;
        private string _scheduledTime;
        private string _location;
        private string _myUserId;
        private List<MeetingData> _meetings;
        private Boolean _isOwner;
        private TokenCheckParam _tokenCheckParam;
        private Boolean _loadingMeetingData;


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
        public TokenCheckParam TokenCheckParam
        {
            get { return _tokenCheckParam; }
            set { SetProperty(ref _tokenCheckParam, value); }
        }
        public bool LoadingMeetingData
        {
            get { return _loadingMeetingData; }
            set { SetProperty(ref _loadingMeetingData, value); }
        }

        public ICommand NavigateMeetingCreatePage { get; }
        public ICommand NavigateMeetingAttendPage { get; }
        public ICommand DeleteMeetingCommand { get; }

        RestService _restService;
        ApplicationProperties _applicationProperties;
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

                //�Ǘ��҂����삷���c�I������
                var select = await Application.Current.MainPage.DisplayAlert("�x��", "��c���폜���Ă���낵���ł��傤���H", "OK", "�L�����Z��");

                if (select)
                {
                    var mid = Convert.ToInt32(id);

                    ////�����폜
                    //_restService.DeleteMeetingDataAsync(MeetingConstants.OpenMeetingEndPoint, mid);

                    //�_���폜

                    //�ΏۂƂȂ��c����1���擾
                    GetMeetingParam getMeetingParam = new GetMeetingParam();
                    getMeetingParam = await _restService.GetMeetingDataAsync(MeetingConstants.OpenMeetingEndPoint, mid);
                    var updateMeetingData = getMeetingParam.MeetingData;
                    //�t���O��false�ɕύX
                    updateMeetingData.IsVisible = false;
                    await _restService.UpdateMeetingDataAsync(MeetingConstants.OpenMeetingEndPoint, updateMeetingData);

                    //��c���Ď擾
                    //��c���S���擾API�̃R�[��
                    Meetings = await _restService.GetMeetingsDataAsync(MeetingConstants.OpenMeetingEndPoint, MyUserId);
                }
                else
                {
                    return;
                }

            });

            //��c�V�K�쐬�y�[�W�ɑJ�ڂ���R�}���h
            NavigateMeetingCreatePage = new DelegateCommand(async () =>
            {
                //��c���g�b�v�y�[�W�ɑJ�ڂ���
                await _navigationService.NavigateAsync("/NavigationPage/MeetingDataCreatePage");

            });

            //��c�o�ȃy�[�W�ɑJ�ڂ���R�}���h
            NavigateMeetingAttendPage = new DelegateCommand<object>(async id =>
            {

                var mid = Convert.ToInt32(id);

                //�w��̉�c�̏���CommandParameter�Ŏ󂯎��A��cid(mid)���擾����
                GetMeetingParam getMeetingParam = new GetMeetingParam();
                getMeetingParam = await _restService.GetMeetingDataAsync(MeetingConstants.OpenMeetingEndPoint, mid);

                var p = new NavigationParameters
                {
                    { "mid", getMeetingParam.MeetingData.Id}
                };

                //��c���g�b�v�y�[�W�ɑJ�ڂ���
                await _navigationService.NavigateAsync("MeetingAttendPage", p);

            });

        }


        public override async void OnNavigatingTo(INavigationParameters parameters)
        {

            base.OnNavigatingTo(parameters);

            LoadingMeetingData = true;

            TokenCheckValidation tokenCheckValidation = new TokenCheckValidation();
            TokenCheckParam = await tokenCheckValidation.Validate();

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

            LoadingMeetingData = false;

        }
    }


}

