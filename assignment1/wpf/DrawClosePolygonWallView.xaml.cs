using assignment1.commands;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using System.Collections.Generic;
using System.Windows;

namespace assignment1.wpf
{
    /// <summary>
    /// Interaction logic for assignment1View.xaml
    /// </summary>
    public partial class DrawClosePolygonWallView : Window
    {
        ExternalCommandData commandDataGlobal;
        public DrawClosePolygonWallView(ExternalCommandData commandData)
        {
            InitializeComponent();

            commandDataGlobal = commandData;

            var document = commandDataGlobal.Application.ActiveUIDocument.Document;

            FilteredElementCollector collector = new FilteredElementCollector(document);

            IList<Element> wallTypes = collector.OfClass(typeof(WallType)).ToElements();

            foreach (var wt in wallTypes)
            {
                wallTypeComboBox.Items.Add(wt.Name);
            }
        }

        private void DrawBtn_Click(object sender, RoutedEventArgs e)
        {
            if (wallTypeComboBox.SelectedItem == null) return;
            string selectedWallType = wallTypeComboBox.SelectedItem.ToString();
            DrawClosePolygonWall.DrawPentagon(commandDataGlobal, selectedWallType);
        }
    }
}
