using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JingLingProject
{
    [Transaction(TransactionMode.Manual)]
    class ProShow : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //【01】获取当前文档（Document）
            Document doc = commandData.Application.ActiveUIDocument.Document;

            MainWindow mainWindow = new MainWindow();
            mainWindow.ShowDialog(); //模态窗体

            TaskDialog.Show("统计完成", "装修工程量已统计完成！请打开生成的Excel文件查看。");

            //【07】程序结束返回执行信息
            return Result.Succeeded;
        }
    }
}
