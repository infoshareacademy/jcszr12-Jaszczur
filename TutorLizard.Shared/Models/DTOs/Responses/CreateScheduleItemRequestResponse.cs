using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorLizard.Shared.Models.DTOs.Responses
{
    public class CreateScheduleItemRequestResponse
    {
        public bool Success { get; set; }
        public int CreatedScheduleItemRequestId { get; set; }
        public bool IsRemote { get; set; }
    }
}
