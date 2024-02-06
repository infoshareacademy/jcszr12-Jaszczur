using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorLizard.BusinessLogic.Models;

public class ScheduleItem
{
    public ScheduleItem(int id, int adId, int studentId, DateTime dateTime)
    {
        Id = id;
        AdId = adId;
        StudentId = studentId;
        DateTime = dateTime;
    }

    public int Id { get; set; }
    public int AdId { get; set; }
    public int StudentId { get; set; }
    public DateTime DateTime { get; set; }
}