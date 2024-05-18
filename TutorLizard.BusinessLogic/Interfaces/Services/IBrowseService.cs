using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface IBrowseService
{
    Task<GetAdDetailsResponse?> GetAdDetails(GetAdDetailsRequest request);
    Task<GetBrowseAdsPageResponse> GetBrowseAdsPage(GetBrowseAdsPageRequest request);
    Task<UsersScheduleResponse> GetUsersSchedule(UsersScheduleRequest request);
}