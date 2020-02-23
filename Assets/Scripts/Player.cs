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
			public float StartingAnxietyLevel; // How much anxiety allowance/health the player has

			[Header("Temporarily added to public vars")]
			public float SpeedBonus; // The player's added speed bonus
			public float SightBonus; // The distance the player can see - this is applied to the camera distance from the player
			public float AnxietyBonus; // A bonus applied to the player's anxiety/health

			[Header("Anxiety Configuration")]
			public float AnxietyIncreaseRate;
			public float TimeToBreakCheese; // How long, in seconds, it takes this player to break down cheese into fragments
			public float TimeToBreakWall; // How long, in seconds, it takes this player to break down a wall object
		}

		[Serializable]
		private class PlayerStateData
		{
			public CharacterController Controller; // The character controller object - does not implement gravity or physics, only collisions!
			public Vector3 Move = Vector3.zero; // The movement vector for the character
			public Vector3 VerticalMove = Vector3.zero; // The vertical movement affected by gravity
			public Vector3 Look = Vector3.zero; // The forward vector for the character
			public float TotalAnxiety; // The total amount of anxiety/health the player starts with
			public float CurrentAnxiety; // The player's current anxiety/health level
		}

		// Return the player's total anxiety level at the start of the game
		public float GetTotalAnxiety(){ return State.TotalAnxiety; }
		// Return the player's current anxiety level
		public float GetCurrentAnxiety(){ return State.CurrentAnxiety; }

		public PlayerConfigurationData Configuration = new PlayerConfigurationData();
		private PlayerStateData State = new PlayerStateData();

		// Start is called before the first frame update
		void Start()
		{
			// Get the character controller component if not null, otherwise create a new controller
			State.Controller = GetComponent<CharacterController>();

			// Set the player's starting/total anxiety level
			State.TotalAnxiety = Configuration.StartingAnxietyLevel + Configuration.AnxietyBonus;
			State.CurrentAnxiety = State.TotalAnxiety; // Start off with full anxiety
		}

		// Update is called once per frame
		void Update()
		{
			// Apply horizontal movement and gravity
			State.Move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			Debug.Assert(State.Move != null);
			// Apply horizontal move vector
			float ActualSpeed = Configuration.Speed + Configuration.SpeedBonus;
			State.Controller.Move(State.Move.normalized * ActualSpeed * Time.deltaTime);

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

			HandleAnxiety();
		}

		private void HandleAnxiety()
		{
			// Handle Anxiety
			if (State.CurrentAnxiety < 0) State.CurrentAnxiety = 0;
			if (State.CurrentAnxiety >= State.TotalAnxiety)
			{
				// TODO: this player dies!
			}
			else
			{
				// Increase anxiety over time
				State.CurrentAnxiety += Configuration.AnxietyIncreaseRate * Time.deltaTime;
				Debug.Log(State.CurrentAnxiety);
			}
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
				{
					State.VerticalMove.y += Configuration.JumpForce;
					//Debug.Log("Player::Jump");
				}
			}
		}

		public void Break()
		{
			// If the player is grounded, add the jumpforce to the vertical vector
			if (IsGrounded())
			{
				// Only add the vertical velocity if it would not exceed the maximum jump force
				if (State.VerticalMove.y + Configuration.JumpForce <= Configuration.JumpForce)
				{
					State.VerticalMove.y += Configuration.JumpForce;
					//Debug.Log("Player::Jump");
				}
			}
		}

		public void PickUp()
		{
			// If the player is grounded, add the jumpforce to the vertical vector
			if (IsGrounded())
			{
				// Only add the vertical velocity if it would not exceed the maximum jump force
				if (State.VerticalMove.y + Configuration.JumpForce <= Configuration.JumpForce)
				{
					State.VerticalMove.y += Configuration.JumpForce;
					//Debug.Log("Player::Jump");
				}
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			switch (other.tag)
			{
				case "CheeseFragment": // Decrease player anxiety and destroy the cheese fragment
					{
						// Decrease the player's anxiety
						State.CurrentAnxiety -= other.gameObject.GetComponent<CheeseFragment>().Configuration.AnxietyValue;
						// Destroy the cheese fragment
						Destroy(other.gameObject);
						break;
					}
				default:
					break;
			}
		}
	}
}