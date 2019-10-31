namespace MeetingApp.Models.Param
{
    public class CreateParticipateParam
    {
        public bool IsSuccessed { get; set; }

        public bool HasError { get; set; }

        public bool Entered { get; set; }   //既に入室済のユーザーであった場合


        public CreateParticipateParam()
        {
            this.IsSuccessed = false;
            this.HasError = false;
            this.Entered = false;
        }
    }
}
