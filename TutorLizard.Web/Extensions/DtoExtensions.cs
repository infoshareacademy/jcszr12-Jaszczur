using System.Text.Json;
using TutorLizard.BusinessLogic.Models.DTOs;

namespace TutorLizard.Web.Extensions;

public static class DtoExtensions
{
    public static string ToBase64String(this AdSearchCriteriaDto searchCriteria)
    {
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(searchCriteria);
        return Convert.ToBase64String(bytes);
    }

    public static AdSearchCriteriaDto? ToAdSearchCriteriaDto(this string base64String)
    {
        try
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            return JsonSerializer.Deserialize<AdSearchCriteriaDto>(bytes);
        }
        catch
        {
            return null;
        }
    }
}
