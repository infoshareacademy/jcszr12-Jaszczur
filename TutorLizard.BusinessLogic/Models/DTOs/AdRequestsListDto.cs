namespace TutorLizard.BusinessLogic.Models.DTOs;

public class AdRequestsListDto
{
    public int Id { get; set; }
    public int StudentId { get; set;}
    public int AdId { get; set;}
    public bool IsAccepted { get; set; }
    public string Message { get; set; }
    public string? ReplyMessage { get; set; }
    public bool IsRemote { get; set; }
    public string AdTitle { get; set; }
    public string AdSubject { get; set; }
    public string CategoryName { get; set; }

    public AdRequestsListDto()
    {

    }

    public AdRequestsListDto(int id,
                                   int studentId,
                                   int adId,
                                   bool isAccepted,
                                   string message,
                                   string replyMessage,
                                   bool isRemote,
                                   string adTitle,
                                   string adSubject,
                                   string categoryName)
    {
        Id = id;
        StudentId = studentId;
        AdId = adId;
        IsAccepted = isAccepted;
        Message = message;
        ReplyMessage = replyMessage;
        IsRemote = isRemote;
        AdTitle = adTitle;
        AdSubject = adSubject;
        CategoryName = categoryName;
    }
}
