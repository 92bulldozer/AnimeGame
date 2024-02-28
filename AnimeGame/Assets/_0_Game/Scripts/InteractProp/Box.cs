using System.Collections;
using System.Collections.Generic;
using EJ;
using UnityEngine;

namespace AnimeGame
{
    public class Box : InteractableObject
    {
        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Interact()
        {
            base.Interact();
            "Box열기".Log();
        }

        public override void ShowInteractPanel()
        {
            base.ShowInteractPanel();
        }

        public override void HideInteractPanel()
        {
            base.HideInteractPanel();
        }
    }
}

