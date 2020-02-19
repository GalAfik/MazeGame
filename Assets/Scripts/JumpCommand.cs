using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
	public class JumpCommand : ICommand
	{
		private Player Player; // The player that jumps

		public JumpCommand(Player Player)
		{
			this.Player = Player;
		}

		public void Execute()
		{
			Debug.Assert(Player != null);
			Player.Jump();
		}
	}
}
