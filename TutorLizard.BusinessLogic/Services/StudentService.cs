using Azure.Core;
using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;
using TutorLizard.BusinessLogic.Models.DTOs.Requests;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.BusinessLogic.Services;
public class StudentService : IStudentService
{
    private readonly IDbRepository<AdRequest> _adRequestRepository;
    public StudentService(IDbRepository<AdRequest> adRequestRepository)
    {
        _adRequestRepository = adRequestRepository;
    }
    public async Task<StudentsAdRequestsResponse> ViewAdRequests(StudentsAdRequestsRequest request)
    {
        var studentId = request.StudentId;

        var adRequests = await _adRequestRepository.GetAll()
            .Where(ar => ar.StudentId == studentId && ar.DateCreated != null)
            .ToListAsync();

        var adRequestsListDtos = adRequests
            .Select(ar => new AdRequestsListDto())
            .ToList();

        return new StudentsAdRequestsResponse
        {
            AdRequests = adRequestsListDtos
        };
    }
}
