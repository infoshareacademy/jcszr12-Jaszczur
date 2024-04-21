namespace TutorLizard.BusinessLogic.Models.DTOs.Requests
{
    public class AvailableScheduleForAdRequest
    {
        public int AdId { get; set; }
        public int StudentId { get; set; }
        public RequestStatus Status { get; set; }
        public AvailableScheduleForAdRequest(int adId, int studentId, RequestStatus status)
        {
            AdId = adId;
            StudentId = studentId;
            Status = status;
        }

        public enum RequestStatus
        {
            Accepted,
            Pending,
            Rejected
        }
    }
}
