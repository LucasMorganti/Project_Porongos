using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Raycast : MonoBehaviour
{
    [SerializeField] private Image uiCrosshair;
    [SerializeField] private int rayLenght = 20;
    [SerializeField] private LayerMask layerMaskInteract;
    
    [SerializeField] private GameObject applePrefab;
    private Transform _parent;
    private int _appleNumber;
    
    public bool ground;
    public bool throwable;

    private GameObject _raycastedObject;
    private PlayerControl _player;

    public GameObject video;
    public VideoPlayer videoGirl;
    public GameObject hud;
    public GameObject text1;
    public GameObject dica;

    private float _timer;

    public Image itemUi;

    private void Awake()
    {
        _player = GetComponentInParent<PlayerControl>();
        
        _parent = GameObject.Find("Hand").transform;
        _appleNumber = 0;
        videoGirl.Stop();
    }

    private void Update()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        
        if (Physics.Raycast(transform.position, fwd, out hit, rayLenght, layerMaskInteract))
        {
            if (_player.isWalking == false)
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    _raycastedObject = hit.collider.gameObject;
                    CrosshairActive();

                    ground = true;
                    throwable = false;

                    if (Input.GetMouseButtonDown(0))
                    {
                        _player.myAgent.SetDestination(hit.point);
                    }
                }
            }
            
            if (hit.collider.CompareTag("Throwable"))
            {
                _raycastedObject = hit.collider.gameObject;
                CrosshairActive();
                
                if (Input.GetMouseButtonDown(0))
                {
                    Instantiate(applePrefab, _parent.position, _parent.rotation, _parent);
                    _appleNumber++;
                    
                    itemUi.color = Color.blue;

                    if (_appleNumber > 3)
                    {
                        Destroy(_raycastedObject);
                        _appleNumber = 0;
                    }
                }

                throwable = true;
                ground = false;
            }

            if (hit.collider.CompareTag("Rat"))
            {

                if (Input.GetMouseButtonDown(0))
                {
                    video.SetActive(true);
                    videoGirl.Play();
                    
                    
                    
                    Destroy(GameObject.FindWithTag("Rat"));
                }
                
                
            }
        }
        else
        {
            CrosshairNormal();

            ground = false;
            throwable = false;
            
            
        }
        
        if (videoGirl.isPlaying)
        {
            hud.SetActive(false);
            _player.myAgent.enabled = false;
            AudioListener.pause = true;
            
            _timer += Time.deltaTime;
            if (_timer >= 40)
            {
                dica.SetActive(true);
                AudioListener.pause = false;
                
                video.SetActive(false);
                _player.myAgent.enabled = true;
                hud.SetActive(true);
            
                text1.SetActive(true);
            }
        }
        
    }

    private void CrosshairActive()
    {
        uiCrosshair.color = Color.red;
    }
    
    private void CrosshairNormal()
    {
        uiCrosshair.color = Color.white;
    }
}
