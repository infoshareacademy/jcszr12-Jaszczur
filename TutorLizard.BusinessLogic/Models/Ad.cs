using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorLizard.BusinessLogic.Models;

public class Ad
{
    public Ad(int id, int tutorId, string subject, string title, string description)
    {
        Id = id;
        TutorId = tutorId;
        Subject = subject;
        Title = title;
        Description = description;
    }

    public int Id { get; set; }
    public int TutorId { get; set; }
    public string Subject { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}