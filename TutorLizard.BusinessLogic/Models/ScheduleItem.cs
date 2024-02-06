using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutorLizard.BusinessLogic.Models;

public class ScheduleItem
{
    public ScheduleItem()
    {
    }

    public ScheduleItem(int adId, DateTime dateTime)
    {
        AdId = adId;
        DateTime = dateTime;
    }

    public int Id { get; set; }
    public int AdId { get; set; }
    public DateTime DateTime { get; set; }
}
