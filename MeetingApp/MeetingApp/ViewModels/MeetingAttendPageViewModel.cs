using MeetingApp.Constants;
using MeetingApp.Data;
using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using MeetingApp.Models.Validate;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MeetingApp.ViewModels
{
    public class MeetingAttendPageViewModel : ViewModelBase
    {
        private MeetingData _targetMeetingData;
        private ObservableCollection<MeetingLabelData> _targetMeetingLabels;
        private string _inputLabelItemName;
        private CreateMeetingLabelItemParam _createMeetingLabelItemParam;
        private GetMeetingLabelsParam _getMeetingLabelsParam;
        private CreateMeetingLabelItemValidation _createMeetingLabelItemValidation;
        private GetMeetingParam _getMeetingParam;

        public MeetingData TargetMeetingData
        {
            get { return _targetMeetingData; }
            set { SetProperty(ref _targetMeetingData, value); }
        }
        public string InputLabelItemName
        {
            get { return _inputLabelItemName; }
            set { SetProperty(ref _inputLabelItemName, value); }
        }
        public ObservableCollection<MeetingLabelData> TargetMeetingLabels
        {
            get { return _targetMeetingLabels; }
            set { SetProperty(ref _targetMeetingLabels, value); }
        }

        public CreateMeetingLabelItemParam CreateMeetingLabelItemParam
        {
            get { return _createMeetingLabelItemParam; }
            set { SetProperty(ref _createMeetingLabelItemParam, value); }
        }
        public GetMeetingLabelsParam GetMeetingLabelsParam
        {
            get { return _getMeetingLabelsParam; }
            set { SetProperty(ref _getMeetingLabelsParam, value); }
        }
        public GetMeetingParam GetMeetingParam
        {
            get { return _getMeetingParam; }
            set { SetProperty(ref _getMeetingParam, value); }
        }


        public ICommand CreateMeetingLabelItemCommand { get; }
        public ICommand EnterMeetingCommand { get; }
        public ICommand ExitMeetingCommand { get; }

        INavigationService _navigationService;

        RestService _restService;
        public MeetingAttendPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _createMeetingLabelItemParam = new CreateMeetingLabelItemParam();
            _createMeetingLabelItemValidation = new CreateMeetingLabelItemValidation();
            _navigationService = navigationService;

            //会議の各ラベルに項目（Item)を追加するコマンド
            CreateMeetingLabelItemCommand = new DelegateCommand<object>((id) =>
            {
                var lid = Convert.ToInt32(id);

                //項目入力値のバリデーション
                CreateMeetingLabelItemParam = _createMeetingLabelItemValidation.InputValidate(InputLabelItemName);
                if (CreateMeetingLabelItemParam.HasError == true) { return; }

                //項目を追加する先のリストを特定し追加
                var meetingLabelItemData = new MeetingLabelItemData(lid, InputLabelItemName);
                TargetMeetingLabels.FirstOrDefault(l => l.Id == lid).MeetingLabelItemDatas.Add(meetingLabelItemData);

                InputLabelItemName = "";

            });

            //会議に入室するコマンド
            EnterMeetingCommand = new DelegateCommand(() =>
            {
                //バリデーション
                //CreateMeetingLabelItemParam = _createMeetingLabelItemValidation.InputValidate();
            });

            //会議入室画面から退室するコマンド
            ExitMeetingCommand = new DelegateCommand(() =>
            {
                _navigationService.NavigateAsync("MeetingDataTopPage");
            });


        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            _restService = new RestService();
            _getMeetingLabelsParam = new GetMeetingLabelsParam();
            _getMeetingParam = new GetMeetingParam();

            //対象の会議データ取得
            GetMeetingParam = await _restService.GetMeetingDataAsync(MeetingConstants.OpenMeetingEndPoint, (int)parameters["mid"]);
            TargetMeetingData = GetMeetingParam.MeetingData;

            //会議のラベルを取得
            GetMeetingLabelsParam = await _restService.GetMeetingLabelsDataAsync(MeetingConstants.OPENMeetingLabelEndPoint, (int)parameters["mid"]);
            TargetMeetingLabels = new ObservableCollection<MeetingLabelData>(GetMeetingLabelsParam.MeetingLabelDatas);

        }
    }
}
