using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
	public class BreakCommand : ICommand
	{
		private Player Player; // The object that breaks the subject
		private float TimeHeldDown; // The amount of time the button was held prior to execution

		public BreakCommand(Player Player, float TimeHeldDown)
		{
			this.Player = Player;
		}

		public void Execute()
		{
			Debug.Assert(Player != null);
			// If the player only pressed the button, pick up the cheese, otherwise, break it!
			if (TimeHeldDown < Player.Configuration.TimeToBreakCheese) Player.PickUp();
			else Player.Break();
		}
	}
}
