using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.Windows.Input;
using TaskManager.Client.Behaviors;
using TaskManager.Client.Behaviors.KanbanBoardDragDrop;
using TaskManager.Client.View.Kanban;

namespace TaskManager.Client.Tests.Behaviors;

[TestFixture]
[Apartment(ApartmentState.STA)]
internal class KanbanBoardDragDropBehaviorTests
{
    Mock<IDragDropHandler> _dragDropHandlerMock = null!;
    Mock<IViewService> _viewServiceMock = null!;

    KanbanBoard _kanbanBoard = null!;
    MouseButtonEventArgs _mouseButtonEventArgs = null!;
    MouseEventArgs _mouseEventArgs = null!;

    KanbanBoardDragDropBehavior _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _dragDropHandlerMock = new Mock<IDragDropHandler>(MockBehavior.Strict);
        _viewServiceMock = new Mock<IViewService>(MockBehavior.Strict);

        App.IoC = new Ioc();
        App.IoC.ConfigureServices(new ServiceCollection()
            .AddTransient(provider => _dragDropHandlerMock.Object)
            .AddTransient(provider => _viewServiceMock.Object)
            .BuildServiceProvider());

        _kanbanBoard = new KanbanBoard();
        _mouseButtonEventArgs = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
        _mouseEventArgs = new MouseEventArgs(Mouse.PrimaryDevice, 0);

        _sut = new KanbanBoardDragDropBehavior();
        _sut.Attach(_kanbanBoard);
    }

    [TearDown]
    public void TearDown()
    {
        _sut.Detach();
    }

    [Test]
    public void PreviewMouseDown_ShouldStartDragDrop()
    {
        // GIVEN
        _dragDropHandlerMock.Setup(x => x.IsStarted).Returns(false);
        _viewServiceMock.Setup(x => x.IsSingleClick(_mouseButtonEventArgs)).Returns(true);
        _viewServiceMock.Setup(x => x.IsKanbanTaskDragged(_mouseButtonEventArgs)).Returns(true);

        var sequence = new MockSequence();
        _viewServiceMock.InSequence(sequence).Setup(x => x.Setup(_kanbanBoard, _mouseButtonEventArgs));
        _viewServiceMock.InSequence(sequence).Setup(x => x.CaptureMouse());
        _dragDropHandlerMock.InSequence(sequence).Setup(x => x.StartDragDrop(_viewServiceMock.Object));

        // WHEN
        _sut.KanbanBoard_PreviewMouseDown(_kanbanBoard, _mouseButtonEventArgs);

        // THEN
        _viewServiceMock.Verify(x => x.Setup(_kanbanBoard, _mouseButtonEventArgs));
        _viewServiceMock.Verify(x => x.CaptureMouse());
        _dragDropHandlerMock.Verify(x => x.StartDragDrop(_viewServiceMock.Object));
    }

    [Test]
    public void PreviewMouseDown_ShouldNotStartDragDrop_WhenDragDropIsStarted()
    {
        // GIVEN
        _dragDropHandlerMock.Setup(x => x.IsStarted).Returns(true);

        // WHEN
        _sut.KanbanBoard_PreviewMouseDown(_kanbanBoard, _mouseButtonEventArgs);

        // THEN
        _viewServiceMock.Verify(x => x.Setup(_kanbanBoard, _mouseButtonEventArgs), Times.Never);
        _viewServiceMock.Verify(x => x.CaptureMouse(), Times.Never);
        _dragDropHandlerMock.Verify(x => x.StartDragDrop(_viewServiceMock.Object), Times.Never);
    }

    [Test]
    public void PreviewMouseDown_ShouldNotStartDragDrop_WhenItIsNotSingleClick()
    {
        // GIVEN
        _dragDropHandlerMock.Setup(x => x.IsStarted).Returns(false);
        _viewServiceMock.Setup(x => x.IsSingleClick(_mouseButtonEventArgs)).Returns(false);

        // WHEN
        _sut.KanbanBoard_PreviewMouseDown(_kanbanBoard, _mouseButtonEventArgs);

        // THEN
        _viewServiceMock.Verify(x => x.Setup(_kanbanBoard, _mouseButtonEventArgs), Times.Never);
        _viewServiceMock.Verify(x => x.CaptureMouse(), Times.Never);
        _dragDropHandlerMock.Verify(x => x.StartDragDrop(_viewServiceMock.Object), Times.Never);
    }

    [Test]
    public void PreviewMouseDown_ShouldNotStartDragDrop_WhenDraggedIsNotKanbanTask()
    {
        // GIVEN
        _dragDropHandlerMock.Setup(x => x.IsStarted).Returns(false);
        _viewServiceMock.Setup(x => x.IsSingleClick(_mouseButtonEventArgs)).Returns(true);
        _viewServiceMock.Setup(x => x.IsKanbanTaskDragged(_mouseButtonEventArgs)).Returns(false);

        // WHEN
        _sut.KanbanBoard_PreviewMouseDown(_kanbanBoard, _mouseButtonEventArgs);

        // THEN
        _viewServiceMock.Verify(x => x.Setup(_kanbanBoard, _mouseButtonEventArgs), Times.Never);
        _viewServiceMock.Verify(x => x.CaptureMouse(), Times.Never);
        _dragDropHandlerMock.Verify(x => x.StartDragDrop(_viewServiceMock.Object), Times.Never);
    }

    [Test]
    public void PreviewMouseMove_ShouldUpdateDragDrop()
    {
        // GIVEN
        _dragDropHandlerMock.Setup(x => x.IsStarted).Returns(true);

        var sequence = new MockSequence();
        _viewServiceMock.InSequence(sequence).Setup(x => x.UpdateMousePosition(_mouseEventArgs));
        _dragDropHandlerMock.InSequence(sequence).Setup(x => x.UpdateDragDrop());

        // WHEN
        _sut.KanbanBoard_PreviewMouseMove(_kanbanBoard, _mouseEventArgs);

        // THEN
        _viewServiceMock.Verify(x => x.UpdateMousePosition(_mouseEventArgs));
        _dragDropHandlerMock.Verify(x => x.UpdateDragDrop());
    }

    [Test]
    public void PreviewMouseMove_ShouldNotUpdateDragDrop_WhenDragDropIsNotStarted()
    {
        // GIVEN
        _dragDropHandlerMock.Setup(x => x.IsStarted).Returns(false);

        // WHEN
        _sut.KanbanBoard_PreviewMouseMove(_kanbanBoard, _mouseEventArgs);

        // THEN
        _viewServiceMock.Verify(x => x.UpdateMousePosition(_mouseEventArgs), Times.Never);
        _dragDropHandlerMock.Verify(x => x.UpdateDragDrop(), Times.Never);
    }

    [Test]
    public void PreviewMouseUp_ShouldStopDragDrop()
    {
        // GIVEN
        _dragDropHandlerMock.Setup(x => x.IsStarted).Returns(true);
        _viewServiceMock.Setup(x => x.ReleaseMouseCapture());
        _dragDropHandlerMock.Setup(x => x.StopDragDrop());

        // WHEN
        _sut.KanbanBoard_PreviewMouseUp(_kanbanBoard, _mouseButtonEventArgs);

        // THEN
        _viewServiceMock.Verify(x => x.ReleaseMouseCapture());
        _dragDropHandlerMock.Verify(x => x.StopDragDrop());
    }

    [Test]
    public void PreviewMouseUp_ShouldNotStopDragDrop_WhenDragDropIsNotStarted()
    {
        // GIVEN
        _dragDropHandlerMock.Setup(x => x.IsStarted).Returns(false);

        // WHEN
        _sut.KanbanBoard_PreviewMouseUp(_kanbanBoard, _mouseButtonEventArgs);

        // THEN
        _viewServiceMock.Verify(x => x.ReleaseMouseCapture(), Times.Never);
        _dragDropHandlerMock.Verify(x => x.StopDragDrop(), Times.Never);
    }
}
