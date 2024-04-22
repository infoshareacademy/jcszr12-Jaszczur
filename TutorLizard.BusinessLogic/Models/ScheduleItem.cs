using System.ComponentModel.DataAnnotations;

namespace TutorLizard.BusinessLogic.Models;

public class ScheduleItem
{
    public ScheduleItem()
    {
    }

    public ScheduleItem(int id, int adId, DateTime dateTime)
    {
        Id = id;
        AdId = adId;
        DateTime = dateTime;
    }

    public int Id { get; set; }
    public int AdId { get; set; }
    [Required]
    public DateTime DateTime { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;

    // EntityFramework
    public Ad Ad { get; set; }
    public ICollection<ScheduleItemRequest> ScheduleItemRequests { get; set; } = new List<ScheduleItemRequest>();
}