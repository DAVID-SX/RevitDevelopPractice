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
    class PickFaceAndGetArea : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // 获取相关文档
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = commandData.Application.ActiveUIDocument.Document;
            // 选择面
            Reference refer = uidoc.Selection.PickObject(ObjectType.Face);
            Object obj = doc.GetElement(refer).GetGeometryObjectFromReference(refer);
            Face face = obj as Face;
            // 显示结果
            TaskDialog.Show("result", face.Area.ToString("0.00"));
            face.Area.ToString("0.00");
            return Result.Succeeded;
        }
    }
}
