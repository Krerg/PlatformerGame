using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnterCollisionComponent : MonoBehaviour
{
    [SerializeField] private String _tag;
    [SerializeField] private EnterEvent _action;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(_tag))
        {
            _action?.Invoke(other.gameObject);
        }
    }

    [Serializable]
    public class EnterEvent : UnityEvent<GameObject>
    {
        
    }
   
}
