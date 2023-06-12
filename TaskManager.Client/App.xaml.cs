using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using TaskManager.Client.Behaviors.KanbanBoardDragDrop;
using TaskManager.Client.Utils;
using TaskManager.Core;
using TaskManager.Core.Models;
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

            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pl");

            try
            {

#warning Potential memory leak
                var configurationRoot = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false, true)
                    .Build();

                var serviceCollection = new ServiceCollection()
                    .AddSingleton<IConfiguration>(configurationRoot)
                    .AddTasksManager(TaskManagerType.SqlLite)
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

                var taskManager = IoC.GetRequiredService<ITaskManager>();

            }
            catch(Exception ex)
            {
                var logger = IoC?.GetRequiredService<ILogger<App>>();
                logger?.LogCritical(ex, "Application unhandled exception.");
                LogManager.Flush();
            }
            
        }
    }
}
