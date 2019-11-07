using MeetingApp.Models.Param;

namespace MeetingApp.Utils
{
    public class APICallLoading
    {
        public APICallLoading()
        {

        }
        public LoadingParam StartLoading(LoadingParam loadingParam)
        {
            loadingParam.IsLoading = true;
            return loadingParam;
        }

        public LoadingParam FinishLoading(LoadingParam loadingParam)
        {
            loadingParam.IsLoading = false;
            return loadingParam;
        }
    }
}
