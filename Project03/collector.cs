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
    class Collector : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //【00】收集当前文档的元素
            //获取界面交互的uidoc
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            //获取实际内容（交互界面之后包含数据信息）的doc
            Document doc = commandData.Application.ActiveUIDocument.Document;


            //【01】创建收集器
            FilteredElementCollector collector = new FilteredElementCollector(doc);


            //【02】过滤，获取墙元素
            //【02-01】快速过滤方法
            // collector.OfCategory(BuiltInCategory.OST_Walls).OfClass(typeof(Wall));
            //【02-02】通用过滤方法-两种方法效果一样，通用方法适用于复杂的过滤条件
            ElementCategoryFilter elementCategoryFileter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);
            ElementClassFilter elementClassFilter = new ElementClassFilter(typeof(Wall));
            collector.WherePasses(elementCategoryFileter).WherePasses(elementClassFilter);


            //【03】某种墙类型所有族实例构件的获取
            //【03-01】foreach获取
            List<Element> elementList = new List<Element>();
            foreach(var item in collector)
            {
                if(item.Name == "A_砖墙_50mm")
                {
                    elementList.Add(item);
                }
            }
            //【03-02】转为list处理
            // List<Element> elementListTwo = collector.ToList<Element>();
            // 之后操作列表筛选出自己需要的元素
            // 【03-03】linq表达式
            // 代码的效果同03-01的效果相同
            //var wallElement = from element in collector
            //                  where element.Name == "A_砖墙_50mm"
            //                  select element;
            //Element wallInstance = wallElement.LastOrDefault<Element>();


            //【04】某个族实例的获取
            //【04-01】确定只有一个实例
            //【04-01-01】list获取
            //Element ele = elementList[0];
            //【04-01-02】IEnumberable获取
            //Element ele = wallElement.FirstOrDefault<Element>();
            //【04-01-03】lambda的一种写法
            //Element ele = collector.OfCategory(BuiltInCategory.OST_Walls).OfClass(typeof(Wall)).FirstOrDefault<Element>(x => x.Name == "墙体名称");
            //【04-02】有很多实例，但是只想获取其中一个，可以使用ElementID或者根据一些特征
            Element ele = doc.GetElement(new ElementId(493697));


            //【05】判断与转化
            foreach (var item in elementList)
            {
                //先判断
                if(item is Wall)
                {
                    //再转化
                    //as转化方法（推荐）
                    Wall wall = item as Wall;
                    //强制转换方法（速度慢，有可能丢失数据）
                    Wall walltwo = (Wall)item;
                }
            }


            //【06】显示收集到的元素
            var sel = uiDoc.Selection.GetElementIds();

            foreach (var item in elementList)
            {
                // TaskDialog.Show("resultOfSelection", item.Name);
                sel.Add(item.Id);
            }

            uiDoc.Selection.SetElementIds(sel);
            //返回执行信息
            return Result.Succeeded;
        }
    }
}
