using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.BusinessLogic.Interfaces.Services;

public interface IStudentService
{
    Task<CreateAdRequestResponse> CreateAdRequest(CreateAdRequestRequest request);
}