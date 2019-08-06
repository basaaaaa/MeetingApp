using System;
using System.Collections.Generic;

namespace MeetingApp.Api.Models
{
    public partial class Meeting
    {
        public int Id { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? Regular { get; set; }
        public string Owner { get; set; }
        public string Location { get; set; }
        public string Title { get; set; }
    }
}
