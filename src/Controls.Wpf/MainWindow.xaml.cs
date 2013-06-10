using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Controls.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<TestDataItem> Items
        {
            get
            {
                return new List<TestDataItem> {
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                    new TestDataItem {Value1 = "One", Value2 = "Two", Value3 = "Three"},
                };
            }
        } 

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
