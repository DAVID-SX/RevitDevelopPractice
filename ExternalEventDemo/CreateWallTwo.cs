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
    class CreateWallTwo : IExternalEventHandler
    {
        //接受文本框输入的参数值
        public double wallHeight { get; set; }
        public void Execute(UIApplication app)
        {
            //【01】获取文档
            // 在这里要把commandData.Application换成app
            Document doc = app.ActiveUIDocument.Document;
            double height = wallHeight / 0.3048;


            //【02】获取Curve
            XYZ point1 = new XYZ(20, 20, 0);
            XYZ point2 = new XYZ(30, 30, 0);
            Line line = Line.CreateBound(point1, point2);
            //【03】获取walltype
            WallType wallType = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls)
            .OfClass(typeof(WallType)).FirstOrDefault(x => x.Name == "A_混凝土砌块墙_100mm") as WallType;
            //【04】获取Level
            Level level = new FilteredElementCollector(doc).OfClass(typeof(Level))
            .FirstOrDefault(x => x.Name == "A-F01") as Level;
            //【05】声明高度和偏移值
            // double height = 15 / 0.3048;
            // double offset = 0.1 / 0.3048;
            //【06】开启事务创建墙体
            Transaction trans = new Transaction(doc, "创建墙体");
            trans.Start();
            Wall.Create(doc, line, wallType.Id, level.Id, height, 0, false, false);
            trans.Commit();
            //【07】因为方法无返回值，所有不需要return
        }

        public string GetName()
        {
            // 固定套路，返回类名字符串即可
            return "CreateWall";
        }
    }
}
