using MeetingApp.Constants;
using MeetingApp.Models.Constants;
using MeetingApp.Models.Data;
using MeetingApp.Models.Param;
using MeetingApp.Models.Validate;
using MeetingApp.Utils;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MeetingApp.ViewModels
{
    public class MeetingLabelItemDataCreatePageViewModel : ViewModelBase
    {
        //データ
        private MeetingLabelData _targetMeetingLabel;
        private string _inputLabelItemName;
        private List<MeetingLabelItemData> _additionalMeetingLabelItemDatas; //DB追加用のItemsリスト
        private ObservableCollection<MeetingLabelItemData> _meetingLabelItemDatas; //
        private ObservableCollection<MeetingLabelItemData> _getMeetingLabelItemList;　//画面初期化時にGetで全件取得するItemsリスト

        //Param
        private CreateMeetingLabelItemParam _createMeetingLabelItemParam;
        private TokenCheckParam _tokenCheckParam;
        private GetUserParam _getUserParam;
        private GetMeetingLabelItemsParam _getMeetingLabelItemsParam;


        public MeetingLabelData TargetMeetingLabel
        {
            get { return _targetMeetingLabel; }
            set { SetProperty(ref _targetMeetingLabel, value); }
        }
        public string InputLabelItemName
        {
            get { return _inputLabelItemName; }
            set { SetProperty(ref _inputLabelItemName, value); }
        }
        public CreateMeetingLabelItemParam CreateMeetingLabelItemParam
        {
            get { return _createMeetingLabelItemParam; }
            set { SetProperty(ref _createMeetingLabelItemParam, value); }
        }
        public ObservableCollection<MeetingLabelItemData> MeetingLabelItemDatas
        {
            get { return _meetingLabelItemDatas; }
            set { SetProperty(ref _meetingLabelItemDatas, value); }
        }
        public List<MeetingLabelItemData> AdditionalMeetingLabelItemDatas
        {
            get { return _additionalMeetingLabelItemDatas; }
            set { SetProperty(ref _additionalMeetingLabelItemDatas, value); }
        }
        public ObservableCollection<MeetingLabelItemData> GetMeetingLabelItemDatas
        {
            get { return _getMeetingLabelItemList; }
            set { SetProperty(ref _getMeetingLabelItemList, value); }
        }
        public TokenCheckParam TokenCheckParam
        {
            get { return _tokenCheckParam; }
            set { SetProperty(ref _tokenCheckParam, value); }
        }
        public GetUserParam GetUserParam
        {
            get { return _getUserParam; }
            set { SetProperty(ref _getUserParam, value); }
        }
        public GetMeetingLabelItemsParam GetMeetingLabelItemsParam
        {
            get { return _getMeetingLabelItemsParam; }
            set { SetProperty(ref _getMeetingLabelItemsParam, value); }
        }

        public ICommand CreateMeetingLabelItemCommand { get; }


        CreateMeetingLabelItemValidation _createMeetingLabelItemValidation;
        TokenCheckValidation _tokenCheckValidation;
        RestService _restService;
        ApplicationProperties _applicationProperties;

        public MeetingLabelItemDataCreatePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _createMeetingLabelItemValidation = new CreateMeetingLabelItemValidation();
            _tokenCheckValidation = new TokenCheckValidation();
            _restService = new RestService();
            _applicationProperties = new ApplicationProperties();
            _meetingLabelItemDatas = new ObservableCollection<MeetingLabelItemData>();
            _createMeetingLabelItemParam = new CreateMeetingLabelItemParam();

            _additionalMeetingLabelItemDatas = new List<MeetingLabelItemData>();

            //会議の各ラベルに項目（Item)を追加するコマンド
            CreateMeetingLabelItemCommand = new DelegateCommand(async () =>
            {

                //項目入力値のバリデーション
                CreateMeetingLabelItemParam = _createMeetingLabelItemValidation.InputValidate(InputLabelItemName);
                if (CreateMeetingLabelItemParam.HasError == true) { return; }



                //uid取得の際のtoken情報照合
                TokenCheckParam = await _tokenCheckValidation.Validate(_applicationProperties.GetFromProperties<TokenData>("token"));

                var inputUid = 0;

                if (TokenCheckParam.HasError == true)
                {
                    return;
                }
                else
                {
                    //token情報照合に成功したらuid取得
                    GetUserParam = await _restService.GetUserDataAsync(UserConstants.OpenUserEndPoint, _applicationProperties.GetFromProperties<string>("userId"));

                    if (GetUserParam.HasError == true)
                    {
                        return;
                    }
                    else
                    {
                        //userDataの取得に成功したらuidを代入
                        var userData = GetUserParam.User;
                        inputUid = userData.Id;

                    }

                }
                var meetingLabelItemData = new MeetingLabelItemData(TargetMeetingLabel.Id, inputUid, InputLabelItemName);
                MeetingLabelItemDatas.Add(meetingLabelItemData);
                AdditionalMeetingLabelItemDatas.Add(meetingLabelItemData);

                InputLabelItemName = "";

            });
        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            //MeetingAttendPageからのラベルのパラメータを取得
            TargetMeetingLabel = (MeetingLabelData)parameters["meetingLabelData"];
            //user情報の取得
            GetUserParam = await _restService.GetUserDataAsync(UserConstants.OpenUserEndPoint, _applicationProperties.GetFromProperties<string>("userId"));
            var uid = GetUserParam.User.Id;

            var targetLid = TargetMeetingLabel.Id;
            GetMeetingLabelItemsParam = await _restService.GetMeetingLabelItemsDataAsync(MeetingConstants.OPENMeetingLabelItemEndPoint, targetLid, uid);
            GetMeetingLabelItemDatas = new ObservableCollection<MeetingLabelItemData>(GetMeetingLabelItemsParam.MeetingLabelItemDatas);
            MeetingLabelItemDatas = new ObservableCollection<MeetingLabelItemData>(GetMeetingLabelItemDatas);

        }

        //戻るボタンを押してMeetingAttendPageに遷移するときの処理
        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {

            base.OnNavigatingTo(parameters);

            parameters.Add("mid", TargetMeetingLabel.Mid);

            //ラベルアイテムをDBにInsert
            foreach (MeetingLabelItemData i in AdditionalMeetingLabelItemDatas)
            {
                CreateMeetingLabelItemParam = await _restService.CreateMeetingLabelItemDataAsync(MeetingConstants.OPENMeetingLabelItemEndPoint, i);
            }



        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {

        }

    }
}
