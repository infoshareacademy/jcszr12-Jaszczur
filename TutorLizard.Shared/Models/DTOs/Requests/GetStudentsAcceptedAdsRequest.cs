using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorLizard.Shared.Models.DTOs.Requests
{
    public class GetStudentsAcceptedAdsRequest
    {
        public int StudentId { get; set; }
        public GetStudentsAcceptedAdsRequest(int? studentId)
        {
            StudentId = (int)studentId;
        }
    }
}
