using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorLizard.BusinessLogic.Models.DTOs.Responses
{
    public class TutorsAdsResponse
    {
        public List<AdListItemDto> AdList { get; set; } = new();
    }
}
