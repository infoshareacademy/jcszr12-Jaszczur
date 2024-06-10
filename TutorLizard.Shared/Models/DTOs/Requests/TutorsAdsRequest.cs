using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorLizard.Shared.Models.DTOs.Responses;

namespace TutorLizard.Shared.Models.DTOs.Requests
{
    public class TutorsAdsRequest
    {
        public int TutorId { get; set; }

        public TutorsAdsRequest(int tutorId)
        {
            TutorId = tutorId;
        }
    }
}
