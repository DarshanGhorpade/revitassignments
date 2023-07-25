using assignment2.ViewModels;
using System.Collections.Generic;
using System.Windows;

namespace assignment2.Views
{
    /// <summary>
    /// Interaction logic for MyWpfWindow.xaml
    /// </summary>
    public partial class MyWpfWindow : Window
    {
        public wpfWindowViewModel ViewModel { get; private set; }

        public MyWpfWindow()
        {
            InitializeComponent();
        }
    }
}
