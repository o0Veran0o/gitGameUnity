using UnityEngine;

namespace hry.labs.NPC
{

    [CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy Data")]
    public class EnemyData : ScriptableObject
    {

        public int HP = 50;
        public float speed = 4.0f;
        public float attackSpeed = 2.0f;

    }

}