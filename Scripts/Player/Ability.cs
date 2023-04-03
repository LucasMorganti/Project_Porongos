using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    [SerializeField] private Image abilityIcon;

    [Header("Values")] 
    //[SerializeField] private GameObject shader;
    public float fullBar = 30;
    public int abilityUses;

    [Header("Functions")] 
    public bool usingAbility;
    public bool stopAbility;

    private float _timer;
    private float _bar;
    private bool _loosingAb;

    private void Start()
    {
        _timer = 0;

        usingAbility = false;
        stopAbility = false;

        _bar = fullBar;
        abilityIcon.fillAmount = _bar / fullBar;
        
        //shader.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            if (_bar >= fullBar && !_loosingAb && abilityUses > 0)
            {
                usingAbility = true;
                stopAbility = false;
            }

            if (_loosingAb)
            {
                usingAbility = false;
                stopAbility = true;
                _loosingAb = false;

                abilityUses--;
            }
        }

        if (usingAbility)
        {
            AbilityFunction();
            gameObject.layer = 8;
        }
        else
        {
            gameObject.layer = 7;

            _timer = 0;
            _bar += Time.deltaTime;
            if (_bar > fullBar)
            {
                _bar = fullBar;
                if (stopAbility)
                {
                    stopAbility = false;
                }
            }

            abilityIcon.fillAmount = _bar / fullBar;
        }
    }

    private void AbilityFunction()
    {
        _loosingAb = true;

        _timer += Time.deltaTime;
        if (_timer > 0)
        {
            _bar -= Time.deltaTime;
            if (_bar <= 0)
            {
                usingAbility = false;
                _loosingAb = false;

                abilityUses--;
            }
        }

        abilityIcon.fillAmount = _bar / fullBar;
    }
}
