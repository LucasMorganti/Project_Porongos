using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectThrow : MonoBehaviour
{
    [SerializeField] private float throwForce;
    public ContactPoint contact;
    public bool hitGround;
    public Transform newWaypoint;

    private Rigidbody _rb;
    private float _velocity;

    private Image itemUi;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _velocity = 0;
    }

    private void Update()
    {
        _rb.detectCollisions = true;

        itemUi = GameObject.Find("Item Icon").GetComponent<Image>();

        if (Input.GetMouseButtonDown(1))
        {
            _rb.AddForce(transform.forward * throwForce);
            _rb.useGravity = true;
            _velocity = 1;

            transform.parent = null;
            
            itemUi.color = Color.gray;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_velocity >= 1)
        {
            contact = other.contacts[0];
            Vector3 position = contact.point;

            hitGround = true;
            _velocity = 0;
            
            Destroy(gameObject, 5);
        }
    }
}
