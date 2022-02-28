using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using System.Diagnostics;

namespace Matrices
{
    public class Measurments
    {
        private int _numOfIterations;

        private string _answPath;
        private string _firstMatrixPath;
        private string _secondMatrixPath;

        /// <summary>
        /// a constructor to create an object of the class with given parameters
        /// </summary>
        /// <param name="numIt">number of iteretions</param>
        /// <param name="answPath">a path to the file with an answer</param>
        /// <param name="firstMatrixPath">a path to the file with the first matrix</param>
        /// <param name="secondMatrixPath">a path to the file with the second matrix</param>
        public Measurments(int numIt, string answPath, string firstMatrixPath, string secondMatrixPath)
        {
            _numOfIterations = numIt;

            _firstMatrixPath = firstMatrixPath;
            _secondMatrixPath = secondMatrixPath;
            _answPath = answPath;
        }

        /// <summary>
        /// counts time of work of a given method 
        /// </summary>
        long count_time(MyMatrix A, MyMatrix B, Func<MyMatrix, MyMatrix, MyMatrix> func)
        {
            var timer = new Stopwatch();

            timer.Start();
            func(A, B);
            timer.Stop();

            return timer.ElapsedMilliseconds;
        }

        /// <summary>
        /// compares two multiplications 
        /// </summary>
        public void Comparison(int dim, StreamWriter sw)
        {
            long time_consistent = 0;
            long time_async = 0;

            long average_cons = 0;
            long average_async = 0;

            long variance_async = 0;
            long variance_cons = 0;

            for (int i = 0; i < _numOfIterations; i++)
            {
                MyMatrix A = MyMatrix.generator(dim, dim);
                MyMatrix B = MyMatrix.generator(dim, dim);

                try
                {
                    long time = count_time(A, B, MyMatrix.mult_async_3);
                    time_async += time;
                    variance_async += time * time;
                }
                catch (Exception)
                {
                    Console.WriteLine("Smth went wrong");
                }

                try
                {
                    long time = count_time(A, B, MyMatrix.mult_consistently);
                    time_consistent += time;
                    variance_cons += time * time;
                }
                catch (Exception)
                {
                    Console.WriteLine("Smth went wrong");
                }
            }
            average_async = time_async / _numOfIterations;
            variance_async /= _numOfIterations;
            variance_async -= average_async * average_async;

            average_cons = time_consistent / _numOfIterations;
            variance_cons /= _numOfIterations;
            variance_cons -= average_cons * average_cons;

            string Stat = "";
            Stat += "среднее время: " + (double)average_async / 1000 + "\t" + (double)average_cons / 1000 +
                "\nсреднеквадратичное отклонение: " + Math.Round(Math.Sqrt((double)variance_async) / 1000, 5) + "\t" + Math.Round(Math.Sqrt((double)variance_cons) / 1000, 5);
            sw.WriteLine(Stat);
            Console.WriteLine(Stat);
        }

        /// <summary>
        /// has a for loop for changing dimentions of the matrices 
        /// </summary>
        public void Measure()
        {
            StreamWriter sw = new StreamWriter(_answPath);

            string table = " Параллельно\tПоследовательно";
            sw.WriteLine(table);

            if (!File.Exists(_answPath))
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

