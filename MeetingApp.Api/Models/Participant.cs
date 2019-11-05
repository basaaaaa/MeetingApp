using System;
using System.Collections.Generic;

namespace MeetingApp.Api.Models
{
    public partial class Participant
    {
        public int Uid { get; set; }
        public int Mid { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public bool Isdeleted { get; set; }
    }
}
