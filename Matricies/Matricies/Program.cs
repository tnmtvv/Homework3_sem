using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Matricies
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                throw new ArgumentException("Неверно количество входных файлов");
            }

            string path = "C:/Users/Татьяна/source/repos/Matricies/Matricies/result.txt";

            MyMatrix new_one = MyMatrix.read_from_file(args[0]);
            MyMatrix new_one_2 = MyMatrix.read_from_file(args[1]);

            MyMatrix result = MyMatrix.mult_async_3(new_one, new_one_2);

            MyMatrix.write_to_file(result, path);



        }
    }
}
