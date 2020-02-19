using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
	public class BreakCommand : ICommand
	{
		private GameObject Actor; // The object that breaks the subject
		private GameObject Subject; // The object to be broken apart

		public BreakCommand(GameObject Actor, GameObject Subject)
		{
			this.Actor = Actor;
			this.Subject = Subject;
		}

		public void Execute()
		{
			// TODO
			Debug.Log("BreakCommand::Executed");
		}
	}
}
