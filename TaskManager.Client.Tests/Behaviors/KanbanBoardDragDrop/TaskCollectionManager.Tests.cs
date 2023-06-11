using TaskManager.Client.Behaviors.KanbanBoardDragDrop;
using Task = TaskManager.Core.Models.Task;

namespace TaskManager.Client.Tests.Behaviors.KanbanBoardDragDrop;

[TestFixture]
internal class TaskCollectionManagerTests
{
    private TaskCollectionManager _sut = null!;
    private List<Task> _tasks = null!;

    [SetUp]
    public void SetUp()
    {
        _tasks = new();
        AddTask(1, 10);
        AddTask(2, 20);
        AddTask(3, 30);

        _sut = new TaskCollectionManager();
        _sut.Setup(_tasks);
    }

    [Test]
    public void AddTask_ShouldAddTaskWithoutChangingOrderValues()
    {
        // GIVEN
        var task = new Task
        {
            Id = 4,
            OrderValue = 11
        };

        // WHEN
        _sut.AddTask(task, updateOrder: false);

        // THEN
        var collection = _tasks.OrderBy(x => x.OrderValue).ToList();
        Assert.Multiple(() =>
        {
            ValidateTask(collection[0], 1, 10);
            ValidateTask(collection[1], 4, 11);
            ValidateTask(collection[2], 2, 20);
            ValidateTask(collection[3], 3, 30);
        });
    }

    [Test]
    public void AddTask_ShouldAddTaskAndUpdateOrderValues()
    {
        // GIVEN
        var task = new Task
        {
            Id = 4,
            OrderValue = 11
        };

        // WHEN
        _sut.AddTask(task, updateOrder: true);

        // THEN
        var collection = _tasks.OrderBy(x => x.OrderValue).ToList();
        Assert.Multiple(() =>
        {
            ValidateTask(collection[0], 1, 10);
            ValidateTask(collection[1], 4, 20);
            ValidateTask(collection[2], 2, 30);
            ValidateTask(collection[3], 3, 40);
        });
    }

    [Test]
    public void RemoveTask_ShouldRemoveTaskWithoutChangingOrderValues()
    {
        // GIVEN
        var task = _tasks.First(x => x.Id == 2);

        // WHEN
        _sut.RemoveTask(task, updateOrder: false);

        // THEN
        var collection = _tasks.OrderBy(x => x.OrderValue).ToList();
        Assert.Multiple(() =>
        {
            ValidateTask(collection[0], 1, 10);
            ValidateTask(collection[1], 3, 30);
        });
    }

    [Test]
    public void RemoveTask_ShouldRemoveTaskAndUpdateOrderValues()
    {
        // GIVEN
        var task = _tasks.First(x => x.Id == 2);

        // WHEN
        _sut.RemoveTask(task, updateOrder: true);

        // THEN
        var collection = _tasks.OrderBy(x => x.OrderValue).ToList();
        Assert.Multiple(() =>
        {
            ValidateTask(collection[0], 1, 10);
            ValidateTask(collection[1], 3, 20);
        });
    }

    private void AddTask(int id, int orderValue)
    {
        _tasks.Add(new Task
        {
            Id = id,
            OrderValue = orderValue
        });
    }

    private static void ValidateTask(Task task, int id, int orderValue)
    {
        Assert.That(task.Id, Is.EqualTo(id));
        Assert.That(task.OrderValue, Is.EqualTo(orderValue));
    }
}
