using UnityEngine;
using System;
using System.Collections.Generic;

namespace hry.labs.Control
{

    [CreateAssetMenu(fileName = "GameState", menuName = "Game State")]
    [Serializable]
    public class GameState : ScriptableObject
    {

        public string defaultCurrentPlayer = "hry";
        public int defaultCurrentScore = 0;

        public List<string> lastPlayers = new List<string>();
        public List<int> lastScores = new List<int>();

    }

}
