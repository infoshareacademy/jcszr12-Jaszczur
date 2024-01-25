using Newtonsoft.Json;
using System.Text.Json;

namespace TutorLizard.BusinessLogic.Data;
public class DataAccess : IUserIdentityDataAccess, IStudentDataAccess, ITutorDataAccess
{
    // This class will handle loading and saving data.

    public static void JsonDataAdder<T>(T objectData)
    {
        var filePath = $@"{Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName}\{typeof(T).Name}.json";

        var jsonData = System.IO.File.ReadAllText(filePath);
        var objectList = JsonConvert.DeserializeObject<List<T>>(jsonData)
                ?? new List<T>();

        objectList.Add(objectData);
        jsonData = JsonConvert.SerializeObject(objectList);
        System.IO.File.WriteAllText(filePath, jsonData);
    }
}
