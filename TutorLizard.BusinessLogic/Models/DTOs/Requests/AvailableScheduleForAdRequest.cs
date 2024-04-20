using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorLizard.BusinessLogic.Models.DTOs.Requests
{
    public class AvailableScheduleForAdRequest
    {
        public int AdId { get; set; }
        public int StudentId { get; set; }
        public AvailableScheduleForAdRequest(int adId, int studentId)
        {
            AdId = adId;
            StudentId = studentId;
        }
    }
}
