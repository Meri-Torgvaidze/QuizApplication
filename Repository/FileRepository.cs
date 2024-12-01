using Models;
using System.Security.Principal;
using System.Text.Json;

namespace Repository
{
    public class FileRepository<T>
    {
        private readonly string _filePath;

        public FileRepository(string filePath)
        {
            _filePath = filePath;
        }

        public List<T> ReadAll()
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }

        public void SaveAll(List<T> items)
        {
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }


        public void Save(T item)
        {
            var items = ReadAll();
            items.Add(item);
            SaveAll(items);
        }
    }
}
