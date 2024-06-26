namespace TutorLizard.Shared.Models.DTOs;
public class AdListItemDto
{
    public int Id { get; set; }
    public int TutorId { get; set; }
    public string TutorName { get; set; }
    public string Subject { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public decimal Price { get; set; }
    public string Location { get; set; }
    public bool IsRemote { get; set; }

    public AdListItemDto(int id,
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
        Id = id;
        TutorId = tutorId;
        TutorName = tutorName;
        Subject = subject;
        Title = title;
        Description = description;
        CategoryId = categoryId;
        CategoryName = categoryName;
        Price = price;
        Location = location;
        IsRemote = isRemote;
    }
    public AdListItemDto()
    {
        
    }
}
