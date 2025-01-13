using System.Collections;
using UnityEngine;

namespace hry.labs.Player
{

    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {

        public PlayerData playerData;
        public Combat.LaserGun laserGun;
        public UI.HealthBar healthBarPrefab;

        private CharacterController _playerCharacterController;
        private UI.HealthBar _healthBar;
        private Vector3 _moveVelocity = Vector3.zero;
        private int _currentHP = 100;


        void Start()
        {
            _playerCharacterController = GetComponent<CharacterController>();
            _healthBar = GameObject.Instantiate<UI.HealthBar>(healthBarPrefab, transform.position, Quaternion.identity);
            _currentHP = playerData.HP;

            // TODO 8 No spawning, so set hard time limit
            StartCoroutine("EndGame");
        }

        IEnumerator EndGame()
        {
            yield return new WaitForSeconds(30.0f);
            Control.GameManager.Instance.GameEnded();
        }

        void Update()
        {
            UpdateVelocity();
            _playerCharacterController.Move(_moveVelocity * Time.deltaTime);

            UpdateRotation();

            // Use left mouse button to shoot lasers from blaster
            if (Input.GetMouseButtonDown(Utility.Constants.MOUSE_BUTTON_LEFT))
            {
                // TODO 3 Shoot to where the user clicked, ignore player and lasers
                Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                // Hit anything but player
                if (Physics.Raycast(ray, out hit, 100.0f, ~LayerMask.GetMask(Utility.Constants.LAYER_PLAYER, Utility.Constants.LAYER_EFFECTS)))
                {
                    laserGun.ShootLaser(hit.point, Mathf.Abs(Mathf.Sqrt(Vector3.Dot(_moveVelocity, new Vector3(1f,0f,1f)))));
                }
            }
        }

        private void LateUpdate()
        {
            _healthBar.transform.position = transform.position;
        }

        private void UpdateRotation()
        {
            //if (Input.GetMouseButton(Utility.Constants.MOUSE_BUTTON_RIGHT))
            {
                Vector3 dir = Input.mousePosition - UnityEngine.Camera.main.WorldToScreenPoint(transform.position);
                transform.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0.0f, dir.y));
            }
        }

        private void UpdateVelocity()
        {
            // TODO 2 Compute velocity
            // Check if on character is on the ground and if so, add velocities
            if (_playerCharacterController.isGrounded)
            {
                // Get vertical and horizontal axis speed and jump to the _moveVelocity
                float forwardSpeed = Input.GetAxis("Vertical");
                float sidewaySpeed = Input.GetAxis("Horizontal");
                bool jump = Input.GetButtonDown("Jump");

                _moveVelocity = (Vector3.right * sidewaySpeed + Vector3.forward * forwardSpeed).normalized * playerData.movementSpeed;

                // If player is grounded and jumps, add y-axis speed
                if (jump)
                {
                    _moveVelocity.y = playerData.jumpSpeed;
                }
            }

            // Apply gravity acceleration
            _moveVelocity.y = _moveVelocity.y - 9.8f * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            // TODO 8 Die on destroy!
            if (other.gameObject.CompareTag(Utility.Constants.TAG_LASER))
            {
                _currentHP -= 10;
                _healthBar.UpdateHealth(Mathf.Max(0.0f, (float)_currentHP / playerData.HP));

                if (_currentHP <= 0)
                {
                    Control.GameManager.Instance.GameEnded();
                }
            }
        }
    }

}
