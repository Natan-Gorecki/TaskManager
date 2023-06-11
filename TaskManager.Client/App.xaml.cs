using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.IO;
using System.Windows;
using TaskManager.Client.Behaviors.KanbanBoardDragDrop;
using TaskManager.Client.Utils;
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

            var configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            var serviceCollection = new ServiceCollection()
                .AddTasksManager()
                .AddLogging(loggingBuidler =>
                {
                    loggingBuidler.ClearProviders();
                    loggingBuidler.AddNLog(configurationRoot);
                });

            // behaviors
            serviceCollection.AddTransient<IDragDropHandler, DragDropHandler>();
            serviceCollection.AddTransient<IAnimationHandler, AnimationHandler>();
            serviceCollection.AddTransient<ITaskCollectionManager, TaskCollectionManager>();
            serviceCollection.AddTransient<IViewService, ViewService>();

            // utils
            serviceCollection.AddSingleton<IModalPageManager, ModalPageManager>();

            IoC = Ioc.Default;
            IoC.ConfigureServices(serviceCollection.BuildServiceProvider());
        }
    }
}
