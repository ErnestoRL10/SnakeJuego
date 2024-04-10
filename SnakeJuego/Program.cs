using System;
using System.Collections.Generic;
using System.Threading;

namespace SnakeGame
{
    class Program
    {
        static int width = 40;
        static int height = 20;
        static int score = 0;
        static int delay = 100;
        static bool gameOver = false;
        static Random random = new Random();

        static List<int[]> snake = new List<int[]>();
        static List<int[]> mines = new List<int[]>(); // Lista para almacenar las minas
        static int[] food = new int[2];

        static int dx = 1;
        static int dy = 0;

        static void Main(string[] args)
        {
            Console.Title = "Snake Game";
            Console.CursorVisible = false;
            Console.SetWindowSize(width + 1, height + 2);
            Console.SetBufferSize(width + 1, height + 2);

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Clear();

            Console.SetCursorPosition(width / 2 - 5, height / 2);
            Console.Write("Press any key to play");

            Console.ReadKey();
            Console.Clear();

            DrawBorder();
            DrawFood();
            DrawSnake();
            DrawMines(); // Dibujar minas

            Thread inputThread = new Thread(ReadInput);
            inputThread.Start();

            while (!gameOver)
            {
                MoveSnake();
                if (IsEatingFood())
                {
                    score++;
                    DrawFood();
                }
                Thread.Sleep(delay);
            }

            Console.SetCursorPosition(width / 2 - 5, height / 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("U LOST");
            Console.SetCursorPosition(width / 2 - 8, height / 2 + 1);
            Console.Write($"Score: {score}");
            Console.SetCursorPosition(0, height + 1);
        }

        static void DrawBorder()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Green;

            for (int i = 0; i < width + 2; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("=");
                Console.SetCursorPosition(i, height + 1);
                Console.Write("=");
            }

            for (int i = 1; i < height + 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("|");
                Console.SetCursorPosition(width + 1, i);
                Console.Write("|");
            }
        }

        static void DrawFood()
        {
            food[0] = random.Next(1, width);
            food[1] = random.Next(1, height);

            Console.SetCursorPosition(food[0], food[1]);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("$");
        }

        static void DrawSnake()
        {
            snake.Clear();
            snake.Add(new int[] { width / 2, height / 2 });
            Console.SetCursorPosition(snake[0][0], snake[0][1]);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("#");
        }

        static void DrawMines()
        {
            for (int i = 0; i < 5; i++) // Puedes ajustar el número de minas
            {
                int[] mine = { random.Next(1, width), random.Next(1, height) };
                mines.Add(mine);
                Console.SetCursorPosition(mine[0], mine[1]);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("X");
            }
        }

        static void MoveSnake()
        {
            int[] newHead = { snake[0][0] + dx, snake[0][1] + dy };

            // Verificar si la nueva cabeza colisiona con una mina
            foreach (var mine in mines)
            {
                if (newHead[0] == mine[0] && newHead[1] == mine[1])
                {
                    gameOver = true;
                    return;
                }
            }

            // Verificar si la nueva cabeza colisiona con el borde del área de juego
            if (newHead[0] <= 0 || newHead[0] >= width + 1 || newHead[1] <= 0 || newHead[1] >= height + 1)
            {
                gameOver = true;
                return;
            }

            Console.SetCursorPosition(snake[snake.Count - 1][0], snake[snake.Count - 1][1]);
            Console.Write(" ");
            snake.RemoveAt(snake.Count - 1);

            snake.Insert(0, newHead);
            Console.SetCursorPosition(newHead[0], newHead[1]);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("#");
        }

        static bool IsEatingFood()
        {
            if (snake[0][0] == food[0] && snake[0][1] == food[1])
            {
                snake.Add(new int[] { food[0], food[1] });
                return true;
            }
            return false;
        }

        static void ReadInput()
        {
            while (!gameOver)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        dx = 0;
                        dy = -1;
                        break;
                    case ConsoleKey.DownArrow:
                        dx = 0;
                        dy = 1;
                        break;
                    case ConsoleKey.LeftArrow:
                        dx = -1;
                        dy = 0;
                        break;
                    case ConsoleKey.RightArrow:
                        dx = 1;
                        dy = 0;
                        break;
                    default:
                        gameOver = true;
                        break;
                }
            }
        }
    }
}


