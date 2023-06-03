using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TaskManager.Client.Behaviors.KanbanBoardDragDrop;
using TasksManager.Core;

namespace TaskManager.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Ioc IoC { get; }

        static App()
        {
            var serviceCollection = new ServiceCollection()
                .AddTasksManager()
                .AddLogging();

            serviceCollection.AddTransient<IDragDropHandler, DragDropHandler>();
            serviceCollection.AddTransient<IAnimationHandler, AnimationHandler>();
            serviceCollection.AddTransient<IAnimationStorage, AnimationStorage>();

            IoC = Ioc.Default;
            IoC.ConfigureServices(serviceCollection.BuildServiceProvider());
        }
    }
}
