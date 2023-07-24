using Autodesk.Revit.UI;

using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace assignment1
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

            RibbonPanel ribbonPanel = application.CreateRibbonPanel("Panel1");

            PushButton pushButton = ribbonPanel.AddItem(new PushButtonData("Draw Wall", "Draw Wall", thisAssemblyPath, "assignment1.commands.DrawClosePolygonWall")) as PushButton;

            pushButton.ToolTip = "Draw Close Polygon Wall";

            pushButton.LargeImage =
                new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(thisAssemblyPath) ?? string.Empty,
                    "Resources", "DrawClosedPolygonwall.ico")));

            return Result.Succeeded;
        }
    }
}
