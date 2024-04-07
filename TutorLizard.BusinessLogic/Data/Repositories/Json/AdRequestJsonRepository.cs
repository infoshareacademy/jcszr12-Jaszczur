using Microsoft.Extensions.Options;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Options;

namespace TutorLizard.BusinessLogic.Data.Repositories.Json;

public class AdRequestJsonRepository: JsonRepositoryBase<AdRequest>, IAdRequestRepository
{

    public AdRequestJsonRepository(IOptions<DataJsonFilePaths> options) : base(options.Value.AdRequests)
    {
        
    }
    public AdRequest CreateAdRequest(int adId,
                     int studentId,
                     string message,
                     bool isRemote)
    {
        AdRequest newAdRequest = new AdRequest(GetNewAdRequestID(),
                                           adId,
                                           studentId,
                                           message,
                                           isRemote,
                                           false);
        _data.Add(newAdRequest);
        SaveToJson();

        return newAdRequest;
    }
    public List<AdRequest> GetAllAdRequests()
    {
        return _data;
    }
    public AdRequest? GetAdRequestById(int adRequestId)
    {
        var adRequest = _data.FirstOrDefault(ar => ar.Id == adRequestId);
        return adRequest;
    }
    public void UpdateAdRequest(AdRequest adRequest)
    {
        var toUpdate = GetAdRequestById(adRequest.Id);
        if (toUpdate is null)
            return;

        toUpdate.AdId = adRequest.AdId;
        toUpdate.StudentId = adRequest.StudentId;
        toUpdate.IsAccepted = adRequest.IsAccepted;
        toUpdate.Message = adRequest.Message;
        toUpdate.IsRemote = adRequest.IsRemote;
        toUpdate.IsRemote = adRequest.IsRemote;

        SaveToJson();
    }
    public void DeleteAdRequestById(int adRequestId)
    {
        var toDelete = GetAdRequestById(adRequestId);
        if (toDelete is null)
            return;

        _data.Remove(toDelete);

        SaveToJson();
    }
    private int GetNewAdRequestID()
    {
        if (_data.Any() == true)
            return _data.Max(x => x.Id) + 1;
        else
            return 1;
    }
}
