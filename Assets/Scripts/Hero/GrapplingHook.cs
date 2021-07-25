using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private SpringJoint2D _joint;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _distance;
    [SerializeField] private LayerMask _rayCastMask;
    [SerializeField] private Rigidbody2D _body;
    [SerializeField] private int _movableObjectForce;

    private bool _isGrappled;
    private bool _movableObjectGrappled;
    private Vector2 _offset;
    private GameObject movableObject;
    private float originalDistance;
    private float releaseHookDistance;

    private bool _isHookDisabled;

    private RaycastHit2D _hit;
    private Vector3 _lineEnd;
    private Vector3 _target;

    [SerializeField] private UnityEvent _hooked;

    private void Update()
    {
        if (_isGrappled)
        {
            if (_movableObjectGrappled)
            {
                _lineEnd = (Vector2) movableObject.transform.position - _offset;

                var currentHookLength = (movableObject.transform.position - transform.position).magnitude;
                if (_movableObjectGrappled && currentHookLength < releaseHookDistance)
                {
                    ReleaseHook();
                }
            }

            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, _lineEnd);
        }
    }


    public void TryGrap()
    {
        if (_isHookDisabled)
        {
            return;
        }

        _target = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        _target.z = 0;

        _hit = Physics2D.Raycast(transform.position, _target - transform.position, _distance, _rayCastMask);

        if (_hit.collider != null)
        {
            _isGrappled = true;

            _hooked?.Invoke();
            if (_hit.collider.gameObject.TryGetComponent<Rigidbody2D>(out var rigidBody))
            {
                if (rigidBody.simulated)
                {
                    rigidBody.AddForce(((Vector2) transform.position - _hit.point).normalized * _movableObjectForce);
                    _movableObjectGrappled = true;
                    originalDistance = (_hit.point - (Vector2) transform.position).magnitude;
                    releaseHookDistance = originalDistance / 2;
                    _offset = (Vector2) _hit.collider.gameObject.transform.position - _hit.point;
                    movableObject = _hit.collider.gameObject;
                    _lineRenderer.enabled = true;
                    return;
                }
            }

            _lineRenderer.enabled = true;
            _lineEnd = _hit.point;

            _joint.enabled = true;
            _joint.connectedAnchor = _hit.point;
            _joint.connectedBody = _hit.collider.gameObject.GetComponent<Rigidbody2D>();
            _joint.distance = Vector2.Distance(transform.position, _hit.point) / 5;
        }
    }

    public void EnableHook()
    {
        _isHookDisabled = false;
    }

    public void DisableHook()
    {
        _isHookDisabled = true;
    }

    public void ReleaseHook()
    {
        _joint.enabled = false;
        _lineRenderer.enabled = false;
        _isGrappled = false;
        _movableObjectGrappled = false;
    }
}