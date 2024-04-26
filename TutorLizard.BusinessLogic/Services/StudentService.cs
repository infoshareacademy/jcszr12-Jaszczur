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
    private readonly IDbRepository<Ad> _adRepository;
    private readonly IDbRepository<AdRequest> _adRequestRepository;
    public StudentService(IDbRepository<Ad> adRepository,
                          IDbRepository<AdRequest> adRequestRepository)
    {
        _adRepository = adRepository;
        _adRequestRepository = adRequestRepository;
    }

    public async Task<StudentsAcceptedAdsResponse> ViewAcceptedAds(StudentsAcceptedAdsRequest request)
    {
        var studentId = request.StudentId;

        var acceptedAdRequests = await _adRequestRepository.GetAll()
            .Where(ar =>  ar.StudentId == studentId && ar.IsAccepted)
            .ToListAsync();

        var acceptedAds = acceptedAdRequests
            .Select(ar => ar.Ad).ToList();

        var adListDtos = acceptedAds
            .Select(ad => new AdListItemDto())
            .ToList();

        return new StudentsAcceptedAdsResponse
        {
            Ads = adListDtos
        };
    }
}
