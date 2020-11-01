using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowModelInWpf
{
    [Transaction(TransactionMode.Manual)]
    class PreviewCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            MainWindow wpf = new MainWindow();

            PreviewControl pc = new PreviewControl(doc, commandData.Application.ActiveUIDocument.ActiveGraphicalView.Id);
            wpf.MainGrid.Children.Add(pc);
            wpf.ShowDialog();

            return Result.Succeeded;
        }
    }
}
