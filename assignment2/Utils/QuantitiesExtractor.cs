using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using System.Collections.Generic;
using System.Linq;

namespace assignment2.Utils
{
    class QuantitiesExtractor
    {
        private UIApplication _uiApp;

        public QuantitiesExtractor(UIApplication uiApp)
        {
            _uiApp = uiApp;
        }

        public Dictionary<string, int> GetMajorElementQuantities()
        {
            Autodesk.Revit.DB.Document doc = _uiApp.ActiveUIDocument.Document;
            FilteredElementCollector collector = new FilteredElementCollector(doc);

            // Filter elements by category (e.g., Wall, Window, Door, Floor).
            List<BuiltInCategory> majorElementCategories = new List<BuiltInCategory>
            {
                BuiltInCategory.OST_Walls,
                BuiltInCategory.OST_Windows,
                BuiltInCategory.OST_Doors,
                BuiltInCategory.OST_Floors
                // Add more categories as needed.
            };

            

            Dictionary<string, int> quantities = new Dictionary<string, int>();
            List<Element> elements = new List<Element>();
            elements = new FilteredElementCollector(doc).WhereElementIsNotElementType().Where(x => x.Category != null && x.Category.Name.ToLower() == "windows").ToList();

            foreach (BuiltInCategory category in majorElementCategories)
            {
                ElementId eId = new ElementId(category);
                ElementCategoryFilter elementCategoryFilter = new ElementCategoryFilter(eId);
                int count = collector.WherePasses(elementCategoryFilter).ToElements().Count;
                quantities.Add(category.ToString(), count);
            }

            return quantities;
        }

        private int GetElementCountWithTypes(Autodesk.Revit.DB.Document doc, BuiltInCategory category)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);

            // Filter elements by the specified category.
            ElementCategoryFilter categoryFilter = new ElementCategoryFilter(category);
            collector.WherePasses(categoryFilter);

            // Filter out element types to get only actual instances.
            ElementClassFilter instanceFilter = new ElementClassFilter(typeof(FamilyInstance));
            collector.WherePasses(instanceFilter);

            // Get the count of instances.
            int instanceCount = collector.GetElementCount();


            // Include the count of element types.
            int typeCount = 0;
            if (category != BuiltInCategory.OST_Walls)
            {
                //FilteredElementCollector typeCollector = new FilteredElementCollector(doc);
                //ElementClassFilter typeFilter = new ElementClassFilter(category);
                //typeCollector.WherePasses(typeFilter);
                //typeCount = typeCollector.GetElementCount();
            }

            return instanceCount + typeCount;
        }
    }
}
