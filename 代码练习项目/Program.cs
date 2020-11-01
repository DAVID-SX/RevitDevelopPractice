using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 代码练习项目
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string path = @"D:\test0628.txt";
                if (File.Exists(path))
                {
                    Console.WriteLine("读取文件");
                    string content = File.ReadAllText(path);
                    Console.WriteLine(content);
                    FileInfo testInfo = new FileInfo(path);
                    Console.WriteLine("文件名:" + testInfo.Name);
                    Console.ReadKey();
                }
                else
                {
                    string content = "床前明月光，\n疑是地上霜，\n举头望明月，\n低头思故乡。";
                    File.WriteAllText(path, content);
                    Console.WriteLine("写入文件");
                    Console.ReadKey();
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }
    }
}
