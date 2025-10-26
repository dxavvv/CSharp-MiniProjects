using System;
using System.Collections.Generic;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Sudoku Solver";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== SUDOKU SOLVER ===\n");
            Console.ResetColor();

            int[,] board = new int[9, 9];

            Console.WriteLine("Choose input method:");
            Console.WriteLine("1 - Enter full grid");
            Console.WriteLine("2 - Enter values one by one");
            Console.Write("Your choice: ");
            
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                EnterFullGrid(board);
            }
            else
            {
                EnterValuesOneByOne(board);
            }

            Console.WriteLine("\nInitial board:");
            PrintBoard(board);

            if (SolveSudoku(board))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nSolved board:");
                PrintBoard(board);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNo solution exists!");
            }

            Console.ResetColor();
        }

        static void EnterFullGrid(int[,] board)
        {
            Console.WriteLine("\nEnter 9 rows (use 0 for empty cells, separate with spaces):");
            for (int i = 0; i < 9; i++)
            {
                Console.Write($"Row {i + 1}: ");
                string[] values = Console.ReadLine().Split(' ');
                
                for (int j = 0; j < 9; j++)
                {
                    board[i, j] = int.Parse(values[j]);
                }
            }
        }

        static void EnterValuesOneByOne(int[,] board)
        {
            Console.WriteLine("\nEnter values (row col value), type 'done' to finish:");
            Console.WriteLine("Example: '1 2 5' puts 5 at row 1, column 2");
            
            while (true)
            {
                Console.Write("Enter value: ");
                string input = Console.ReadLine().Trim().ToLower();
                
                if (input == "done") break;
                
                string[] parts = input.Split(' ');
                if (parts.Length == 3)
                {
                    int row = int.Parse(parts[0]) - 1;
                    int col = int.Parse(parts[1]) - 1;
                    int value = int.Parse(parts[2]);
                    
                    if (row >= 0 && row < 9 && col >= 0 && col < 9 && value >= 1 && value <= 9)
                    {
                        board[row, col] = value;
                        Console.WriteLine($"Placed {value} at ({row + 1}, {col + 1})");
                    }
                    else
                    {
                        Console.WriteLine("Invalid position or value!");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid format! Use: row col value");
                }
            }
        }

        static bool SolveSudoku(int[,] board)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col] == 0)
                    {
                        for (int num = 1; num <= 9; num++)
                        {
                            if (IsValid(board, row, col, num))
                            {
                                board[row, col] = num;
                                
                                if (SolveSudoku(board))
                                    return true;
                                
                                board[row, col] = 0;
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        static bool IsValid(int[,] board, int row, int col, int num)
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[row, i] == num) return false;
                if (board[i, col] == num) return false;
            }

            int startRow = row - row % 3;
            int startCol = col - col % 3;
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[startRow + i, startCol + j] == num)
                        return false;
                }
            }

            return true;
        }

        static void PrintBoard(int[,] board)
        {
            Console.WriteLine();
            for (int i = 0; i < 9; i++)
            {
                if (i % 3 == 0 && i != 0)
                    Console.WriteLine("------+-------+------");
                
                for (int j = 0; j < 9; j++)
                {
                    if (j % 3 == 0 && j != 0)
                        Console.Write("| ");
                    
                    if (board[i, j] == 0)
                        Console.Write(". ");
                    else
                        Console.Write(board[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
