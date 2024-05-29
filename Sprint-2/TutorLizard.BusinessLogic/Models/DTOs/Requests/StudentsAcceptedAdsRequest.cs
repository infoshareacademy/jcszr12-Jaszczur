using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorLizard.BusinessLogic.Models.DTOs.Requests
{
    public class StudentsAcceptedAdsRequest
    {
        public int StudentId { get; set; }
        public StudentsAcceptedAdsRequest(int? studentId)
        {
            StudentId = (int)studentId;
        }
    }
}
