using UnityEngine;
using System;

namespace MazeGame
{

	[RequireComponent(typeof(CharacterController))]
	[DisallowMultipleComponent]
	[SelectionBase]

	public class Player : MonoBehaviour
	{
		[Serializable]
		public class PlayerConfigurationData
		{
			[Header("Movement Variables")]
			public float Speed; // The player's speed
			public float Gravity; // The player's downward acceleration
			public float JumpForce; // How much force to apply to a player character when they jump

			[Header("Temporarily added to public vars")]
			public float SpeedBonus; // The player's added speed bonus
			public float SightBonus; // The distance the player can see - this is applied to the camera distance from the player
			public float AnxietyBonus; // A bonus applied to the player's anxiety/health
		}

		[Serializable]
		private class PlayerStateData
		{
			public CharacterController Controller; // The character controller object - does not implement gravity or physics, only collisions!
			public Vector3 Move = Vector3.zero; // The movement vector for the character
			public Vector3 VerticalMove = Vector3.zero; // The vertical movement affected by gravity
			public Vector3 Look = Vector3.zero; // The forward vector for the character
		}

		public PlayerConfigurationData Configuration = new PlayerConfigurationData();
		private PlayerStateData State = new PlayerStateData();

		// Start is called before the first frame update
		void Start()
		{
			// Get the character controller component if not null, otherwise create a new controller
			State.Controller = GetComponent<CharacterController>();
		}

		// Update is called once per frame
		void Update()
		{
			// Apply horizontal movement and gravity
			State.Move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			Debug.Assert(State.Move != null);
			// Apply horizontal move vector
			State.Controller.Move(State.Move.normalized * Configuration.Speed * Time.deltaTime);

			// Apply gravity move vector
			State.VerticalMove.y -= Configuration.Gravity * Time.deltaTime;
			State.Controller.Move(State.VerticalMove * Time.deltaTime);

			// Reset the vertical velocity vector when grounded
			if (State.Controller.isGrounded)
			{
				State.VerticalMove = Vector3.zero;
			}

			// look toward the forward direction
			if (State.Move != Vector3.zero) transform.rotation = Quaternion.LookRotation(State.Move);
			Debug.DrawLine(transform.position, transform.position + transform.forward, Color.blue);
		}

		private bool IsGrounded()
		{
			// Check if the player is standing on another object
			return Physics.Raycast(transform.position, Vector3.down, 1f);
		}

		public void Jump()
		{
			// If the player is grounded, add the jumpforce to the vertical vector
			if (IsGrounded())
			{
				// Only add the vertical velocity if it would not exceed the maximum jump force
				if (State.VerticalMove.y + Configuration.JumpForce <= Configuration.JumpForce)
					State.VerticalMove.y += Configuration.JumpForce;
					//Debug.Log("Player::Jump");
			}
		}
	}
}