using MeetingApp.Models.Data;
using Prism.Navigation;

namespace MeetingApp.ViewModels
{
    public class MeetingExecuteUserPageViewModel : ViewModelBase
    {
        public MeetingExecuteUserPageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            var participantData = (ParticipantData)parameters["participantData"];




        }
    }
}
