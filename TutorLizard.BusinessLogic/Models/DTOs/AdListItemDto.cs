namespace TutorLizard.BusinessLogic.Models.DTOs;
public class AdListItemDto(int id,
                                       int tutorId,
                                       string tutorName,
                                       string subject,
                                       string title,
                                       string description,
                                       int categoryId,
                                       string categoryName,
                                       decimal price,
                                       string location,
                                       bool isRemote)
{
    public int Id { get; set; } = id;
    public int TutorId { get; set; } = tutorId;
    public string TutorName { get; set; } = tutorName;
    public string Subject { get; set; } = subject;
    public string Title { get; set; } = title;
    public string Description { get; set; } = description;
    public int CategoryId { get; set; } = categoryId;
    public string CategoryName { get; set; } = categoryName;
    public decimal Price { get; set; } = price;
    public string Location { get; set; } = location;
    public bool IsRemote { get; set; } = isRemote;
}
