using System;

namespace MeetingApp.Utils
{
    public class OperateDateTime
    {
        public DateTime CurrentDateTime { get; set; }

        public OperateDateTime()
        {
            CurrentDateTime = DateTime.UtcNow.AddHours(9);
        }
    }


}
