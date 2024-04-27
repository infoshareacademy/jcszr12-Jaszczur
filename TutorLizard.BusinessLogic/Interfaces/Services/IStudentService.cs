using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface IStudentService
{
    Task<StudentsAcceptedAdsResponse> ViewAcceptedAds(StudentsAcceptedAdsRequest request);
    Task<StudentsAdRequestsResponse> ViewAdRequests(StudentsAdRequestsRequest request);
    Task<CreateScheduleItemRequestResponse> CreateScheduleItemRequest(CreateScheduleItemRequestRequest request);
}