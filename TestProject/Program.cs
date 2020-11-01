using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    class Program
    {
        public static void Main(string[] args)
        {
            string sourceFilePath = @"E:\#5-BIM资料收集\06-项目成果整理\07-精装修样板房（扬州）\02-项目模型\20200518_今也东南项目_F1户型_ALL_史鑫.png";
            CopyFile(sourceFilePath);
            Console.ReadKey();
        }
        
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
    }
}
