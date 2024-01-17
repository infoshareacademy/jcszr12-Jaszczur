using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorLizard.BusinessLogic.Models;

public class Schedule
{
    public int ScheduleId { get; set; }
    public int TutorId { get; set; }
    public int StudentId { get; set; }
    public DateTime DateTime { get; set; }
}
