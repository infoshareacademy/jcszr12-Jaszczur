using TutorLizard.Shared.Models.DTOs.Requests;
using TutorLizard.Shared.Models.DTOs.Responses;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface IStudentService
{
    Task<GetStudentsAcceptedAdsResponse> GetStudentsAcceptedAds(GetStudentsAcceptedAdsRequest request);
    Task<GetStudentsAdRequestsResponse> GetStudentsAdRequests(GetStudentsAdRequestsRequest request);
    Task<GetAdRequestStatusResponse> GetAdRequestStatus(GetAdRequestStatusRequest request);
    Task<DeleteAdRequestResponse> DeleteAdRequest(DeleteAdRequestRequest request);
    Task<CreateScheduleItemRequestResponse> CreateScheduleItemRequest(CreateScheduleItemRequestRequest request);
    Task<GetAvailableScheduleForAdResponse> GetAvailableScheduleForAd(GetAvailableScheduleForAdRequest request);
    Task<CreateAdRequestResponse> CreateAdRequest(CreateAdRequestRequest request);
}