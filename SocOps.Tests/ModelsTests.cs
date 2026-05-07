using System.Collections.Generic;
using Xunit;
using SocOps.Models;

namespace SocOps.Tests.Models
{
    public class BingoSquareDataTests
    {
        [Fact]
        public void Default_Initialization_Should_Have_Empty_Text()
        {
            // Arrange & Act
            var square = new BingoSquareData();

            // Assert
            Assert.Equal(string.Empty, square.Text);
        }

        [Fact]
        public void Default_Initialization_Should_Have_Id_Zero()
        {
            // Arrange & Act
            var square = new BingoSquareData();

            // Assert
            Assert.Equal(0, square.Id);
        }

        [Fact]
        public void Default_Initialization_Should_Not_Be_Marked()
        {
            // Arrange & Act
            var square = new BingoSquareData();

            // Assert
            Assert.False(square.IsMarked);
        }

        [Fact]
        public void Default_Initialization_Should_Not_Be_Free_Space()
        {
            // Arrange & Act
            var square = new BingoSquareData();

            // Assert
            Assert.False(square.IsFreeSpace);
        }

        [Fact]
        public void Should_Set_And_Get_Properties_Correctly()
        {
            // Arrange
            var square = new BingoSquareData
            {
                Id = 5,
                Text = "Test Question",
                IsMarked = true,
                IsFreeSpace = false
            };

            // Assert
            Assert.Equal(5, square.Id);
            Assert.Equal("Test Question", square.Text);
            Assert.True(square.IsMarked);
            Assert.False(square.IsFreeSpace);
        }
    }

    public class BingoLineTests
    {
        [Fact]
        public void Default_Initialization_Should_Have_Empty_Type()
        {
            // Arrange & Act
            var line = new BingoLine();

            // Assert
            Assert.Equal(string.Empty, line.Type);
        }

        [Fact]
        public void Default_Initialization_Should_Have_Index_Zero()
        {
            // Arrange & Act
            var line = new BingoLine();

            // Assert
            Assert.Equal(0, line.Index);
        }

        [Fact]
        public void Default_Initialization_Should_Have_Empty_Squares_List()
        {
            // Arrange & Act
            var line = new BingoLine();

            // Assert
            Assert.NotNull(line.Squares);
            Assert.Empty(line.Squares);
        }

        [Fact]
        public void Should_Set_And_Get_Properties_Correctly()
        {
            // Arrange
            var line = new BingoLine
            {
                Type = "row",
                Index = 2,
                Squares = new List<int> { 10, 11, 12, 13, 14 }
            };

            // Assert
            Assert.Equal("row", line.Type);
            Assert.Equal(2, line.Index);
            Assert.Equal(5, line.Squares.Count);
            Assert.Contains(12, line.Squares);
        }
    }

    public class GameStateTests
    {
        [Fact]
        public void Should_Have_Start_Value()
        {
            // Assert
            Assert.Equal(0, (int)GameState.Start);
        }

        [Fact]
        public void Should_Have_Playing_Value()
        {
            // Assert
            Assert.Equal(1, (int)GameState.Playing);
        }

        [Fact]
        public void Should_Have_Bingo_Value()
        {
            // Assert
            Assert.Equal(2, (int)GameState.Bingo);
        }
    }

    public class GameModeTests
    {
        [Fact]
        public void Should_Have_Bingo_Value()
        {
            // Assert
            Assert.Equal(0, (int)GameMode.Bingo);
        }

        [Fact]
        public void Should_Have_ScavengerHunt_Value()
        {
            // Assert
            Assert.Equal(1, (int)GameMode.ScavengerHunt);
        }
    }
}
