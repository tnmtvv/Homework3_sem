using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Matricies
{
    public class MyMatrix
    {
        /// <summary>
        /// class matrix for creating objects and operation on them
        /// </summary>
        static Random rand = new Random();
        private int Rows, Cols;
        private int[,] Matrix;

        /// <summary>
        /// constuctor for creating a matrix by given parametrs 
        /// </summary>
       
        public MyMatrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            int[,] matrix = new int[rows, cols];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    matrix[i, j] = 0;
                }
            }
            this.Matrix = matrix;
        }
        /// <summary>
        /// Create matrix from an array
        /// </summary>

        public MyMatrix(int[,] matrix)
        {
            this.Matrix = matrix;
            Rows = matrix.GetLength(0);
            Cols = matrix.GetLength(1);
        }




        public int[,] _Matrix
        {
            get => Matrix;
            set => Matrix = value;
        }

        public int _Rows
        {
            get => Rows;
            set => Rows = value;
        }

        public int _Cols
        {
            get => Cols;
            set => Cols = value;
        }
        /// <summary>
        /// reloaded operators 
        /// </summary>
        public static bool operator ==(MyMatrix A, MyMatrix B)
        {
            if (A.Rows != B.Rows || A.Cols != B.Cols)
                return false;
            else
            {
                for (int i = 0; i < A.Rows; i++)
                {
                    for (int j = 0; j < A.Cols; j++)
                    {
                        if (A.Matrix[i, j] != B.Matrix[i, j])
                        {
                            Console.WriteLine(A.Matrix[i, j] + "!= " + B.Matrix[i, j]);
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        public static bool operator !=(MyMatrix A, MyMatrix B)
        {
            if (A.Rows != B.Rows || A.Cols != B.Cols)
                return true;
            else
            {
                for (int i = 0; i < A.Rows; i++)
                {
                    for (int j = 0; j < A.Cols; j++)
                    {
                        if (A.Matrix[i, j] != B.Matrix[i, j]) return true;
                    }
                }
                return false;
            }
        }


        /// <summary>
        /// generates a random matrix 
        /// </summary>
 
        public static MyMatrix generator(int rows, int cols, double sparcity = 1)
        {
            int[,] matrix = new int[rows, cols];
            double indicator;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    indicator = rand.NextDouble();
                    if (indicator > sparcity)
                        matrix[i, j] = 0;
                    else matrix[i, j] = rand.Next();
                }
            }
            MyMatrix next_Matrix = new MyMatrix(rows, cols);
            next_Matrix.Matrix = matrix;
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
        /// writes matrix to file
        /// </summary>
        
        static public void write_to_file(MyMatrix myMatrix, string path)
        {
            StreamWriter sw = new StreamWriter(path);
            string new_row;
            int[] new_arr = new int[myMatrix.Cols];

            for (int i = 0; i < myMatrix.Rows; i++)
            {
                for (int j = 0; j < myMatrix.Cols; j++)
                {
                    new_arr[j] = myMatrix.Matrix[i, j];
                }
                new_row = array_to_string(new_arr);
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
        /// multiplies matrices consistently
        /// </summary>
       
        public static MyMatrix mult_consistently(MyMatrix A, MyMatrix B)
        {
            if (A.Cols != B.Rows)
                throw new ArgumentException("Wrong sizes");


            int a_rows = A.Rows;
            int a_cols = A.Cols;
            int b_rows = B.Rows;
            int b_cols = B.Cols;

            int[,] new_matrix = new int[a_rows, b_cols];


            for (int i = 0; i < a_rows; i++)
            {
                //int[] new_row = new int[b_cols];
                for (int j = 0; j < b_cols; j++)
                {
                    for (int k = 0; k < a_cols; k++)
                    {
                        new_matrix[i, j] += A.Matrix[i, k] * B.Matrix[k, j];
                    }
                }
            }
            MyMatrix matrix = new MyMatrix(a_rows, b_cols);
            matrix.Matrix = new_matrix;

            return matrix;
        }

        /// <summary>
        /// multiplies marices parallel
        /// </summary>
        
        public static MyMatrix mult_async_3(MyMatrix A, MyMatrix B)
        {
            if (A.Cols != B.Rows)
                throw new ArgumentException("Wrong sizes");

            MyMatrix res = new MyMatrix(A.Rows, B.Cols);
            int[,] new_matrix = new int[A.Rows, B.Cols];

            int ammount_of_threads = Math.Min(Environment.ProcessorCount, A.Rows);

            int chunk_size = A.Rows / ammount_of_threads;
            if (A.Rows % ammount_of_threads != 0)
            {
                chunk_size++;
            }

            Thread[] threads = new Thread[ammount_of_threads];

            for (int i = 0; i < ammount_of_threads; i++)
            {
                int cur_i = i * chunk_size;


                threads[i] = new Thread(() =>
                {
                    for (int j = cur_i; j < Math.Min(cur_i + chunk_size, A.Rows); j++)
                    {
                        for (int k = 0; k < B.Cols; k++)
                        {
                            for (int l = 0; l < A.Cols; l++)
                            {
                                new_matrix[j, k] += A.Matrix[j, l] * B.Matrix[l, k];
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

            res.Matrix = new_matrix;
            return res;

        }

        /// <summary>
        /// reads a matrix from a file
        /// </summary>
       
        public static MyMatrix read_from_file(string path)
        {
            List<string> list_strings = new List<string>();

            if (!File.Exists(path))
                throw new FileNotFoundException("File is not found");

            StreamReader sr = new StreamReader(path);

            string new_one = "";

            while (!sr.EndOfStream)
            {
                new_one = sr.ReadLine();
                if (new_one[new_one.Length - 1] == ' ')
                    new_one = new_one.Substring(0, new_one.Length - 1);
                list_strings.Add(new_one);
            }


            int expected_size = string_to_Array(list_strings.First()).Length;
            int[,] matrix = new int[list_strings.Count, expected_size];
            int[] new_arr = new int[expected_size];

            for (int i = 0; i < list_strings.Count; i++)
            {
                new_arr = string_to_Array(list_strings[i]);

                for (int j = 0; j < expected_size; j++)
                {
                    matrix[i, j] = new_arr[j];
                }
            }

            MyMatrix mymtrx = new MyMatrix(matrix.GetLength(0), matrix.GetLength(1));
            mymtrx.Matrix = matrix;

            sr.Close();

            return mymtrx;
        }


    }



}

