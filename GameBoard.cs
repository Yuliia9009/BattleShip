using System;
using System.Collections.Generic;

public class GameBoard
{
    private List<List<Cell>> board;
    public int BoardSize { get; private set; }

    public GameBoard(int size)
    {
        BoardSize = size;
        board = new List<List<Cell>>(size);
        for (int i = 0; i < size; i++)
        {
            var row = new List<Cell>(size);
            for (int j = 0; j < size; j++)
            {
                row.Add(new Cell());
            }
            board.Add(row);
        }
    }

    public void Display(bool showShips)
    {
        char colLabel = 'A';
        Console.Write(" ");
        for (int i = 0; i < BoardSize; ++i)
        {
            Console.Write(" " + colLabel++);
        }
        Console.WriteLine();

        for (int i = 0; i < BoardSize; ++i)
        {
            Console.Write(i + 1);
            for (int j = 0; j < BoardSize; ++j)
            {
                switch (board[i][j].GetStatus())
                {
                    case Cell.Status.EMPTY:
                        Console.Write(" .");
                        break;
                    case Cell.Status.SHIP:
                        Console.Write(showShips ? " S" : " .");
                        break;
                    case Cell.Status.HIT:
                        Console.Write(" X");
                        break;
                    case Cell.Status.MISS:
                        Console.Write(" O");
                        break;
                }
            }
            Console.WriteLine();
        }
    }

    public static void DisplayBoardsSideBySide(GameBoard playerBoard, GameBoard computerBoard)
    {
        int boardSize = playerBoard.BoardSize;
        char colLabel = 'A';

        Console.Write("   ");
        for (int i = 0; i < boardSize; ++i)
        {
            Console.Write(" " + colLabel++);
        }
        Console.Write("       ");
        colLabel = 'A';
        for (int i = 0; i < boardSize; ++i)
        {
            Console.Write(" " + colLabel++);
        }
        Console.WriteLine();

        for (int i = 0; i < boardSize; ++i)
        {
            Console.Write(i + 1);
            if (i + 1 < 10) Console.Write(" ");

            for (int j = 0; j < boardSize; ++j)
            {
                switch (playerBoard.board[i][j].GetStatus())
                {
                    case Cell.Status.EMPTY:
                        Console.Write(" .");
                        break;
                    case Cell.Status.SHIP:
                        Console.Write(" S");
                        break;
                    case Cell.Status.HIT:
                        Console.Write(" X");
                        break;
                    case Cell.Status.MISS:
                        Console.Write(" O");
                        break;
                }
            }

            Console.Write("       ");
            Console.Write(i + 1);
            if (i + 1 < 10) Console.Write(" ");

            for (int j = 0; j < boardSize; ++j)
            {
                switch (computerBoard.board[i][j].GetStatus())
                {
                    case Cell.Status.EMPTY:
                        Console.Write(" .");
                        break;
                    case Cell.Status.SHIP:
                        Console.Write(" .");
                        break;
                    case Cell.Status.HIT:
                        Console.Write(" X");
                        break;
                    case Cell.Status.MISS:
                        Console.Write(" O");
                        break;
                }
            }
            Console.WriteLine();
        }
    }

    public bool IsValidPlacement(int row, int col, int size, bool horizontal)
    {
        if (horizontal)
        {
            if (col + size > BoardSize) return false;

            for (int j = col; j < col + size; ++j)
            {
                if (!IsCellEmpty(row, j)) return false;
                if (row > 0 && !IsCellEmpty(row - 1, j)) return false;
                if (row < BoardSize - 1 && !IsCellEmpty(row + 1, j)) return false;
            }

            if (col > 0 && !IsCellEmpty(row, col - 1)) return false;
            if (col + size < BoardSize && !IsCellEmpty(row, col + size)) return false;

            if (row > 0 && col > 0 && !IsCellEmpty(row - 1, col - 1)) return false;
            if (row > 0 && col + size < BoardSize && !IsCellEmpty(row - 1, col + size)) return false;
            if (row < BoardSize - 1 && col > 0 && !IsCellEmpty(row + 1, col - 1)) return false;
            if (row < BoardSize - 1 && col + size < BoardSize && !IsCellEmpty(row + 1, col + size)) return false;
        }
        else
        {
            if (row + size > BoardSize) return false;

            for (int i = row; i < row + size; ++i)
            {
                if (!IsCellEmpty(i, col)) return false;

                if (col > 0 && !IsCellEmpty(i, col - 1)) return false;
                if (col < BoardSize - 1 && !IsCellEmpty(i, col + 1)) return false;
            }

            if (row > 0 && !IsCellEmpty(row - 1, col)) return false;
            if (row + size < BoardSize && !IsCellEmpty(row + size, col)) return false;

            if (row > 0 && col > 0 && !IsCellEmpty(row - 1, col - 1)) return false;
            if (row > 0 && col < BoardSize - 1 && !IsCellEmpty(row - 1, col + 1)) return false;
            if (row + size < BoardSize && col > 0 && !IsCellEmpty(row + size, col - 1)) return false;
            if (row + size < BoardSize && col < BoardSize - 1 && !IsCellEmpty(row + size, col + 1)) return false;
        }

        return true;
    }

    public void PlaceShip(int row, int col, int size, bool horizontal)
    {
        if (horizontal)
        {
            for (int j = col; j < col + size; ++j)
            {
                board[row][j].SetStatus(Cell.Status.SHIP);
            }
        }
        else
        {
            for (int i = row; i < row + size; ++i)
            {
                board[i][col].SetStatus(Cell.Status.SHIP);
            }
        }
    }

    private bool IsCellEmpty(int row, int col)
    {
        return board[row][col].GetStatus() == Cell.Status.EMPTY;
    }

    public bool Shoot(int row, int col)
    {
        if (board[row][col].GetStatus() == Cell.Status.SHIP)
        {
            board[row][col].SetStatus(Cell.Status.HIT);
            return true;
        }
        else if (board[row][col].GetStatus() == Cell.Status.EMPTY)
        {
            board[row][col].SetStatus(Cell.Status.MISS);
            return false;
        }
        return false;
    }

    public void RandomlyPlaceShips()
    {
        Random rand = new Random();
        int[] shipSizes = { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };

        foreach (int shipSize in shipSizes)
        {
            bool placed = false;
            while (!placed)
            {
                int row = rand.Next(BoardSize);
                int col = rand.Next(BoardSize);
                bool horizontal = rand.Next(2) == 0;

                if (IsValidPlacement(row, col, shipSize, horizontal))
                {
                    PlaceShip(row, col, shipSize, horizontal);
                    placed = true;
                }
            }
        }
    }

    public bool HasShipsLeft()
    {
        for (int i = 0; i < BoardSize; ++i)
        {
            for (int j = 0; j < BoardSize; ++j)
            {
                if (board[i][j].GetStatus() == Cell.Status.SHIP)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public Cell GetCell(int row, int col)
    {
        return board[row][col];
    }
}
