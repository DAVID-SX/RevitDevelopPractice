using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace DimensionMark
{
    [Transaction(TransactionMode.Manual)]
    public class Class1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            // 【获取想要标注的墙】
            Wall wall = doc.GetElement(uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element)) as Wall;
            // 【获取墙的中心线】
            Line line = (wall.Location as LocationCurve).Curve as Line;
            // 【获取ReferenceArray】
            ReferenceArray references = new ReferenceArray();
            references.Append(uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Face));
            references.Append(uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Face));
            // 【新建事务准备开始创建标注】
            Transaction trans = new Transaction(doc, "创建标注");
            trans.Start();
            // 【创建标注】
            doc.Create.NewDimension(doc.ActiveView, line, references);
            trans.Commit();

            return Result.Succeeded;
        }
    }
}
