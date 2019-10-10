using MeetingApp.Api.Util;
using System;

namespace MeetingApp.Api.Models
{
    public class LoginService
    {

        RandomText _randomText;

        //token発行
        public Token CreateToken()
        {
            var token = new Token();
            _randomText = new RandomText();

            var tokenText = _randomText.CreateRandomText();
            DateTime startTime = DateTime.UtcNow.AddHours(9);
            DateTime endTime = startTime.AddHours(1);

            token.TokenText = tokenText;
            token.StartTime = startTime;
            token.EndTime = endTime;

            return token;
        }
    }
}
