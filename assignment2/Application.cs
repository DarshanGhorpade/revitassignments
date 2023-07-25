using Autodesk.Revit.UI;

using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace assignment2
{
    class Application : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            RibbonPanel ribbonPanel = application.CreateRibbonPanel("Panel2");

            PushButton pushButton = ribbonPanel.AddItem(new PushButtonData("Major Elements", "Major Elements", thisAssemblyPath, "assignment2.commands.QuantitiesExtractorCommand")) as PushButton;

            pushButton.ToolTip = "get major elements in model";

            pushButton.LargeImage =
                new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(thisAssemblyPath) ?? string.Empty,
                    "Resources", "count_icon.png")));

            return Result.Succeeded;
        }
    }
}
