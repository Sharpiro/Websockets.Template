using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Websockets.Template.CoreX.Models;
using Websockets.Template.CoreX.OwinSocketServer;

namespace Websockets.Template.CoreX.CardApp
{
    public sealed class PokerApplication : CardApplication, IWebSocketApplication
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
            MaxPlayers = 2;
        }

        public void HandleMessage(DataTransferModel messageObject)
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
                    PlaceBet(messageObject.SocketId, messageObject.Data);
                    if (AllPlayersHaveBet())
                    {
                        ClearBets();
                        GameState = GameState.DealCards;
                    }
                    break;
            }
        }

        public void Start()
        {
            DealPlayerCards(CardsToDeal);
            IsStarted = true;
            AutoUpdate(_cancellationTokenSource.Token);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            var remainingPlayerId = Players.FirstOrDefault().Value.SocketId;
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
                        GameState = Round < 4 ? GameState.WaitingForBets : GameState.GameOver;
                        break;
                }
                // ReSharper disable once MethodSupportsCancellation
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

        private void DealPlayerCards(int numberOfCards)
        {
            for (var i = 0; i < numberOfCards; i++)
            {
                foreach (var playerEntry in Players)
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
                foreach (var playerEntry in Players)
                {
                    var player = playerEntry.Value;
                    SocketHandler.SendMessageById(player.SocketId, "update", response);
                }
            }
        }
    }

    public enum GameState
    {
        WaitingForBets, DealCards, GameOver
    }
}
