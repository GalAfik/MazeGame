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
			public float OffsetBonus; // This is a bonus to the camera's follow distance that is applied if the object followed has a player component
		}

		public CameraConfigurationData Configuration = new CameraConfigurationData();
		private CameraStateData State = new CameraStateData();

		private void Start()
		{
			// Look at the followed object
			if (Configuration.ObjectFollowing != null)
				transform.LookAt(Configuration.ObjectFollowing.transform);
		}

		private void Update()
		{
			// Determine if this object is a player object and, if so, determine the camear offset bonus to be applied to this camera
			// If this is determined to be null, the bonus is set to 0.
			// TODO : figure out if this can be decoupled from the player class using a pattern...
			State.OffsetBonus = Configuration.ObjectFollowing.GetComponentInChildren<Player>()?.Configuration.SightBonus ?? 0;
	}

		// Update is called once per frame
		void LateUpdate()
		{
			// Move towards the player + offset
			Vector3 desiredPosition = Configuration.ObjectFollowing.transform.position + Configuration.CameraPositionOffset;
			// Add the sightStat variable distance
			desiredPosition.y += State.OffsetBonus;

			// Move toward the desiredPosition using the Slerp function
			transform.position = Vector3.Slerp(transform.position, desiredPosition, Configuration.SmoothFactor * Time.deltaTime);
		}
	}
}
