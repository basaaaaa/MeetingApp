using System;
using System.Collections.Generic;

namespace MeetingApp.Droid.Models
{
    public partial class Token
    {
        public int Id { get; set; }
        public string TokenText { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
