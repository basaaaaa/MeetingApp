using MeetingApp.Data;
using Prism.Navigation;
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
        private List<MeetingData> _meetings;
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


        RestService _restService;


        public MeetingDataTopPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            //Test = "aaaaaaaaaaaaaa";
            _restService = new RestService();


        }


        public override async void OnNavigatingTo(INavigationParameters parameters)
        {

            base.OnNavigatingTo(parameters);


            var meetingData = await _restService.GetMeetingDataAsync("https://meeting-app-api.azurewebsites.net/api/meetings/2");

            MeetingTitle = meetingData.Title;
            ScheduledDate = meetingData.ScheduledDate.ToShortDateString();
            ScheduledTime = meetingData.ScheduledDate.ToShortTimeString();
            Location = meetingData.Location;
        }


    }
}
