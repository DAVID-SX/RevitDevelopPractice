using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;
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
    class CreateColumn : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //【01】获取当前文档
            Document doc = commandData.Application.ActiveUIDocument.Document;
            //【02】创建xyz
            XYZ xyz = new XYZ(0, 0, 0);
            //【03】获取族类型
            FamilySymbol familySymbol = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralColumns)
                .OfClass(typeof(FamilySymbol)).FirstOrDefault(x => x.Name == "S_混凝土结构柱_GBZ1") as FamilySymbol;
            //【04】获取标高
            Level level = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).OfClass(typeof(Level))
                .FirstOrDefault(x => x.Name == "S-F01") as Level;
            //【05】开启事务创建柱子
            Transaction trans = new Transaction(doc, "创建柱子");
            trans.Start();
            FamilyInstance column =  doc.Create.NewFamilyInstance(xyz, familySymbol, level, StructuralType.NonStructural);
            trans.Commit();
            //【06】创建完成返回程序执行信息
            return Result.Succeeded;

        }
    }
}
