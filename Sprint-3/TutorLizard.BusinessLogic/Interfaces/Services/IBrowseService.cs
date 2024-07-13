using TutorLizard.Shared.Models.DTOs.Requests;
using TutorLizard.Shared.Models.DTOs.Responses;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface IBrowseService
{
    Task<GetAdDetailsResponse?> GetAdDetails(GetAdDetailsRequest request);
    Task<GetBrowseAdsPageResponse> GetBrowseAdsPage(GetBrowseAdsPageRequest request);
    Task<GetUsersScheduleResponse> GetUsersSchedule(GetUsersScheduleRequest request);
}