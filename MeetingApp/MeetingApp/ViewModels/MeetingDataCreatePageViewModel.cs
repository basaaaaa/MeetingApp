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

        #region private data

        private ObservableCollection<MeetingLabelData> _labels;
        private string _createLabelName;
        private string _inputMeetingTitle;
        private DateTime _inputMeetingDate;
        private TimeSpan _inputMeetingStartTime;
        private TimeSpan _inputMeetingEndTime;
        private string _inputMeetingLocation;
        private int _labelListViewHeight;
        private DateTime _selectedDate;
        private TimeSpan _selectedTime;

        #endregion

        #region private param

        private CreateMeetingParam _createMeetingParam;
        private CreateMeetingLabelParam _createMeetingLabelParam;

        #endregion


        #region public data
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

        public DateTime InputMeetingDate
        {
            get { return _inputMeetingDate; }
            set { SetProperty(ref _inputMeetingDate, value); }
        }
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { SetProperty(ref _selectedDate, value); }
        }
        public TimeSpan SelectedTime
        {
            get { return _selectedTime; }
            set { SetProperty(ref _selectedTime, value); }
        }

        public TimeSpan InputMeetingStartTime
        {
            get { return _inputMeetingStartTime; }
            set { SetProperty(ref _inputMeetingStartTime, value); }
        }

        public TimeSpan InputMeetingEndTime
        {
            get { return _inputMeetingEndTime; }
            set { SetProperty(ref _inputMeetingEndTime, value); }
        }

        public string InputMeetingLocation
        {
            get { return _inputMeetingLocation; }
            set { SetProperty(ref _inputMeetingLocation, value); }
        }

        public int LabelListViewHeight
        {
            get { return _labelListViewHeight; }
            set { SetProperty(ref _labelListViewHeight, value); }
        }

        public MeetingData CreateMeetingData;

        #endregion


        #region public param
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

        #endregion

        #region command

        public ICommand CreateMeetingDataCommand { get; }
        public ICommand CreateMeetingLabelCommand { get; }
        public ICommand NavigateMeetingDataTopPage { get; }

        #endregion

        #region ohers
        RestService _restService;
        ApplicationProperties _applicationProperties;
        INavigationService _navigationService;
        CreateMeetingValidation _createMeetingValidation;
        CreateMeetingLabelValidation _createMeetingLabelValidation;

        #endregion

        public MeetingDataCreatePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;

            _restService = new RestService();
            _applicationProperties = new ApplicationProperties();

            _createMeetingParam = new CreateMeetingParam();
            _createMeetingLabelParam = new CreateMeetingLabelParam();

            _createMeetingValidation = new CreateMeetingValidation();
            _createMeetingLabelValidation = new CreateMeetingLabelValidation();

            Labels = new ObservableCollection<MeetingLabelData>();


            //入力初期値
            InputMeetingDate = DateTime.UtcNow;

            CreateMeetingDataCommand = new DelegateCommand(async () =>

            {
                //会議情報入力値のバリデーション処理
                CreateMeetingParam = _createMeetingValidation.InputValidate(InputMeetingTitle, InputMeetingDate, InputMeetingStartTime, InputMeetingEndTime, InputMeetingLocation, Labels);

                //バリデーションエラーが存在すれば失敗で返す
                if (CreateMeetingParam.HasError == true) { return; }

                //DateTimeとTimeSpanを結合
                var InputMeetingStartDateTime = InputMeetingDate + InputMeetingStartTime;
                var InputMeetingEndDateTime = InputMeetingDate + InputMeetingEndTime;

                //userIdに対するidを取得
                var getUserParam = await _restService.GetUserDataAsync(UserConstants.OpenUserEndPoint, _applicationProperties.GetFromProperties<string>("userId"));

                //ユーザー取得時にエラーがあれば処理終了
                if (getUserParam.HasError == true) { return; }

                //Json用のモデルを作成
                CreateMeetingData = new MeetingData();

                CreateMeetingData.Title = InputMeetingTitle;
                CreateMeetingData.StartDatetime = InputMeetingStartDateTime;
                CreateMeetingData.EndDatetime = InputMeetingEndDateTime;
                CreateMeetingData.Regular = false;
                CreateMeetingData.Owner = getUserParam.User.Id;
                CreateMeetingData.Location = InputMeetingLocation;
                CreateMeetingData.IsVisible = true;

                //会議作成APIのコール
                CreateMeetingParam = await _restService.CreateMeetingDataAsync(MeetingConstants.OpenMeetingEndPoint, CreateMeetingData, Labels);

                //会議作成が成功すればMeetingDataTopPageに遷移する
                if (CreateMeetingParam.IsSuccessed == true)
                {
                    await _navigationService.NavigateAsync("/NavigationPage/MeetingDataTopPage");
                }

            });

            //ラベル追加の際のコマンド
            CreateMeetingLabelCommand = new DelegateCommand(() =>
            {
                //作成するラベル名のバリデーション
                CreateMeetingLabelParam = _createMeetingLabelValidation.InputValidate(CreateLabelName);
                //ラベル作成に失敗すれば処理中断
                if (CreateMeetingLabelParam.HasError == true) { return; }

                //リストにラベル情報を保持
                var label = new MeetingLabelData(CreateLabelName);
                Labels.Add(label);
                CreateLabelName = "";

                //ViewにおけるFrameの高さを大きくする
                LabelListViewHeight += 85;

            });

            NavigateMeetingDataTopPage = new DelegateCommand(() =>
            {
                //会議情報TOPページへ遷移
                _navigationService.NavigateAsync("/NavigationPage/MeetingDataTopPage");
            });
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {

            base.OnNavigatingTo(parameters);

            LabelListViewHeight = 20;

        }

    }
}
