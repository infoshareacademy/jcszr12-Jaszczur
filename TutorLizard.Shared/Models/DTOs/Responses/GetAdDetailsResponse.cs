using TutorLizard.Shared.Enums;

namespace TutorLizard.Shared.Models.DTOs.Responses;
public class GetAdDetailsResponse
{
    public int AdId { get; set; }
    public int TutorId { get; set; }
    public string TutorName { get; set; }
    public string Title { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string Subject { get; set; }
    public string Location { get; set; }
    public decimal Price { get; set; }
    public bool IsRemote { get; set; }
    public string Description { get; set; }
    public AdToUserRelationship UserRelationship { get; set; }
}
