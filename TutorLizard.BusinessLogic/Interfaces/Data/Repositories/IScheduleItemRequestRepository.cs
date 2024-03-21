using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Interfaces.Data.Repositories
{
    public interface IScheduleItemRequestRepository
    {
        public ScheduleItemRequest CreateScheduleItemRequest(int scheduleItemId, int userId, bool isAccepted, bool isRemote);
        public void UpdateScheduleItemRequest(ScheduleItemRequest scheduleItemRequest);
        public void DeleteScheduleItemRequestById(int id);
        ScheduleItemRequest? GetScheduleItemRequestById(int id);
        public List<ScheduleItemRequest> GetAllScheduleItemRequests();
        public int GetNewScheduleItemRequestId();
    }
}
