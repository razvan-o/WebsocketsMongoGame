using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsocketMongoGame.Services;

namespace WebsocketMongoGame
{
	public class GameHub : Hub
	{
		public Dictionary<string, Game> games;
		private readonly PlayersService playersService;

		public GameHub(PlayersService playersService)
		{
			games = new Dictionary<string, Game>();
			this.playersService = playersService;
		}

		public async Task StartGame(string playerId, string friends, string gameId)
		{
			if (!CheckIfGameIdIsValid(gameId, friends, playerId))
			{
				return;
			}

			games[gameId] = new Game();
			games[gameId].Players.Add(new PlayerObj { PlayerId = playerId, Joined = true });
			friends.Split(",").ToList()
				.ForEach(f => games[gameId].Players.Add(new PlayerObj { PlayerId = f }));

			// TODO: persist in DB

			await Groups.AddToGroupAsync(Context.ConnectionId, gameId); 
			await Clients.Group(gameId).SendAsync("PlayerJoined", playerId);
		}

		public async Task JoinGame(string playerId, string gameId)
		{
			if (!CheckIfGameIdIsValid(gameId, playerId))
			{
				return;
			}

			// TODO: persist in DB
			games[gameId].Players.SingleOrDefault(v => v.PlayerId == playerId).Joined = true;

			await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
			await Clients.Group(gameId).SendAsync("PlayerJoined", playerId);
		}

		public async Task EndGame(string playerId, string gameId)
		{
			if (!CheckIfGameIdIsValid(gameId, playerId))
			{
				return;
			}

			// TODO: instead of allowing the owner to manually end the game, schedule jobs accordingly
			if (games[gameId].Players.First().PlayerId != playerId)
			{
				return;
			}

			var winners = await DetermineWinners(gameId);
			UpdateSkillPoints(gameId, winners);

			games.Remove(gameId);
		}

		private async Task<List<string>> DetermineWinners(string gameId)
		{
			var winnerNumber = new Random().Next(100);

			var winners = new List<string>();
			var minDiff = Int32.MaxValue;
			foreach (var player in games[gameId].Players)
			{
				var delta = Math.Abs(player.ChosenNumber - winnerNumber);
				if (delta < minDiff)
				{
					winners.Clear();
					winners.Add(player.PlayerId);

					minDiff = delta;
				}
				else if (delta == minDiff)
				{
					winners.Add(player.PlayerId);
				}
			}

			await Clients.Group(gameId).SendAsync("WinnersDetermined", gameId, String.Join(",", winners));

			return winners;
		}

		private void UpdateSkillPoints(string gameId, List<string> winners)
		{
			// TODO: increment skill points for winner(s), decrease skill of losers
		}

		public async Task Vote(string playerId, string gameId, string predictedNumber)
		{
			// TODO: validate input

			if (!CheckIfGameIdIsValid(gameId, playerId))
			{
				return;
			}

			// TODO: persist in DB
			games[gameId].Players.SingleOrDefault(v => v.PlayerId == playerId).ChosenNumber = Int32.Parse(predictedNumber);

			await Clients.Group(gameId).SendAsync("ReceivedVote", playerId, predictedNumber);
		}

		private bool CheckIfGameIdIsValid(string gameId, string playerId)
		{
			if (!games.ContainsKey(gameId))
			{
				// TODO: inform user
				return false;
			}

			// check if player is invited
			if (games[gameId].Players.SingleOrDefault(v => v.PlayerId == playerId) == null)
			{
				// TODO: inform user
				return false;
			}

			return true;
		}

		private bool CheckIfGameIdIsValid(string gameId, string friends, string playerId)
		{
			// validate "friends" input - should be valid coma separated values;

			if (games.ContainsKey(gameId))
			{
				// TODO: inform user
				return false;
			}

			var players = playersService.Get(friends.Split(","));
			players.Add(playersService.Get(playerId));

			var skillPoints = players.Select(p => p.SkillPoints);
			if (skillPoints.Max() - skillPoints.Min() > 100)
			{
				// TODO: inform user
				return false;
			}

			return true;
		}
	}
}
