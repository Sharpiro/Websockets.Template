using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Cards.Core
{
    public static class PokerHandsHandler
    {
        public static Player CheckWinner(ConcurrentDictionary<string, Player> players)
        {
            foreach (var playerEntry in players)
            {
                var player = playerEntry.Value;
                player.BestHand = GetBestHand(player.Hand);
            }
            var orderedPlayers = players.OrderByDescending(p => p.Value.BestHand.HandType).ThenByDescending(p => p.Value.BestHand.HighCardNumber);
            return orderedPlayers.FirstOrDefault().Value;
        }

        public static PokerHand GetBestHand(List<Card> hand)
        {
            var pairsResult = GetPairs(hand);
            var straightResult = GetStraights(hand);

            var flushResult = GetFlushes(hand);
            var straightFlushResult = GetStraightFlushes(straightResult, flushResult);

            var max1 = PokerHand.Max(pairsResult, straightResult);
            var max2 = PokerHand.Max(flushResult, straightFlushResult);
            var bestHand = PokerHand.Max(max1, max2);
            return bestHand;
        }

        public static PokerHand GetStraightFlushes(PokerHand straightResult, PokerHand flushResult)
        {
            var bestPokerHand = new PokerHand();
            if (straightResult.HandType == PokerHandType.Straight && flushResult.HandType == PokerHandType.Flush)
            {
                bestPokerHand.HandType = PokerHandType.StraightFlush;
                bestPokerHand.HighCardNumber = Math.Max((int)straightResult.HighCardNumber, (int)flushResult.HighCardNumber);
                if (bestPokerHand.HighCardNumber == 14)
                    bestPokerHand.HandType = PokerHandType.RoyalFlush;
            }
            return bestPokerHand;
        }

        public static PokerHand GetStraights(List<Card> hand)
        {
            var bestPokerHand = new PokerHand();
            var orderedCards = hand.OrderByDescending(c => c.Number).ToList();
            for (int i = 0, cardCount = 0, skips = 0; i < orderedCards.Count - 1; i++)
            {
                if (orderedCards[i].Number == orderedCards[i + 1].Number)
                {
                    skips++;
                }
                else if (orderedCards[i].Number - 1 == orderedCards[i + 1].Number)
                {
                    cardCount++;
                    if (cardCount == 4)
                    {
                        bestPokerHand.HandType = PokerHandType.Straight;
                        bestPokerHand.HighCardNumber = orderedCards[i - cardCount - skips + 1].Number;
                        break;
                    }
                }
                else
                {
                    bestPokerHand.HighCardNumber = orderedCards.LastOrDefault()?.Number;
                    break;
                }
            }
            return bestPokerHand;
        }

        public static PokerHand GetFlushes(List<Card> hand)
        {
            var bestPokerHand = new PokerHand();
            var flushCards = hand.GroupBy(c => c.Suite).Select(g => new
            {
                Suite = g.Key,
                Count = g.Count(),
                HighCard = g.OrderByDescending(c => c.Number).FirstOrDefault()?.Number,
                Cards = g.OrderByDescending(c => c.Number)
            }).FirstOrDefault(gc => gc.Count > 4);
            if (flushCards != null)
            {
                bestPokerHand.HandType = PokerHandType.Flush;
                bestPokerHand.HighCardNumber = flushCards.HighCard;
            }
            else
            {
                bestPokerHand.HandType = PokerHandType.HighCard;
                bestPokerHand.HighCardNumber = hand.OrderByDescending(c => c.Number).FirstOrDefault()?.Number;
            }
            return bestPokerHand;
        }

        public static PokerHand GetPairs(List<Card> hand)
        {
            var bestPokerHand = new PokerHand();
            var groupedCards = hand.GroupBy(c => c.Number).Select(g => new
            {
                Number = g.Key,
                Count = g.Count()
            }).Where(gc => gc.Count > 1).ToList();
            var groupedCardsCount = groupedCards.Count;
            var threeOfAKind = groupedCards.Where(gc => gc.Count >= 3).ToList();
            var fourOfAKind = groupedCards.Where(gc => gc.Count >= 4).ToList();
            if (fourOfAKind.Any())
            {
                bestPokerHand.HandType = PokerHandType.FourOfAKind;
                bestPokerHand.HighCardNumber = threeOfAKind.OrderByDescending(gc => gc.Number).FirstOrDefault()?.Number;
            }
            else if (groupedCardsCount >= 2 && threeOfAKind.Any())
            {
                bestPokerHand.HandType = PokerHandType.FullHouse;
                bestPokerHand.HighCardNumber = threeOfAKind.OrderByDescending(gc => gc.Number).FirstOrDefault()?.Number;
            }
            else if (threeOfAKind.Any())
            {
                bestPokerHand.HandType = PokerHandType.ThreeOfAKind;
                bestPokerHand.HighCardNumber = threeOfAKind.OrderByDescending(gc => gc.Number).FirstOrDefault()?.Number;
            }
            else if (groupedCardsCount >= 2)
            {
                bestPokerHand.HandType = PokerHandType.TwoPair;
                bestPokerHand.HighCardNumber = groupedCards.OrderByDescending(gc => gc.Number).FirstOrDefault()?.Number;
            }
            else if (groupedCardsCount >= 1)
            {
                bestPokerHand.HandType = PokerHandType.Pair;
                bestPokerHand.HighCardNumber = groupedCards.FirstOrDefault()?.Number;
            }
            else
            {
                bestPokerHand.HandType = PokerHandType.HighCard;
                bestPokerHand.HighCardNumber = hand.OrderByDescending(c => c.Number).FirstOrDefault()?.Number;
            }
            return bestPokerHand;
        }
    }

    public class PokerHand
    {
        public PokerHandType HandType { get; set; } = PokerHandType.HighCard;
        public int? HighCardNumber { get; set; } = 2;

        public static PokerHand Max(PokerHand first, PokerHand second)
        {
            if (first.HandType == second.HandType)
            {
                return first.HighCardNumber > second.HighCardNumber ? first : second;
            }
            return first.HandType > second.HandType ? first : second;
        }
    }

    public enum PokerHandType
    {
        HighCard, Pair, TwoPair, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraightFlush, RoyalFlush
    }
}