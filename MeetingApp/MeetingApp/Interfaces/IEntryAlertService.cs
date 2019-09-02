using System.Threading.Tasks;

namespace MeetingApp.Interfaces
{
    public interface IEntryAlertService
    {
        Task<EntryAlertResult> Show(string title, string message,
            string accepte, string cancel, bool isPassword = false);
    }

    public class EntryAlertResult
    {
        public string PressedButtonTitle { get; set; }
        public string Text { get; set; }
    }
}
