using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EraseGame;
using System;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public int Score;

    private EventHub _eventHub;
    private int _targetScore;
    private Text _text;
    // Use this for initialization
    void Start()
    {
        _text = GetComponent<Text>();
        _eventHub = EventHub.GetEventHub();
        _eventHub.OnFireSuccess += EventOnFireSuccess;
    }

    private void EventOnFireSuccess()
    {
        _targetScore += 57;
    }

    // Update is called once per frame
    void Update()
    {
        if (Score < _targetScore)
        {
            Score++;
            _text.text = Score.ToString();
        }
    }
}