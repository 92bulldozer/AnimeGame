using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteract
{
    public bool isInteracted { get; set; }
    public void Interact();
    public void ShowInteractPanel();
    public void HideInteractPanel();
}
