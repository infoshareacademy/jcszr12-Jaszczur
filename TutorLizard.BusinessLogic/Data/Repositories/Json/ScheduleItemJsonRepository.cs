﻿using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Options;

namespace TutorLizard.BusinessLogic.Data.Repositories.Json
{
    public class ScheduleItemJsonRepository : JsonRepositoryBase<ScheduleItem>, IScheduleItemRepository
    {
        public ScheduleItemJsonRepository(IOptions<DataJsonFilePaths> options) : base(options.Value.ScheduleItems)
        {
        }

        public ScheduleItem CreateScheduleItem(int adId, DateTime dateTime)
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

        public void DeleteScheduleItemById(int id)
        {
            var toDelete = GetScheduleItemById(id);
            if (toDelete == null)
            {
                return;
            }

            _data.Remove(toDelete);
        }

        public ScheduleItem? GetScheduleItemById(int id)
        {
            var scheduleItem = _data.FirstOrDefault(s => s.Id == id);
            return scheduleItem;
        }

        public List<ScheduleItem> GetAllScheduleItems()
        {
            return _data;
        }

        public int GetNewScheduleItemId()
        {
            if (_data.Any())
            {
                return _data.Max(s => s.Id) + 1;
            }

            return 1;
        }
    }
}
