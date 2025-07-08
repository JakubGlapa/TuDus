using System.Globalization;
using System.Windows;
using TuDus.Helpers;
using TuDus.Models;
using TuDus.Parser;
using System.IO;

namespace TuDus
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application 
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            LocalizationManager.Initialize(Path.Combine(basePath, "Loc/dictionary.json"), CultureInfo.CurrentUICulture);
            base.OnStartup(e);
        }
    }
}
