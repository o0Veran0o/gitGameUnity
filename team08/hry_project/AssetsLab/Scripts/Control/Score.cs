using UnityEngine;
using UnityEngine.UI;


namespace hry.labs.Control
{
    /// <summary>
    /// When the object is created it fills the text score with current game score.
    /// When enabled/disabled listener is registered/removed.
    /// </summary>
    public class Score : MonoBehaviour
    {

        public Text scoreText;

        // TODO 7 Implement initialization in Awake, callback registration in OnEnable and OnDisable stop listening.
        private void Awake()
        {
            scoreText.text = GameManager.Instance.currentScore.ToString();
        }

        private void OnEnable()
        {
            EventManager.Instance.OnEnemyDestroyed.AddListener(ScoreIncreased);
        }

        private void OnDisable()
        {
            if (EventManager.Instance)
            {
                EventManager.Instance.OnEnemyDestroyed.RemoveListener(ScoreIncreased);
            }
        }

        public void ScoreIncreased()
        {
            GameManager.Instance.currentScore += 1;
            scoreText.text = GameManager.Instance.currentScore.ToString();
        }
    }

}