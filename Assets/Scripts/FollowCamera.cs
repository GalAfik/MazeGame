using System;
using UnityEngine;

namespace MazeGame
{
	public class FollowCamera : MonoBehaviour
	{
		[Serializable]
		public class CameraConfigurationData
		{
			[Tooltip("Make sure that the object following is the actual object, not a container!")]
			[SerializeField] public GameObject ObjectFollowing; // The game object this camera should focus on
			[SerializeField] public Vector3 CameraPositionOffset; // Relative position to the player
			[Range(1, 10)] [SerializeField] public float SmoothFactor; // A damping effect for the camera
		}

		[Serializable]
		private class CameraStateData
		{
			public float OffsetBonus = 0; // This is a bonus to the camera's follow distance that is applied if the object followed has a player component
		}

		public CameraConfigurationData Configuration = new CameraConfigurationData();
		private CameraStateData State = new CameraStateData();

		private void Start()
		{
			// Set the camera's position relative to the object following at initial start
			transform.position = CalculateNewPosition();

			// Look at the followed object
			if (Configuration.ObjectFollowing != null)
				transform.LookAt(Configuration.ObjectFollowing.transform);
		}

		// Update is called once per frame
		void Update()
		{
			// Move toward the desiredPosition using the Slerp function
			transform.position = Vector3.Slerp(transform.position, CalculateNewPosition(), Configuration.SmoothFactor * Time.deltaTime);
		}

		// Returns the desired position of the camera, accounting for the initial set offset vector + any bonus offset provided by the followed object
		private Vector3 CalculateNewPosition()
		{
			// Determine if this object is a player object and, if so, determine the camear offset bonus to be applied to this camera
			// If this is determined to be null, the bonus is set to 0.
			// TODO : figure out if this can be decoupled from the player class using a pattern...
			State.OffsetBonus = Configuration.ObjectFollowing.GetComponentInChildren<Player>()?.Configuration.SightBonus ?? 0;

			Vector3 desiredPosition = Configuration.ObjectFollowing.transform.position + Configuration.CameraPositionOffset;
			Vector3 bonusOffsetPosition = Configuration.CameraPositionOffset.normalized * State.OffsetBonus;
			return desiredPosition + bonusOffsetPosition;
		}
	}
}
