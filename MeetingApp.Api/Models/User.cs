using System;
using System.Collections.Generic;

namespace MeetingApp.Api.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
