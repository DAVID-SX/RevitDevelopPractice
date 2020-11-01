using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace DimensionMark
{
    [Transaction(TransactionMode.Manual)]
    class AutomaticallyCreateDimensionMark : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // 【获取句柄】
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            // 【获取墙】
            Wall wall = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element)) as Wall;
            if (wall != null)
            {
                // 【获取定位线】
                Line wallLine = (wall.Location as LocationCurve).Curve as Line;
                // 【获取方向，用来寻找参照】
                XYZ wallDir = wallLine.Direction;
                // 【获取墙定位线的参照】
                ReferenceArray refArry = new ReferenceArray();
                // 【定义几何选项】
                Options opt = new Options();
                opt.ComputeReferences = true;
                opt.DetailLevel = ViewDetailLevel.Fine;
                //【获取墙的几何】
                GeometryElement gelem = wall.get_Geometry(opt);
                foreach (GeometryObject gobj in gelem)
                {
                    if (gobj is Solid)
                    {
                        Solid solid = gobj as Solid;
                        if (solid.Volume > 0)
                        {
                            foreach (Face face in solid.Faces)
                            {
                                if (face is PlanarFace)
                                {
                                    XYZ faceDir = face.ComputeNormal(new UV());
                                    //判断是否平行
                                    if (faceDir.IsAlmostEqualTo(wallDir) || faceDir.IsAlmostEqualTo(-wallDir))
                                    {
                                        refArry.Append(face.Reference);
                                        TaskDialog.Show("1", "1");
                                    }
                                }
                            }
                        }
                    }
                }
                Transaction trans = new Transaction(doc, "开始标注");
                trans.Start();
                Dimension dimension = doc.Create.NewDimension(doc.ActiveView, wallLine, refArry);
                trans.Commit();
            }
            return Result.Succeeded;
        }
    }
}
