using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project03
{
    [Transaction(TransactionMode.Manual)]
    class GetWindowParameter : IExternalCommand

    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //【01】获取文档
            Document doc = commandData.Application.ActiveUIDocument.Document;
            //【02】获取目标窗户
            FamilyInstance fam = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Windows).OfClass(typeof(FamilyInstance))
                .FirstOrDefault(x => x.Name == "A_双扇推拉窗_LC1215_1200×1500mm") as FamilyInstance;
            //【03】获取窗户的底标高
            double bottomHeight = fam.LookupParameter("底高度").AsDouble() * 0.3048;
            //【04】输出获取到的参数
            TaskDialog.Show("窗户高度", $"{bottomHeight}");
            //【05】修改窗户的底标高
            Transaction trans = new Transaction(doc,"修改窗户的底标高");
            trans.Start();
            fam.LookupParameter("底高度").Set(1.5 / 0.3048);
            trans.Commit();
            //【06】输出修改后的窗户底标高
            double newButtomHeight = fam.LookupParameter("底高度").AsDouble() * 0.3048;
            TaskDialog.Show("窗户高度", $"{newButtomHeight}");
            //【07】返回执行结果
            return Result.Succeeded;
        }
    }
}
