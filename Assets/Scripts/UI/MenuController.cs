using EraseGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private EventHub _eventHub;
    private Animator _animator;

    public void Start()
    {
        _eventHub = EventHub.GetEventHub();
        _animator = GetComponent<Animator>();
        _animator.SetTrigger("OpenMenu");

        SetupEventListeners();
    }

    private void SetupEventListeners()
    {
        _eventHub.OnFireFailed += EventOnFireFailed;
    }

    private void EventOnFireFailed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnStartClicked()
    {
        _eventHub.InvokeOnScrollComplete(this, null);
        _animator.SetTrigger("CloseMenu");
    }
}