using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTime : MonoBehaviour
{
    [SerializeField] private GameObject textOne;
    [SerializeField] private GameObject textTwo;
    [SerializeField] private GameObject textThree;
    [SerializeField] private GameObject textFour;
    [SerializeField] private GameObject textFive;
    [SerializeField] private GameObject textSix;
    [SerializeField] private GameObject textSeven;

    public Animation fade1;

    private float _timer;

    private void Start()
    {
        _timer += Time.deltaTime;
        if (_timer >= 2)
        {
            textOne.SetActive(true);
            _timer = 0;
        }
        
        textTwo.SetActive(false);
        textThree.SetActive(false);
        textFour.SetActive(false);
        textFive.SetActive(false);
        textSix.SetActive(false);
        textSeven.SetActive(false);
    }

    private void Update()
    {
        if (fade1.isPlaying == false)
        {
            textTwo.SetActive(true);
        }
    }
}
