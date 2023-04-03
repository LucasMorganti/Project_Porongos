using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Raycast ray;
    [SerializeField] private PlayerControl collide;
    [SerializeField] private GameObject applePrefab;

    private Transform _parent;
    private int _appleNumber;

    private void Start()
    {
        _parent = GameObject.Find("Hand").transform;
        _appleNumber = 0;
    }

    private void Update()
    {
        if (ray.throwable && collide.pick)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(applePrefab, _parent.position, _parent.rotation, _parent);
                _appleNumber++;

                if (_appleNumber > 3)
                {
                    Destroy(this.gameObject);
                    _appleNumber = 0;
                }
            }
        }
    }
}
