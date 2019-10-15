using Xamarin.Forms;

namespace MeetingApp.Views
{
    public partial class MeetingAttendPage : ContentPage
    {
        public MeetingAttendPage()
        {
            InitializeComponent();
        }
        private void onItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            ((ListView)sender).SelectedItem = null;
        }
    }
}
