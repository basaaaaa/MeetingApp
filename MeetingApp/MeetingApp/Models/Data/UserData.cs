using Newtonsoft.Json;
using System;

namespace MeetingApp.Models.Data
{
    class UserData
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("userId")]
        public String UserId { get; set; }

        [JsonProperty("password")]
        public String Password { get; set; }

        public UserData(int id, string userId, string password)
        {
            this.Id = id;
            this.UserId = userId;
            this.Password = password;
        }

    }
}
