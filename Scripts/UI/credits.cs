using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class credits : MonoBehaviour
{

    public GameObject fade;

    private float timer;
    private void Update()
    {
        if (fade.activeSelf)
        {
            timer += Time.deltaTime;
            if (timer >= 15)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
