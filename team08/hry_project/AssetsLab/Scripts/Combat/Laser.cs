using UnityEngine;
using System.Collections;

namespace hry.labs.Combat
{

    public class Laser : MonoBehaviour
    {

        public float duration = 3.0f;
        private IEnumerator _coroutine;

        public void Init(Vector3 laserStart, Vector3 laserEnd, float speed)
        {
            _coroutine = AnimateLaser(laserStart, laserEnd, speed);
            StartCoroutine(_coroutine);
        }

        IEnumerator AnimateLaser(Vector3 laserStart, Vector3 laserEnd, float speed)
        {
            Vector3 laserDirection = (laserEnd - laserStart).normalized;
            Vector3 currentPosition = laserStart;
            float cumTime = 0.0f;

            // Animate position in fixed update since collision detection is done there
            while (cumTime < duration)
            {
                yield return new WaitForFixedUpdate();
                currentPosition = currentPosition + laserDirection * speed * Time.fixedDeltaTime;
                gameObject.transform.position = currentPosition;
                cumTime += Time.fixedDeltaTime;
            }

            DestroyObject();
        }

        private void OnCollisionEnter(Collision collision)
        {
            DestroyObject();
        }

        private void OnTriggerEnter(Collider other)
        {
            DestroyObject();
        }

        public void DestroyObject()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            Destroy(this.gameObject);
        }
    }

}