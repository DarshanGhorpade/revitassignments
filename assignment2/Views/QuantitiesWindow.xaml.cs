using System.Collections.Generic;
using System.Windows;

namespace assignment2.Views
{
    /// <summary>
    /// Interaction logic for View1.xaml
    /// </summary>
    public partial class QuantitiesWindow : Window
    {
        public QuantitiesWindow(Dictionary<string, int> quantities)
        {
            InitializeComponent();
            UpdateUI(quantities);
        }

        private void UpdateUI(Dictionary<string, int> quantities)
        {
            foreach (var kvp in quantities)
            {
                QuantityListBox.Items.Add($"{kvp.Key}: {kvp.Value}");
            }
        }
    }
}
