using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingApp.Utils
{
    public class JsonProcessing
    {
        public string RemoveOuterBrackets(string json)
        {
            json = json.TrimStart('[');
            json = json.TrimEnd(']');

            return json;
        }
    }
}
