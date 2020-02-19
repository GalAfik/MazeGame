using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
	public class CommandInvoker : MonoBehaviour
	{
		private static Queue<ICommand> CommandBuffer; // Holds all yet to be performed commands

		private void Awake()
		{
			CommandBuffer = new Queue<ICommand>();
		}

		public static void AddCommand(ICommand Command)
		{
			CommandBuffer.Enqueue(Command);
		}

		// Update is called once per frame
		void Update()
		{
			if (CommandBuffer.Count > 0)
			{
				CommandBuffer.Dequeue().Execute(); // Get the first command in the queue and execute it
			}
		}
	}
}
