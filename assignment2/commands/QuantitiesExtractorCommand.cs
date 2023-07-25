using assignment2.ViewModels;
using assignment2.Views;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace assignment2.commands
{
    [Transaction(TransactionMode.ReadOnly)]
    public class QuantitiesExtractorCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApplication = commandData.Application;
            Document document = uiApplication.ActiveUIDocument.Document;
            MyWpfWindow window = new MyWpfWindow();
            wpfWindowViewModel viewModel = new wpfWindowViewModel(document, window);
            window.DataContext = viewModel;
            window.Show();
            return Result.Succeeded;
        }
    }
}
