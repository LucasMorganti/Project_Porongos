using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class CutsceneControl : MonoBehaviour
{
    [SerializeField] private PlayerControl character;
    
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject gameHUD;
    //[SerializeField] private GameObject menu;
    [SerializeField] private GameObject cutsceneCamera;

    [SerializeField] private PlayableDirector director;
    [SerializeField] private CinemachineDollyCart cart;
    [SerializeField] private GameObject hand;

    private float _timer;

    private void Start()
    {
        cutsceneCamera.SetActive(false);
        
        director.Stop();
        cart.m_Speed = 0;
        //hand.SetActive(false);
    }

    private void Update()
    {
        if (character.play)
        {
            player.SetActive(false);
            gameHUD.SetActive(false);
            //menu.SetActive(false);
            
            hand.SetActive(true);
            cutsceneCamera.SetActive(true);
            director.Play();
            cart.m_Speed = 40;

            _timer += Time.deltaTime;
            if (_timer >= 15)
            {
                player.SetActive(true);
                gameHUD.SetActive(true);
                //menu.SetActive(true);
            
                cutsceneCamera.SetActive(false);
                director.Stop();
                character.play = false;
                cart.enabled = false;
            }
        }
    }
}
