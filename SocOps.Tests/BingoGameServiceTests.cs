#nullable enable
using Xunit;
using Moq;
using Microsoft.JSInterop;
using SocOps.Services;
using SocOps.Models;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace SocOps.Tests.Services
{
    public class BingoGameServiceTests
    {
        private readonly Mock<IJSRuntime> _mockJsRuntime;
        private readonly BingoGameService _service;

        public BingoGameServiceTests()
        {
            _mockJsRuntime = new Mock<IJSRuntime>();
            _service = new BingoGameService(_mockJsRuntime.Object);
        }

        [Fact]
        public void Initial_GameState_Should_Be_Start()
        {
            // Assert
            Assert.Equal(GameState.Start, _service.CurrentGameState);
        }

        [Fact]
        public void Initial_GameMode_Should_Be_Bingo()
        {
            // Assert
            Assert.Equal(GameMode.Bingo, _service.CurrentGameMode);
        }

        [Fact]
        public void Initial_Board_Should_Be_Empty()
        {
            // Assert
            Assert.NotNull(_service.Board);
            Assert.Empty(_service.Board);
        }

        [Fact]
        public void Initial_ScavengerHuntList_Should_Be_Empty()
        {
            // Assert
            Assert.NotNull(_service.ScavengerHuntList);
            Assert.Empty(_service.ScavengerHuntList);
        }

        [Fact]
        public void Initial_WinningLine_Should_Be_Null()
        {
            // Assert
            Assert.Null(_service.WinningLine);
        }

        [Fact]
        public void Initial_ShowBingoModal_Should_Be_False()
        {
            // Assert
            Assert.False(_service.ShowBingoModal);
        }

        [Fact]
        public void Initial_WinningSquareIds_Should_Be_Empty()
        {
            // Assert
            Assert.NotNull(_service.WinningSquareIds);
            Assert.Empty(_service.WinningSquareIds);
        }

        [Fact]
        public void StartGame_With_Bingo_Mode_Should_Generate_Board()
        {
            // Act
            _service.StartGame(GameMode.Bingo);

            // Assert
            Assert.Equal(GameMode.Bingo, _service.CurrentGameMode);
            Assert.Equal(GameState.Playing, _service.CurrentGameState);
            Assert.Equal(25, _service.Board.Count);
            Assert.Empty(_service.ScavengerHuntList);
        }

        [Fact]
        public void StartGame_With_Bingo_Mode_Should_Have_Free_Space_In_Center()
        {
            // Act
            _service.StartGame(GameMode.Bingo);

            // Assert
            var centerSquare = _service.Board[12];
            Assert.True(centerSquare.IsFreeSpace);
            Assert.True(centerSquare.IsMarked);
        }

        [Fact]
        public void StartGame_With_ScavengerHunt_Mode_Should_Generate_FlatList()
        {
            // Act
            _service.StartGame(GameMode.ScavengerHunt);

            // Assert
            Assert.Equal(GameMode.ScavengerHunt, _service.CurrentGameMode);
            Assert.Equal(GameState.Playing, _service.CurrentGameState);
            Assert.Equal(24, _service.ScavengerHuntList.Count);
            Assert.Empty(_service.Board);
        }

        [Fact]
        public void StartGame_With_ScavengerHunt_Mode_Should_Not_Have_Free_Space()
        {
            // Act
            _service.StartGame(GameMode.ScavengerHunt);

            // Assert
            Assert.DoesNotContain(_service.ScavengerHuntList, s => s.IsFreeSpace);
        }

        [Fact]
        public void StartGame_Should_Reset_WinningLine()
        {
            // Arrange
            _service.StartGame(GameMode.Bingo);
            // Simulate a win first
            var board = _service.Board;
            // Mark a complete row
            for (int i = 0; i < 5; i++)
            {
                _service.HandleSquareClick(i);
            }

            // Act
            _service.StartGame(GameMode.Bingo);

            // Assert
            Assert.Null(_service.WinningLine);
            Assert.False(_service.ShowBingoModal);
        }

        [Fact]
        public void HandleSquareClick_Should_Toggle_Square_Marked_State()
        {
            // Arrange
            _service.StartGame(GameMode.Bingo);
            var squareId = 0;
            var originalState = _service.Board[0].IsMarked;

            // Act
            _service.HandleSquareClick(squareId);

            // Assert
            Assert.NotEqual(originalState, _service.Board[0].IsMarked);
        }

        [Fact]
        public void HandleSquareClick_Should_Not_Toggle_Free_Space()
        {
            // Arrange
            _service.StartGame(GameMode.Bingo);
            var freeSpace = _service.Board.First(s => s.IsFreeSpace);
            var originalState = freeSpace.IsMarked;

            // Act
            _service.HandleSquareClick(freeSpace.Id);

            // Assert
            var resultFreeSpace = _service.Board.First(s => s.Id == freeSpace.Id);
            Assert.Equal(originalState, resultFreeSpace.IsMarked);
        }

        [Fact]
        public void HandleSquareClick_Should_Detect_Bingo_On_Row()
        {
            // Arrange
            _service.StartGame(GameMode.Bingo);

            // Act - Mark first row (indices 0-3, index 4 is free space or needs toggle)
            for (int i = 0; i < 4; i++)
            {
                if (!_service.Board[i].IsFreeSpace)
                    _service.HandleSquareClick(i);
            }
            // Mark index 4 if not free space
            if (!_service.Board[4].IsFreeSpace)
                _service.HandleSquareClick(4);

            // Assert
            Assert.Equal(GameState.Bingo, _service.CurrentGameState);
            Assert.NotNull(_service.WinningLine);
            Assert.True(_service.ShowBingoModal);
        }

        [Fact]
        public void HandleSquareClick_Should_Not_Detect_Bingo_With_Only_4_In_Row()
        {
            // Arrange
            _service.StartGame(GameMode.Bingo);

            // Act - Mark only 4 squares in first row
            for (int i = 0; i < 4; i++)
            {
                if (!_service.Board[i].IsFreeSpace)
                    _service.HandleSquareClick(i);
            }

            // Assert
            Assert.Equal(GameState.Playing, _service.CurrentGameState);
            Assert.Null(_service.WinningLine);
        }

        [Fact]
        public void ToggleScavengerHuntItem_Should_Toggle_Item_Marked_State()
        {
            // Arrange
            _service.StartGame(GameMode.ScavengerHunt);
            var itemId = _service.ScavengerHuntList[0].Id;
            var originalState = _service.ScavengerHuntList[0].IsMarked;

            // Act
            _service.ToggleScavengerHuntItem(itemId);

            // Assert
            var toggledItem = _service.ScavengerHuntList.First(s => s.Id == itemId);
            Assert.NotEqual(originalState, toggledItem.IsMarked);
        }

        [Fact]
        public void ToggleScavengerHuntItem_Should_Not_Work_In_Bingo_Mode()
        {
            // Arrange
            _service.StartGame(GameMode.Bingo);
            var board = _service.Board;

            // Act
            _service.ToggleScavengerHuntItem(0);

            // Assert - Board should remain unchanged
            Assert.Equal(board.Count, _service.Board.Count);
        }

        [Fact]
        public void ScavengerHuntProgress_Should_Return_Zero_When_List_Is_Empty()
        {
            // Assert
            Assert.Equal(0.0, _service.ScavengerHuntProgress);
        }

        [Fact]
        public void ScavengerHuntProgress_Should_Calculate_Correctly()
        {
            // Arrange
            _service.StartGame(GameMode.ScavengerHunt);
            // Mark 6 out of 24 items (25%)
            for (int i = 0; i < 6; i++)
            {
                _service.ToggleScavengerHuntItem(i);
            }

            // Assert
            Assert.Equal(0.25, _service.ScavengerHuntProgress, 2);
        }

        [Fact]
        public void IsScavengerHuntComplete_Should_Be_False_Below_Threshold()
        {
            // Arrange
            _service.StartGame(GameMode.ScavengerHunt);
            // Mark 18 out of 24 items (75% - below 80% threshold)
            for (int i = 0; i < 18; i++)
            {
                _service.ToggleScavengerHuntItem(i);
            }

            // Assert
            Assert.False(_service.IsScavengerHuntComplete);
        }

        [Fact]
        public void IsScavengerHuntComplete_Should_Be_True_At_Threshold()
        {
            // Arrange
            _service.StartGame(GameMode.ScavengerHunt);
            // Mark 20 out of 24 items (83.33% - above 80% threshold)
            for (int i = 0; i < 20; i++)
            {
                _service.ToggleScavengerHuntItem(i);
            }

            // Assert
            Assert.True(_service.IsScavengerHuntComplete);
        }

        [Fact]
        public void ResetGame_Should_Set_State_To_Start()
        {
            // Arrange
            _service.StartGame(GameMode.Bingo);

            // Act
            _service.ResetGame();

            // Assert
            Assert.Equal(GameState.Start, _service.CurrentGameState);
            Assert.Empty(_service.Board);
            Assert.Null(_service.WinningLine);
            Assert.False(_service.ShowBingoModal);
        }

        [Fact]
        public void DismissModal_Should_Set_ShowBingoModal_To_False()
        {
            // Arrange
            _service.StartGame(GameMode.Bingo);
            // Force a bingo state
            var board = _service.Board;
            // Mark a complete row to trigger bingo
            for (int i = 0; i < 5; i++)
            {
                if (!board[i].IsFreeSpace)
                    _service.HandleSquareClick(i);
            }

            // Act
            _service.DismissModal();

            // Assert
            Assert.False(_service.ShowBingoModal);
        }

        [Fact]
        public void OnStateChanged_Event_Should_Be_Invoked_On_State_Change()
        {
            // Arrange
            var eventRaised = false;
            _service.OnStateChanged += () => eventRaised = true;

            // Act
            _service.StartGame(GameMode.Bingo);

            // Assert
            Assert.True(eventRaised);
        }

        [Fact]
        public async Task InitializeAsync_Should_Not_Throw_When_No_Saved_State()
        {
            // Arrange
            _mockJsRuntime
                .Setup(js => js.InvokeAsync<string?>("localStorage.getItem", It.IsAny<object[]>()))
                .ReturnsAsync((string?)null);

            // Act & Assert
            await _service.InitializeAsync();
            Assert.Equal(GameState.Start, _service.CurrentGameState);
        }
    }
}
