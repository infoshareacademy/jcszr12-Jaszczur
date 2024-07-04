using System.Text.Json;
using TutorLizard.Shared.Models.DTOs;
using TutorLizard.Shared.Models.DTOs.Responses;

namespace TutorLizard.Web.Extensions;

public static class ScheduleDtoExtensions
{
    public static string Serialize(this IEnumerable<StudentsScheduleItemSummaryDto> scheduleItemSummaryDtos)
    {
        return JsonSerializer.Serialize(scheduleItemSummaryDtos);
    }

    public static string Serialize(this IEnumerable<TutorsScheduleItemSummaryDto> scheduleItemSummaryDtos)
    {
        return JsonSerializer.Serialize(scheduleItemSummaryDtos);
    }
}
