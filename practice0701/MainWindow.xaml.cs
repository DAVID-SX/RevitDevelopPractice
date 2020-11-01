using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace practice0701
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                ColumnDefinition clmDefinition = new ColumnDefinition();
                gridMain.ColumnDefinitions.Add(clmDefinition);
                RowDefinition rowDefinition = new RowDefinition();
                gridMain.RowDefinitions.Add(rowDefinition);
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Button btn = new Button();
                    btn.Content = "(" + i + "," + j + ")";
                    Grid.SetRow(btn, i);
                    Grid.SetColumn(btn, j);
                    gridMain.Children.Add(btn);
                }
            }
        }
    }
}
