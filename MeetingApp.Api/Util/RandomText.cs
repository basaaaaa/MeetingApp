using System;

namespace MeetingApp.Api.Util
{
    public class RandomText
    {
        public string CreateRandomText()
        {
            string random = Guid.NewGuid().ToString("N");
            return random;
        }
    }
}
