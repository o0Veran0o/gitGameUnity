using UnityEngine;
using UnityEngine.UI;

namespace hry.labs.UI.Menu
{

    public class Menu : MonoBehaviour
    {

        public Text playerNameText;
        public Text[] names;
        public Text[] scores;

        /// <summary>
        /// Checks if user did enter his name and if so, loads the game level
        /// with the user name and with score set to 0.
        /// </summary>
        public void StartGame()
        {
            if (!string.IsNullOrEmpty(playerNameText.text))
            {
                Control.GameManager.Instance.playerName = playerNameText.text;

                // TODO 1 Make it work. Scenes might be missing in the build settings?
                Control.GameManager.Instance.LoadGameLevel();
            }
        }

        private void Start()
        {
            // Clean all texts and set them to saved state or default state
            for (int i = 0; i < names.Length; ++i)
            {
                names[i].text = "";
                scores[i].text = "";
            }

            for (int i = 0; i < Control.GameManager.Instance.gameState.lastPlayers.Count; ++i)
            {
                names[i].text = Control.GameManager.Instance.gameState.lastPlayers[i].ToString();
                scores[i].text = Control.GameManager.Instance.gameState.lastScores[i].ToString();
            }
        }

    }

}

