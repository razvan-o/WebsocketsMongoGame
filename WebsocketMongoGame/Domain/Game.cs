using System;
using System.Collections.Generic;

namespace WebsocketMongoGame
{
	public class Game
	{
		public DateTime StartTime { get; set; }
		public List<PlayerObj> Players { get; set; }

		public Game()
		{
			Players = new List<PlayerObj>();
		}
	}
}
