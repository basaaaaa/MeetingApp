namespace MeetingApp.Utils
{
    public enum ErrorPageType
    {
        /// <summary>
        /// 会議が既に終了している際のエラー
        /// </summary>
        FinishedMeeting,
        /// <summary>
        /// トークン失効
        /// </summary>
        ExpiredToken,
        /// <summary>
        /// 予期せぬエラー
        /// </summary>
        Unexpected

    }
}
