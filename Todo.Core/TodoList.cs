using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
namespace Todo.Core
{
    public class TodoList
    {
        private readonly List<TodoItem> _items = new();
        public IReadOnlyList<TodoItem> Items => _items.AsReadOnly();
        public TodoItem Add(string title)
        {
            var item = new TodoItem(title);
            _items.Add(item);
            return item;
        }
        public bool Remove(Guid id) => _items.RemoveAll(i => i.Id == id) > 0;
        public IEnumerable<TodoItem> Find(string substring) =>
        _items.Where(i => i.Title.Contains(substring ?? string.Empty,
        StringComparison.OrdinalIgnoreCase));
        public int Count => _items.Count;

        public void Save(string path)
        {
            string json = JsonSerializer.Serialize(_items, new JsonSerializerOptions{WriteIndented = true});
            File.WriteAllText(path, json);
        }

        public void Load(string path)
        {
            if(!File.Exists(path))
                return;

            string json = File.ReadAllText(path);
            var items = JsonSerializer.Deserialize<List<TodoItem>>(json);
             if(items != null)
            {
                _items.Clear();
                _items.AddRange(items);
            }
        }
    }
}