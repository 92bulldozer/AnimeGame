using System.Collections;
using System.Collections.Generic;
using AnimeGame;
using DarkTonic.MasterAudio;
using EPOOutline;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InteractableObject : MonoBehaviour ,IPointerEnterHandler ,IPointerClickHandler ,IPointerExitHandler
{
    public string enterSfx;
        public string clickSfx;
        public UnityEvent onClickEvent;

        [SerializeField]
        private bool affectOutlinable = true;

        private Outlinable outlinable;

        private void Start()
        {
            if (!affectOutlinable)
                return;
            outlinable = GetComponent<Outlinable>();
            outlinable.enabled = false;


        }

        public void OnPointerEnter()
        {
            if (!affectOutlinable || !Player.Instance.canInteract)
                return;

            
            
            MasterAudio.PlaySound3DAtTransform(enterSfx, transform);

        }

        public void OnPointerExit()
        {
            if (!affectOutlinable)
                return;


        }

        public void OnPointerClick()
        {
            if ( !Player.Instance.canInteract)
                return;
            
            //AudioSource.PlayClipAtPoint(interactionSound, transform.position, 1.0f);
            MasterAudio.PlaySound3DAtTransform(clickSfx, transform);
            onClickEvent?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!affectOutlinable  || !Player.Instance.canInteract)
                return;
            
            MasterAudio.PlaySound3DAtTransform(enterSfx, transform);
            outlinable.enabled = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if ( !Player.Instance.canInteract)
                return;
            
            Player.Instance.canInteract = false;

            //AudioSource.PlayClipAtPoint(interactionSound, transform.position, 1.0f);
            MasterAudio.PlaySound3DAtTransform(clickSfx, transform);
            onClickEvent?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!affectOutlinable  || !Player.Instance.canInteract)
                return;

            outlinable.enabled = false;

        }

        public void ResetInteract()
        {
            outlinable.enabled = false;
        }
}
