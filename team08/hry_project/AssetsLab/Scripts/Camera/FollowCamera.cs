using UnityEngine;


namespace hry.labs.Camera
{

    public class FollowCamera : MonoBehaviour
    {

        /// <summary>
        /// Target the camera is following.
        /// </summary>
        public Transform targetTransform;
        /// <summary>
        /// Direction in which the camera is following.
        /// </summary>
        public Vector3 followDirection = Vector3.forward;
        /// <summary>
        /// Distance in the direction from the target in which the camera follows.
        /// </summary>
        public float followDistance;


        void LateUpdate()
        {
            // TODO 4 Follow target in some distance
            transform.position = followDirection.normalized * followDistance + targetTransform.position;
        }
    }

}
