using FluentTranslate.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FluentTranslate.WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider? _serviceProvider;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Initialize FluentTranslate
            // The default behavior should pickup the folder
            var services = new ServiceCollection()
                .AddSingleton<MainWindow>()
                .AddFluentTranslate();

            _serviceProvider = services.BuildServiceProvider();

            var main = _serviceProvider.GetService<MainWindow>();
            main?.Show();
        }
    }
}
