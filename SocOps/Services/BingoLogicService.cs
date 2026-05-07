using SocOps.Data;
using SocOps.Models;

namespace SocOps.Services;

public class BingoLogicService
{
    private const int BOARD_SIZE = 5;
    private const int TOTAL_SQUARES = 25;
    private const int SCAVENGER_HUNT_COUNT = 24;
    private const int CENTER_INDEX = 12; // 5x5 grid, center is index 12 (row 2, col 2)
    private static readonly Random _random = new();

    /// <summary>
    /// Shuffle an array using Fisher-Yates algorithm
    /// </summary>
    private static List<T> ShuffleArray<T>(List<T> array)
    {
        var shuffled = new List<T>(array);
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }
        return shuffled;
    }

    /// <summary>
    /// Create a new BingoSquareData instance
    /// </summary>
    public static BingoSquareData CreateSquare(int id, string text, bool isMarked = false, bool isFreeSpace = false)
    {
        return new BingoSquareData
        {
            Id = id,
            Text = text,
            IsMarked = isMarked,
            IsFreeSpace = isFreeSpace
        };
    }

    /// <summary>
    /// Generate a new 5x5 bingo board
    /// </summary>
    public static List<BingoSquareData> GenerateBoard()
    {
        var shuffledQuestions = ShuffleArray(Questions.QuestionsList).Take(SCAVENGER_HUNT_COUNT).ToList();
        var board = new List<BingoSquareData>();

        int questionIndex = 0;
        for (int i = 0; i < TOTAL_SQUARES; i++)
        {
            if (i == CENTER_INDEX)
            {
                board.Add(CreateSquare(i, Questions.FREE_SPACE, isMarked: true, isFreeSpace: true));
            }
            else
            {
                board.Add(CreateSquare(i, shuffledQuestions[questionIndex++]));
            }
        }

        return board;
    }

    /// <summary>
    /// Toggle a square's marked state
    /// </summary>
    public static List<BingoSquareData> ToggleSquare(List<BingoSquareData> board, int squareId)
    {
        return board.Select(square =>
            square.Id == squareId && !square.IsFreeSpace
                ? CreateSquare(square.Id, square.Text, !square.IsMarked, square.IsFreeSpace)
                : square
        ).ToList();
    }

    /// <summary>
    /// Get all possible winning lines
    /// </summary>
    private static List<BingoLine> GetWinningLines()
    {
        var lines = new List<BingoLine>();

        // Rows
        for (int row = 0; row < BOARD_SIZE; row++)
        {
            var squares = Enumerable.Range(0, BOARD_SIZE)
                .Select(col => row * BOARD_SIZE + col)
                .ToList();
            lines.Add(new BingoLine { Type = "row", Index = row, Squares = squares });
        }

        // Columns
        for (int col = 0; col < BOARD_SIZE; col++)
        {
            var squares = Enumerable.Range(0, BOARD_SIZE)
                .Select(row => row * BOARD_SIZE + col)
                .ToList();
            lines.Add(new BingoLine { Type = "column", Index = col, Squares = squares });
        }

        // Diagonals
        lines.Add(new BingoLine { Type = "diagonal", Index = 0, Squares = new List<int> { 0, 6, 12, 18, 24 } });
        lines.Add(new BingoLine { Type = "diagonal", Index = 1, Squares = new List<int> { 4, 8, 12, 16, 20 } });

        return lines;
    }

    /// <summary>
    /// Check if there's a bingo and return the winning line
    /// </summary>
    public static BingoLine? CheckBingo(List<BingoSquareData> board)
    {
        var lines = GetWinningLines();

        foreach (var line in lines)
        {
            var isComplete = line.Squares.All(idx => board[idx].IsMarked);
            if (isComplete)
            {
                return line;
            }
        }

        return null;
    }

    /// <summary>
    /// Get the square IDs that are part of a winning line
    /// </summary>
    public static HashSet<int> GetWinningSquareIds(BingoLine? line)
    {
        if (line == null) return new HashSet<int>();
        return new HashSet<int>(line.Squares);
    }

    /// <summary>
    /// Generate a flat list of 24 shuffled questions (no free space, no grid)
    /// Used for Scavenger Hunt mode
    /// </summary>
    public static List<BingoSquareData> GenerateFlatList()
    {
        var shuffledQuestions = ShuffleArray(Questions.QuestionsList).Take(SCAVENGER_HUNT_COUNT).ToList();
        var list = new List<BingoSquareData>();

        for (int i = 0; i < SCAVENGER_HUNT_COUNT; i++)
        {
            list.Add(CreateSquare(i, shuffledQuestions[i]));
        }

        return list;
    }
}
