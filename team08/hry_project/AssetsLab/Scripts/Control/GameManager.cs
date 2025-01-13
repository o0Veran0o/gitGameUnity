using UnityEngine;
using System.IO;

namespace hry.labs.Control
{

    /// <summary>
    /// GameManager implements singleton pattern. This implementation is not thread safe.
    /// </summary>
    public class GameManager : MonoBehaviour
    {

        public GameState gameState;
        public string playerName = "";
        public int currentScore = 0;

        
        public void LoadGameState()
        {
            // Check if there is existing save, if not use default state
            if (File.Exists(Utility.Constants.PATH_TO_SAVE))
            {
                var saveJsonString = File.ReadAllText(Utility.Constants.PATH_TO_SAVE);
                gameState = ScriptableObject.CreateInstance<GameState>();
                JsonUtility.FromJsonOverwrite(saveJsonString, gameState);
            }
        }

        public void SaveGameState()
        {
            var gameStateString = JsonUtility.ToJson(gameState);
            File.WriteAllText(Utility.Constants.PATH_TO_SAVE, gameStateString);
        }

        public void LoadGameLevel()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("2_Game");
        }

        public void LoadMenuLevel()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("1_MainScreen");
        }

        public void GameEnded()
        {
            gameState.lastPlayers.Insert(0, playerName);
            gameState.lastScores.Insert(0, currentScore);

            if (gameState.lastPlayers.Count > 3)
            {
                gameState.lastPlayers.RemoveAt(3);
                gameState.lastScores.RemoveAt(3);
            }

            SaveGameState();
            LoadMenuLevel();
        }

        private void Init()
        {
            // Make sure the object is not deleted between scene switching
            DontDestroyOnLoad(this.gameObject);

            // Load game state if save exists
            LoadGameState();

            // Set some global settings
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer(Utility.Constants.LAYER_EFFECTS), LayerMask.NameToLayer(Utility.Constants.LAYER_EFFECTS), true);

            // Update defaults
            playerName = gameState.defaultCurrentPlayer;
            currentScore = gameState.defaultCurrentScore;
        }

        private static GameManager _instance = null;

        /// <summary>
        /// If there is no game object with EventManager component,
        /// new object is created. This means, EventManager. Instance should
        /// be called from Unity thread or it must be certain, that the class
        /// was already instantiated inside the Unity thread. Returns null
        /// when exiting application.
        /// </summary>
        public static GameManager Instance
        {
            get
            {
                if (_applicationIsQuitting)
                    return null;


                if (_instance == null)
                {
                    // Try to find it in the scene
                    _instance = FindObjectOfType<GameManager>();
                    if (_instance == null)
                    {
                        // Doesn't exist in the scene, so create it
                        _instance = Resources.Load<GameManager>("Prefabs/GameManager");
                    }

                    _instance.Init();
                }

                return _instance;
            }
        }

        private static bool _applicationIsQuitting = false;
        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application or scene quits.
        /// If any script calls Instance after it have been destroyed, 
        /// it will create a buggy ghost object that will stay on the Editor scene
        /// even after stopping playing the Application. Bad script! This prevents
        /// such nasty behaviour.
        /// </summary>
        public void OnDestroy()
        {
            _applicationIsQuitting = true;
        }
    }

}
