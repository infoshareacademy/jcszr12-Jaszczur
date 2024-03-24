using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Interfaces.Data.Repositories
{
    public interface IScheduleItemRepository
    {
        public ScheduleItem CreateScheduleItem(int adId, DateTime dateTime);
        public void UpdateScheduleItem(ScheduleItem scheduleItem);
        public void DeleteScheduleItemById(int id);
        ScheduleItem? GetScheduleItemById(int id);
        public List<ScheduleItem> GetAllScheduleItems();
        public int GetNewScheduleItemId();
    }
}
