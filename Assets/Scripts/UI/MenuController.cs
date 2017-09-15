using EraseGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private EventHub _eventHub;
    private Animator _animator;

    public void Start()
    {
        _eventHub = EventHub.GetEventHub();
        _animator = GetComponent<Animator>();
        _animator.SetTrigger("OpenMenu");
    }

    public void OnStartClicked()
    {
        _eventHub.InvokeOnScrollComplete(this, null);
        _animator.SetTrigger("CloseMenu");
    }
}