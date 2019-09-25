using MeetingApp.Constants;
using MeetingApp.Data;
using MeetingApp.Models.Constants;
using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using MeetingApp.Models.Validate;
using MeetingApp.Utils;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MeetingApp.ViewModels
{
    public class MeetingDataCreatePageViewModel : ViewModelBase
    {
        //private List<MeetingLabelData> _labels;
        private ObservableCollection<MeetingLabelData> _labels;
        private string _createLabelName;

        private string _inputMeetingTitle;
        private string _inputMeetingDate;
        private string _inputMeetingStartTime;
        private string _inputMeetingEndTime;
        private string _inputMeetingLocation;
        private CreateMeetingParam _createMeetingParam;
        private CreateMeetingLabelParam _createMeetingLabelParam;



        public ObservableCollection<MeetingLabelData> Labels
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
        public CreateMeetingParam CreateMeetingParam
        {
            get { return _createMeetingParam; }
            set { SetProperty(ref _createMeetingParam, value); }
        }
        public CreateMeetingLabelParam CreateMeetingLabelParam
        {
            get { return _createMeetingLabelParam; }
            set { SetProperty(ref _createMeetingLabelParam, value); }
        }

        public ICommand CreateMeetingDataCommand { get; }
        public ICommand CreateMeetingLabelCommand { get; }
        public MeetingData CreateMeetingData;

        RestService _restService;
        ApplicationProperties _applicationProperties;
        INavigationService _navigationService;
        CreateMeetingValidation _createMeetingValidation;
        CreateMeetingLabelValidation _createMeetingLabelValidation;

        public MeetingDataCreatePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _restService = new RestService();
            _navigationService = navigationService;
            _applicationProperties = new ApplicationProperties();
            _createMeetingParam = new CreateMeetingParam();
            _createMeetingLabelParam = new CreateMeetingLabelParam();
            Labels = new ObservableCollection<MeetingLabelData>();
            _createMeetingValidation = new CreateMeetingValidation();
            _createMeetingLabelValidation = new CreateMeetingLabelValidation();

            ////仮入力データ
            InputMeetingTitle = "テスト会議";
            InputMeetingDate = "2019-09-07";
            InputMeetingStartTime = "10:00";
            InputMeetingEndTime = "10:15";
            InputMeetingLocation = "どこか";

            CreateMeetingDataCommand = new DelegateCommand(async () =>

            {
                //入力値のバリデーション
                CreateMeetingParam = _createMeetingValidation.InputValidate(InputMeetingTitle, InputMeetingDate, InputMeetingStartTime, InputMeetingEndTime, InputMeetingLocation, Labels);
                //バリデーションエラーが存在すれば失敗で返す
                if (CreateMeetingParam.HasError == true) { return; }

                //会議開始時間と終了時間をDateTime型に変換
                var InputMeetingStartDateTime = DateTime.Parse(InputMeetingDate + " " + InputMeetingStartTime);
                var InputMeetingEndDateTime = DateTime.Parse(InputMeetingDate + " " + InputMeetingEndTime);

                //userIdに対するidを取得
                //ローカルにid情報を保持する
                var getUserParam = await _restService.GetUserDataAsync(UserConstants.OpenUserEndPoint, _applicationProperties.GetFromProperties<string>("userId"));

                if (getUserParam.HasError == true) { return; }

                //Json用のモデルを作成
                CreateMeetingData = new MeetingData();

                CreateMeetingData.Title = InputMeetingTitle;
                CreateMeetingData.StartDatetime = InputMeetingStartDateTime;
                CreateMeetingData.EndDatetime = InputMeetingEndDateTime;
                CreateMeetingData.Regular = false;
                CreateMeetingData.Owner = getUserParam.User.Id;
                CreateMeetingData.Location = InputMeetingLocation;
                CreateMeetingData.Deleted = false;

                CreateMeetingParam = await _restService.CreateMeetingDataAsync(MeetingConstants.OpenMeetingEndPoint, CreateMeetingData, Labels);

                if (CreateMeetingParam.IsSuccessed == true)
                {
                    //会議情報トップページに遷移する
                    await _navigationService.NavigateAsync("MeetingDataTopPage");
                }

            });

            //カードラベルの追加ボタンを押したときのコマンド Labelsリストにlabelオブジェクトを追加する
            CreateMeetingLabelCommand = new DelegateCommand(() =>
            {
                CreateMeetingLabelParam = _createMeetingLabelValidation.InputValidate(CreateLabelName);
                if (CreateMeetingLabelParam.HasError == true) { return; }

                var label = new MeetingLabelData(CreateLabelName);
                Labels.Add(label);
                CreateLabelName = "";

            });
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {

            base.OnNavigatingTo(parameters);


        }

    }
}
