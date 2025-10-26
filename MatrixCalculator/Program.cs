using System;
using System.Linq;

namespace MatrixCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Matrix Calculator";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== MATRIX CALCULATOR ===\n");
            Console.ResetColor();

            Console.WriteLine("Select operation:");
            Console.WriteLine("1 - Add matrices");
            Console.WriteLine("2 - Multiply matrices");
            Console.WriteLine("3 - Multiply by scalar");
            Console.WriteLine("4 - Calculate determinant");
            Console.WriteLine("5 - Calculate inverse matrix");
            Console.Write("Your choice: ");
            string choice = Console.ReadLine();

            int[,] matrixA, matrixB, result;
            double[,] doubleResult;
            int scalar;
            double determinant;

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nMatrix A:");
                    matrixA = ReadMatrix();
                    Console.WriteLine("\nMatrix B:");
                    matrixB = ReadMatrix();
                    
                    if (matrixA.GetLength(0) != matrixB.GetLength(0) || matrixA.GetLength(1) != matrixB.GetLength(1))
                    {
                        Console.WriteLine("Error: Matrices must have same dimensions for addition.");
                        return;
                    }
                    result = AddMatrices(matrixA, matrixB);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nResult:");
                    PrintMatrix(result);
                    break;

                case "2":
                    Console.WriteLine("\nMatrix A:");
                    matrixA = ReadMatrix();
                    Console.WriteLine("\nMatrix B:");
                    matrixB = ReadMatrix();
                    
                    if (matrixA.GetLength(1) != matrixB.GetLength(0))
                    {
                        Console.WriteLine("Error: Columns of A must equal rows of B for multiplication.");
                        return;
                    }
                    result = MultiplyMatrices(matrixA, matrixB);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nResult:");
                    PrintMatrix(result);
                    break;

                case "3":
                    Console.WriteLine("\nMatrix:");
                    matrixA = ReadMatrix();
                    Console.Write("Enter scalar: ");
                    scalar = int.Parse(Console.ReadLine() ?? "0");
                    result = MultiplyByScalar(matrixA, scalar);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nResult:");
                    PrintMatrix(result);
                    break;

                case "4":
                    Console.WriteLine("\nMatrix:");
                    matrixA = ReadMatrix();
                    if (matrixA.GetLength(0) != matrixA.GetLength(1))
                    {
                        Console.WriteLine("Error: Matrix must be square for determinant calculation.");
                        return;
                    }
                    determinant = CalculateDeterminant(matrixA);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nDeterminant: {determinant}");
                    break;

                case "5":
                    Console.WriteLine("\nMatrix:");
                    matrixA = ReadMatrix();
                    if (matrixA.GetLength(0) != matrixA.GetLength(1))
                    {
                        Console.WriteLine("Error: Matrix must be square for inverse calculation.");
                        return;
                    }
                    if (CalculateDeterminant(matrixA) == 0)
                    {
                        Console.WriteLine("Error: Matrix is singular, no inverse exists.");
                        return;
                    }
                    doubleResult = CalculateInverse(matrixA);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nInverse Matrix:");
                    PrintDoubleMatrix(doubleResult);
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    return;
            }

            Console.ResetColor();
        }

        static int[,] ReadMatrix()
        {
            Console.Write("Enter number of rows: ");
            int rows = int.Parse(Console.ReadLine() ?? "0");
            Console.Write("Enter number of columns: ");
            int cols = int.Parse(Console.ReadLine() ?? "0");

            int[,] matrix = new int[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                Console.WriteLine($"Enter row {i + 1} values separated by spaces:");
                var values = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse).ToArray();
                if (values == null || values.Length != cols)
                {
                    Console.WriteLine("Invalid row input. Try again.");
                    i--;
                    continue;
                }
                for (int j = 0; j < cols; j++)
                    matrix[i, j] = values[j];
            }

            return matrix;
        }

        static int[,] AddMatrices(int[,] a, int[,] b)
        {
            int rows = a.GetLength(0), cols = a.GetLength(1);
            int[,] result = new int[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[i, j] = a[i, j] + b[i, j];
            return result;
        }

        static int[,] MultiplyMatrices(int[,] a, int[,] b)
        {
            int rows = a.GetLength(0), cols = b.GetLength(1), n = a.GetLength(1);
            int[,] result = new int[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    for (int k = 0; k < n; k++)
                        result[i, j] += a[i, k] * b[k, j];
            return result;
        }

        static int[,] MultiplyByScalar(int[,] matrix, int scalar)
        {
            int rows = matrix.GetLength(0), cols = matrix.GetLength(1);
            int[,] result = new int[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[i, j] = matrix[i, j] * scalar;
            return result;
        }

        static double CalculateDeterminant(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            if (n == 1) return matrix[0, 0];
            if (n == 2) return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

            double determinant = 0;
            for (int j = 0; j < n; j++)
            {
                determinant += matrix[0, j] * Math.Pow(-1, j) * CalculateDeterminant(GetMinor(matrix, 0, j));
            }
            return determinant;
        }

        static int[,] GetMinor(int[,] matrix, int row, int col)
        {
            int n = matrix.GetLength(0);
            int[,] minor = new int[n - 1, n - 1];
            
            for (int i = 0, r = 0; i < n; i++)
            {
                if (i == row) continue;
                for (int j = 0, c = 0; j < n; j++)
                {
                    if (j == col) continue;
                    minor[r, c] = matrix[i, j];
                    c++;
                }
                r++;
            }
            return minor;
        }

        static double[,] CalculateInverse(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            double determinant = CalculateDeterminant(matrix);
            double[,] inverse = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    double cofactor = Math.Pow(-1, i + j) * CalculateDeterminant(GetMinor(matrix, i, j));
                    inverse[j, i] = cofactor / determinant;
                }
            }
            return inverse;
        }

        static void PrintMatrix(int[,] matrix)
        {
            int rows = matrix.GetLength(0), cols = matrix.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    Console.Write(matrix[i, j] + "\t");
                Console.WriteLine();
            }
        }

        static void PrintDoubleMatrix(double[,] matrix)
        {
            int rows = matrix.GetLength(0), cols = matrix.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    Console.Write(matrix[i, j].ToString("F2") + "\t");
                Console.WriteLine();
            }
        }
    }
}
