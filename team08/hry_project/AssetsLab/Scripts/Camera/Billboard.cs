using UnityEngine;


namespace hry.labs.Camera
{
    [ExecuteInEditMode]
    public class Billboard : MonoBehaviour
    {

        private void LateUpdate()
        {
            transform.rotation = Quaternion.LookRotation((UnityEngine.Camera.main.transform.position - transform.position).normalized, Vector3.up);
        }
    }

}
