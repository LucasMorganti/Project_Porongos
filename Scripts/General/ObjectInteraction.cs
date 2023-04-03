using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    [SerializeField] private Raycast interaction;

    [Header("Functions Check")] 
    public bool isHolding;
    public bool hitGround;

    [Header("Distraction and Throw Values")]
    public Transform newWaypoint;
    public ContactPoint contact;
    [SerializeField] private float throwForce;
    private float _velocity;

    private Rigidbody _rb;
    private Transform _parent;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _parent = GameObject.Find("Hand").transform;
        _velocity = 0;
    }

    private void Update()
    {
        if (interaction.throwable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isHolding = true;
            }
        }

        if (isHolding)
        {
            _rb.useGravity = false;
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
            _rb.detectCollisions = true;

            transform.position = _parent.position;
            transform.parent = _parent.transform;
            
            if (Input.GetMouseButtonDown(1))
            {
                _rb.AddForce(_parent.transform.forward * throwForce);
                _rb.useGravity = true;
                isHolding = false;

                _velocity = 1;
            }
        }
        else
        {
            _rb.useGravity = true;
            _rb.constraints = RigidbodyConstraints.None;

            transform.parent = null;
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (gameObject.CompareTag("Throwable") && _velocity >= 1)
        {
            contact = other.contacts[0];
            Vector3 position = contact.point;

            hitGround = true;
            _velocity = 0;
        }
    }
}
