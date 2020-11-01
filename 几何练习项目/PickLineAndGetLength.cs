using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 几何练习项目
{
    [Transaction(TransactionMode.Manual)]
    class PickLineAndGetLength : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            // 选取线
            Reference refer = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Edge);
            object obj  = doc.GetElement(refer).GetGeometryObjectFromReference(refer);
            Edge edge = obj as Edge;
            // 显示获取的结果
            TaskDialog.Show("LineLength", edge.ApproximateLength.ToString("0.00"));
            return Result.Succeeded;
        }
    }
}
