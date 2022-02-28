using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Matrices
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length != 2)
            {
                throw new ArgumentException("Неверно количество входных файлов");
            }
            
            string path = "C:/Users/Татьяна/source/repos/Matrices/Matrices/result.txt";



            string f_file_name = args[0];
            string s_file_name = args[1];

            MyMatrix new_one = MyMatrix.generator(25, 25);
            MyMatrix new_one_2 = MyMatrix.generator(25, 25);

            MyMatrix.write_to_file(new_one, f_file_name); 
            MyMatrix.write_to_file(new_one_2, s_file_name);

            MyMatrix matrix_1 = MyMatrix.read_from_file(f_file_name);
            MyMatrix matrix_2 = MyMatrix.read_from_file(s_file_name);

            Measurements try_f = new Measurements(20, path, f_file_name, s_file_name);
            try_f.Measure();

        }
    }
}
