using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Architecture;

namespace GetMaterial
{
    [Transaction(TransactionMode.Manual)]
    public class GetMaterial : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //获取当前文档
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = commandData.Application.ActiveUIDocument.Document;
            //指定输出信息的文件
            FileStream fs = new FileStream(@"C:\Users\11265\Desktop\0.txt", FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            //指定存放贴图路径的列表
            List<string> oldPathList = new List<string>();
            List<string> newPathList = new List<string>();
            List<string> fileNameList = new List<string>();
            Dictionary<string, string> dicPath = new Dictionary<string, string>();




            // 获取元素
            IList<Reference> referList = uiDoc.Selection.PickObjects(ObjectType.Element, "请框选需要的构件");
            Element elem = doc.GetElement(referList.First().ElementId);
            //获取元素的材质
            ICollection<ElementId> ids = elem.GetMaterialIds(false);
            Material material = doc.GetElement(ids.First()) as Material;
            AppearanceAssetElement appearanceAssetElement = doc.GetElement(material.AppearanceAssetId) as AppearanceAssetElement;
            Asset asset = appearanceAssetElement.GetRenderingAsset();
            for (int i = 0; i < asset.Size; i++)
            {
                AssetProperty prop = asset[i];
                ReadAssetProperty(prop, sw, oldPathList);
            }
            sw.Flush();
            sw.Close();
            TaskDialog.Show("demo", "转换完成");
            foreach (var item in oldPathList)
            {
                TaskDialog.Show("result", item);
            }

            //复制文件更改材质路径




            return Result.Succeeded;
        }
        // 定义复制文件的方法
        public static void CopyFile(string sourceFilePath)
        {
            // 获取目标文件夹的路径
            Console.WriteLine("请输入目标文件夹的路径(请勿输入双引号,双击 Enter 确认)：");
            string targetFolderPath = Console.ReadLine();

            // 若目录不存在，建立目录
            if (!System.IO.Directory.Exists(targetFolderPath))
            {
                System.IO.Directory.CreateDirectory(targetFolderPath);
            }

            // 根据目标文件夹及源文件路径复制文件
            String targetPath = targetFolderPath + @"/" + Path.GetFileName(sourceFilePath);
            bool isrewrite = true;   // true=覆盖已存在的同名文件,false则反之
            System.IO.File.Copy(sourceFilePath, targetPath, isrewrite);
        }
        // 定义获取材质路径的方法
        public void ReadAssetProperty(AssetProperty prop, StreamWriter objWriter, List<string> oldPath)
        {
            switch (prop.Type)
            {
                // Retrieve the value from simple type property is easy.  
                // for example, retrieve bool property value.  
                case AssetPropertyType.Integer:
                    var AssetPropertyInt = prop as AssetPropertyInteger;
                    objWriter.WriteLine(AssetPropertyInt.Name + "= " + AssetPropertyInt.Value.ToString() + ";" + AssetPropertyInt.IsReadOnly.ToString());
                    break;
                case AssetPropertyType.Distance:
                    var AssetPropertyDistance = prop as AssetPropertyDistance;
                    objWriter.WriteLine(AssetPropertyDistance.Name + "= " + AssetPropertyDistance.Value + ";" + AssetPropertyDistance.IsReadOnly.ToString());
                    break;
                case AssetPropertyType.Double1:
                    var AssetPropertyDouble = prop as AssetPropertyDouble;
                    objWriter.WriteLine(AssetPropertyDouble.Name + "= " + AssetPropertyDouble.Value.ToString() + ";" + AssetPropertyDouble.IsReadOnly.ToString());
                    break;
                case AssetPropertyType.Double2:
                    var AssetPropertyDoubleArray2d = prop as AssetPropertyDoubleArray2d;
                    objWriter.WriteLine(AssetPropertyDoubleArray2d.Name + "= " + AssetPropertyDoubleArray2d.Value.ToString() + ";" + AssetPropertyDoubleArray2d.IsReadOnly.ToString());
                    break;
                case AssetPropertyType.Double4:
                    var AssetPropertyDoubleArray4d = prop as AssetPropertyDoubleArray4d;
                    objWriter.WriteLine(AssetPropertyDoubleArray4d.Name + "= " + AssetPropertyDoubleArray4d.Value.ToString() + ";" + AssetPropertyDoubleArray4d.IsReadOnly.ToString());
                    break;
                case AssetPropertyType.String:
                    AssetPropertyString val = prop as AssetPropertyString;
                    objWriter.WriteLine(val.Name + "= " + val.Value + ";" + "str" + val.IsReadOnly.ToString());
                    if (val.Name == "unifiedbitmap_Bitmap")
                    {
                        oldPath.Add(val.Value);

                    }
                    break;
                case AssetPropertyType.Boolean:
                    AssetPropertyBoolean boolProp = prop as AssetPropertyBoolean;
                    objWriter.WriteLine(boolProp.Name + "= " + boolProp.Value.ToString() + ";" + boolProp.IsReadOnly.ToString());
                    break;
                // When you retrieve the value from the data array property,  
                // you may need to get which value the property stands for.  
                // for example, the APT_Double44 may be a transform data.  
                case AssetPropertyType.Double44:
                    AssetPropertyDoubleArray4d transformProp = prop as AssetPropertyDoubleArray4d;
                    Autodesk.Revit.DB.DoubleArray tranformValue = transformProp.Value;
                    objWriter.WriteLine(transformProp.Name + "= " + transformProp.Value.ToString() + ";" + tranformValue.IsReadOnly.ToString());
                    break;
                // The APT_List contains a list of sub asset properties with same type.  
                case AssetPropertyType.List:
                    AssetPropertyList propList = prop as AssetPropertyList;
                    IList<AssetProperty> subProps = propList.GetValue();
                    if (subProps.Count == 0)
                        break;
                    switch (subProps[0].Type)
                    {
                        case AssetPropertyType.Integer:
                            foreach (AssetProperty subProp in subProps)
                            {
                                AssetPropertyInteger intProp = subProp as AssetPropertyInteger;
                                int intValue = intProp.Value;
                                objWriter.WriteLine(intProp.Name + "= " + intProp.Value.ToString() + ";" + intProp.IsReadOnly.ToString());
                            }
                            break;
                        case AssetPropertyType.String:
                            foreach (AssetProperty subProp in subProps)
                            {
                                AssetPropertyString intProp = subProp as AssetPropertyString;
                                string intValue = intProp.Value;
                                objWriter.WriteLine(intProp.Name + "= " + intProp.Value.ToString() + ";" + intProp.IsReadOnly.ToString());
                            }
                            break;
                    }
                    break;
                case AssetPropertyType.Asset:
                    Asset propAsset = prop as Asset;
                    for (int i = 0; i < propAsset.Size; i++)
                    {
                        ReadAssetProperty(propAsset[i], objWriter, oldPath);
                    }
                    break;
                case AssetPropertyType.Reference:
                    break;
                default:
                    objWriter.WriteLine("居然有啥都不是类型的" + prop.Type.ToString());
                    break;
            }
            // Get the connected properties.  
            // please notice that the information of many texture stores here.  
            if (prop.NumberOfConnectedProperties == 0)
                return;
            foreach (AssetProperty connectedProp in prop.GetAllConnectedProperties())
            {
                ReadAssetProperty(connectedProp, objWriter, oldPath);
            }
        }




        void ChangeRenderingTexturePath(Document doc)
        {
            // As there is only one material in the sample 
            // project, we can use FilteredElementCollector 
            // and grab the first result

            Material mat = new FilteredElementCollector(doc).OfClass(typeof(Material)).FirstElement() as Material;

            // Fixed path for new texture
            // Texture included in sample files

            string texturePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "new_texture.png");

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Changing material texture path");

                using (AppearanceAssetEditScope editScope = new AppearanceAssetEditScope(doc))
                {
                    Asset editableAsset = editScope.Start(mat.AppearanceAssetId);

                    // Getting the correct AssetProperty

                    AssetProperty assetProperty = editableAsset["generic_diffuse"];


                    Asset connectedAsset = assetProperty.GetConnectedProperty(0) as Asset;

                    // Getting the right connected Asset

                    if (connectedAsset.Name == "UnifiedBitmapSchema")
                    {
                        AssetPropertyString path = connectedAsset.FindByName(UnifiedBitmap.UnifiedbitmapBitmap) as AssetPropertyString;

                        if (path.IsValidValue(texturePath)) path.Value = texturePath;
                    }
                    editScope.Commit(true);
                }
                TaskDialog.Show("Material texture path", "Material texture path changed to:\n" + texturePath);

                t.Commit();
                t.Dispose();
            }
        }
    }
}
