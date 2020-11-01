using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project03
{
    [Transaction(TransactionMode.Manual)]
    class CreateWallDemo : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //【01】获取当前文档（Document）
            Document doc = commandData.Application.ActiveUIDocument.Document;
            //【02】获取Curve
            XYZ point1 = new XYZ(0, 0, 0);
            XYZ point2 = new XYZ(10, 10, 0);
            Line line = Line.CreateBound(point1, point2);
            //【03】获取walltype
            WallType wallType = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls)
                .OfClass(typeof(WallType)).FirstOrDefault(x => x.Name == "A_混凝土砌块墙_100mm") as WallType;
            //【04】获取Level
            Level level = new FilteredElementCollector(doc).OfClass(typeof(Level))
                .FirstOrDefault(x => x.Name == "A-F01") as Level;
            //【05】声明高度和偏移值
            double height = 10 / 0.3048;
            double offset = 0.1 / 0.3048;
            //【06】开启事务创建墙体
            Transaction trans = new Transaction(doc, "创建墙体");
            trans.Start();
            Wall.Create(doc, line, wallType.Id, level.Id, height, offset, false, false);
            trans.Commit();
            //【07】程序结束返回执行信息
            return Result.Succeeded;
        }
    }


}
