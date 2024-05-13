using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorLizard.BusinessLogic.Models.DTOs
{
    public class StudentScheduleItemRequestDto
    {
        public int Id { get; set; }
        public bool IsAccepted {  get; set; }
        public DateTime DateCreated { get; set; }
    }
}
