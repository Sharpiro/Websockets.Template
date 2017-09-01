using System.Collections.Generic;

namespace Cards.Core
{
    public static class CardDefinitions
    {
        public static List<Card> GetDeck()
        {
            var deck = new List<Card>(52)
            {
                new Card {DeckPosition = 0 , Number = 2,  Value = "2",     Suite = "Hearts"},
                new Card {DeckPosition = 1 , Number = 3,  Value = "3",     Suite = "Hearts"},
                new Card {DeckPosition = 2 , Number = 4,  Value = "4",     Suite = "Hearts"},
                new Card {DeckPosition = 3 , Number = 5,  Value = "5",     Suite = "Hearts"},
                new Card {DeckPosition = 4 , Number = 6,  Value = "6",     Suite = "Hearts"},
                new Card {DeckPosition = 5 , Number = 7,  Value = "7",     Suite = "Hearts"},
                new Card {DeckPosition = 6 , Number = 8,  Value = "8",     Suite = "Hearts"},
                new Card {DeckPosition = 7 , Number = 9,  Value = "9",     Suite = "Hearts"},
                new Card {DeckPosition = 8 , Number = 10, Value = "10",    Suite = "Hearts"},
                new Card {DeckPosition = 9 , Number = 11, Value = "Jack",  Suite = "Hearts"},
                new Card {DeckPosition = 10, Number = 12, Value = "Queen", Suite = "Hearts"},
                new Card {DeckPosition = 11, Number = 13, Value = "King",  Suite = "Hearts"},
                new Card {DeckPosition = 12, Number = 14, Value = "Ace",   Suite = "Hearts"},

                new Card {DeckPosition = 13, Number = 2,  Value = "2",     Suite = "Diamonds"},
                new Card {DeckPosition = 14, Number = 3,  Value = "3",     Suite = "Diamonds"},
                new Card {DeckPosition = 15, Number = 4,  Value = "4",     Suite = "Diamonds"},
                new Card {DeckPosition = 16, Number = 5,  Value = "5",     Suite = "Diamonds"},
                new Card {DeckPosition = 17, Number = 6,  Value = "6",     Suite = "Diamonds"},
                new Card {DeckPosition = 18, Number = 7,  Value = "7",     Suite = "Diamonds"},
                new Card {DeckPosition = 19, Number = 8,  Value = "8",     Suite = "Diamonds"},
                new Card {DeckPosition = 20, Number = 9,  Value = "9",     Suite = "Diamonds"},
                new Card {DeckPosition = 21, Number = 10, Value = "10",    Suite = "Diamonds"},
                new Card {DeckPosition = 22, Number = 11, Value = "Jack",  Suite = "Diamonds"},
                new Card {DeckPosition = 23, Number = 12, Value = "Queen", Suite = "Diamonds"},
                new Card {DeckPosition = 24, Number = 13, Value = "King",  Suite = "Diamonds"},
                new Card {DeckPosition = 25, Number = 14, Value = "Ace",   Suite = "Diamonds"},

                new Card {DeckPosition = 26, Number = 2,  Value = "2",     Suite = "Clubs"},
                new Card {DeckPosition = 27, Number = 3,  Value = "3",     Suite = "Clubs"},
                new Card {DeckPosition = 28, Number = 4,  Value = "4",     Suite = "Clubs"},
                new Card {DeckPosition = 29, Number = 5,  Value = "5",     Suite = "Clubs"},
                new Card {DeckPosition = 30, Number = 6,  Value = "6",     Suite = "Clubs"},
                new Card {DeckPosition = 31, Number = 7,  Value = "7",     Suite = "Clubs"},
                new Card {DeckPosition = 32, Number = 8,  Value = "8",     Suite = "Clubs"},
                new Card {DeckPosition = 33, Number = 9,  Value = "9",     Suite = "Clubs"},
                new Card {DeckPosition = 34, Number = 10, Value = "10",    Suite = "Clubs"},
                new Card {DeckPosition = 35, Number = 11, Value = "Jack",  Suite = "Clubs"},
                new Card {DeckPosition = 36, Number = 12, Value = "Queen", Suite = "Clubs"},
                new Card {DeckPosition = 37, Number = 13, Value = "King",  Suite = "Clubs"},
                new Card {DeckPosition = 38, Number = 14, Value = "Ace",   Suite = "Clubs"},

                new Card {DeckPosition = 39, Number = 2,  Value = "2",     Suite = "Spades"},
                new Card {DeckPosition = 40, Number = 3,  Value = "3",     Suite = "Spades"},
                new Card {DeckPosition = 41, Number = 4,  Value = "4",     Suite = "Spades"},
                new Card {DeckPosition = 42, Number = 5,  Value = "5",     Suite = "Spades"},
                new Card {DeckPosition = 43, Number = 6,  Value = "6",     Suite = "Spades"},
                new Card {DeckPosition = 44, Number = 7,  Value = "7",     Suite = "Spades"},
                new Card {DeckPosition = 45, Number = 8,  Value = "8",     Suite = "Spades"},
                new Card {DeckPosition = 46, Number = 9,  Value = "9",     Suite = "Spades"},
                new Card {DeckPosition = 47, Number = 10, Value = "10",    Suite = "Spades"},
                new Card {DeckPosition = 48, Number = 11, Value = "Jack",  Suite = "Spades"},
                new Card {DeckPosition = 49, Number = 12, Value = "Queen", Suite = "Spades"},
                new Card {DeckPosition = 50, Number = 13, Value = "King",  Suite = "Spades"},
                new Card {DeckPosition = 51, Number = 14, Value = "Ace",   Suite = "Spades"},
            };
            return deck;
        }
    }
}