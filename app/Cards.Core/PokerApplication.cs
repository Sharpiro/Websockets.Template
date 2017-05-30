using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Websockets.Core.Models;
using Websockets.Core.OwinSocketServer;

namespace Cards.Core
{
    public sealed class PokerApplication : CardApplication
    {
        private GameState GameState { get; set; }
        private CancellationTokenSource _cancellationTokenSource;

        public PokerApplication(ISocketHandler socketHandler) : base(socketHandler)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            CardsPerRound = new Dictionary<int, int>
            {
                {1, 3},
                {2, 1},
                {3, 1}
            };
            SocketHandler = socketHandler;
            MaxUsers = 2;
        }

        public override void HandleMessage(DataTransferModel messageObject)
        {
            UpdatingRequest = true;
            switch (GameState)
            {
                case GameState.WaitingForBets:
                    HandleWaitingForBets(messageObject);
                    break;
            }
            UpdatingRequest = false;
        }

        private void HandleWaitingForBets(DataTransferModel messageObject)
        {
            switch (messageObject.DataTitle)
            {
                case "bet":
                    if (!HasBet(messageObject.SocketId))
                        PlaceBet(messageObject.SocketId, messageObject.Data);
                    if (AllUsersHaveBet())
                    {
                        ClearBets();
                        GameState = GameState.DealCards;
                    }
                    break;
            }
        }

        public override void Start()
        {
            DealPlayerCards(CardsToDeal);
            IsStarted = true;
            AutoUpdate(_cancellationTokenSource.Token);
        }

        public override void Stop()
        {
            _cancellationTokenSource.Cancel();
            var remainingPlayerId = Users.FirstOrDefault().Value.SocketId;
            SocketHandler.SendMessageById(remainingPlayerId, "reset", "reset");
            IsStarted = false;
            ResetDeck();
            Round = 1;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private async void AutoUpdate(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (UpdatingRequest) return;
                switch (GameState)
                {
                    case GameState.DealCards:
                        DealCommunityCards(CardsPerRound[Round]);
                        Round++;
                        GameState = Round < 4 ? GameState.WaitingForBets : GameState.CheckWinner;
                        break;
                    case GameState.CheckWinner:
                        CheckWinner();
                        GameState = GameState.GameOver;
                        break;
                }
                // ReSharper disable once MethodSupportsCancellation
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

        private void CheckWinner()
        {
            //var winner = PokerHandsHandler.CheckWinner(Players.Select(p => p.Value as Player));
            //SocketHandler.SendMessageById(winner.SocketId, "update", "you win");
        }

        private void DealPlayerCards(int numberOfCards)
        {
            for (var i = 0; i < numberOfCards; i++)
            {
                foreach (var playerEntry in Users)
                {
                    var player = playerEntry.Value;
                    var response = AddCardToPlayer(player.SocketId);
                    SocketHandler.SendMessageById(player.SocketId, "update", response);
                }
            }
        }

        private void DealCommunityCards(int numberOfCards)
        {
            for (var i = 0; i < numberOfCards; i++)
            {
                var response = AddCardToCommunity();
                foreach (var playerEntry in Users)
                {
                    var player = playerEntry.Value;
                    SocketHandler.SendMessageById(player.SocketId, "update", response);
                }
            }
        }
    }

    public enum GameState
    {
        WaitingForBets, DealCards, CheckWinner, GameOver
    }
}