using assignment2.Views;
using Autodesk.Revit.DB;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Document = Autodesk.Revit.DB.Document;

namespace assignment2.ViewModels
{
    public class ElementQuantityViewModel : INotifyPropertyChanged
    {
        private string _category;
        public string Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged(nameof(Category));
                }
            }
        }

        private double _quantity;
        public double Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        private string _unit;
        public string Unit
        {
            get => _unit;
            set
            {
                if (_unit != value)
                {
                    _unit = value;
                    OnPropertyChanged(nameof(Unit));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class wpfWindowViewModel : INotifyPropertyChanged
    {
        private MyWpfWindow _window;
        public ObservableCollection<ElementQuantityViewModel> ElementQuantities { get; set; }
        public RelayCommand ExportCommand { get; private set; }

        private Autodesk.Revit.DB.Document _doc;

        public wpfWindowViewModel(Document doc, MyWpfWindow window)
        {
            _window = window;
            _doc = doc;
            ElementQuantities = new ObservableCollection<ElementQuantityViewModel>();
            ExtractQuantities();
            ExportCommand = new RelayCommand(ExportQuantities, CanExportQuantities);
        }

        private void ExtractQuantities()
        {
            ElementQuantities.Clear();

            var doorCollector = new FilteredElementCollector(_doc)
                .OfCategory(BuiltInCategory.OST_Doors)
                .WhereElementIsNotElementType();

            int doorCount = doorCollector.Count();

            ElementQuantities.Add(new ElementQuantityViewModel
            {
                Category = "Doors",
                Quantity = doorCount,
                Unit = "Count"
            });

            var windowCollector = new FilteredElementCollector(_doc)
                .OfCategory(BuiltInCategory.OST_Windows)
                .WhereElementIsNotElementType();

            int windowCount = windowCollector.Count();

            ElementQuantities.Add(new ElementQuantityViewModel
            {
                Category = "Windows",
                Quantity = windowCount,
                Unit = "Count"
            });

            var wallCollector = new FilteredElementCollector(_doc)
                .OfCategory(BuiltInCategory.OST_Walls)
                .WhereElementIsNotElementType();
            int wallCount = wallCollector.Count();
            double totalWallVolume = 0;
            foreach (var wall in wallCollector)
            {
                Parameter volumeParameter = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                if (volumeParameter != null)
                {
                    totalWallVolume += volumeParameter.AsDouble();
                }
            }

            ElementQuantities.Add(new ElementQuantityViewModel
            {
                Category = "Walls",
                Quantity = wallCount,
                Unit = "Count"
            });
        }

        private void ExportQuantities()
        {
            StringBuilder csvContent = new StringBuilder();

            csvContent.AppendLine("Category,Quantity,Unit,Materials");

            foreach (var item in ElementQuantities)
            {
                string materials = GetElementMaterials(item.Category);

                csvContent.AppendLine($"{item.Category},{item.Quantity},{item.Unit},{materials}");
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
            saveFileDialog.FileName = "ElementQuantities"; // Default file name
            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string filePath = saveFileDialog.FileName;
                File.WriteAllText(filePath, csvContent.ToString());

                MessageBox.Show("Quantities and materials exported successfully.", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            _window.Close();
        }

        private string GetElementMaterials(string category)
        {
            BuiltInCategory builtInCategory;
            switch (category)
            {
                case "Doors":
                    builtInCategory = BuiltInCategory.OST_Doors;
                    break;
                case "Walls":
                    builtInCategory = BuiltInCategory.OST_Walls;
                    break;
                case "Windows":
                    builtInCategory = BuiltInCategory.OST_Windows;
                    break;
                default:
                    return string.Empty;
            }

            FilteredElementCollector collector = new FilteredElementCollector(_doc)
                .OfCategory(builtInCategory)
                .WhereElementIsNotElementType();

            List<string> materials = new List<string>();

            foreach (Element element in collector)
            {
                ICollection<ElementId> materialIds = element.GetMaterialIds(false);

                foreach (ElementId materialId in materialIds)
                {
                    Material material = _doc.GetElement(materialId) as Material;
                    if (material != null)
                    {
                        materials.Add(material.Name);
                    }
                }
            }

            return string.Join(",", materials);
        }


        private bool CanExportQuantities()
        {
            return ElementQuantities.Count > 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
