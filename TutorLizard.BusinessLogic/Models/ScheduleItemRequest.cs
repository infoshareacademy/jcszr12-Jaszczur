using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorLizard.BusinessLogic.Models;

public class ScheduleItemRequest
{
    public int Id { get; set; }
    public int ScheduleItemId { get; set; }
    public int UserId { get; set; }
    public bool IsAccepted {  get; set; }
}
