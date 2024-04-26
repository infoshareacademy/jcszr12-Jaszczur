using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorLizard.BusinessLogic.Models.DTOs.Responses;

namespace TutorLizard.BusinessLogic.Models.DTOs.Requests
{
    public class TutorsAdsRequest
    {
        public int TutorId { get; set; }

        public TutorsAdsRequest(int? tutorId)
        {
            TutorId = (int)tutorId;
        }
    }
}
