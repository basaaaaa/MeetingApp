using MeetingApp.Constants;
using MeetingApp.Models.Constants;
using MeetingApp.Models.Param;
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

        [JsonProperty("active")]
        public Boolean Active { get; set; }


        public string UserId { get; set; }
        public List<MeetingLabelData> LabelItems { get; set; }


        public ParticipantData()
        {
            LabelItems = new List<MeetingLabelData>();
        }

        public ParticipantData(int uid, int mid)
        {
            this.Uid = uid;
            this.Mid = mid;
            this.LastUpdateTime = DateTime.Now;
            this.Active = false;
            LabelItems = new List<MeetingLabelData>();
        }

        //participant���g��userId���擾
        public async Task GetMyUserId()
        {
            RestService _restService = new RestService();
            GetUserParam getUserParam = await _restService.GetUserDataAsync(UserConstants.OpenUserEndPoint, this.Uid);
            this.UserId = getUserParam.User.UserId;

        }
        //participant���g�������x���ɑ΂���Items���擾
        //participant���g�������x���ɑ΂���Items���擾
        public async Task GetMyLabelItems()
        {
            RestService _restService = new RestService();

            //mid����Labels�擾
            GetMeetingLabelsParam getMeetingLabelsParam = await _restService.GetMeetingLabelsDataAsync(MeetingConstants.OPENMeetingLabelEndPoint, this.Mid, this.Uid);
            this.LabelItems = getMeetingLabelsParam.MeetingLabelDatas;

        }

    }
}
