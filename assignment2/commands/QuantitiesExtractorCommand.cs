using assignment2.Utils;
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
            QuantitiesExtractor quantitiesExtractor = new QuantitiesExtractor(uiApplication);
            var quantities = quantitiesExtractor.GetMajorElementQuantities();

            QuantitiesWindow quantitiesWindow = new QuantitiesWindow(quantities);
            quantitiesWindow.Show();

            //TaskDialog.Show("Message", "Hello World");

            return Result.Succeeded;
        }
    }
}
