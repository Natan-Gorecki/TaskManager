using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using TaskManager.Client.View.Kanban;
using TaskManager.Core.Models;

namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

public class Animation
{
    public required KanbanTask KanbanTask { get; set; }
    public required double From { get; set; }
    public required Direction Direction { get; set; }
}
