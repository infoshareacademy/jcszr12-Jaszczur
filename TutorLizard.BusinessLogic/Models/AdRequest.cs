using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorLizard.BusinessLogic.Models;

public class AdRequest
{
    public int Id { get; set; }
    public int AdId { get; set; }
    public int StudentId { get; set; }
    public bool IsAccepted { get; set; }
    public string Message { get; set; }
}
