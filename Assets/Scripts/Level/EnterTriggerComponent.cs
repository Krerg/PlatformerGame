using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnterTriggerComponent : MonoBehaviour
{

    [SerializeField] private String _tag;
    [SerializeField] private EnterCollisionComponent.EnterEvent _action;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(_tag))
        {
            _action?.Invoke(other.gameObject);
        }
    }
}
