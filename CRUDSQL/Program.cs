using System;
using System.Linq;
using System.Text;
public delegate void GetMethod();

namespace CRUDSQL
{
        class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            DataBaseCRUD databasereader  = new DataBaseCRUD();
            try
            {
                databasereader.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            Console.ReadKey();

        }
    }
}
