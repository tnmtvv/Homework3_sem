using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace Matrices
{
    public class MyMatrix
    {
        /// <summary>
        /// class _Matrix for creating objects and operation on them
        /// </summary>
        static private Random rand = new Random();
        private int _Rows, _Cols;
        private int[,] _Matrix;

        /// <summary>
        /// constuctor for creating a _Matrix by given parametrs 
        /// </summary>
        public MyMatrix(int _rows, int _cols)
        {
            _Rows = _rows;
            _Cols = _cols;
            int[,] _Matrix = new int[_Rows, _Cols];

            for (int i = 0; i < _Rows; i++)
            {
                for (int j = 0; j < _Cols; j++)
                {
                    _Matrix[i, j] = 0;
                }
            }
            this._Matrix = _Matrix;
        }

        /// <summary>
        /// Create _Matrix from an array
        /// </summary>
        public MyMatrix(int[,] _Matrix)
        {
            _Matrix = _Matrix;
            _Rows = _Matrix.GetLength(0);
            _Cols = _Matrix.GetLength(1);
        }

        public int[,] Matrix
        {
            get => _Matrix;
            set => _Matrix = value;
        }

        public int Rows
        {
            get => _Rows;
            set => _Rows = value;
        }

        public int Cols
        {
            get => _Cols;
            set => _Cols = value;
        }

        /// <summary>
        /// reloaded operators 
        /// </summary>
        public static bool operator ==(MyMatrix A, MyMatrix B)
        {
            if (A._Rows != B._Rows || A._Cols != B._Cols)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < A._Rows; i++)
                {
                    for (int j = 0; j < A._Cols; j++)
                    {
                        if (A._Matrix[i, j] != B._Matrix[i, j])
                        {
                            Console.WriteLine(A._Matrix[i, j] + "!= " + B._Matrix[i, j]);
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static bool operator !=(MyMatrix A, MyMatrix B)
        {
            if (A._Rows != B._Rows || A._Cols != B._Cols)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < A._Rows; i++)
                {
                    for (int j = 0; j < A._Cols; j++)
                    {
                        if (A._Matrix[i, j] != B._Matrix[i, j]) return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// generates a random _Matrix 
        /// </summary>
        public static MyMatrix generator(int _Rows, int _Cols, double sparcity = 1)
        {
            int[,] _Matrix = new int[_Rows, _Cols];
            double indicator;
            for (int i = 0; i < _Rows; i++)
            {
                for (int j = 0; j < _Cols; j++)
                {
                    indicator = rand.NextDouble();
                    if (indicator > sparcity)
                        _Matrix[i, j] = 0;
                    else _Matrix[i, j] = rand.Next();
                }
            }
            MyMatrix next_Matrix = new MyMatrix(_Rows, _Cols);
            next_Matrix._Matrix = _Matrix;
            return next_Matrix;
        }

        /// <summary>
        /// makes a string out of an array
        /// </summary>
        static string array_to_string(int[] array)
        {
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < array.Length; i++)
            {
                sb.Append(array[i]);
                if (i != array.Length - 1)
                    sb.Append(" ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// writes _Matrix to file
        /// </summary>
        static public void write_to_file(MyMatrix myMatrix, string path)
        {
            StreamWriter sw = new StreamWriter(path);
            int[] new_arr = new int[myMatrix._Cols];

            for (int i = 0; i < myMatrix._Rows; i++)
            {
                for (int j = 0; j < myMatrix._Cols; j++)
                {
                    new_arr[j] = myMatrix._Matrix[i, j];
                }
                string new_row = array_to_string(new_arr);
                sw.WriteLine(new_row);
            }
            sw.Close();
        }

        /// <summary>
        /// mkaes an array out of a string
        /// </summary>
        static int[] string_to_Array(string row)
        {
            string[] nums = row.Split(' ');
            int[] array = new int[nums.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Int32.Parse(nums[i]);
            }
            return array;
        }

        /// <summary>
        /// multiplies matrices sequentially
        /// </summary>
        public static MyMatrix mult_consistently(MyMatrix A, MyMatrix B)
        {
            if (A._Cols != B._Rows)
                throw new ArgumentException("Wrong sizes");

            int a__Rows = A._Rows;
            int a__Cols = A._Cols;
            int b__Rows = B._Rows;
            int b__Cols = B._Cols;

            int[,] new__Matrix = new int[a__Rows, b__Cols];

            for (int i = 0; i < a__Rows; i++)
            {
                for (int j = 0; j < b__Cols; j++)
                {
                    for (int k = 0; k < a__Cols; k++)
                    {
                        new__Matrix[i, j] += A._Matrix[i, k] * B._Matrix[k, j];
                    }
                }
            }
            MyMatrix _Matrix = new MyMatrix(a__Rows, b__Cols);
            _Matrix._Matrix = new__Matrix;

            return _Matrix;
        }

        /// <summary>
        /// multiplies marices parallel
        /// </summary>
        public static MyMatrix mult_async_3(MyMatrix A, MyMatrix B)
        {
            if (A._Cols != B._Rows)
                throw new ArgumentException("Wrong sizes");

            MyMatrix res = new MyMatrix(A._Rows, B._Cols);
            int[,] new__Matrix = new int[A._Rows, B._Cols];

            int ammount_of_threads = Math.Min(Environment.ProcessorCount, A._Rows);

            int chunk_size = A._Rows / ammount_of_threads;
            if (A._Rows % ammount_of_threads != 0)
            {
                chunk_size++;
            }

            Thread[] threads = new Thread[ammount_of_threads];

            for (int i = 0; i < ammount_of_threads; i++)
            {
                int cur_i = i * chunk_size;

                threads[i] = new Thread(() =>
                {
                    for (int j = cur_i; j < Math.Min(cur_i + chunk_size, A._Rows); j++)
                    {
                        for (int k = 0; k < B._Cols; k++)
                        {
                            for (int l = 0; l < A._Cols; l++)
                            {
                                new__Matrix[j, k] += A._Matrix[j, l] * B._Matrix[l, k];
                            }
                        }
                    }
                });
            }

            foreach (var th in threads)
            {
                th.Start();
            }

            foreach (var th in threads)
            {
                th.Join();
            }

            res._Matrix = new__Matrix;
            return res;
        }

        /// <summary>
        /// reads a _Matrix from a file
        /// </summary>
        public static MyMatrix read_from_file(string path)
        {
            List<string> list_strings = new List<string>();

            if (!File.Exists(path))
                throw new FileNotFoundException("File is not found");

            StreamReader sr = new StreamReader(path);

            while (!sr.EndOfStream)
            {
                string new_one = sr.ReadLine();
                if (new_one[new_one.Length - 1] == ' ')
                    new_one = new_one.Substring(0, new_one.Length - 1);
                list_strings.Add(new_one);
            }

            int expected_size = string_to_Array(list_strings.First()).Length;
            int[,] _Matrix = new int[list_strings.Count, expected_size];
            int[] new_arr = new int[expected_size];

            for (int i = 0; i < list_strings.Count; i++)
            {
                new_arr = string_to_Array(list_strings[i]);

                for (int j = 0; j < expected_size; j++)
                {
                    _Matrix[i, j] = new_arr[j];
                }
            }

            MyMatrix mymtrx = new MyMatrix(_Matrix.GetLength(0), _Matrix.GetLength(1));
            mymtrx._Matrix = _Matrix;

            sr.Close();

            return mymtrx;
        }
    }
}


