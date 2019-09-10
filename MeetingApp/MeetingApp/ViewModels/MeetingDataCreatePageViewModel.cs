using MeetingApp.Constants;
using MeetingApp.Data;
using MeetingApp.Models.Data;
using MeetingApp.Utils;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace MeetingApp.ViewModels
{
    public class MeetingDataCreatePageViewModel : ViewModelBase
    {
        private List<MeetingLabelData> _labels;
        private string _createLabelName;

        private string _inputMeetingTitle;
        private string _inputMeetingDate;
        private string _inputMeetingStartTime;
        private string _inputMeetingEndTime;
        private string _inputMeetingLocation;
        private

        public List<MeetingLabelData> Labels
        {
            get { return _labels; }
            set { SetProperty(ref _labels, value); }
        }

        public string CreateLabelName
        {
            get { return _createLabelName; }
            set { SetProperty(ref _createLabelName, value); }
        }

        public string InputMeetingTitle
        {
            get { return _inputMeetingTitle; }
            set { SetProperty(ref _inputMeetingTitle, value); }
        }

        public string InputMeetingDate
        {
            get { return _inputMeetingDate; }
            set { SetProperty(ref _inputMeetingDate, value); }
        }

        public string InputMeetingStartTime
        {
            get { return _inputMeetingStartTime; }
            set { SetProperty(ref _inputMeetingStartTime, value); }
        }

        public string InputMeetingEndTime
        {
            get { return _inputMeetingEndTime; }
            set { SetProperty(ref _inputMeetingEndTime, value); }
        }

        public string InputMeetingLocation
        {
            get { return _inputMeetingLocation; }
            set { SetProperty(ref _inputMeetingLocation, value); }
        }

        public ICommand CreateMeetingDataCommand { get; }
        public ICommand CreateMeetingLabelCommand { get; }
        public MeetingData CreateMeetingData;

        RestService _restService;
        ApplicationProperties _applicationProperties;

        public MeetingDataCreatePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _restService = new RestService();
            _applicationProperties = new ApplicationProperties();

            InputMeetingTitle = "テスト会議";
            InputMeetingDate = "2019-09-07";
            InputMeetingStartTime = "10:00";
            InputMeetingEndTime = "10:15";
            InputMeetingLocation = "どこか";

            CreateMeetingDataCommand = new DelegateCommand(async () =>

            {
                var InputMeetingStartDateTime = DateTime.Parse(InputMeetingDate + InputMeetingStartTime);

                //Json用のモデルを作成
                CreateMeetingData.Title = InputMeetingTitle;


                CreateMeetingData.StartDatetime = InputMeetingStartDateTime;

                var InputMeetingEndDateTime = DateTime.Parse(InputMeetingDate + InputMeetingEndTime);
                CreateMeetingData.EndDatetime = InputMeetingEndDateTime;

                CreateMeetingData.Regular = false;
                CreateMeetingData.Owner = _applicationProperties.GetFromProperties<string>("userId");
                CreateMeetingData.Location = InputMeetingLocation;

                await _restService.CreateMeetingDataAsync(MeetingConstants.OpenMeetingEndPoint, CreateMeetingData);


            });

            //カードラベルの追加ボタンを押したときのコマンド
            //LabelsリストにLabelオブジェクトを追加する
            CreateMeetingLabelCommand = new DelegateCommand(() =>
            {

                var label = new MeetingLabelData(CreateLabelName);
                Console.WriteLine(label);
                Console.WriteLine(Labels);
                Labels.Add(label);

            });
        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {

            base.OnNavigatingTo(parameters);


        }

    }
}
