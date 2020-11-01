using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
// 为几何库取一个别名
using DyGeometry = Autodesk.DesignScript.Geometry;

namespace ImportDynamo
{
    [Transaction(TransactionMode.Manual)]
    public class Class1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // 【利用Dynamo库创建弧线】
            DyGeometry.Point point1 = DyGeometry.Point.ByCoordinates(0, 0, 0);
            DyGeometry.Point point2 = DyGeometry.Point.ByCoordinates(0, 5, 3);
            DyGeometry.Point point3 = DyGeometry.Point.ByCoordinates(0, 10, 0);
            DyGeometry.Arc arc1 = DyGeometry.Arc.ByThreePoints(point1, point2, point3);
            // 【利用Dynamo创建直线】
            DyGeometry.Point point4 = DyGeometry.Point.ByCoordinates(10, 10, 0);
            DyGeometry.Point point5 = DyGeometry.Point.ByCoordinates(10, 0, 0);
            DyGeometry.Line line1 = DyGeometry.Line.ByStartPointEndPoint(point4, point5);
            // 【利用Dynamo在直线和弧线之间创建连接线】
            DyGeometry.Curve curve1 = DyGeometry.Curve.ByBlendBetweenCurves(arc1, line1);
            // 【利用Dynamo获取生成的连接线上的某个点】
            DyGeometry.Point outPoint = curve1.PointAtSegmentLength(3);
            // 【转化为Revit的坐标点】
            XYZ result = new XYZ(outPoint.X / 0.3048, outPoint.Y / 0.3048, outPoint.Z / 0.3048);
            // 【输出生成的结果】
            TaskDialog.Show("Result", $"X:{result.X.ToString("0.00")}-Y:{result.Y.ToString("0.00")}-Z:{result.Z.ToString("0.00")}");
            return Result.Succeeded;
        }
    }
}
