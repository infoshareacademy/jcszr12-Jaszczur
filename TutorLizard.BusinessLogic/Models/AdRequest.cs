using System;
namespace TutorLizard.BusinessLogic.Models;

public class AdRequest
{
    public AdRequest()
    {
    }

    public AdRequest(int id, int adId, int studentId, bool isAccepted, string message)
    {
        Id = id;
        AdId = adId;
        StudentId = studentId;
        IsAccepted = isAccepted;
        Message = message;
    }

    public int Id { get; set; }
    public int AdId { get; set; }
    public int StudentId { get; set; }
    public bool IsAccepted { get; set; }
    public string Message { get; set; }
}
