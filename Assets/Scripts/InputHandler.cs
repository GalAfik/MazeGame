using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
	public class InputHandler : MonoBehaviour
	{
		[Serializable]
		public class InputConfigurationData
		{
			public GameObject PlayerObject;
		}

		[Serializable]
		private class InputStateData
		{
			public float ButtonDownTime; // The amount of time in seconds a button is held down before being released
		}

		public InputConfigurationData Configuration = new InputConfigurationData();
		private InputStateData State = new InputStateData();

		// Update is called once per frame
		void Update()
		{
			if (Input.GetButtonDown("Jump")) CommandInvoker.AddCommand(new JumpCommand(Configuration.PlayerObject.GetComponentInChildren<Player>()));

			// Increase button down time while holding the break button
			if (Input.GetButton("Break")) State.ButtonDownTime += Time.deltaTime;
			// Reset the button down time and execute the break command
			if (Input.GetButtonUp("Break"))
			{
				State.ButtonDownTime = 0;
				CommandInvoker.AddCommand(new BreakCommand(Configuration.PlayerObject.GetComponentInChildren<Player>(), State.ButtonDownTime));
			}
		}
	}
}
