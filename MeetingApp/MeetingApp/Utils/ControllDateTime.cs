using System;
namespace MeetingApp.Utils
{
    public class ControllDateTime
    {
        public ControllDateTime()
        {

        }
        public DateTime GetCurrentDateTime()
        {
            var currentDateTime = DateTime.UtcNow.AddHours(9);
            return currentDateTime;
        }
    }
}
