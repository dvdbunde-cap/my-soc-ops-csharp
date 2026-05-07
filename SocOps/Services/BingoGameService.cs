using SocOps.Models;
using System.Text.Json;
using Microsoft.JSInterop;

namespace SocOps.Services;

public class BingoGameService
{
    private const string STORAGE_KEY = "bingo-game-state";
    private const int STORAGE_VERSION = 2;
    private const double SCAVENGER_HUNT_WIN_THRESHOLD = 0.80;

    private readonly IJSRuntime _jsRuntime;

    public GameState CurrentGameState { get; private set; } = GameState.Start;
    public GameMode CurrentGameMode { get; private set; } = GameMode.Bingo;
    public List<BingoSquareData> Board { get; private set; } = new();
    public List<BingoSquareData> ScavengerHuntList { get; private set; } = new();
    public BingoLine? WinningLine { get; private set; }
    public HashSet<int> WinningSquareIds => BingoLogicService.GetWinningSquareIds(WinningLine);
    public bool ShowBingoModal { get; private set; }

    public double ScavengerHuntProgress
    {
        get
        {
            if (ScavengerHuntList.Count == 0) return 0.0;
            var markedCount = ScavengerHuntList.Count(s => s.IsMarked);
            return (double)markedCount / ScavengerHuntList.Count;
        }
    }

    private static List<BingoSquareData> ToggleItemInList(List<BingoSquareData> items, int itemId)
    {
        return items.Select(item =>
            item.Id == itemId
                ? BingoLogicService.CreateSquare(item.Id, item.Text, !item.IsMarked, item.IsFreeSpace)
                : item
        ).ToList();
    }

    public bool IsScavengerHuntComplete => ScavengerHuntProgress >= SCAVENGER_HUNT_WIN_THRESHOLD;

    public event Action? OnStateChanged;

    public BingoGameService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        await LoadGameStateAsync().ConfigureAwait(false);
    }

    public void StartGame(GameMode mode = GameMode.Bingo)
    {
        CurrentGameMode = mode;

        if (mode == GameMode.Bingo)
        {
            Board = BingoLogicService.GenerateBoard();
            ScavengerHuntList = new();
        }
        else
        {
            ScavengerHuntList = BingoLogicService.GenerateFlatList();
            Board = new();
        }

        WinningLine = null;
        CurrentGameState = GameState.Playing;
        ShowBingoModal = false;
        SaveStateAndNotify();
    }

    public void HandleSquareClick(int squareId)
    {
        if (CurrentGameMode == GameMode.Bingo)
        {
            Board = BingoLogicService.ToggleSquare(Board, squareId);

            // Check for bingo after toggling
            if (WinningLine == null)
            {
                var bingo = BingoLogicService.CheckBingo(Board);
                if (bingo != null)
                {
                    WinningLine = bingo;
                    CurrentGameState = GameState.Bingo;
                    ShowBingoModal = true;
                }
            }
        }

        SaveStateAndNotify();
    }

    public void ToggleScavengerHuntItem(int itemId)
    {
        if (CurrentGameMode != GameMode.ScavengerHunt) return;

        ScavengerHuntList = ToggleItemInList(ScavengerHuntList, itemId);

        SaveStateAndNotify();
    }

    public void ResetGame()
    {
        CurrentGameState = GameState.Start;
        Board = new();
        WinningLine = null;
        ShowBingoModal = false;
        SaveStateAndNotify();
    }

    public void DismissModal()
    {
        ShowBingoModal = false;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnStateChanged?.Invoke();

    private void SaveStateAndNotify()
    {
        _ = SaveGameStateAsync(); // Fire and forget
        NotifyStateChanged();
    }

    private async Task LoadGameStateAsync()
    {
        try
        {
            var saved = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", STORAGE_KEY).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(saved))
            {
                var data = JsonSerializer.Deserialize<StoredGameData>(saved);
                if (data != null && data.Version == STORAGE_VERSION)
                {
                    CurrentGameState = data.GameState;
                    CurrentGameMode = data.GameMode;
                    Board = data.Board ?? [];
                    ScavengerHuntList = data.ScavengerHuntList ?? [];
                    WinningLine = data.WinningLine;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load game state: {ex.Message}");
        }
    }

    private async Task SaveGameStateAsync()
    {
        try
        {
            var data = new StoredGameData
            {
                Version = STORAGE_VERSION,
                GameState = CurrentGameState,
                GameMode = CurrentGameMode,
                Board = Board,
                ScavengerHuntList = ScavengerHuntList,
                WinningLine = WinningLine
            };
            var json = JsonSerializer.Serialize(data);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", STORAGE_KEY, json).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save game state: {ex.Message}");
        }
    }

    private class StoredGameData
    {
        public int Version { get; set; }
        public GameState GameState { get; set; }
        public GameMode GameMode { get; set; } = GameMode.Bingo;
        public List<BingoSquareData> Board { get; set; } = new();
        public List<BingoSquareData> ScavengerHuntList { get; set; } = new();
        public BingoLine? WinningLine { get; set; }
    }
}
