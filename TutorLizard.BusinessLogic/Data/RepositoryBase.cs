using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using TutorLizard.BusinessLogic.Models;
using System.Diagnostics;

namespace TutorLizard.BusinessLogic.Data
{
    public abstract class RepositoryBase<T>
    {
        protected List<T> _data = new();
        protected abstract string FilePath { get; }

        protected void LoadFromJson()
        {
            string filePath = Path.Combine(FilePath.Split('/'));
            string fullPath;
            if (Path.IsPathRooted(filePath))
                fullPath = filePath;
            else
                fullPath = Path.Combine(AppContext.BaseDirectory, filePath);

            if (!File.Exists(fullPath))
                return;

            var jsonData = File.ReadAllText(fullPath);

            var outputList = JsonSerializer.Deserialize<List<T>>(jsonData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                IncludeFields = true
            })
                        ?? new List<T>();
            _data = outputList;
        }


        protected void SaveToJson()
        {
            string filePath = Path.Combine(FilePath.Split('/');
            string fullPath;
            if (Path.IsPathRooted(filePath))
                fullPath = filePath;
            else
                fullPath = Path.Combine(AppContext.BaseDirectory, filePath);

            string? directoryPath = Path.GetDirectoryName(fullPath);

            var jsonData = JsonSerializer.Serialize(_data, new JsonSerializerOptions
            {
                WriteIndented = true,
            });

            if (directoryPath is not null && Directory.Exists(directoryPath) == false)
                Directory.CreateDirectory(directoryPath);

            File.WriteAllText(fullPath, jsonData);
        }     
    }
}
