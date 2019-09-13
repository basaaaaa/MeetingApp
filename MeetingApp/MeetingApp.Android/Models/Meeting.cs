using System;
using System.Collections.Generic;

namespace MeetingApp.Droid.Models
{
    public partial class Meeting
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDatetime { get; set; }
        public DateTime EndDatetime { get; set; }
        public bool Regular { get; set; }
        public int Owner { get; set; }
        public string Location { get; set; }
    }
}
