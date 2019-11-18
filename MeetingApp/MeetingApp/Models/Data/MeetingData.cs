using MeetingApp.Models.Data;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace MeetingApp.Data
{
    public class MeetingData
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public String Title { get; set; }

        [JsonProperty("startDatetime")]
        public DateTime StartDatetime { get; set; }

        [JsonProperty("endDatetime")]
        public DateTime EndDatetime { get; set; }

        [JsonProperty("regular")]
        public Boolean Regular { get; set; }

        [JsonProperty("owner")]
        public int Owner { get; set; }

        [JsonProperty("location")]
        public String Location { get; set; }
        [JsonProperty("isvisible")]
        public Boolean IsVisible { get; set; }

        public Boolean IsOwner { get; set; }
        public Boolean IsGeneral { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }
        public string Date { get; set; }

        public ObservableCollection<MeetingLabelData> MeetingLabelDatas { get; set; }

        public MeetingData() { }

    }
}
