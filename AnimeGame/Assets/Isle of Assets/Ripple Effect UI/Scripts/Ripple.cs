using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace RippleEffectUI
{
    [HelpURL("https://assetstore.unity.com/packages/slug/250779")]
    public class Ripple : MonoBehaviour
    {
        /// <summary>
        /// Initializing this ripple instance
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="shapeSize"></param>
        /// <param name="gradient"></param>
        public void Init(float speed, float shapeSize, Gradient gradient)
        {
            StartCoroutine(RippleAnimation(speed, shapeSize, gradient));
        }

        /// <summary>
        /// Playing the ripple animation
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="shapeSize"></param>
        /// <param name="gradient"></param>
        /// <returns></returns>
        private IEnumerator RippleAnimation(float speed, float shapeSize, Gradient gradient)
        {
            Image image = GetComponent<Image>();
            transform.localScale = Vector3.zero;
            while (true)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * shapeSize, Time.deltaTime * speed);
                image.color = gradient.Evaluate(transform.localScale.x / shapeSize);
                if (transform.localScale.x * 1.05f >= shapeSize)
                    Destroy(gameObject);
                yield return null;
            }
        }
    }
}