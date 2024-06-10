using TutorLizard.BusinessLogic.Models;
using TutorLizard.Shared.Models.DTOs;

namespace TutorLizard.BusinessLogic.Extensions;
public static class DtoExtensions
{
    public static CategoryDto ToDto(this Category category)
        => new CategoryDto(category.Id,
                           category.Name,
                           category.Description);

    public static UserDto ToDto(this User user)
        => new UserDto(user.Id,
                       user.Name,
                       user.UserType,
                       user.Email,
                       user.DateCreated);
}
