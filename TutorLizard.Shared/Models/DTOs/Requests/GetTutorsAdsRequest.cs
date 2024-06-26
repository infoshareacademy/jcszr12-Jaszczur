using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorLizard.Shared.Models.DTOs.Responses;

namespace TutorLizard.Shared.Models.DTOs.Requests
{
    public class GetTutorsAdsRequest
    {
        public int TutorId { get; set; }

        public GetTutorsAdsRequest(int tutorId)
        {
            TutorId = tutorId;
        }
    }
}
