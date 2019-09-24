using Newtonsoft.Json;

namespace MeetingApp.Models.Data
{
    class UserData
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }


        public UserData(string userId, string password)
        {
            this.UserId = userId;
            this.Password = password;
        }

        public UserData(int id, string userId, string password)
        {
            this.Id = id;
            this.UserId = userId;
            this.Password = password;
        }

        public UserData() { }

    }
}
