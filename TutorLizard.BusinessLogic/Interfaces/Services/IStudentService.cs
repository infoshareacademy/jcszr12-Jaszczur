using TutorLizard.Shared.Models.DTOs.Requests;
using TutorLizard.Shared.Models.DTOs.Responses;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface IStudentService
{
    Task<StudentsAcceptedAdsResponse> ViewAcceptedAds(StudentsAcceptedAdsRequest request);
    Task<StudentsAdRequestsResponse> ViewAdRequests(StudentsAdRequestsRequest request);
    Task<AdRequestStatusResponse> ViewAdRequestStatus(AdRequestStatusRequest request);
    Task<StudentCancelAdRequestResponse> DeleteAdRequest(StudentCancelAdRequestRequest request);
    Task<CreateScheduleItemRequestResponse> CreateScheduleItemRequest(CreateScheduleItemRequestRequest request);
    Task<AvailableScheduleForAdResponse> GetAvailableScheduleForAd(AvailableScheduleForAdRequest request);
    Task<CreateAdRequestResponse> CreateAdRequest(CreateAdRequestRequest request);
}