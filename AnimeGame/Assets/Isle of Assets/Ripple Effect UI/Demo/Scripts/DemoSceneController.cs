using UnityEngine;
using UnityEngine.UI;

namespace RippleEffectUI.Demo
{
    [HelpURL("https://assetstore.unity.com/packages/slug/250779")]
    public class DemoSceneController : MonoBehaviour
    {
        [SerializeField]
        private Toggle[] toggles = null;

        [SerializeField]
        private Slider slider = null;

        private RippleEffect[] rippleEffects;
        private float[] maxSpeedValues;

        /// <summary>
        /// Preparing data before starting
        /// </summary>
        private void Start()
        {
            rippleEffects = FindObjectsOfType<RippleEffect>();
            maxSpeedValues = new float[rippleEffects.Length];
            for (int i = 0; i < rippleEffects.Length; i++)
                maxSpeedValues[i] = rippleEffects[i].Speed / slider.value;
        }

        /// <summary>
        /// Updating the values of all RippleEffect on the scene
        /// </summary>
        private void Update()
        {
            for (int i = 0; i < rippleEffects.Length; i++)
            {
                rippleEffects[i].Overlap = toggles[0].isOn;
                rippleEffects[i]._InputType = toggles[1].isOn ? RippleEffect.InputType.Down : RippleEffect.InputType.Up;
                rippleEffects[i]._StartingPosition = toggles[2].isOn ? RippleEffect.StartingPosition.CenterPos : RippleEffect.StartingPosition.ClickPos;
                rippleEffects[i].Speed = maxSpeedValues[i] * slider.value;
            }
        }
    }
}