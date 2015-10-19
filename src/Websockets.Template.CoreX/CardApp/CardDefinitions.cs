using System.Collections.Generic;

namespace Websockets.Template.CoreX.CardApp
{
    public static class CardDefinitions
    {
        public static List<Card> GetDeck()
        {
            var deck = new List<Card>(52)
            {
                new Card {DeckPosition = 0, Value = "2", Suite = "Hearts"},
                new Card {DeckPosition = 1, Value = "3", Suite = "Hearts"},
                new Card {DeckPosition = 2, Value = "4", Suite = "Hearts"},
                new Card {DeckPosition = 3, Value = "5", Suite = "Hearts"},
                new Card {DeckPosition = 4, Value = "6", Suite = "Hearts"},
                new Card {DeckPosition = 5, Value = "7", Suite = "Hearts"},
                new Card {DeckPosition = 6, Value = "8", Suite = "Hearts"},
                new Card {DeckPosition = 7, Value = "9", Suite = "Hearts"},
                new Card {DeckPosition = 8, Value = "10", Suite = "Hearts"},
                new Card {DeckPosition = 9, Value = "Jack", Suite = "Hearts"},
                new Card {DeckPosition = 10, Value = "Queen", Suite = "Hearts"},
                new Card {DeckPosition = 11, Value = "King", Suite = "Hearts"},
                new Card {DeckPosition = 12, Value = "Ace", Suite = "Hearts"},

                new Card {DeckPosition = 13, Value = "2", Suite = "Diamonds"},
                new Card {DeckPosition = 14, Value = "3", Suite = "Diamonds"},
                new Card {DeckPosition = 15, Value = "4", Suite = "Diamonds"},
                new Card {DeckPosition = 16, Value = "5", Suite = "Diamonds"},
                new Card {DeckPosition = 17, Value = "6", Suite = "Diamonds"},
                new Card {DeckPosition = 18, Value = "7", Suite = "Diamonds"},
                new Card {DeckPosition = 19, Value = "8", Suite = "Diamonds"},
                new Card {DeckPosition = 20, Value = "9", Suite = "Diamonds"},
                new Card {DeckPosition = 21, Value = "10", Suite = "Diamonds"},
                new Card {DeckPosition = 22, Value = "Jack", Suite = "Diamonds"},
                new Card {DeckPosition = 23, Value = "Queen", Suite = "Diamonds"},
                new Card {DeckPosition = 24, Value = "King", Suite = "Diamonds"},
                new Card {DeckPosition = 25, Value = "Ace", Suite = "Diamonds"},

                new Card {DeckPosition = 26, Value = "2", Suite = "Clubs"},
                new Card {DeckPosition = 27, Value = "3", Suite = "Clubs"},
                new Card {DeckPosition = 28, Value = "4", Suite = "Clubs"},
                new Card {DeckPosition = 29, Value = "5", Suite = "Clubs"},
                new Card {DeckPosition = 30, Value = "6", Suite = "Clubs"},
                new Card {DeckPosition = 31, Value = "7", Suite = "Clubs"},
                new Card {DeckPosition = 32, Value = "8", Suite = "Clubs"},
                new Card {DeckPosition = 33, Value = "9", Suite = "Clubs"},
                new Card {DeckPosition = 34, Value = "10", Suite = "Clubs"},
                new Card {DeckPosition = 35, Value = "Jack", Suite = "Clubs"},
                new Card {DeckPosition = 36, Value = "Queen", Suite = "Clubs"},
                new Card {DeckPosition = 37, Value = "King", Suite = "Clubs"},
                new Card {DeckPosition = 38, Value = "Ace", Suite = "Clubs"},

                new Card {DeckPosition = 39, Value = "2", Suite = "Spades"},
                new Card {DeckPosition = 40, Value = "3", Suite = "Spades"},
                new Card {DeckPosition = 41, Value = "4", Suite = "Spades"},
                new Card {DeckPosition = 42, Value = "5", Suite = "Spades"},
                new Card {DeckPosition = 43, Value = "6", Suite = "Spades"},
                new Card {DeckPosition = 44, Value = "7", Suite = "Spades"},
                new Card {DeckPosition = 45, Value = "8", Suite = "Spades"},
                new Card {DeckPosition = 46, Value = "9", Suite = "Spades"},
                new Card {DeckPosition = 47, Value = "10", Suite = "Spades"},
                new Card {DeckPosition = 48, Value = "Jack", Suite = "Spades"},
                new Card {DeckPosition = 49, Value = "Queen", Suite = "Spades"},
                new Card {DeckPosition = 50, Value = "King", Suite = "Spades"},
                new Card {DeckPosition = 51, Value = "Ace", Suite = "Spades"},
            };
            return deck;
        }
    }
}
