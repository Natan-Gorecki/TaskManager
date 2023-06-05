using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
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
        public static Ioc IoC { get; set; }

        static App()
        {
            var serviceCollection = new ServiceCollection()
                .AddTasksManager()
                .AddLogging();

            serviceCollection.AddTransient<IDragDropHandler, DragDropHandler>();
            serviceCollection.AddTransient<IAnimationHandler, AnimationHandler>();
            serviceCollection.AddTransient<IAnimationStorage, AnimationStorage>();
            serviceCollection.AddTransient<IViewService, ViewService>();

            IoC = Ioc.Default;
            IoC.ConfigureServices(serviceCollection.BuildServiceProvider());
        }
    }
}
