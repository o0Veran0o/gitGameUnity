using System.Collections;
using UnityEngine;

namespace hry.labs.Combat
{

    public class LaserGun : MonoBehaviour {


        public Transform laserStart;
        public Laser laserPrefab;
        public float laserSpeed = 8.0f;
        public float shootSpeed = 0.5f;

        private bool _canShoot = true;


        private IEnumerator CooldownLaserGun()
        {
            _canShoot = false;
            yield return new WaitForSeconds(shootSpeed);
            _canShoot = true;
        }

        public void ShootLaser(Vector3 target, float speed)
        {
            if (_canShoot)
            {
                // We got our target point, so get the laser from the gun to this point
                Vector3 laserDirection = target - laserStart.position;
                laserDirection.Normalize();
                // Create the laser shot
                Laser laser = GameObject.Instantiate<Laser>(laserPrefab, laserStart.position + laserDirection * 0.01f, Quaternion.LookRotation(laserDirection)); // move the start position to preven self collision
                laser.Init(laserStart.position, target, laserSpeed);
                // TODO 5 Cool down the laser gun!
                StartCoroutine("CooldownLaserGun");
            }
        }
    }

}
