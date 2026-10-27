using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;

namespace Todo.Core.Tests;

public class TodoListTests
{
    [Fact]
    public void Add_IncreasesCount()
    {
        var list = new TodoList();
        list.Add(" task ");
        Assert.Equal(1, list.Count);
        Assert.Equal("task", list.Items.First().Title);
    }
    [Fact]
    public void Remove_ById_Works()
    {
        var list = new TodoList();
        var item = list.Add("a");
        Assert.True(list.Remove(item.Id));
        Assert.Equal(0, list.Count);
        }
    [Fact]
    public void Find_ReturnsMatches()
    {
        var list = new TodoList();
        list.Add("Buy milk");
        list.Add("Read book");
        var found = list.Find("buy").ToList();
        Assert.Single(found);
        Assert.Equal("Buy milk", found[0].Title);
    }

    [Fact]
    public void Save_and_Load_WorkCheck()
    {
        string filePath = Path.GetTempFileName();
        var list = new TodoList();
        list.Add("hi");
        list.Add("buy book");
        var item = list.Add("to me");
        item.MarkDone();

        list.Save(filePath);

        var loadedData = new TodoList();
        loadedData.Load(filePath);

        Assert.Equal(list.Count, loadedData.Count);
        Assert.Equal("buy book", loadedData.Items[1].Title);
        Assert.True(loadedData.Items[2].IsDone);
    }

}