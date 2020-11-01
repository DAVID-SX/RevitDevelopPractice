using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Linq;

namespace RfaByCode
{
    [Transaction(TransactionMode.Manual)]
    public class CreateRfaAndSave : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //【通过APP打开族文件】
            Application app = commandData.Application.Application;
            string rftPath = @"C:\ProgramData\Autodesk\RVT 2020\Family Templates\Chinese\公制柱.rft";
            Document docRfa = app.NewFamilyDocument(rftPath);
            //【创建几何】
            double width = 2.5 / 0.3048;
            double height = 10 / 0.3048;
            XYZ point1 = new XYZ(width, width, 0);
            XYZ point2 = new XYZ(-width, width, 0);
            XYZ point3 = new XYZ(-width, -width, 0);
            XYZ point4 = new XYZ(width, -width, 0);
            CurveArray curveArray = new CurveArray();
            curveArray.Append(Line.CreateBound(point1, point2));
            curveArray.Append(Line.CreateBound(point2, point3));
            curveArray.Append(Line.CreateBound(point3, point4));
            curveArray.Append(Line.CreateBound(point4, point1));
            CurveArrArray curveArrArray = new CurveArrArray();
            curveArrArray.Append(curveArray);
            //【创建草图平面】
            SketchPlane sketchPlane = new FilteredElementCollector(docRfa).OfClass(typeof(SketchPlane)).First(x => x.Name == "低于参照标高") as SketchPlane;
            //【开始创建】
            Transaction trans = new Transaction(docRfa, "创建柱族");
            trans.Start();
            Extrusion extrusion = docRfa.FamilyCreate.NewExtrusion(true, curveArrArray, sketchPlane, height);
            trans.Commit();
            //【保存出来】
            SaveAsOptions saveAsOptions = new SaveAsOptions();
            saveAsOptions.MaximumBackups = 1;
            saveAsOptions.OverwriteExistingFile = true;
            docRfa.SaveAs(@"E:\编程创建的柱子.rfa", saveAsOptions);
            docRfa.Close(false);
            return Result.Succeeded;
        }
    }
}
