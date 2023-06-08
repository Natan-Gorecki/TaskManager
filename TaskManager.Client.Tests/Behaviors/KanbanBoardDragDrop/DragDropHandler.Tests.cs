using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.Windows;
using TaskManager.Client.Behaviors.KanbanBoardDragDrop;
using TaskManager.Core.Models;
using TaskManager.Core.Utils;
using Task = TaskManager.Core.Models.Task;

namespace TaskManager.Client.Tests.Behaviors.KanbanBoardDragDrop;

[TestFixture]
internal class DragDropHandlerTests
{
    private Mock<IAnimationHandler> _animationHandlerMock = null!;
    private Mock<ITaskCollectionManager> _taskCollectionManagerMock = null!;
    private Mock<IViewService> _viewServiceMock = null!;

    private DragDropHandler _sut = null!;
    private Task _coreTask = null!;

    [SetUp]
    public void SetUp()
    {
        _animationHandlerMock = new Mock<IAnimationHandler>();
        _taskCollectionManagerMock = new Mock<ITaskCollectionManager>();
        _viewServiceMock = new Mock<IViewService>();

        App.IoC = new Ioc();
        App.IoC.ConfigureServices(new ServiceCollection()
            .AddTransient(provider => _animationHandlerMock.Object)
            .AddTransient(provider => _taskCollectionManagerMock.Object)
            .BuildServiceProvider());

        _sut = new DragDropHandler();
        _coreTask = new Task
        {
            OrderValue = 10,
            Status = ETaskStatus.InProgress,
            IsPreview = false
        };
        _viewServiceMock.Setup(x => x.CoreTask).Returns(_coreTask);
    }

    #region StartDragDrop
    [Test]
    public void StartDragDrop_ShouldSetupValues()
    {
        // WHEN
        _sut.StartDragDrop(_viewServiceMock.Object);

        // THEN
        Assert.That(_sut.IsStarted, Is.True);
        _animationHandlerMock.Verify(x => x.Setup(_viewServiceMock.Object));
        _viewServiceMock.Verify(x => x.SetupTaskCollectionManager(_taskCollectionManagerMock.Object));
    }

    [Test]
    public void StartDragDrop_ShouldAddPreviewTask()
    {
        // WHEN
        _sut.StartDragDrop(_viewServiceMock.Object);

        // THEN
        _taskCollectionManagerMock.Verify(x => x.RemoveTask(
            IsTask(false, ETaskStatus.InProgress, 10), true));
        _taskCollectionManagerMock.Verify(x => x.AddTask(
            IsTask(true, ETaskStatus.InProgress, 1), false));
    }

    [Test]
    public void StartDragDrop_ShouldShowDraggedKanbanTask()
    {
        // GIVEN
        _viewServiceMock.Setup(x => x.InitialPosition).Returns(new Point(100, 100));
        _viewServiceMock.Setup(x => x.MouseInsideControl).Returns(new Point(10, 10));

        // WHEN
        _sut.StartDragDrop(_viewServiceMock.Object);

        // THEN
        _viewServiceMock.Verify(x => x.ShowDraggedKanbanTask(90, 90));
    }
    #endregion

    #region UpdateDragDrop
    [Test]
    public void UpdateDragDrop_ShouldUpdateDraggedKanbanTask()
    {
        // GIVEN
        _sut.StartDragDrop(_viewServiceMock.Object);
        _viewServiceMock.Setup(x => x.CurrentPosition).Returns(new Point(150, 150));
        _viewServiceMock.Setup(x => x.InitialPosition).Returns(new Point(100, 200));
        
        // WHEN 
        _sut.UpdateDragDrop();

        // THEN
        _viewServiceMock.Verify(x => x.UpdateDraggedKanbanTask(50, -50));
    }

    [Test]
    public void UpdateDragDrop_ShouldRemovePreviewTask_WhenDraggedTaskIsOutsideBoard()
    {
        // GIVEN
        _sut.StartDragDrop(_viewServiceMock.Object);
        _viewServiceMock.Setup(x => x.IsDraggedKanbanTaskOutsideKanbanBoard()).Returns(true);
        var previewTask = ReflectionUtils.GetFieldValue<Task>(_sut, "_previewTask");

        // WHEN 
        _sut.UpdateDragDrop();

        // THEN
        _taskCollectionManagerMock.Verify(x => x.RemoveTask(previewTask, false));
        _animationHandlerMock.Verify(x => x.HandleAnimation(previewTask, null));
        var actual = ReflectionUtils.GetFieldValue<Task?>(_sut, "_previewTask");
        Assert.That(actual, Is.Null);
    }

    [Test]
    public void UpdateDragDrop_ShouldReturn_WhenDraggedTaskIsOutsideBoard()
    {
        // GIVEN
        _sut.StartDragDrop(_viewServiceMock.Object);
        _viewServiceMock.Setup(x => x.IsDraggedKanbanTaskOutsideKanbanBoard()).Returns(true);
        ReflectionUtils.SetFieldValue<Task?>(_sut, "_previewTask", null);

        _taskCollectionManagerMock.Reset();
        _animationHandlerMock.Reset();

        // WHEN 
        _sut.UpdateDragDrop();

        // THEN
        _taskCollectionManagerMock.VerifyNoOtherCalls();
        _animationHandlerMock.VerifyNoOtherCalls();
    }

    [Test]
    public void UpdateDragDrop_ShouldReturn_WhenDraggedTaskIsNotOverKanbanColumn()
    {
        // GIVEN
        _sut.StartDragDrop(_viewServiceMock.Object);
        
        ETaskStatus columnStatus;
        double offsetInsideColumn;

        _viewServiceMock.Setup(x => x.IsDraggedKanbanTaskOutsideKanbanBoard()).Returns(false);
        _viewServiceMock.Setup(x => x.IsDraggedKanbanTaskOverKandanColumn(out columnStatus, out offsetInsideColumn)).Returns(false);

        _taskCollectionManagerMock.Reset();
        _animationHandlerMock.Reset();

        // WHEN 
        _sut.UpdateDragDrop();

        // THEN
        _taskCollectionManagerMock.VerifyNoOtherCalls();
        _animationHandlerMock.VerifyNoOtherCalls();
    }

    [Test]
    public void UpdateDragDrop_ShouldAddPreviewTask_WhenPreviewTaskIsNull()
    {
        // GIVEN
        _sut.StartDragDrop(_viewServiceMock.Object);

        ETaskStatus columnStatus = ETaskStatus.Completed;
        double offsetInsideColumn = 190;

        ReflectionUtils.SetFieldValue<Task?>(_sut, "_previewTask", null);
        _viewServiceMock.Setup(x => x.KanbanTaskTotalHeight).Returns(100);
        _viewServiceMock.Setup(x => x.IsDraggedKanbanTaskOutsideKanbanBoard()).Returns(false);
        _viewServiceMock.Setup(x => x.IsDraggedKanbanTaskOverKandanColumn(out columnStatus, out offsetInsideColumn)).Returns(true);

        // WHEN 
        _sut.UpdateDragDrop();

        // THEN
        _taskCollectionManagerMock.Verify(x => x.AddTask(
            IsTask(true, ETaskStatus.Completed, 11), false));
        _animationHandlerMock.Verify(x => x.HandleAnimation(null,
            IsTask(true, ETaskStatus.Completed, 11)));
    }

    [Test]
    public void UpdateDragDrop_ShouldReturn_WhenPreviewTaskPositionDidntChange()
    {
        // GIVEN
        _sut.StartDragDrop(_viewServiceMock.Object);

        ETaskStatus columnStatus = ETaskStatus.InProgress;
        double offsetInsideColumn = 100;
        var previewTask = new Task
        {
            IsPreview = true,
            OrderValue = 11,
            Status = ETaskStatus.InProgress
        };
        
        ReflectionUtils.SetFieldValue<Task?>(_sut, "_previewTask", previewTask);
        _viewServiceMock.Setup(x => x.KanbanTaskTotalHeight).Returns(100);
        _viewServiceMock.Setup(x => x.IsDraggedKanbanTaskOutsideKanbanBoard()).Returns(false);
        _viewServiceMock.Setup(x => x.IsDraggedKanbanTaskOverKandanColumn(out columnStatus, out offsetInsideColumn)).Returns(true);

        _taskCollectionManagerMock.Reset();
        _animationHandlerMock.Reset();

        // WHEN 
        _sut.UpdateDragDrop();

        // THEN
        _taskCollectionManagerMock.VerifyNoOtherCalls();
        _animationHandlerMock.VerifyNoOtherCalls();
    }

    [Test]
    [TestCase(ETaskStatus.Completed, 100)]
    [TestCase(ETaskStatus.InProgress, 200)]
    [TestCase(ETaskStatus.Completed, 200)]
    public void UpdateDragDrop_ShouldUpdatePreviewTask_WhenItChangedPosition(ETaskStatus columnStatus, double offsetInsideColumn)
    {
        // GIVEN
        _sut.StartDragDrop(_viewServiceMock.Object);

        var previewTask = new Task
        {
            IsPreview = true,
            OrderValue = 11,
            Status = ETaskStatus.InProgress
        };

        ReflectionUtils.SetFieldValue<Task?>(_sut, "_previewTask", previewTask);
        _viewServiceMock.Setup(x => x.KanbanTaskTotalHeight).Returns(100);
        _viewServiceMock.Setup(x => x.IsDraggedKanbanTaskOutsideKanbanBoard()).Returns(false);
        _viewServiceMock.Setup(x => x.IsDraggedKanbanTaskOverKandanColumn(out columnStatus, out offsetInsideColumn)).Returns(true);

        // WHEN 
        _sut.UpdateDragDrop();

        // THEN
        var orderValue = 1 + (int)Math.Floor(offsetInsideColumn / 100) * 10;
        _taskCollectionManagerMock.Verify(x => x.RemoveTask(previewTask, false));
        _taskCollectionManagerMock.Verify(x => x.AddTask(
            IsTask(true, columnStatus, orderValue), false));
        _animationHandlerMock.Verify(x => x.HandleAnimation(previewTask, 
            IsTask(true, columnStatus, orderValue)));
    }

    [Test]
    public void UpdateDragDrop_ShouldUpdatePreviewTask_WithNoNegativeOrderValue()
    {
        // GIVEN
        _sut.StartDragDrop(_viewServiceMock.Object);

        ETaskStatus columnStatus = ETaskStatus.InProgress;
        double offsetInsideColumn = -100;
        var previewTask = new Task
        {
            IsPreview = true,
            OrderValue = 11,
            Status = ETaskStatus.InProgress
        };

        ReflectionUtils.SetFieldValue<Task?>(_sut, "_previewTask", previewTask);
        _viewServiceMock.Setup(x => x.KanbanTaskTotalHeight).Returns(100);
        _viewServiceMock.Setup(x => x.IsDraggedKanbanTaskOutsideKanbanBoard()).Returns(false);
        _viewServiceMock.Setup(x => x.IsDraggedKanbanTaskOverKandanColumn(out columnStatus, out offsetInsideColumn)).Returns(true);

        // WHEN 
        _sut.UpdateDragDrop();

        // THEN
        _taskCollectionManagerMock.Verify(x => x.RemoveTask(previewTask, false));
        _taskCollectionManagerMock.Verify(x => x.AddTask(
            IsTask(true, columnStatus, 1), false));
        _animationHandlerMock.Verify(x => x.HandleAnimation(previewTask, 
            IsTask(true, columnStatus, 1)));
    }
    #endregion

    #region StopDragDrop
    [Test]
    public void StopDragDrop_ShouldChangeIsStartedValueToFalse()
    {
        // GIVEN
        _sut.StartDragDrop(_viewServiceMock.Object);
        Assert.That(_sut.IsStarted, Is.True);

        // WHEN
        _sut.StopDragDrop();

        // THEN
        Assert.That(_sut.IsStarted, Is.False);
    }

    [Test]
    public void StopDragDrop_ShouldReplacePreviewTask()
    {
        // GIVEN
        _sut.StartDragDrop(_viewServiceMock.Object);

        var previewTask = new Task
        {
            OrderValue = 11,
            Status = ETaskStatus.Completed,
            IsPreview = true
        };
        ReflectionUtils.SetFieldValue(_sut, "_previewTask", previewTask);

        // WHEN
        _sut.StopDragDrop();

        // THEN
        _taskCollectionManagerMock.Verify(x => x.RemoveTask(previewTask, false));
        _taskCollectionManagerMock.Verify(x => x.AddTask(_coreTask, true));
        Assert.Multiple(() =>
        {
            Assert.That(_coreTask.Status, Is.EqualTo(ETaskStatus.Completed));
            Assert.That(_coreTask.OrderValue, Is.EqualTo(11));
        });
    }

    [Test]
    public void StopDragDrop_ShouldRestoreOldTask()
    {
        // GIVEN
        _sut.StartDragDrop(_viewServiceMock.Object);
        ReflectionUtils.SetFieldValue<Task?>(_sut, "_previewTask", null);

        // WHEN
        _sut.StopDragDrop();

        // THEN
        _taskCollectionManagerMock.Verify(x => x.AddTask(_coreTask, true));
        Assert.Multiple(() =>
        {
            Assert.That(_coreTask.Status, Is.EqualTo(ETaskStatus.InProgress));
            Assert.That(_coreTask.OrderValue, Is.EqualTo(10));
        });
    }

    [Test]
    public void StopDragDrop_ShouldHideDraggedKanbanTask()
    {
        // GIVEN
        _sut.StartDragDrop(_viewServiceMock.Object);

        // WHEN
        _sut.StopDragDrop();

        // THEN
        _viewServiceMock.Verify(x => x.HideDraggedKanbanTask());
    }
    #endregion

    private static Task IsTask(bool isPreview, ETaskStatus taskStatus, int orderValue)
    {
        return It.Is<Task>(x => x.IsPreview == isPreview && x.Status == taskStatus && x.OrderValue == orderValue);
    }
}
