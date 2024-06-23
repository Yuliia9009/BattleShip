using System;

class Program
{
    static void Main()
    {
        int boardSize = 10; 
        GameBoard playerBoard = new GameBoard(boardSize);
        GameBoard computerBoard = new GameBoard(boardSize);

        computerBoard.RandomlyPlaceShips();

        Console.WriteLine("Добро пожаловать в игру Морской бой!");

        for (int shipSize = 4; shipSize >= 1; --shipSize)
        {
            for (int shipCount = 1; shipCount <= 5 - shipSize; ++shipCount)
            {
                Console.Clear();
                GameBoard.DisplayBoardsSideBySide(playerBoard, computerBoard);
                Console.WriteLine($"Размещаем {shipSize}-палубный корабль {shipCount}.");

                int row;
                char col;
                string orientation;
                Console.Write("Введите координаты носа корабля (например, 'A3'): ");
                string input = Console.ReadLine().ToUpper();

                if (input.Length < 2 || !char.IsLetter(input[0]) || !char.IsDigit(input[1]))
                {
                    Console.WriteLine("Ошибка! Неправильный ввод координат.");
                    shipCount--;
                    continue;
                }

                col = input[0];
                row = int.Parse(input.Substring(1));

                if (col < 'A' || col > 'J' || row < 1 || row > 10)
                {
                    Console.WriteLine("Ошибка! Неправильный ввод координат.");
                    shipCount--;
                    continue;
                }

                row -= 1;

                Console.Write("Введите ориентацию корабля (v - вертикально, h - горизонтально): ");
                orientation = Console.ReadLine().ToLower();

                if (orientation != "v" && orientation != "h")
                {
                    Console.WriteLine("Ошибка! Неправильный ввод ориентации.");
                    shipCount--;
                    continue;
                }

                bool horizontal = orientation == "h";

                if (playerBoard.IsValidPlacement(row, col - 'A', shipSize, horizontal))
                {
                    playerBoard.PlaceShip(row, col - 'A', shipSize, horizontal);
                    Console.WriteLine("Корабль успешно размещен!");

                    Console.Clear(); 
                    Console.WriteLine("Игровое поле после размещения корабля:");
                    GameBoard.DisplayBoardsSideBySide(playerBoard, computerBoard);
                }
                else
                {
                    Console.WriteLine("Ошибка! Невозможно разместить корабль в указанном месте.");
                    shipCount--;
                }
            }
        }

        while (playerBoard.HasShipsLeft() && computerBoard.HasShipsLeft())
        {
            Console.WriteLine("Ваш ход:");
            int row;
            char col;
            Console.Write("Введите координаты выстрела (например, 'A3'): ");
            string input = Console.ReadLine().ToUpper();

            if (input.Length < 2 || !char.IsLetter(input[0]) || !char.IsDigit(input[1]))
            {
                Console.WriteLine("Ошибка! Неправильный ввод координат.");
                continue;
            }

            col = input[0];
            row = int.Parse(input.Substring(1));

            if (col < 'A' || col > 'J' || row < 1 || row > 10)
            {
                Console.WriteLine("Ошибка! Неправильный ввод координат.");
                continue;
            }

            row -= 1;

            if (computerBoard.Shoot(row, col - 'A'))
            {
                Console.WriteLine("Попадание!");
            }
            else
            {
                Console.WriteLine("Промах.");
            }

            Console.WriteLine("Поле после вашего выстрела:");
            GameBoard.DisplayBoardsSideBySide(playerBoard, computerBoard);

            if (!computerBoard.HasShipsLeft())
            {
                Console.WriteLine("Вы победили! Поздравляем!");
                break;
            }

            Random rand = new Random();
            int computerRow = rand.Next(0, 10);
            int computerCol = rand.Next(0, 10);

            while (playerBoard.GetCell(computerRow, computerCol).GetStatus() == Cell.Status.HIT || 
                   playerBoard.GetCell(computerRow, computerCol).GetStatus() == Cell.Status.MISS)
            {
                computerRow = rand.Next(0, 10);
                computerCol = rand.Next(0, 10);
            }

            if (playerBoard.Shoot(computerRow, computerCol))
            {
                Console.WriteLine($"Компьютер попал в ваш корабль на клетке {Convert.ToChar('A' + computerCol)}{computerRow + 1}!");
            }
            else
            {
                Console.WriteLine($"Компьютер промазал в клетку {Convert.ToChar('A' + computerCol)}{computerRow + 1}.");
            }

            Console.WriteLine("Ваше поле после хода компьютера:");
            GameBoard.DisplayBoardsSideBySide(playerBoard, computerBoard);

            if (!playerBoard.HasShipsLeft())
            {
                Console.WriteLine("Компьютер победил. Попробуйте еще раз!");
                break;
            }
        }

        Console.WriteLine("Игра окончена. Спасибо за игру!");
    }
}

