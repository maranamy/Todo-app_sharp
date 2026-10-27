// See https://aka.ms/new-console-template for more information
using Todo.Core;

string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tasks.json");

var todoList = new TodoList();

if (File.Exists(filePath)){
    todoList.Load(filePath);
    Console.WriteLine($"Информация загружена из {filePath}");
}

bool exit = false;
while (!exit)
{
    Console.WriteLine("\n==================================");
    Console.WriteLine("    УПРАВЛЕНИЕ СПИСКОМ ЗАДАЧ");
    Console.WriteLine("==================================");
    Console.WriteLine("1. Показать все задачи");
    Console.WriteLine("2. Добавить задачу");
    Console.WriteLine("3. Отметить задачу как выполненную");
    Console.WriteLine("4. Переименовать задачу");
    Console.WriteLine("5. Удалить задачу");
    Console.WriteLine("6. Найти задачу по тексту");
    Console.WriteLine("7. Сохранить в JSON");
    Console.WriteLine("8. Загрузить из JSON");
    Console.WriteLine("0. Выход");
    Console.Write("Выберите действие: ");

    string? choice = Console.ReadLine();
    Console.WriteLine();

    switch (choice)
    {
        case "1": ShowAllTasks(todoList);
        break;
        case "2": AddNewTask(todoList);
        break;
        case "3": TaskMarkDone(todoList);
        break;
        case "4": RenameTask(todoList);
        break;
        case "5": DeleteTask(todoList);
        break;
        case "6": FindTask(todoList);
        break;
        case "7": todoList.Save(filePath);
        Console.WriteLine($"Информация выгружена в {filePath}");
        break;
        case "8": todoList.Load(filePath);
        Console.WriteLine($"Информация загружена из {filePath}");
        break;
        case "0": todoList.Load(filePath);
        exit = true;
        Console.WriteLine("Изменения созранены. Выход из программы ... ");
        break;
    }
}

void ShowAllTasks(TodoList list)
{
    if(list.Count == 0)
    {
        Console.WriteLine("Список пуст!");
        return;
    }

    Console.WriteLine("Список задач:");
    for (int i = 0; i < list.Items.Count; i++)
    {
        var item = list.Items[i];
        Console.WriteLine($"{i + 1}. {(item.IsDone ? "[X]" : "[ ]")} {item.Title} (ID: {item.Id})");
    }
}

void AddNewTask(TodoList list)
{
    Console.Write("Введите название задачи: ");
    string? title = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(title))
    {
        Console.WriteLine("Название задачи не может быть пустым.");
        return;
    }

    var item = list.Add(title);
    Console.WriteLine($"Добавлена задача: \"{item.Title}\"");
}

void TaskMarkDone(TodoList list)
{
    ShowAllTasks(list);
    Console.Write("Введите номер задачи для отметки: ");

    if (int.TryParse(Console.ReadLine(), out int index ) && index > 0 && index <= list.Count)
    {
        var item = list.Items[index-1];
        item.MarkDone();
        Console.WriteLine($"Отмечена задача: \"{item.Id}. {item.Title}\"");
    }
    else
    {
        Console.WriteLine("Неверный номер задачи.");
    }
}

void RenameTask(TodoList list)
{
    ShowAllTasks(list);
    if (list.Count == 0) return;

    Console.Write("\nВведите номер задачи для переименования: ");
    if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= list.Count)
    {
        var item = list.Items[index - 1];
        Console.Write("Введите новое название задачи: ");
        string? newTitle = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(newTitle))
        {
            item.Rename(newTitle);
            Console.WriteLine("[Успех] Название задачи обновлено.");
        }
        else
        {
            Console.WriteLine("[Ошибка] Новое название не может быть пустым.");
       }
        }
        else
       {
           Console.WriteLine("[Ошибка] Неверный номер задачи.");
        }
}

void DeleteTask(TodoList list)
{
    ShowAllTasks(list);
    if (list.Count == 0) return;

    Console.Write("\nВведите номер задачи для удаления: ");
    if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= list.Count)
    {
        var item = list.Items[index - 1];
        if (list.Remove(item.Id))
        {
            Console.WriteLine($"[Успех] Задача \"{item.Title}\" удалена.");
        }
    }
    else Console.WriteLine("[Ошибка] Неверный номер задачи.");
}

void FindTask(TodoList list)
{
    Console.Write("Введите текст для поиска: ");
    string? query = Console.ReadLine();

    var matches = list.Find(query ?? string.Empty);
    Console.WriteLine("\nНайденные задачи:");
    int count = 0;
    foreach (var item in matches)
    {
        string status = item.IsDone ? "[X]" : "[ ]";
        Console.WriteLine($"- {status} {item.Title}");
        count++;
     }

        if (count == 0)
        {
            Console.WriteLine("Совпадений не найдено.");
        }       
}