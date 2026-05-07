using Microsoft.JSInterop;
using SocOps.Data;
using SocOps.Models;
using System.Text.Json;

namespace SocOps.Services;

public class CardDeckService
{
    private const string STORAGE_KEY = "card-deck-state";
    private const int STORAGE_VERSION = 1;

    private readonly IJSRuntime _jsRuntime;
    private readonly Random _random = new();

    public List<BingoSquareData> Deck { get; private set; } = new();
    public BingoSquareData? CurrentCard { get; private set; }
    public int CardsDrawn { get; private set; }
    public int TotalCards => Deck.Count;
    public bool HasMoreCards => CurrentCard != null || Deck.Any();

    public event Action? OnStateChanged;

    public CardDeckService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public void InitializeDeck()
    {
        var allQuestions = Questions.QuestionsList.ToList();
        Deck = allQuestions.Select((text, index) =>
            new BingoSquareData
            {
                Id = index,
                Text = text,
                IsMarked = false,
                IsFreeSpace = false
            }
        ).ToList();

        // Shuffle the deck
        ShuffleDeck();
        CurrentCard = null;
        CardsDrawn = 0;

        NotifyStateChanged();
    }

    private void ShuffleDeck()
    {
        for (int i = Deck.Count - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (Deck[i], Deck[j]) = (Deck[j], Deck[i]);
        }
    }

    public BingoSquareData? DrawCard()
    {
        if (!Deck.Any())
        {
            CurrentCard = null;
            NotifyStateChanged();
            return null;
        }

        CurrentCard = Deck[0];
        Deck.RemoveAt(0);
        CardsDrawn++;

        NotifyStateChanged();
        return CurrentCard;
    }

    public void MarkCurrentCard()
    {
        if (CurrentCard != null)
        {
            CurrentCard.IsMarked = true;
            NotifyStateChanged();
        }
    }

    public void ResetDeck()
    {
        InitializeDeck();
    }

    private void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();
    }
}
