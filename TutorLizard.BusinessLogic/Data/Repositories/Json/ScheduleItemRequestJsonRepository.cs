﻿using Microsoft.Extensions.Options;
using TutorLizard.BusinessLogic.Interfaces.Data.Repositories;
using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Options;

namespace TutorLizard.BusinessLogic.Data.Repositories.Json
{
    public class ScheduleItemRequestJsonRepository : JsonRepositoryBase<ScheduleItemRequest>, IScheduleItemRequestRepository
    {
        public ScheduleItemRequestJsonRepository(IOptions<DataJsonFilePaths> options) : base(options.Value.ScheduleItemRequests)
        {
        }

        public ScheduleItemRequest CreateScheduleItemRequest(int scheduleItemId, int userId, bool isAccepted, bool isRemote)
        {
            ScheduleItemRequest newScheduleItemRequest = new(GetNewScheduleItemRequestId(), scheduleItemId, userId, isAccepted, isRemote);
            _data.Add(newScheduleItemRequest);
            SaveToJson();

            return newScheduleItemRequest;
        }

        public void DeleteScheduleItemRequestById(int id)
        {
            var toDelete = GetScheduleItemRequestById(id);
            if (toDelete == null)
            {
                return;
            }

            _data.Remove(toDelete);
            SaveToJson();
        }

        public List<ScheduleItemRequest> GetAllScheduleItemRequests()
        {
            return _data;
        }

        public int GetNewScheduleItemRequestId()
        {
            if (_data.Any())
            {
                return _data.Max(s => s.Id) + 1;
            }

            return 1;
        }

        public ScheduleItemRequest? GetScheduleItemRequestById(int id)
        {
            var scheduleItemRequest = _data.FirstOrDefault(s => s.Id == id);
            return scheduleItemRequest;
        }

        public void UpdateScheduleItemRequest(ScheduleItemRequest scheduleItemRequest)
        {
            var toUpdate = GetScheduleItemRequestById(scheduleItemRequest.Id);
            if (toUpdate == null)
            {
                return;
            }

            toUpdate.Id = scheduleItemRequest.Id;
            toUpdate.ScheduleItemId = scheduleItemRequest.ScheduleItemId;
            toUpdate.StudentId = scheduleItemRequest.StudentId;
            toUpdate.IsAccepted = scheduleItemRequest.IsAccepted;
            toUpdate.IsRemote = scheduleItemRequest.IsRemote;

            SaveToJson();
        }
    }
}
