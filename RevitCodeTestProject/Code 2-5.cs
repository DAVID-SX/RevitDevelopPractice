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
    class Code_2_5 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // 获取文档和应用
            Document doc = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Application app = commandData.Application.Application;
            UIApplication uiapp = commandData.Application;

            string ele = doc.SiteLocation.Elevation.ToString();
            string n = app.VersionNumber;
            TaskDialog.Show("version", n);
            return Result.Succeeded;
        }
    }
}
