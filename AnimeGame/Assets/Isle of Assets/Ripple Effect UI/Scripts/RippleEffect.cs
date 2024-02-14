using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RippleEffectUI
{
    [HelpURL("https://assetstore.unity.com/packages/slug/250779")]
    [RequireComponent(typeof(Mask))]
    public class RippleEffect : MonoBehaviour
    {
        public const float SPEED_MIN_VALUE = 0.05f, SPEED_MAX_VALUE = 7f;
        public const int MAX_RIPPLES_NUMBER_MIN_VALUE = 1, MAX_RIPPLES_NUMBER_MAX_VALUE = 10;

        public Sprite Shape
        {
            get
            {
                return shape;
            }
            set
            {
                shape = value;
            }
        }

        public float Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = Mathf.Clamp(value, SPEED_MIN_VALUE, SPEED_MAX_VALUE);
            }
        }

        public int MaxRipplesNumber
        {
            get
            {
                return maxRipplesNumber;
            }
            set
            {
                maxRipplesNumber = Mathf.Clamp(value, MAX_RIPPLES_NUMBER_MIN_VALUE, MAX_RIPPLES_NUMBER_MAX_VALUE);
            }
        }

        public bool Overlap
        {
            get
            {
                return overlap;
            }
            set
            {
                overlap = value;
            }
        }

        public StartingPosition _StartingPosition
        {
            get
            {
                return startingPosition;
            }
            set
            {
                startingPosition = value;
            }
        }

        public InputType _InputType
        {
            get
            {
                return inputType;
            }
            set
            {
                inputType = value;
            }
        }

        public ButtonType _ButtonType
        {
            get
            {
                return buttonType;
            }
            set
            {
                buttonType = value;
            }
        }

        public bool AutomaticFilling
        {
            get
            {
                return automaticFilling;
            }
            set
            {
                automaticFilling = value;
            }
        }

        [Tooltip("The shape in which the ripples will go")]
        [SerializeField]
        private Sprite shape;

        [Tooltip("Here you can adjust the color and transparency of the effect throughout its lifetime")]
        [SerializeField]
        private Gradient gradient;

        [Tooltip("The speed of propagation of ripples")]
        [Range(SPEED_MIN_VALUE, SPEED_MAX_VALUE)]
        [SerializeField]
        private float speed = 0.8f;

        [Tooltip("The number of simultaneously existing ripples on this object")]
        [Range(MAX_RIPPLES_NUMBER_MIN_VALUE, MAX_RIPPLES_NUMBER_MAX_VALUE)]
        [SerializeField]
        private int maxRipplesNumber = 1;

        [Tooltip("Allow ripples to be displayed on top of other objects (for buttons, the ripples will overlap the text)")]
        [SerializeField]
        private bool overlap = false;

        [Tooltip("The position where the ripples will be created")]
        [SerializeField]
        private StartingPosition startingPosition = StartingPosition.ClickPos;

        [Tooltip("The type of click after which the effect is displayed")]
        [SerializeField]
        private InputType inputType = InputType.Down;

        [Tooltip("The type of button after clicking on which the effect is displayed")]
        [SerializeField]
        private ButtonType buttonType = ButtonType.Primary;

        [Tooltip("Automatic filling of ripples throughout the RectTransform")]
        public bool automaticFilling = true;

        [Tooltip("Maximum ripple size")]
        [HideInInspector]
        public float shapeSize = 3f;

        [Tooltip("Events that will occur after the creation of the next ripple")]
        [HideInInspector]
        public UnityEvent onCreate;

        private GameObject[] ripples;
        private RectTransform rectTransform;
        private Canvas canvas;

        public enum StartingPosition { ClickPos, CenterPos }
        public enum InputType { Down, Up }
        public enum ButtonType { Primary, Secondary, Middle }

        /// <summary>
        /// Controlling the number of ripples in this RippleEffect instance
        /// </summary>
        /// <param name="ripple"></param>
        private void ClampRipplesNumber(GameObject ripple)
        {
            for (int i = 0; i < maxRipplesNumber; i++)
            {
                if (ripples[i] == null)
                {
                    ripples[i] = ripple;
                    return;
                }
            }
            Destroy(ripples[0]);
            for (int i = 0; i < maxRipplesNumber - 1; i++)
            {
                ripples[i] = ripples[i + 1];
            }
            ripples[maxRipplesNumber - 1] = ripple;
        }

        /// <summary>
        /// Creating a ripple for this UI object
        /// </summary>
        /// <param name="pos"></param>
        public void Create(Vector3 pos)
        {
            GameObject ripple = new GameObject();
            ClampRipplesNumber(ripple);
            ripple.name = "Ripple(Clone)";
            ripple.AddComponent<Image>().sprite = shape;
            ripple.transform.SetParent(transform);
            if (!overlap)
            {
                ripple.transform.SetAsFirstSibling();
            }
            if (startingPosition == StartingPosition.ClickPos)
            {
                ripple.transform.position = pos;
            }
            else
            {
                ripple.transform.localPosition = Vector2.zero;
            }
            if (automaticFilling)
            {
                Rect rect = rectTransform.rect;
                float ratio = (float)Mathf.Abs(rect.width) / Mathf.Abs(rect.height);
                shapeSize = 4f * (ratio < 1f ? (1f / ratio) : ratio);
            }
            ripple.AddComponent<Ripple>().Init(speed, shapeSize, gradient);
            onCreate?.Invoke();
        }

        /// <summary>
        /// Initializing this instance
        /// </summary>
        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            ripples = new GameObject[maxRipplesNumber];
        }

        /// <summary>
        /// Checking for clicking in the UI object area
        /// </summary>
        private void Update()
        {
            if (canvas == null)
                canvas = transform.GetComponentInParent<Canvas>();
            Vector3 mousePosition = Input.mousePosition;
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                mousePosition = canvas.worldCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, canvas.planeDistance));
            }
            else if (canvas.renderMode == RenderMode.WorldSpace)
            {
                Debug.LogError("RippleEffect does not support World Space Render Mode in Canvas");
            }
            if (Mathf.Abs(transform.position.x - mousePosition.x) * 2f < rectTransform.rect.width * rectTransform.lossyScale.x &&
                Mathf.Abs(transform.position.y - mousePosition.y) * 2f < rectTransform.rect.height * rectTransform.lossyScale.y &&
                (Input.GetMouseButtonDown((int)buttonType) && inputType == InputType.Down || Input.GetMouseButtonUp((int)buttonType) && inputType == InputType.Up))
            {
                Create(mousePosition);
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(RippleEffect))]
    public class RippleEffectEditor : Editor
    {
        private SerializedProperty shapeSize, onCreate;

        /// <summary>
        /// Fetch the object from the GameObject script to display in the inspector
        /// </summary>
        private void OnEnable()
        {
            shapeSize = serializedObject.FindProperty("shapeSize");
            onCreate = serializedObject.FindProperty("onCreate");
        }

        /// <summary>
        /// Implementation of the interface in the editor
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            RippleEffect rippleEffect = (RippleEffect)target;
            EditorGUI.BeginDisabledGroup(rippleEffect.automaticFilling);
            EditorGUILayout.PropertyField(shapeSize, new GUIContent("Shape Size"));
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.PropertyField(onCreate, new GUIContent("On Create"));
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}