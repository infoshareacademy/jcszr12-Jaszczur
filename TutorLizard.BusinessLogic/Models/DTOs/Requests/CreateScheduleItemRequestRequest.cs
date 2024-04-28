using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorLizard.BusinessLogic.Models.DTOs.Requests
{
    public class CreateScheduleItemRequestRequest
    {
        public int StudentId { get; set; }
        public int ScheduleItemRequestId { get; set; }
        public int ScheduleItemId { get; set; }
    }
}
