using TutorLizard.Shared.Enums;

namespace TutorLizard.Shared.Models.DTOs
{
    public class LogInResult
    {
        public LogInResultCode ResultCode { get; set; }
        public UserDto? User { get; set; }
    }
}
