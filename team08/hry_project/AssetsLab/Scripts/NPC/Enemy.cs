using UnityEngine;


namespace hry.labs.NPC
{

    public class Enemy : MonoBehaviour
    {

        public EnemyData enemyData;
        public UI.HealthBar healthBarPrefab;
        public Combat.LaserGun laserGun;

        private Player.Player _player;
        private int _currentHP;
        private UI.HealthBar _healthBar;


        private void Start()
        {
            _player = FindObjectOfType<Player.Player>();
            _currentHP = enemyData.HP;
            _healthBar = GameObject.Instantiate<UI.HealthBar>(healthBarPrefab, transform.position, Quaternion.identity);
        }

        private void Update()
        {
            // Target player and shoot
            transform.LookAt(_player.transform);
            laserGun.ShootLaser(_player.transform.position + Vector3.up, 0.0f);
        }

        private void LateUpdate()
        {
            _healthBar.transform.position = transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            // TODO 6 Collide with laser and remove the some HP
            if (other.gameObject.CompareTag(Utility.Constants.TAG_LASER))
            {
                _currentHP -= 10;
                _healthBar.UpdateHealth(Mathf.Max(0.0f, (float)_currentHP / enemyData.HP));

                if (_currentHP <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnDestroy()
        {
            // TODO 7 Inform anyone that is listening, that the enemy was destroyed.
            if (EventManager.Instance)
            {
                EventManager.Instance.OnEnemyDestroyed.Invoke();
            }
        }

    }

}
