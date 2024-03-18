using System.Security.Cryptography.X509Certificates;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;

namespace TutorLizard.BusinessLogic.Data.Repositories
{
    public class ScheduleItemJsonRepository : RepositoryBase<ScheduleItem>, IScheduleItemRepository
    {
        protected override string FilePath => "Data/scheduleItems.json";
        private List<ScheduleItem> _scheduleItems = new();

        public ScheduleItemJsonRepository() 
        {
            LoadFromJson();
        }

        public ScheduleItem CreateScheduleItem(int id, int  adId, DateTime dateTime)
        {
            ScheduleItem newScheduleItem = new(GetNewScheduleItemId(), adId, dateTime);
            _data.Add(newScheduleItem);
            SaveToJson();

            return newScheduleItem;
        }

        public void UpdateScheduleItem(ScheduleItem scheduleItem)
        {
            var toUpdate = GetScheduleItemById(scheduleItem.Id);
            if (toUpdate == null)
            {
                return;
            }

            toUpdate.Id = scheduleItem.Id;
            toUpdate.AdId = scheduleItem.AdId;
            toUpdate.DateTime = scheduleItem.DateTime;

            SaveToJson();
        }

        public void DeleteScheduleItem(int id)
        {
            var toDelete = GetScheduleItemById(id);
            if (toDelete == null)
            {
                return;
            }

            _scheduleItems.Remove(toDelete);
        }

        public ScheduleItem? GetScheduleItemById(int id)
        {
            var scheduleItem = _scheduleItems.FirstOrDefault(s => s.Id == id);
            return scheduleItem;
        }

        public List<ScheduleItem> GetAllScheduleItems()
        {
            return _scheduleItems;
        }

        public int GetNewScheduleItemId()
        {
            if (_scheduleItems.Any())
            {
                return _scheduleItems.Max(s => s.Id) + 1;
            }

            return 1;
        }
    }
}
