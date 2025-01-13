using UnityEngine;

namespace hry.labs.Player
{

    [CreateAssetMenu(fileName = "PlayerData", menuName = "Player Data")]
    public class PlayerData : ScriptableObject
    {

        public int HP;
        public float movementSpeed = 7.0f;
        public float jumpSpeed = 5.0f;
        
    }

}