using Autodesk.Revit.UI;
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

namespace ExternalEventDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //【01】注册外部事件1
        CreateWall createWallCommand = null;
        ExternalEvent createWallEvent = null;
        //【01】注册外部事件2
        CreateWallTwo createWallTwoCommand = null;
        ExternalEvent createWallTwoEvent = null;

        public MainWindow()
        {
            InitializeComponent();
            //【02】初始化第一个命令
            createWallCommand = new CreateWall();
            createWallEvent = ExternalEvent.Create(createWallCommand);
            //【02】初始化第二个命令
            createWallTwoCommand = new CreateWallTwo();
            createWallTwoEvent = ExternalEvent.Create(createWallTwoCommand);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //【03】执行命令
            createWallCommand.wallHeight = Convert.ToDouble( this.textBox.Text );  //属性传值1

            createWallEvent.Raise();
        }

        private void ButtonTwo_Click(object sender, RoutedEventArgs e)
        {
            createWallTwoCommand.wallHeight = Convert.ToDouble(this.textBowTwo.Text);  //属性传值1
            createWallTwoEvent.Raise();
        }
    }
}
