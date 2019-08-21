using Newtonsoft.Json;
using System;

namespace MeetingApp.Models.Data
{
    class UserData
    {
        [JsonProperty("userId")]
        public String UserId { get; set; }

        [JsonProperty("password")]
        public String Password { get; set; }

        public UserData(string userId, string password)
        {
            this.UserId = userId;
            this.Password = password;
        }

    }
}
