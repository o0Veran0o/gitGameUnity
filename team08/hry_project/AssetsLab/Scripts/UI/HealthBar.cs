using UnityEngine;
using UnityEngine.UI;


namespace hry.labs.UI
{

    public class HealthBar : MonoBehaviour
    {

        public Image healthImage;


        private void Start()
        {
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            // Find the canvas with world space render settings
            foreach (Canvas c in canvases)
            {
                if (c.renderMode == RenderMode.WorldSpace)
                {
                    transform.SetParent(c.transform, false);
                    break;
                }
            }

            UpdateHealth(1.0f);
        }

        public void UpdateHealth(float fraction)
        {
            healthImage.fillAmount = fraction;
            healthImage.color = Color.Lerp(Color.red, Color.green, fraction);
        }
    }

}