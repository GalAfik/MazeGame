using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGame
{
	public class InputHandler : MonoBehaviour
	{
		[SerializeField] private GameObject PlayerObject;

		// Update is called once per frame
		void Update()
		{
			if(Input.GetButtonDown("Jump"))
			{
				CommandInvoker.AddCommand(new JumpCommand(PlayerObject.GetComponentInChildren<Player>()));
			}
		}
	}
}
