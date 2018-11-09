using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Matrix
    {
        public int[,] pos;
        public int size;
        public int zero_i = 0;
        public int zero_j = 0;
        public List<int> way;
        public int difference;

        public Matrix(int size, int[,] numbers)
        {
            difference = 0;
            way = new List<int>();
            this.size = size;
            //copy array
            this.pos = new int[size,size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    this.pos[i, j] = numbers[i, j];
                    if (this.pos[i, j] == 0)
                    {
                        zero_i = i;
                        zero_j = j;
                    }
                }
            }
        }

        //new matrix with new pos
        public Matrix(Matrix m, int direction)
        {
            difference = 0;
            int zero_i_new = 0, zero_j_new = 0;
            zero_i_new += (direction == 0? -1: direction == 6? +1: 0);
            zero_j_new += (direction == 9 ? -1 : direction == 3 ? +1 : 0);
            //copy way
            way = new List<int>();
            for (int i = 0; i < m.way.Count; i++)
                way.Add(m.way[i]);
            this.size = m.size;
            way.Add(direction);
            //copy array
            this.pos = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    this.pos[i, j] = m.pos[i, j];
                    if (this.pos[i, j] == 0)
                    {
                        zero_i = i;
                        zero_j = j;
                    }
                }
            }

            //change zero position
            zero_i_new += zero_i;
            zero_j_new += zero_j;
            pos[zero_i, zero_j] = pos[zero_i_new, zero_j_new];
            pos[zero_i_new, zero_j_new] = 0;
            zero_i = zero_i_new;
            zero_j = zero_j_new;
        }

        //compare matrix current and target
        public int Compare(Matrix end)
        {
            //find quantity of different numbers on the same position in 2 matrix
            int num = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (pos[i, j] != end.pos[i, j])
                    {
                        num++;
                    }
                }
            }
            difference = num;
            return num;
        }

        public List<Matrix> FindWays()
        //returns array of matrix
        //opens all posible
        {
            List<Matrix> res = new List<Matrix>();
            //last step need to block
            int l = -1;
            if (this.way.Count > 0)
                l = this.way.Last();
            
            if (zero_i > 0 && l != 6)
            {
                //up
                res.Add(new Matrix(this, 0));
            }
            if (zero_i < size - 1 && l != 0)
            {
                //down
                res.Add(new Matrix(this, 6));
            }
            if (zero_j > 0 && l != 3)
            {
                //left
                res.Add(new Matrix(this, 9));
            }
            if (zero_j < size - 1 && l != 9)
            {
                //rigth
                res.Add(new Matrix(this, 3));
            }
            return res;
        }
        
    }



class Program
    {
        static void Main(string[] args)
        {
            List<Matrix> start = new List<Matrix>();
            Matrix end, first;
            int qstep = 0;
            int size = 3;
            int difference = size*size;
            
            int[,] m = new int[size, size];

            //enter initial data
            //start matrix
            Console.WriteLine("Enter start matrix:");
            for (int i = 0; i < size; i++)
            {
                string[] num = Console.ReadLine().Split(' ');
                for (int j = 0; j < size; j++)
                {
                    m[i, j] = int.Parse(num[j]);
                }
            }
            start.Add(new Matrix(size, m));
            first = new Matrix(size, start[0].pos);
            
            Console.Write("\n");
            //target matrix
            Console.WriteLine("Enter target matrix:");
            for (int i = 0; i < size; i++)
            {
                string[] num = Console.ReadLine().Split(' ');
                for (int j = 0; j < size; j++)
                {
                    m[i, j] = int.Parse(num[j]);
                }
            }
            end = new Matrix(size, m);
            Console.Write("\n");

            difference = start[0].Compare(end);


            //solve
            while (difference != 0) {
                int min_difference = size * size;
                //раскрыть все вершины
                List<Matrix> temp = new List<Matrix>();
                for (int g = 0; g < start.Count; g++) {
                    //Console.WriteLine("G =" + g + "/" + start.Count);
                    foreach (Matrix temp_m in start[g].FindWays()) {
                        if (temp_m.Compare(end) < min_difference) {
                            min_difference = temp_m.difference;
                        }
                        temp.Add(temp_m);
                    }
                }
                //refresh
                start.Clear();

                foreach (Matrix temp_m in temp) {
                    //print all matrix opened
                    for (int i = 0; i < size; i++) {
                        for (int j = 0; j < size; j++) {
                            Console.Write(temp_m.pos[i, j] + " ");
                        }
                        Console.Write("\n");
                    }
                    Console.WriteLine("weight:  " + (qstep + temp_m.difference));

                    //refresh start
                    if (temp_m.difference == min_difference) {
                        start.Add(temp_m);
                    }
                }

                difference = min_difference;
                Console.WriteLine("MIN:  " + (qstep + min_difference));
                qstep++;
            }

            
            //solution
            //тут надо раскрыть все последовательности для каждой матрицы из старта
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(first.pos[i,j] + " ");
                }
                Console.Write("\n");
            }
            Console.Write("\n");
            Console.WriteLine("Steps =" + (start[0].way.Count));
            foreach (var st in start)
            {
                Console.WriteLine("Step #" + st.way.Count);
                string str = "The way is: ";
                foreach (int step in st.way)
                {
                    str += (step == 9? "<=": step == 3? "=>": step == 0? @"/\": @"\/") + " ";
                }
                Console.WriteLine(str);
            }
            
            Console.ReadKey();
        }
    }
}
