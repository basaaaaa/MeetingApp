using MeetingApp.Constants;
using MeetingApp.Models.Constants;
using MeetingApp.Models.Param;
using MeetingApp.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeetingApp.Models.Data
{
    public class ParticipantData
    {

        [JsonProperty("uid")]
        public int Uid { get; set; }

        [JsonProperty("mid")]
        public int Mid { get; set; }

        [JsonProperty("LastUpdateTime")]
        public DateTime LastUpdateTime { get; set; }

        [JsonProperty("isdeleted")]
        public Boolean isDeleted { get; set; }


        public string UserId { get; set; }
        public List<MeetingLabelData> LabelItems { get; set; }


        public ParticipantData()
        {
            LabelItems = new List<MeetingLabelData>();
        }

        public ParticipantData(int uid, int mid)
        {
            var operateDateTime = new OperateDateTime();
            this.Uid = uid;
            this.Mid = mid;
            this.LastUpdateTime = operateDateTime.CurrentDateTime;
            this.isDeleted = false;
            LabelItems = new List<MeetingLabelData>();
        }


        public async Task GetMyUserId()
        {
            RestService _restService = new RestService();
            GetUserParam getUserParam = await _restService.GetUserDataAsync(UserConstants.OpenUserEndPoint, this.Uid);
            this.UserId = getUserParam.User.UserId;

        }

        public async Task GetMyLabelItems()
        {
            RestService _restService = new RestService();


            GetMeetingLabelsParam getMeetingLabelsParam = await _restService.GetMeetingLabelsDataAsync(MeetingConstants.OPENMeetingLabelEndPoint, this.Mid, this.Uid);
            this.LabelItems = getMeetingLabelsParam.MeetingLabelDatas;

        }

    }
}
