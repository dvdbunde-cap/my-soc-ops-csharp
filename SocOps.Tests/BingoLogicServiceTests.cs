using Xunit;
using SocOps.Services;
using SocOps.Models;
using System.Collections.Generic;
using System.Linq;

namespace SocOps.Tests.Services
{
    public class BingoLogicServiceTests
    {
        [Fact]
        public void GenerateBoard_Should_Return_25_Squares()
        {
            // Act
            var board = BingoLogicService.GenerateBoard();

            // Assert
            Assert.Equal(25, board.Count);
        }

        [Fact]
        public void GenerateBoard_Should_Have_Free_Space_In_Center()
        {
            // Act
            var board = BingoLogicService.GenerateBoard();

            // Assert
            var centerSquare = board[12];
            Assert.True(centerSquare.IsFreeSpace);
            Assert.Equal("FREE SPACE", centerSquare.Text);
            Assert.True(centerSquare.IsMarked);
        }

        [Fact]
        public void GenerateBoard_Should_Have_24_Non_Free_Space_Squares()
        {
            // Act
            var board = BingoLogicService.GenerateBoard();

            // Assert
            var nonFreeSquares = board.Where(s => !s.IsFreeSpace).ToList();
            Assert.Equal(24, nonFreeSquares.Count);
        }

        [Fact]
        public void GenerateBoard_Should_Assign_Unique_Ids()
        {
            // Act
            var board = BingoLogicService.GenerateBoard();

            // Assert
            var ids = board.Select(s => s.Id).ToList();
            Assert.Equal(25, ids.Distinct().Count());
        }

        [Fact]
        public void GenerateBoard_Should_Have_Sequential_Ids_From_0_To_24()
        {
            // Act
            var board = BingoLogicService.GenerateBoard();

            // Assert
            for (int i = 0; i < 25; i++)
            {
                Assert.Equal(i, board[i].Id);
            }
        }

        [Fact]
        public void ToggleSquare_Should_Toggle_IsMarked_From_False_To_True()
        {
            // Arrange
            var board = CreateTestBoardWithOneUnmarkedSquare();
            var squareId = 0;

            // Act
            var result = BingoLogicService.ToggleSquare(board, squareId);

            // Assert
            var toggledSquare = result.First(s => s.Id == squareId);
            Assert.True(toggledSquare.IsMarked);
        }

        [Fact]
        public void ToggleSquare_Should_Toggle_IsMarked_From_True_To_False()
        {
            // Arrange
            var board = CreateTestBoardWithOneMarkedSquare();
            var squareId = 0;

            // Act
            var result = BingoLogicService.ToggleSquare(board, squareId);

            // Assert
            var toggledSquare = result.First(s => s.Id == squareId);
            Assert.False(toggledSquare.IsMarked);
        }

        [Fact]
        public void ToggleSquare_Should_Not_Toggle_Free_Space()
        {
            // Arrange
            var board = BingoLogicService.GenerateBoard();
            var freeSpace = board.First(s => s.IsFreeSpace);
            var originalMarkedState = freeSpace.IsMarked;

            // Act
            var result = BingoLogicService.ToggleSquare(board, freeSpace.Id);

            // Assert
            var resultFreeSpace = result.First(s => s.Id == freeSpace.Id);
            Assert.Equal(originalMarkedState, resultFreeSpace.IsMarked);
        }

        [Fact]
        public void ToggleSquare_Should_Not_Modify_Other_Squares()
        {
            // Arrange
            var board = BingoLogicService.GenerateBoard();
            var originalSquare1 = board[0];
            var originalSquare3 = board[3];
            var squareIdToToggle = 1;

            // Act
            var result = BingoLogicService.ToggleSquare(board, squareIdToToggle);

            // Assert
            Assert.Equal(originalSquare1.IsMarked, result[0].IsMarked);
            Assert.Equal(originalSquare3.IsMarked, result[3].IsMarked);
        }

        [Fact]
        public void ToggleSquare_Should_Return_New_List_Instance()
        {
            // Arrange
            var board = BingoLogicService.GenerateBoard();

            // Act
            var result = BingoLogicService.ToggleSquare(board, 0);

            // Assert
            Assert.NotSame(board, result);
        }

        [Fact]
        public void CheckBingo_Should_Return_Null_When_No_Bingo()
        {
            // Arrange
            var board = BingoLogicService.GenerateBoard();
            // Only mark 4 squares in a row (not all 5)
            for (int i = 0; i < 4; i++)
            {
                board = BingoLogicService.ToggleSquare(board, i);
            }

            // Act
            var result = BingoLogicService.CheckBingo(board);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CheckBingo_Should_Return_Row_Winning_Line()
        {
            // Arrange
            var board = BingoLogicService.GenerateBoard();
            // Mark all squares in first row (indices 0-4)
            for (int i = 0; i < 5; i++)
            {
                if (!board[i].IsFreeSpace)
                    board = BingoLogicService.ToggleSquare(board, i);
            }

            // Act
            var result = BingoLogicService.CheckBingo(board);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("row", result.Type);
            Assert.Equal(0, result.Index);
            Assert.Equal(5, result.Squares.Count);
        }

        [Fact]
        public void CheckBingo_Should_Return_Column_Winning_Line()
        {
            // Arrange
            var board = BingoLogicService.GenerateBoard();
            // Mark all squares in first column (indices 0, 5, 10, 15, 20)
            int[] columnIndices = { 0, 5, 10, 15, 20 };
            foreach (var idx in columnIndices)
            {
                if (!board[idx].IsFreeSpace)
                    board = BingoLogicService.ToggleSquare(board, idx);
            }

            // Act
            var result = BingoLogicService.CheckBingo(board);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("column", result.Type);
            Assert.Equal(0, result.Index);
        }

        [Fact]
        public void CheckBingo_Should_Return_Diagonal_Winning_Line_TopLeft_To_BottomRight()
        {
            // Arrange
            var board = BingoLogicService.GenerateBoard();
            // Mark diagonal (indices 0, 6, 12, 18, 24) - 12 is free space
            int[] diagonalIndices = { 0, 6, 18, 24 };
            foreach (var idx in diagonalIndices)
            {
                board = BingoLogicService.ToggleSquare(board, idx);
            }

            // Act
            var result = BingoLogicService.CheckBingo(board);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("diagonal", result.Type);
            Assert.Equal(0, result.Index);
        }

        [Fact]
        public void CheckBingo_Should_Return_Diagonal_Winning_Line_TopRight_To_BottomLeft()
        {
            // Arrange
            var board = BingoLogicService.GenerateBoard();
            // Mark diagonal (indices 4, 8, 12, 16, 20) - 12 is free space
            int[] diagonalIndices = { 4, 8, 16, 20 };
            foreach (var idx in diagonalIndices)
            {
                board = BingoLogicService.ToggleSquare(board, idx);
            }

            // Act
            var result = BingoLogicService.CheckBingo(board);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("diagonal", result.Type);
            Assert.Equal(1, result.Index);
        }

        [Fact]
        public void GetWinningSquareIds_Should_Return_Empty_HashSet_When_Line_Is_Null()
        {
            // Act
            var result = BingoLogicService.GetWinningSquareIds(null);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetWinningSquareIds_Should_Return_Correct_Ids()
        {
            // Arrange
            var line = new BingoLine
            {
                Type = "row",
                Index = 0,
                Squares = new List<int> { 0, 1, 2, 3, 4 }
            };

            // Act
            var result = BingoLogicService.GetWinningSquareIds(line);

            // Assert
            Assert.Equal(5, result.Count);
            Assert.Contains(0, result);
            Assert.Contains(4, result);
        }

        [Fact]
        public void GenerateFlatList_Should_Return_24_Items()
        {
            // Act
            var result = BingoLogicService.GenerateFlatList();

            // Assert
            Assert.Equal(24, result.Count);
        }

        [Fact]
        public void GenerateFlatList_Should_Not_Have_Free_Space()
        {
            // Act
            var result = BingoLogicService.GenerateFlatList();

            // Assert
            Assert.DoesNotContain(result, s => s.IsFreeSpace);
            Assert.DoesNotContain(result, s => s.Text == "FREE SPACE");
        }

        [Fact]
        public void GenerateFlatList_Should_Assign_Sequential_Ids()
        {
            // Act
            var result = BingoLogicService.GenerateFlatList();

            // Assert
            for (int i = 0; i < 24; i++)
            {
                Assert.Equal(i, result[i].Id);
            }
        }

        [Fact]
        public void GenerateFlatList_Should_Not_Have_Marked_Items()
        {
            // Act
            var result = BingoLogicService.GenerateFlatList();

            // Assert
            Assert.DoesNotContain(result, s => s.IsMarked);
        }

        private static List<BingoSquareData> CreateTestBoardWithOneUnmarkedSquare()
        {
            return new List<BingoSquareData>
            {
                new BingoSquareData { Id = 0, Text = "Test", IsMarked = false, IsFreeSpace = false }
            };
        }

        private static List<BingoSquareData> CreateTestBoardWithOneMarkedSquare()
        {
            return new List<BingoSquareData>
            {
                new BingoSquareData { Id = 0, Text = "Test", IsMarked = true, IsFreeSpace = false }
            };
        }
    }
}
