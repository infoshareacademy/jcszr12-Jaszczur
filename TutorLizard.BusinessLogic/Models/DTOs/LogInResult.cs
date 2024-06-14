namespace TutorLizard.BusinessLogic.Models.DTOs
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
