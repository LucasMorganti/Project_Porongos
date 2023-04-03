using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameMaster _gm;

    private void Start()
    {
        _gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _gm.lastCheckpointPos = transform.position;
        }
    }
}
