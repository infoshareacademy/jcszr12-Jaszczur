using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorLizard.BusinessLogic.Models;

public class LessonRequest
{
    public int Id { get; set; }
    public int AdId { get; set; }
    public int ScheduleId { get; set; }
    public int UserId { get; set; }
    public bool IsAccepted {  get; set; }
}
