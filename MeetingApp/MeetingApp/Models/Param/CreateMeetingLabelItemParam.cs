namespace MeetingApp.Models.Param
{
    public class CreateMeetingLabelItemParam
    {

        public bool IsSuccessed { get; set; }   //

        public bool HasError { get; set; }  //

        public bool BlankItemName { get; set; }

        public CreateMeetingLabelItemParam()
        {
            this.IsSuccessed = false;
            this.HasError = false;
            this.BlankItemName = false;
        }
    }
}
