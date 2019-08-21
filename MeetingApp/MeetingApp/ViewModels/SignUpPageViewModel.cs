using MeetingApp.Models.Constants;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

namespace MeetingApp.ViewModels
{


    public class SignUpPageViewModel : BindableBase
    {
        public ICommand SignUpCommand { get; }
        RestService _restService;

        public SignUpPageViewModel()
        {
            _restService = new RestService();

            SignUpCommand = new DelegateCommand(() =>
            {
                _restService.SignUpUserDataAsync(UserConstants.OpenUserEndPoint, "kurokawa_r", "password");
            });
        }


    }
}
