using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project02
{
    static class Program
    {

        static void Main()
        {
            //List<string> str = new List<string>();

            List<string> str = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                str.Add(i.ToString());
                System.IO.File.WriteAllText(@"C: \Users\11265\Desktop\WriteLines.txt", str.ToString());
                //Console.WriteLine(i);
                //Console.ReadKey();
            }

        }


    }
}
