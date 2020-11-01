using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalEventDemo
{
    [Transaction(TransactionMode.Manual)]
    //这里把方法类型改为了公共方法
    public class MainProgram : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show(); //非模态窗体
            return Result.Succeeded;
        }
    }
}
