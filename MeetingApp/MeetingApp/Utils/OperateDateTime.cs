using System;

namespace MeetingApp.Utils
{
    public class OperateDateTime
    {
        /// <summary>
        /// ���ݎ���
        /// </summary>
        public DateTime CurrentDateTime { get; set; }

        /// <summary>
        /// Utc����v�Z���Č��݂̓��{���Ԃ��Z�o����
        /// </summary>
        public OperateDateTime()
        {
            CurrentDateTime = DateTime.UtcNow.AddHours(9);
        }
    }


}
