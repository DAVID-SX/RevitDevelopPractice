using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitCodeTestProject
{
    [Transaction(TransactionMode.Manual)]
    class Code_6_1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // 获取文档和应用
            Document doc = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Application app = commandData.Application.Application;
            UIApplication uiapp = commandData.Application;


            int i = 0;
            int j = 0;

            GetWallGeometry(doc, app, i, j);

            return Result.Succeeded;
        }

        // 【6-1 创建几何选项】
        public Options OptionsGetGeometryOption(Application app)
        {
            Options option = app.Create.NewGeometryOptions();
            option.ComputeReferences = true;                     // 打开几何引用
            option.DetailLevel = ViewDetailLevel.Fine;           //视图的详图程度设置为精细
            return option;
        }

        // 【6-2 获取墙的面和边】
        public void GetWallGeometry(Document doc, Application app, int faceNum, int edgeNum)
        {
            // 通过ID获取元素
            Wall awall = doc.GetElement(new ElementId(379653)) as Wall;
            // 获取option
            Options option = app.Create.NewGeometryOptions();
            option.ComputeReferences = true;                     // 打开几何引用
            option.DetailLevel = ViewDetailLevel.Fine;
            // 获取几何元素
            GeometryElement geomElement = awall.get_Geometry(option);
            foreach (GeometryObject geomObj in geomElement)
            {
                Solid geomSolid = geomObj as Solid;
                if (geomSolid != null)
                {
                    foreach (Face face in geomSolid.Faces)
                    {
                        faceNum++;
                    }
                    foreach (Edge edge in geomSolid.Edges)
                    {
                        edgeNum++;
                    }
                }
            }
            TaskDialog.Show(faceNum.ToString(), edgeNum.ToString());
        }

    }
}
