using Microsoft.EntityFrameworkCore;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Interfaces.Services;
using TutorLizard.BusinessLogic.Models;
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

    public async Task<CreateAdRequestResponse> CreateAdRequest(CreateAdRequestRequest request)
    {
        Ad? ad = await _adRepository.GetById(request.AdId);

        if (ad is null)
        {
            return new()
            {
                Success = false
            };
        }

        bool userIsOwnerOfAd = ad.TutorId == request.StudentId;

        if (userIsOwnerOfAd)
        {
            return new()
            {
                Success = false
            };
        }

        var userAlreadySentRequest = await _adRequestRepository.GetAll()
            .AnyAsync(r => r.StudentId == request.StudentId);

        if (userAlreadySentRequest)
        {
            return new()
            {
                Success = false
            };
        }

        AdRequest toCreate = new()
        {
            AdId = request.AdId,
            IsAccepted = false,
            IsRemote = request.IsRemote,
            Message = request.Message,
            StudentId = request.StudentId
        };

        try
        {
            await _adRequestRepository.Create(toCreate);
        }
        catch
        {
            return new()
            {
                Success = false
            };
        }

        return new()
        {
            Success = true
        };
    }
}
