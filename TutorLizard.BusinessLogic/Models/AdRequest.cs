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
    public int UserId { get; set; } //nie jestem pewna czy to dajemy - AdId przechowuje Id tutora, jak dla mnie tutaj jest potrzebny też ID ucznia
    public string Message { get; set; }
}
