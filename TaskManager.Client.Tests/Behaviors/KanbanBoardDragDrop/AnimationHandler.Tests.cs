using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.Windows.Input;
using TaskManager.Client.Behaviors.KanbanBoardDragDrop;
using TaskManager.Client.Behaviors;
using TaskManager.Client.View.Kanban;
using Microsoft.Extensions.Logging;
using TaskManager.Core.Models;
using Task = TaskManager.Core.Models.Task;
using System.Windows.Controls;
using TaskManager.Core.Utils;
using System.Windows;

namespace TaskManager.Client.Tests.Behaviors.KanbanBoardDragDrop;

[TestFixture]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
[Apartment(ApartmentState.STA)]
internal class AnimationHandlerTests
{
    private Mock<ILogger<AnimationHandler>> _loggerMock = null!;
    private Mock<IViewService> _viewServiceMock = null!;

    private Mock<AnimationHandler> _sutMock = null!;
    private AnimationHandler _sut = null!;

    private List<KanbanTask> _kabanTaskList = new List<KanbanTask>();

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<AnimationHandler>>();
        _viewServiceMock = new Mock<IViewService>(MockBehavior.Strict);

        App.IoC = new Ioc();
        App.IoC.ConfigureServices(new ServiceCollection()
            .AddTransient(provider => _loggerMock.Object)
            .AddTransient(provider => _viewServiceMock.Object)
            .BuildServiceProvider());

        _sutMock = new Mock<AnimationHandler>(MockBehavior.Strict);
        _sutMock.CallBase = true;
        
        _sut = _sutMock.Object;
        _sut.Setup(_viewServiceMock.Object);
        
        AddKanbanTaskToList(ETaskStatus.Waiting, 10, 1);
        AddKanbanTaskToList(ETaskStatus.Waiting, 20, 2);
        AddKanbanTaskToList(ETaskStatus.Waiting, 30, 3);
        AddKanbanTaskToList(ETaskStatus.InProgress, 10, 4);
        AddKanbanTaskToList(ETaskStatus.InProgress, 20, 5);
        AddKanbanTaskToList(ETaskStatus.InProgress, 30, 6);
        AddKanbanTaskToList(ETaskStatus.Completed, 10, 7);
        AddKanbanTaskToList(ETaskStatus.Completed, 20, 8);
        AddKanbanTaskToList(ETaskStatus.Completed, 30, 9);
    }

    [Test]
    public void HandleAnimation_ShouldAddAnimations_WhenTaskIsAdded()
    {
        // GIVEN
        Task? newTask = new Task
        {
            Status = ETaskStatus.InProgress,
            OrderValue = 11
        };
        
        _viewServiceMock.Setup(x => x.KanbanTaskTotalHeight).Returns(100);
        SetupKanbanIteration(ETaskStatus.InProgress);
        SetupAnimation(5, ETaskStatus.InProgress, 20, -100, Direction.Bottom);
        SetupAnimation(6, ETaskStatus.InProgress, 30, -100, Direction.Bottom);

        // WHEN
        _sut.HandleAnimation(null, newTask);

        // THEN
        _sutMock.CallBase = false;
        _sutMock.Verify();
    }

    [Test]
    public void HandleAnimation_ShouldAddAnimations_WhenTaskIsRemoved()
    {
        // GIVEN
        Task? oldTask = new Task
        {
            Status = ETaskStatus.InProgress,
            OrderValue = 11
        };

        _viewServiceMock.Setup(x => x.KanbanTaskTotalHeight).Returns(100);
        SetupKanbanIteration(ETaskStatus.InProgress);
        SetupAnimation(5, ETaskStatus.InProgress, 20, 100, Direction.Top);
        SetupAnimation(6, ETaskStatus.InProgress, 30, 100, Direction.Top);

        // WHEN
        _sut.HandleAnimation(oldTask, null);

        // THEN
        _sutMock.CallBase = false;
        _sutMock.Verify();
    }

    [Test]
    public void HandleAnimation_ShouldReturn_WhenBothParametersAreNull()
    {
        // WHEN
        _sut.HandleAnimation(null, null);

        // THEN
        _sutMock.CallBase = false;
        _sutMock.Verify(x => x.AddAnimation(It.IsAny<Animation>()), Times.Never);
    }

    [Test]
    public void HandleAnimation_ShouldAddAnimations_WhenTaskChangedColumn()
    {
        // GIVEN
        Task? oldTask = new Task
        {
            Status = ETaskStatus.InProgress,
            OrderValue = 11
        };
        Task? newTask = new Task
        {
            Status = ETaskStatus.Completed,
            OrderValue = 11
        };

        _viewServiceMock.Setup(x => x.KanbanTaskTotalHeight).Returns(100);
        SetupKanbanIteration(ETaskStatus.InProgress);
        SetupKanbanIteration(ETaskStatus.Completed);
        SetupAnimation(5, ETaskStatus.InProgress, 20, 100, Direction.Top);
        SetupAnimation(6, ETaskStatus.InProgress, 30, 100, Direction.Top);
        SetupAnimation(8, ETaskStatus.Completed, 20, -100, Direction.Bottom);
        SetupAnimation(9, ETaskStatus.Completed, 30, -100, Direction.Bottom);

        // WHEN
        _sut.HandleAnimation(oldTask, newTask);

        // THEN
        _sutMock.CallBase = false;
        _sutMock.Verify();
    }

    [Test]
    public void HandleAnimation_ShouldAddAnimations_WhenTaskIsMovedToTop()
    {
        // GIVEN
        Task? oldTask = new Task
        {
            Status = ETaskStatus.InProgress,
            OrderValue = 11
        };
        Task? newTask = new Task
        {
            Status = ETaskStatus.InProgress,
            OrderValue = 1
        };

        _viewServiceMock.Setup(x => x.KanbanTaskTotalHeight).Returns(100);
        SetupKanbanIteration(ETaskStatus.InProgress);
        SetupAnimation(4, ETaskStatus.InProgress, 10, -100, Direction.Bottom);

        // WHEN
        _sut.HandleAnimation(oldTask, newTask);

        // THEN
        _sutMock.CallBase = false;
        _sutMock.Verify();
    }

    [Test]
    public void HandleAnimation_ShouldAddAnimations_WhenTaskIsMovedToBottom()
    {
        // GIVEN
        Task? oldTask = new Task
        {
            Status = ETaskStatus.InProgress,
            OrderValue = 11
        };
        Task? newTask = new Task
        {
            Status = ETaskStatus.InProgress,
            OrderValue = 21
        };

        _viewServiceMock.Setup(x => x.KanbanTaskTotalHeight).Returns(100);
        SetupKanbanIteration(ETaskStatus.InProgress);
        SetupAnimation(5, ETaskStatus.InProgress, 20, 100, Direction.Top);

        // WHEN
        _sut.HandleAnimation(oldTask, newTask);

        // THEN
        _sutMock.CallBase = false;
        _sutMock.Verify();
    }

    [Test]
    public void AddAnimation_ShouldAddNewAnimation()
    {
        // GIVEN
        var kanbanTask = GetKanbanTask(ETaskStatus.InProgress, 30);
        var animation = new Animation
        {
            Id = 6,
            KanbanTask = kanbanTask,
            From = -100,
            Direction = Direction.Bottom
        };

        _sut = new();
        _sut.Setup(_viewServiceMock.Object);
        var ongoingAnimations = ReflectionUtils.GetFieldValue<Dictionary<int, Animation>>(_sut, "_ongoingAnimations");
        var animationDuration = ReflectionUtils.GetFieldValue<TimeSpan>(_sut, "_animationDuration");

        _viewServiceMock.Setup(x => x.GetCurrentTransformValue(kanbanTask)).Returns(0);
        _viewServiceMock.Setup(x => x.StartDoubleAnimation(kanbanTask, -100, animationDuration));

        // WHEN
        _sut.AddAnimation(animation);

        // THEN
        _viewServiceMock.VerifyAll();
        Assert.That(ongoingAnimations.Count, Is.EqualTo(1));
        Assert.That(ongoingAnimations[6], Is.EqualTo(animation));
    }

    [Test]
    public void AddAnimation_ShouldAddNewAnimation_WhenPreviousIsCompleted()
    {
        // GIVEN
        var kanbanTask = GetKanbanTask(ETaskStatus.InProgress, 30);
        var previousAnimation = new Animation
        {
            Id = 6,
            KanbanTask = kanbanTask,
            From = 100,
            Direction = Direction.Top
        };
        var animation = new Animation
        {
            Id = 6,
            KanbanTask = kanbanTask,
            From = -100,
            Direction = Direction.Bottom
        };

        _sut = new();
        _sut.Setup(_viewServiceMock.Object);
        var ongoingAnimations = ReflectionUtils.GetFieldValue<Dictionary<int, Animation>>(_sut, "_ongoingAnimations");
        var animationDuration = ReflectionUtils.GetFieldValue<TimeSpan>(_sut, "_animationDuration");

        ongoingAnimations.Add(6, previousAnimation);
        _viewServiceMock.Setup(x => x.GetCurrentTransformValue(kanbanTask)).Returns(0);
        _viewServiceMock.Setup(x => x.StartDoubleAnimation(kanbanTask, -100, animationDuration));

        // WHEN
        _sut.AddAnimation(animation);

        // THEN
        _viewServiceMock.VerifyAll();
        Assert.That(ongoingAnimations.Count, Is.EqualTo(1));
        Assert.That(ongoingAnimations[6], Is.EqualTo(animation));
    }

    [Test]
    public void AddAnimation_ShouldReturn_WhenThereIsPreviousWithTheSameDirection()
    {
        // GIVEN
        var kanbanTask = GetKanbanTask(ETaskStatus.InProgress, 30);
        var previousAnimation = new Animation
        {
            Id = 6,
            KanbanTask = kanbanTask,
            From = -100,
            Direction = Direction.Bottom
        };
        var animation = new Animation
        {
            Id = 6,
            KanbanTask = kanbanTask,
            From = -100,
            Direction = Direction.Bottom
        };

        _sut = new();
        _sut.Setup(_viewServiceMock.Object);
        var ongoingAnimations = ReflectionUtils.GetFieldValue<Dictionary<int, Animation>>(_sut, "_ongoingAnimations");
        var animationDuration = ReflectionUtils.GetFieldValue<TimeSpan>(_sut, "_animationDuration");

        ongoingAnimations.Add(6, previousAnimation);
        _viewServiceMock.Setup(x => x.GetCurrentTransformValue(kanbanTask)).Returns(-50);

        // WHEN
        _sut.AddAnimation(animation);

        // THEN
        _viewServiceMock.VerifyAll();
        Assert.That(ongoingAnimations.Count, Is.EqualTo(1));
        Assert.That(ongoingAnimations[6], Is.EqualTo(previousAnimation));
    }

    [Test]
    public void AddAnimation_ShouldUpdateAnimation_FromTopDirection()
    {
        // GIVEN
        var kanbanTask = GetKanbanTask(ETaskStatus.InProgress, 30);
        var previousAnimation = new Animation
        {
            Id = 6,
            KanbanTask = kanbanTask,
            From = 100,
            Direction = Direction.Top
        };
        var animation = new Animation
        {
            Id = 6,
            KanbanTask = kanbanTask,
            From = -100,
            Direction = Direction.Bottom
        };

        _sut = new();
        _sut.Setup(_viewServiceMock.Object);
        var ongoingAnimations = ReflectionUtils.GetFieldValue<Dictionary<int, Animation>>(_sut, "_ongoingAnimations");
        var animationDuration = ReflectionUtils.GetFieldValue<TimeSpan>(_sut, "_animationDuration");

        ongoingAnimations.Add(6, previousAnimation);
        _viewServiceMock.Setup(x => x.GetCurrentTransformValue(kanbanTask)).Returns(30);
        _viewServiceMock.Setup(x => x.KanbanTaskTotalHeight).Returns(100);
        _viewServiceMock.Setup(x => x.StartDoubleAnimation(kanbanTask, -70, 0.7 * animationDuration));

        // WHEN
        _sut.AddAnimation(animation);

        // THEN
        _viewServiceMock.VerifyAll();
        Assert.That(ongoingAnimations.Count, Is.EqualTo(1));
        Assert.That(ongoingAnimations[6], Is.EqualTo(animation));
    }

    [Test]
    public void AddAnimation_ShouldUpdateAnimation_FromBottomDirection()
    {
        // GIVEN
        var kanbanTask = GetKanbanTask(ETaskStatus.InProgress, 30);
        var previousAnimation = new Animation
        {
            Id = 6,
            KanbanTask = kanbanTask,
            From = -100,
            Direction = Direction.Bottom
        };
        var animation = new Animation
        {
            Id = 6,
            KanbanTask = kanbanTask,
            From = 100,
            Direction = Direction.Top
        };

        _sut = new();
        _sut.Setup(_viewServiceMock.Object);
        var ongoingAnimations = ReflectionUtils.GetFieldValue<Dictionary<int, Animation>>(_sut, "_ongoingAnimations");
        var animationDuration = ReflectionUtils.GetFieldValue<TimeSpan>(_sut, "_animationDuration");

        ongoingAnimations.Add(6, previousAnimation);
        _viewServiceMock.Setup(x => x.GetCurrentTransformValue(kanbanTask)).Returns(-30);
        _viewServiceMock.Setup(x => x.KanbanTaskTotalHeight).Returns(100);
        _viewServiceMock.Setup(x => x.StartDoubleAnimation(kanbanTask, 70, 0.7 * animationDuration));

        // WHEN
        _sut.AddAnimation(animation);

        // THEN
        _viewServiceMock.VerifyAll();
        Assert.That(ongoingAnimations.Count, Is.EqualTo(1));
        Assert.That(ongoingAnimations[6], Is.EqualTo(animation));
    }

    private void SetupKanbanIteration(ETaskStatus columnStatus)
    {
        _viewServiceMock.Setup(x => x.ForEachKanbanTask(columnStatus, It.IsAny<Action<KanbanTask, Task>>()))
            .Callback<ETaskStatus, Action<KanbanTask, Task>>((columnStatus, action) => 
            {
                foreach(var kanbanTask in _kabanTaskList)
                {
                    Task? coreTask = kanbanTask.DataContext as Task;
                    Assert.That(coreTask, Is.Not.Null);

                    if (coreTask.Status == columnStatus)
                    {
                        action(kanbanTask, coreTask);
                    }
                }
            });
    }

    private void SetupAnimation(int id, ETaskStatus columnStatus, int order, double from, Direction direction)
    {
        _sutMock.Setup(x => x.AddAnimation(It.Is<Animation>(a =>
            a.Id == id &&
            a.KanbanTask == GetKanbanTask(columnStatus, order) &&
            a.From == from &&
            a.Direction == direction
        ))).Verifiable();
    }

    private void AddKanbanTaskToList(ETaskStatus columnStatus, int order, int id)
    {
        Task coreTask = new Task
        {
            Id = id,
            Status = columnStatus,
            OrderValue = order
        };

        KanbanTask kanbanTask = new KanbanTask();
        kanbanTask.DataContext = coreTask;

        _kabanTaskList.Add(kanbanTask);
    }

    private KanbanTask GetKanbanTask(ETaskStatus columnStatus, int order)
    {
        var kanbanTask = _kabanTaskList.FirstOrDefault(x =>
        {
            var coreTask = x.DataContext as Task;
            return coreTask?.Status == columnStatus && coreTask?.OrderValue == order;
        });

        Assert.That(kanbanTask, Is.Not.Null);
        return kanbanTask;
    }
}
