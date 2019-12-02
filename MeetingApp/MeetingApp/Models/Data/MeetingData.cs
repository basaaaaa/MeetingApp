using MeetingApp.Models.Data;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace MeetingApp.Data
{
    public class MeetingData
    {
        /// <summary>
        /// ��cID
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// ��c�^�C�g��
        /// </summary>

        [JsonProperty("title")]
        public String Title { get; set; }

        /// <summary>
        /// ��c�J�n����
        /// </summary>

        [JsonProperty("startDatetime")]
        public DateTime StartDatetime { get; set; }


        /// <summary>
        /// ��c�I������
        /// </summary>
        [JsonProperty("endDatetime")]
        public DateTime EndDatetime { get; set; }


        /// <summary>
        /// ��c�����true �s���false
        /// </summary>
        [JsonProperty("regular")]
        public Boolean Regular { get; set; }

        /// <summary>
        /// ��c�̊Ǘ��ҁi�쐬�ҁj��uid
        /// </summary>

        [JsonProperty("owner")]
        public int Owner { get; set; }


        /// <summary>
        /// ��c���{�ꏊ
        /// </summary>

        [JsonProperty("location")]
        public String Location { get; set; }


        /// <summary>
        /// ��c��񂪗L�����ǂ����i�I�����Ă��邩�ǂ����j
        /// </summary>
        [JsonProperty("isvisible")]
        public Boolean IsVisible { get; set; }



        /// <summary>
        /// ��c�ŊǗ��҂ł��邩�ۂ�
        /// </summary>
        public Boolean IsOwner { get; set; }


        /// <summary>
        /// ��c�̎Q���҂ł��邩�ۂ�
        /// </summary>
        public Boolean IsGeneral { get; set; }


        /// <summary>
        /// ��c�J�n����������
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// ��c�I������������
        /// </summary>
        /// 
        public string EndTime { get; set; }

        /// <summary>
        /// ��c���{����������
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        ///��c�ɕR�Â����x���Q
        /// </summary>
        public ObservableCollection<MeetingLabelData> MeetingLabelDatas { get; set; }

        public MeetingData() { }

    }
}
