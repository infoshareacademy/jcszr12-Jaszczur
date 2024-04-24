using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorLizard.BusinessLogic.Models.DTOs.Responses
{
    public class AvailableScheduleForAdResponse
    {
        public List<ScheduleItemDto> Items { get; set; } = new();
        public bool IsAccepted { get; set; } = true;
    }
}
