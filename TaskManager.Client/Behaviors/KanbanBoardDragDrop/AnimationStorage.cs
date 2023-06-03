using System;
using System.Linq;
using System.Collections.Specialized;
using System.Collections.Generic;
using TaskManager.Client.Model;
using TaskManager.Client.View.Kanban;
using TaskManager.Core.Models;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Controls;
using Microsoft.VisualBasic;
using System.Windows;
using System.Diagnostics;
using System.Windows.Media.Media3D;
using System.Security.Cryptography.Xml;

namespace TaskManager.Client.Behaviors.KanbanBoardDragDrop;

public class AnimationStorage : IAnimationStorage
{
    private readonly TimeSpan _animationDuration = TimeSpan.FromSeconds(0.15);
    private Dictionary<int, Animation> _ongoingAnimations = new();

    private double? _kanbanTaskHeight;

    public void Setup(double kanbanTaskHeight)
    {
        _kanbanTaskHeight = kanbanTaskHeight;
    }

    public void AddAnimation(Animation animation)
    {
        ArgumentNullException.ThrowIfNull(_kanbanTaskHeight);
        var coreTask = animation.KanbanTask.DataContext as Task;
        ArgumentNullException.ThrowIfNull(coreTask);
        int taskId = coreTask.Id;

        RemoveIfCompleted(taskId, animation.KanbanTask);
        if (!_ongoingAnimations.TryGetValue(taskId, out Animation? ongoingAnimation))
        {
            StartDoubleAnimation(animation.KanbanTask, animation.From, _animationDuration);
            _ongoingAnimations.Add(taskId, animation);
            return;
        }

        if (animation.Direction == ongoingAnimation.Direction)
        {
            return;
        }

        double yTransform = GetCurrentTransformValue(animation.KanbanTask);
        double from = yTransform >= 0 ? yTransform - _kanbanTaskHeight.Value : yTransform + _kanbanTaskHeight.Value;
        Duration duration = Math.Abs(from / _kanbanTaskHeight.Value) * _animationDuration;

        StartDoubleAnimation(animation.KanbanTask, from, duration);
        
        _ongoingAnimations.Remove(taskId);
        _ongoingAnimations.Add(taskId, animation);
    }

    private void StartDoubleAnimation(KanbanTask kanbanTask, double from, Duration duration)
    {
        var yAnimation = new DoubleAnimation
        {
            From = from,
            To = 0,
            Duration = duration
        };

        var transform = new TranslateTransform();
        kanbanTask.RenderTransform = transform;
        transform.BeginAnimation(TranslateTransform.YProperty, yAnimation, HandoffBehavior.SnapshotAndReplace);
    }

    private void RemoveIfCompleted(int taskId, KanbanTask kanbanTask)
    {
        if (GetCurrentTransformValue(kanbanTask) != 0)
        {
            return;
        }

        _ongoingAnimations.Remove(taskId);
    }

    private double GetCurrentTransformValue(KanbanTask kanbanTask)
    {
        return (double)kanbanTask.RenderTransform.GetValue(TranslateTransform.YProperty);
    }
}
