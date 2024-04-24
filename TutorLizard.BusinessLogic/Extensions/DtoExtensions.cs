using TutorLizard.BusinessLogic.Models;
using TutorLizard.BusinessLogic.Models.DTOs;

namespace TutorLizard.BusinessLogic.Extensions;
public static class DtoExtensions
{
    private static readonly ScheduleItemDto.ScheduleItemRequestStatus status;

    public static AdDto ToDto(this Ad ad) => new AdDto(ad);
    public static AdRequestDto ToDto(this AdRequest adRequest) => new AdRequestDto(adRequest);
    public static CategoryDto ToDto(this Category category) => new CategoryDto(category);
    public static ScheduleItemDto ToDto(this ScheduleItem scheduleItem) => new ScheduleItemDto(scheduleItem, status);
    public static ScheduleItemRequestDto ToDto(this ScheduleItemRequest scheduleItemRequest) => new ScheduleItemRequestDto(scheduleItemRequest);
    public static UserDto ToDto(this User user) => new UserDto(user);
}
