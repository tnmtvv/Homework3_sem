using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using System.Diagnostics;


namespace Matricies
{
    public class Measurments
    {
            int numOfIterations;

            string res_path;

            string f_path;
            string s_path;

            public Measurments(int numIt, string path, string path_1, string path_2)
            {
                numOfIterations = numIt;

                f_path = path_1;
                s_path = path_2;
                res_path = path;
            }

        /// <summary>
        /// counts time of work of a given method 
        /// </summary>
        
            long count_time(MyMatrix A, MyMatrix B, Func<MyMatrix, MyMatrix, MyMatrix> func)
            {
                Stopwatch timer = new Stopwatch();

                timer.Start();
                func(A, B);
                timer.Stop();

                return timer.ElapsedMilliseconds;
            }

        /// <summary>
        /// compares to multiplications 
        /// </summary>
       
            public void Comparison(int dim, StreamWriter sw)
            {
                long time_consistent = 0;
                long time_async = 0;

                long average_cons = 0;
                long average_async = 0;

                long variance_async = 0;
                long variance_cons = 0;


                for (int i = 0; i < numOfIterations; i++)
                {

                    MyMatrix A = MyMatrix.generator(dim, dim);
                    MyMatrix B = MyMatrix.generator(dim, dim);

                    try
                    {
                        long time = count_time(A, B, MyMatrix.mult_async_3); ;
                        time_async += time;
                        variance_async += time * time;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Smth went wrong");
                    }

                    try
                    {
                        long time = count_time(A, B, MyMatrix.mult_consistently);
                        time_consistent += time;
                        variance_cons += time * time;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Smth went wrong");
                    }
                }
                average_async = time_async / numOfIterations;
                variance_async /= numOfIterations;
                variance_async -= average_async * average_async;

                average_cons = time_consistent / numOfIterations;
                variance_cons /= numOfIterations;
                variance_cons -= average_cons * average_cons;


                string Stat = "";
                Stat += "среднее время: " + (double)average_async / 1000 + "\t" + (double)average_cons / 1000 +
                    "\nсреднеквадратичное отклонение: " + Math.Round(Math.Sqrt((double)variance_async) / 1000, 5) + "\t" + Math.Round(Math.Sqrt((double)variance_cons) / 1000, 5);
                sw.WriteLine(Stat);
                Console.WriteLine(Stat);
            }

        /// <summary>
        /// have a for loop of the dimentions of matrices 
        /// </summary>
      
            public void Measure()
            {
                StreamWriter sw = new StreamWriter(res_path);


                string table = " Параллельно\tПоследовательно";
                sw.WriteLine(table);

                if (!File.Exists(res_path))
                    throw new FileNotFoundException("File is not found");

                int step = 100;
                for (int dim = 1300; dim < 2000; dim += step)
                {
                    Comparison(dim, sw);
                }
                sw.Close();

            }

        }



    }

