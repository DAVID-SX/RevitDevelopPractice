using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Linq;

namespace RfaByCode
{
    [Transaction(TransactionMode.Manual)]
    public class SDKFamilyCreate : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //【通过APP打开族文件】
            Application app = commandData.Application.Application;
            string rftPath = @"C:\ProgramData\Autodesk\RVT 2020\Family Templates\Chinese\公制柱.rft";
            Document docRfa = app.NewFamilyDocument(rftPath);
            //【开始创建】
            Transaction trans = new Transaction(docRfa, "创建常规模型");
            trans.Start();
            CreateExtrusion(docRfa);
            CreateBlend(docRfa);
            CreateRevolution(docRfa);
            CreateSweep(docRfa, app);
            CreateSweptBlend(docRfa, app);
            trans.Commit();
            //【保存出来】
            SaveAsOptions saveAsOptions = new SaveAsOptions();
            saveAsOptions.MaximumBackups = 1;
            saveAsOptions.OverwriteExistingFile = true;
            docRfa.SaveAs(@"E:\编程创建的柱子.rfa", saveAsOptions);
            docRfa.Close(false);
            return Result.Succeeded;
        }

        /// <summary>
        /// Create one rectangular extrusion
        /// </summary>
        private void CreateExtrusion(Document docRfa)
        {
            try
            {
                #region Create rectangle profile
                CurveArrArray curveArrArray = new CurveArrArray();
                CurveArray curveArray1 = new CurveArray();

                Autodesk.Revit.DB.XYZ normal = Autodesk.Revit.DB.XYZ.BasisZ;
                SketchPlane sketchPlane = CreateSketchPlane(docRfa, normal, Autodesk.Revit.DB.XYZ.Zero);

                // create one rectangular extrusion
                Autodesk.Revit.DB.XYZ p0 = Autodesk.Revit.DB.XYZ.Zero;
                Autodesk.Revit.DB.XYZ p1 = new Autodesk.Revit.DB.XYZ(10, 0, 0);
                Autodesk.Revit.DB.XYZ p2 = new Autodesk.Revit.DB.XYZ(10, 10, 0);
                Autodesk.Revit.DB.XYZ p3 = new Autodesk.Revit.DB.XYZ(0, 10, 0);
                Line line1 = Line.CreateBound(p0, p1);
                Line line2 = Line.CreateBound(p1, p2);
                Line line3 = Line.CreateBound(p2, p3);
                Line line4 = Line.CreateBound(p3, p0);
                curveArray1.Append(line1);
                curveArray1.Append(line2);
                curveArray1.Append(line3);
                curveArray1.Append(line4);

                curveArrArray.Append(curveArray1);
                #endregion
                // here create rectangular extrusion
                Extrusion rectExtrusion = docRfa.FamilyCreate.NewExtrusion(true, curveArrArray, sketchPlane, 10);
                // move to proper place
                Autodesk.Revit.DB.XYZ transPoint1 = new Autodesk.Revit.DB.XYZ(-16, 0, 0);
                ElementTransformUtils.MoveElement(docRfa, rectExtrusion.Id, transPoint1);
            }
            catch (Exception e)
            {
                TaskDialog.Show("Error!!!", "Unexpected exceptions occur in CreateExtrusion: " + e.ToString() + "\r\n");
            }
        }

        /// <summary>
        /// Create one blend
        /// </summary>
        private void CreateBlend(Document docRfa)
        {
            try
            {
                #region Create top and base profiles
                CurveArray topProfile = new CurveArray();
                CurveArray baseProfile = new CurveArray();

                Autodesk.Revit.DB.XYZ normal = Autodesk.Revit.DB.XYZ.BasisZ;
                SketchPlane sketchPlane = CreateSketchPlane(docRfa, normal, Autodesk.Revit.DB.XYZ.Zero);

                // create one blend
                Autodesk.Revit.DB.XYZ p00 = Autodesk.Revit.DB.XYZ.Zero;
                Autodesk.Revit.DB.XYZ p01 = new Autodesk.Revit.DB.XYZ(10, 0, 0);
                Autodesk.Revit.DB.XYZ p02 = new Autodesk.Revit.DB.XYZ(10, 10, 0);
                Autodesk.Revit.DB.XYZ p03 = new Autodesk.Revit.DB.XYZ(0, 10, 0);
                Line line01 = Line.CreateBound(p00, p01);
                Line line02 = Line.CreateBound(p01, p02);
                Line line03 = Line.CreateBound(p02, p03);
                Line line04 = Line.CreateBound(p03, p00);

                baseProfile.Append(line01);
                baseProfile.Append(line02);
                baseProfile.Append(line03);
                baseProfile.Append(line04);

                Autodesk.Revit.DB.XYZ p10 = new Autodesk.Revit.DB.XYZ(5, 2, 10);
                Autodesk.Revit.DB.XYZ p11 = new Autodesk.Revit.DB.XYZ(8, 5, 10);
                Autodesk.Revit.DB.XYZ p12 = new Autodesk.Revit.DB.XYZ(5, 8, 10);
                Autodesk.Revit.DB.XYZ p13 = new Autodesk.Revit.DB.XYZ(2, 5, 10);
                Line line11 = Line.CreateBound(p10, p11);
                Line line12 = Line.CreateBound(p11, p12);
                Line line13 = Line.CreateBound(p12, p13);
                Line line14 = Line.CreateBound(p13, p10);

                topProfile.Append(line11);
                topProfile.Append(line12);
                topProfile.Append(line13);
                topProfile.Append(line14);
                #endregion
                // here create one blend
                Blend blend = docRfa.FamilyCreate.NewBlend(true, topProfile, baseProfile, sketchPlane);
                // move to proper place
                Autodesk.Revit.DB.XYZ transPoint1 = new Autodesk.Revit.DB.XYZ(0, 11, 0);
                ElementTransformUtils.MoveElement(docRfa, blend.Id, transPoint1);
            }
            catch (Exception e)
            {
                TaskDialog.Show("Error!!!", "Unexpected exceptions occur in CreateBlend: " + e.ToString() + "\r\n");
            }
        }

        /// <summary>
        /// Create one rectangular profile revolution
        /// </summary>
        private void CreateRevolution(Document docRfa)
        {
            try
            {
                #region Create rectangular profile
                CurveArrArray curveArrArray = new CurveArrArray();
                CurveArray curveArray = new CurveArray();

                Autodesk.Revit.DB.XYZ normal = Autodesk.Revit.DB.XYZ.BasisZ;
                SketchPlane sketchPlane = CreateSketchPlane(docRfa, normal, Autodesk.Revit.DB.XYZ.Zero);

                // create one rectangular profile revolution
                Autodesk.Revit.DB.XYZ p0 = Autodesk.Revit.DB.XYZ.Zero;
                Autodesk.Revit.DB.XYZ p1 = new Autodesk.Revit.DB.XYZ(10, 0, 0);
                Autodesk.Revit.DB.XYZ p2 = new Autodesk.Revit.DB.XYZ(10, 10, 0);
                Autodesk.Revit.DB.XYZ p3 = new Autodesk.Revit.DB.XYZ(0, 10, 0);
                Line line1 = Line.CreateBound(p0, p1);
                Line line2 = Line.CreateBound(p1, p2);
                Line line3 = Line.CreateBound(p2, p3);
                Line line4 = Line.CreateBound(p3, p0);

                Autodesk.Revit.DB.XYZ pp = new Autodesk.Revit.DB.XYZ(1, -1, 0);
                Line axis1 = Line.CreateBound(Autodesk.Revit.DB.XYZ.Zero, pp);
                curveArray.Append(line1);
                curveArray.Append(line2);
                curveArray.Append(line3);
                curveArray.Append(line4);

                curveArrArray.Append(curveArray);
                #endregion
                // here create rectangular profile revolution
                Revolution revolution1 = docRfa.FamilyCreate.NewRevolution(true, curveArrArray, sketchPlane, axis1, -Math.PI, 0);
                // move to proper place
                Autodesk.Revit.DB.XYZ transPoint1 = new Autodesk.Revit.DB.XYZ(0, 32, 0);
                ElementTransformUtils.MoveElement(docRfa, revolution1.Id, transPoint1);
            }
            catch (Exception e)
            {
                TaskDialog.Show("Error!!!","Unexpected exceptions occur in CreateRevolution: " + e.ToString() + "\r\n");
            }
        }

        /// <summary>
        /// Create one sweep
        /// </summary>
        private void CreateSweep(Document docRfa, Application app)
        {
            try
            {
                #region Create rectangular profile and path curve
                CurveArrArray arrarr = new CurveArrArray();
                CurveArray arr = new CurveArray();

                Autodesk.Revit.DB.XYZ normal = Autodesk.Revit.DB.XYZ.BasisZ;
                SketchPlane sketchPlane = CreateSketchPlane(docRfa, normal, Autodesk.Revit.DB.XYZ.Zero);

                Autodesk.Revit.DB.XYZ pnt1 = new Autodesk.Revit.DB.XYZ(0, 0, 0);
                Autodesk.Revit.DB.XYZ pnt2 = new Autodesk.Revit.DB.XYZ(2, 0, 0);
                Autodesk.Revit.DB.XYZ pnt3 = new Autodesk.Revit.DB.XYZ(1, 1, 0);
                arr.Append(Arc.Create(pnt2, 1.0d, 0.0d, 180.0d, Autodesk.Revit.DB.XYZ.BasisX, Autodesk.Revit.DB.XYZ.BasisY));
                arr.Append(Arc.Create(pnt1, pnt3, pnt2));
                arrarr.Append(arr);
                SweepProfile profile = app.Create.NewCurveLoopsProfile(arrarr);

                Autodesk.Revit.DB.XYZ pnt4 = new Autodesk.Revit.DB.XYZ(10, 0, 0);
                Autodesk.Revit.DB.XYZ pnt5 = new Autodesk.Revit.DB.XYZ(0, 10, 0);
                Curve curve = Line.CreateBound(pnt4, pnt5);

                CurveArray curves = new CurveArray();
                curves.Append(curve);
                #endregion
                // here create one sweep with two arcs formed the profile
                Sweep sweep1 = docRfa.FamilyCreate.NewSweep(true, curves, sketchPlane, profile, 0, ProfilePlaneLocation.Start);
                // move to proper place
                Autodesk.Revit.DB.XYZ transPoint1 = new Autodesk.Revit.DB.XYZ(11, 0, 0);
                ElementTransformUtils.MoveElement(docRfa, sweep1.Id, transPoint1);
            }
            catch (Exception e)
            {
                TaskDialog.Show("Error!!!", "Unexpected exceptions occur in CreateSweep: " + e.ToString() + "\r\n");
            }
        }

        /// <summary>
        /// Create one SweptBlend
        /// </summary>
        private void CreateSweptBlend(Document docRfa, Application app)
        {
            try
            {
                #region Create top and bottom profiles and path curve
                Autodesk.Revit.DB.XYZ pnt1 = new Autodesk.Revit.DB.XYZ(0, 0, 0);
                Autodesk.Revit.DB.XYZ pnt2 = new Autodesk.Revit.DB.XYZ(1, 0, 0);
                Autodesk.Revit.DB.XYZ pnt3 = new Autodesk.Revit.DB.XYZ(1, 1, 0);
                Autodesk.Revit.DB.XYZ pnt4 = new Autodesk.Revit.DB.XYZ(0, 1, 0);
                Autodesk.Revit.DB.XYZ pnt5 = new Autodesk.Revit.DB.XYZ(0, 0, 1);

                CurveArrArray arrarr1 = new CurveArrArray();
                CurveArray arr1 = new CurveArray();
                arr1.Append(Line.CreateBound(pnt1, pnt2));
                arr1.Append(Line.CreateBound(pnt2, pnt3));
                arr1.Append(Line.CreateBound(pnt3, pnt4));
                arr1.Append(Line.CreateBound(pnt4, pnt1));
                arrarr1.Append(arr1);

                Autodesk.Revit.DB.XYZ pnt6 = new Autodesk.Revit.DB.XYZ(0.5, 0, 0);
                Autodesk.Revit.DB.XYZ pnt7 = new Autodesk.Revit.DB.XYZ(1, 0.5, 0);
                Autodesk.Revit.DB.XYZ pnt8 = new Autodesk.Revit.DB.XYZ(0.5, 1, 0);
                Autodesk.Revit.DB.XYZ pnt9 = new Autodesk.Revit.DB.XYZ(0, 0.5, 0);
                CurveArrArray arrarr2 = new CurveArrArray();
                CurveArray arr2 = new CurveArray();
                arr2.Append(Line.CreateBound(pnt6, pnt7));
                arr2.Append(Line.CreateBound(pnt7, pnt8));
                arr2.Append(Line.CreateBound(pnt8, pnt9));
                arr2.Append(Line.CreateBound(pnt9, pnt6));
                arrarr2.Append(arr2);

                SweepProfile bottomProfile = app.Create.NewCurveLoopsProfile(arrarr1);
                SweepProfile topProfile = app.Create.NewCurveLoopsProfile(arrarr2);

                Autodesk.Revit.DB.XYZ pnt10 = new Autodesk.Revit.DB.XYZ(5, 0, 0);
                Autodesk.Revit.DB.XYZ pnt11 = new Autodesk.Revit.DB.XYZ(0, 20, 0);
                Curve curve = Line.CreateBound(pnt10, pnt11);

                Autodesk.Revit.DB.XYZ normal = Autodesk.Revit.DB.XYZ.BasisZ;
                SketchPlane sketchPlane = CreateSketchPlane(docRfa, normal, Autodesk.Revit.DB.XYZ.Zero);
                #endregion
                // here create one swept blend
                SweptBlend newSweptBlend1 = docRfa.FamilyCreate.NewSweptBlend(true, curve, sketchPlane, bottomProfile, topProfile);
                // move to proper place
                Autodesk.Revit.DB.XYZ transPoint1 = new Autodesk.Revit.DB.XYZ(11, 32, 0);
                ElementTransformUtils.MoveElement(docRfa, newSweptBlend1.Id, transPoint1);
            }
            catch (Exception e)
            {
                TaskDialog.Show("Error!!!", "Unexpected exceptions occur in CreateSweptBlend: " + e.ToString() + "\r\n");
            }
        }

        internal SketchPlane CreateSketchPlane(Document docRfa, XYZ normal, XYZ origin)
        {
            // First create a Geometry.Plane which need in NewSketchPlane() method
            Plane geometryPlane = Plane.CreateByNormalAndOrigin(normal, origin);
            if (null == geometryPlane)  // assert the creation is successful
            {
                throw new Exception("Create the geometry plane failed.");
            }
            // Then create a sketch plane using the Geometry.Plane
            SketchPlane plane = SketchPlane.Create(docRfa, geometryPlane);
            // throw exception if creation failed
            if (null == plane)
            {
                throw new Exception("Create the sketch plane failed.");
            }
            return plane;
        }
    }
   
}
