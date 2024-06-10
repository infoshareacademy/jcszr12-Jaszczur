using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorLizard.Shared.Models.DTOs.Responses
{
    public class GetStudentsAcceptedAdsResponse
    {
        public List<AdListItemDto> Ads { get; set; } = new();
    }
}
