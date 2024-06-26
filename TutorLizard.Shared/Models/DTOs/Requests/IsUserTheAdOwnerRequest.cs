namespace TutorLizard.Shared.Models.DTOs.Requests
{
    public class IsUserTheAdOwnerRequest(int adId, int? userId)
    {
        public int AdId { get; set; } = adId;
        public int? UserId { get; set; } = userId;
    }
}
