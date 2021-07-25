using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CheatControler : MonoBehaviour
{
    private string _currentInput;
    [SerializeField] private float _inputTimeToLive;

    [SerializeField] private CheatItem[] cheats;
    private float _inputTime;

    private void Awake()
    {
        Keyboard.current.onTextInput += OnTextInput;
    }

    private void OnDestroy()
    {
        Keyboard.current.onTextInput -= OnTextInput;
    }

    private void OnTextInput(char inputChar)
    {
        _currentInput += inputChar;
        _inputTime = _inputTimeToLive;
        FindAnyCheats();
    }

    private void FindAnyCheats()
    {
        foreach (var cheat in cheats)
        {
            if (_currentInput.Contains(cheat.name))
            {
                cheat.action.Invoke();
                _currentInput = String.Empty;
                break;
            }
        }
    }

    private void Update()
    {
        if (_inputTime < 0)
        {
            _currentInput = String.Empty;
        }
        else
        {
            _inputTime -= Time.deltaTime;
        }
    }
}

[Serializable]
public class CheatItem
{
    public string name;
    public UnityEvent action;
}