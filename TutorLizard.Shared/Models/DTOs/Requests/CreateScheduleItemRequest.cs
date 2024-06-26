using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorLizard.Shared.Models.DTOs.Requests
{
    public class CreateScheduleItemRequest
    {
        public CreateScheduleItemRequest()
        {
        }

        public CreateScheduleItemRequest(int adId,
                                         int userId,
                                         DateTime dateTime)
        {
            AdId = adId;
            UserId = userId;
            DateTime = dateTime;
        }

        public int AdId { get; set; }
        public int UserId { get; set; }

        [DisplayName("Data")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        public DateTime DateTime { get; set; }

    }
}
