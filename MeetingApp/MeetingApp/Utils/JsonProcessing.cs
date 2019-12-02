namespace MeetingApp.Utils
{
    public class JsonProcessing
    {
        /// <summary>
        /// jsonの配列文字を除去する
        /// </summary>
        /// <param name="json">加工前のjson文字列</param>
        /// <returns>[]を取り除いたjson文字列</returns>
        public string RemoveOuterBrackets(string json)
        {
            json = json.TrimStart('[');
            json = json.TrimEnd(']');

            return json;
        }
    }
}
