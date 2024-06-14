using TutorLizard.BusinessLogic.Models.DTOs;

namespace TutorLizard.Web.Models
{
    public class LogInResult
    {
        public LogInResultCode ResultCode { get; set; }
        public UserDto? User { get; set; }
    }

    public enum LogInResultCode
    {
        Success,
        UserNotFound,
        InactiveAccount,
        InvalidPassword
    }
}
