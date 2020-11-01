using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 几何练习项目
{
    [Transaction(TransactionMode.Manual)]
    public class GetWallCurvePoint : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // 获取相关文档
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = commandData.Application.ActiveUIDocument.Document;

            Reference reference = uidoc.Selection.PickObject(ObjectType.Element);
            Element element = doc.GetElement(reference);
            Wall wall = element as Wall;
            LocationCurve locationCurve = (LocationCurve)wall.Location;
            Curve curve = locationCurve.Curve;
            XYZ startPoint = curve.GetEndPoint(0);
            XYZ endPoint = curve.GetEndPoint(1);



            TaskDialog.Show("hello, revit2020",startPoint.ToString() + "\n" + endPoint.ToString());
            return Result.Succeeded;
        }
    }
}
