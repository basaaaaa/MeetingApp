using System;

namespace MeetingApp.Utils
{
    public class OperateDateTime
    {
        /// <summary>
        /// Œ»İ
        /// </summary>
        public DateTime CurrentDateTime { get; set; }

        /// <summary>
        /// Utc‚©‚çŒvZ‚µ‚ÄŒ»İ‚Ì“ú–{ŠÔ‚ğZo‚·‚é
        /// </summary>
        public OperateDateTime()
        {
            CurrentDateTime = DateTime.UtcNow.AddHours(9);
        }
    }


}
