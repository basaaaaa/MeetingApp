using MeetingApp.Models.Data;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace MeetingApp.Data
{
    public class MeetingData
    {
        /// <summary>
        /// ‰ï‹cID
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// ‰ï‹cƒ^ƒCƒgƒ‹
        /// </summary>

        [JsonProperty("title")]
        public String Title { get; set; }

        /// <summary>
        /// ‰ï‹cŠJn
        /// </summary>

        [JsonProperty("startDatetime")]
        public DateTime StartDatetime { get; set; }


        /// <summary>
        /// ‰ï‹cI—¹
        /// </summary>
        [JsonProperty("endDatetime")]
        public DateTime EndDatetime { get; set; }


        /// <summary>
        /// ‰ï‹c‚ª’èŠútrue •s’èŠúfalse
        /// </summary>
        [JsonProperty("regular")]
        public Boolean Regular { get; set; }

        /// <summary>
        /// ‰ï‹c‚ÌŠÇ—Òiì¬Òj‚Ìuid
        /// </summary>

        [JsonProperty("owner")]
        public int Owner { get; set; }


        /// <summary>
        /// ‰ï‹cÀ{êŠ
        /// </summary>

        [JsonProperty("location")]
        public String Location { get; set; }


        /// <summary>
        /// ‰ï‹cî•ñ‚ª—LŒø‚©‚Ç‚¤‚©iI—¹‚µ‚Ä‚¢‚é‚©‚Ç‚¤‚©j
        /// </summary>
        [JsonProperty("isvisible")]
        public Boolean IsVisible { get; set; }



        /// <summary>
        /// ‰ï‹c‚ÅŠÇ—Ò‚Å‚ ‚é‚©”Û‚©
        /// </summary>
        public Boolean IsOwner { get; set; }


        /// <summary>
        /// ‰ï‹c‚ÌQ‰ÁÒ‚Å‚ ‚é‚©”Û‚©
        /// </summary>
        public Boolean IsGeneral { get; set; }


        /// <summary>
        /// ‰ï‹cŠJn•¶š—ñ
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// ‰ï‹cI—¹•¶š—ñ
        /// </summary>
        /// 
        public string EndTime { get; set; }

        /// <summary>
        /// ‰ï‹cÀ{“ú•¶š—ñ
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        ///‰ï‹c‚É•R‚Ã‚­ƒ‰ƒxƒ‹ŒQ
        /// </summary>
        public ObservableCollection<MeetingLabelData> MeetingLabelDatas { get; set; }

        public MeetingData() { }

    }
}
