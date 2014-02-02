using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    class Program
    {
        private static float[,] parse_string(string input)
        {
            string[] elements = input.Split(new char[1] { ',' }, 100);
            int size = (int)Math.Sqrt(elements.Length);
            float[,] matrix = new float[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    matrix[i, j] = Convert.ToInt32(elements[size * i + j]);
            return matrix;
        }

        private static void print_matrix(float[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j].ToString() + " ");
                }
                Console.WriteLine();
            }
        }

        private static float[,] multiply(float[,] A, float[,] B)
        {
            int size = A.GetLength(0);
            float[,] C = new float[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for(int k = 0;k < size;k++)                    
                        C[i, j] += A[i, k] * B[k, j];
                }
            }
            return C;
        }

        private static float[,] sum(float[,] A, float[,] B)
        {
            int size = A.GetLength(0);
            float[,] C = new float[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    C[i, j] += A[i, j] + B[i, j];
                }
            }
            return C;
        }

        private static float[,] subtract(float[,] A, float[,] B)
        {
            int size = A.GetLength(0);
            float[,] C = new float[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    C[i, j] += A[i, j] - B[i, j];
                }
            }
            return C;
        }

        private static void reduce(float[,] A)
        {
            for (int i = 0; i < A.GetLength(0) - 1; i++)
            {
                for (int j = i + 1; j < A.GetLength(0); j++)
                {
                    if (A[j, i] != 0 && A[i, i] != 0)
                    {
                        float ratio = -A[j, i] / A[i, i];
                        for (int k = 0; k < A.GetLength(1); k++)
                        {                            
                            A[j, k] = ratio * A[i, k] + A[j, k];
                            if (Math.Abs(A[j, k]) < .00001)
                                A[j, k] = 0;
                        }
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            Dictionary<string, float[,]> variables = new Dictionary<string, float[,]>();
            bool cont = true;
            string command = "";
            while (cont)
            {
                command = Console.ReadLine();
                string[] tokens = command.Split(new char[1] { ' ' }, 1000);
                if (command.Equals("end"))
                    cont = false;
                else if (tokens[0].Equals("store"))
                    if(variables.ContainsKey(tokens[1]))
                        variables[tokens[1]] = parse_string(tokens[2]);
                    else
                        variables.Add(tokens[1], parse_string(tokens[2]));
                else if (tokens[0].Equals("show"))
                {
                    print_matrix(variables[tokens[1]]);
                }
                else if (tokens[0].Equals("list"))
                {
                    foreach (string s in variables.Keys)
                    {
                        Console.WriteLine("---------------\n" + s);
                        print_matrix(variables[s]);
                    }
                }
                else if (tokens[0].Equals("clear"))
                {
                    variables.Clear();
                }
                else if (tokens[0].Equals("mult"))
                {
                    if (variables.Keys.Contains(tokens[3]))
                        variables[tokens[3]] = multiply(variables[tokens[1]], variables[tokens[2]]);
                    else
                        variables.Add(tokens[3], multiply(variables[tokens[1]], variables[tokens[2]]));
                }
                else if (tokens[0].Equals("sum"))
                {
                    if (variables.Keys.Contains(tokens[3]))
                        variables[tokens[3]] = sum(variables[tokens[1]], variables[tokens[2]]);
                    else
                        variables.Add(tokens[3], sum(variables[tokens[1]], variables[tokens[2]]));
                }
                else if (tokens[0].Equals("sub"))
                {
                    if (variables.Keys.Contains(tokens[3]))
                        variables[tokens[3]] = subtract(variables[tokens[1]], variables[tokens[2]]);
                    else
                        variables.Add(tokens[3], subtract(variables[tokens[1]], variables[tokens[2]]));
                }
                else if (tokens[0].Equals("reduce"))
                {
                    reduce(variables[tokens[1]]);
                }
                    
            }
        }
    }
}
