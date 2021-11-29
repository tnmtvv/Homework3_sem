using NUnit.Framework;
using Matricies;




namespace TestMatrix
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void read_from_file_1()
        {
                string path_1 = "A.txt";

                for (int i = 0; i < 100; i++)
                {
                    MyMatrix new_one = MyMatrix.generator(25, 25);
                    // MyMatrix new_one_2 = MyMatrix.generator(25, 25);

                    MyMatrix.write_to_file(new_one, path_1);
                    // MyMatrix.write_to_file(new_one_2, s_file_name);

                    MyMatrix matrix_1 = MyMatrix.read_from_file(path_1);
                    //MyMatrix matrix_2 = MyMatrix.read_from_file(s_file_name);

                    CollectionAssert.AreEqual(new_one._Matrix, matrix_1._Matrix);
                }

            
        }

        [Test]
        public void mult_correct()
        {
            int[,] matrix1 =
            {
                            { 2, 4 },
                            { 5, 6 }
                        };

            int[,] matrix2 =
            {
                            { 1, 2 },
                            { 3, 4 }
                        };

            int[,] resMatr1 =
            {
                            { 14, 20 },
                            { 23, 34 }
                        };



            MyMatrix m_1 = new MyMatrix(matrix1);
            MyMatrix m_2 = new MyMatrix(matrix2);

            MyMatrix res = new MyMatrix(resMatr1);

            MyMatrix m_1res = MyMatrix.mult_async_3(m_1, m_2);
            MyMatrix m_2res = MyMatrix.mult_consistently(m_1, m_2);

            CollectionAssert.AreEqual(m_2res._Matrix, res._Matrix);
            CollectionAssert.AreEqual(m_1res._Matrix, res._Matrix);

        }

        [Test]
        public void mult_correct_2()
        {


            var matrix3 = new int[,]
            {
                            {1, 2},
                            {3, 4},
                            {6, 9}
            };
            var matrix4 = new int[,]
            {
                            {5, 2, 7},
                            {5, 4, 9}
            };
            var resMatr2 = new int[,]
            {
                            {15, 10, 25},
                            {35, 22, 57},
                            {75, 48, 123}
            };



            MyMatrix m_3 = new MyMatrix(matrix3);
            MyMatrix m_4 = new MyMatrix(matrix4);

            MyMatrix res2 = new MyMatrix(resMatr2);

            MyMatrix m_3res = MyMatrix.mult_async_3(m_3, m_4);
            MyMatrix m_4res = MyMatrix.mult_consistently(m_3, m_4);

            Assert.IsTrue(m_3res == res2);
            Assert.IsTrue(m_4res == res2);
        }

        [Test]
        public void mult_correct_3()
        {
        string f_file_name = "A.txt";
        string s_file_name = "B.txt";

        MyMatrix m_1;
        MyMatrix m_2;

        MyMatrix res_1 = new MyMatrix(20, 20);
        MyMatrix res_2 = new MyMatrix(20, 20); ;

            for (int i = 0; i< 100; i++)
            {
                MyMatrix.write_to_file(MyMatrix.generator(20, 30), f_file_name);
                MyMatrix.write_to_file(MyMatrix.generator(30, 20), s_file_name);
                
                m_1 = MyMatrix.read_from_file(f_file_name);
                m_2 = MyMatrix.read_from_file(s_file_name);

                res_1 = MyMatrix.mult_consistently(m_1, m_2);
                res_2 = MyMatrix.mult_async_3(m_1, m_2);

                CollectionAssert.AreEqual(res_1._Matrix, res_2._Matrix);
            }

        }

    }
}