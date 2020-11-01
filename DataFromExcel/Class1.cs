using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFromExcel
{
    [Transaction(TransactionMode.Manual)]
    public class Class1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Excel文件所在的地址
            FileInfo file = new FileInfo(@"C:\Users\11265\Desktop\test001.xlsx");
            using (ExcelPackage excelPackage = new ExcelPackage(file))
            {
                //指定需要读入的sheet名
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets["Sheet1"];
                //比如读取第一行,第一列的值数据
                object a = excelWorksheet.Cells[1, 1].Value;
                //读取第一行,第二列的值为
                object b = excelWorksheet.Cells[1, 2].Value;
                //然后根据需要对a，b转为字符串，或者double，int等..
                //修改第一行,第二列的值为 你好
                excelWorksheet.Cells[1, 2].Value = "你好";
                //然后保存即可
                excelPackage.Save();

                return Result.Succeeded;
            }
        }
    }
}
