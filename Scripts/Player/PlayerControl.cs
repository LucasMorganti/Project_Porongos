using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
   [SerializeField] private Crouch crouch;
   [SerializeField] private GameObject crouchUI;
   [SerializeField] private GameObject crouch2;

   public GameObject finalMessage;
   
   public bool isWalking;
   public bool play;
   public bool pick;
   public GameObject hud;

   [SerializeField] private GameObject text2;
   [SerializeField] private GameObject text3;
   [SerializeField] private GameObject text4;
   [SerializeField] private GameObject text5;
   [SerializeField] private GameObject text6;

   [SerializeField] private GameObject dica2;
   [SerializeField] private GameObject dica3;
   [SerializeField] private GameObject dica4;

      [HideInInspector] public NavMeshAgent myAgent;
   [HideInInspector] public bool quicktimeOne;
   [HideInInspector] public bool quicktimeTwo;
   
   // private Animator _anim;
   private GameMaster _gm;
   private float _timer;

   private Camera cam;

   //private Animator _anim;

   private void Awake()
   {
      _gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
      transform.position = _gm.lastCheckpointPos;

      cam = GetComponentInChildren<Camera>();
      
      /*Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Confined;
      Cursor.lockState = CursorLockMode.Locked;*/

      myAgent = GetComponent<NavMeshAgent>();
      myAgent.isStopped = true;
      //_anim = GetComponentInChildren<Animator>();
      
      isWalking = false;
   }

   private void Update()
   {
      isWalking = false;

      if (myAgent.hasPath)
      {
         isWalking = true;
      }

      if (isWalking)
      {
         //_anim.Play("Walk");
      }
      else
      {
         //_anim.Play("Idle");
      }

      if (crouch.IsCrouched)
      {
         myAgent.height = 1.5f;
         myAgent.speed = 3f;

         crouchUI.SetActive(true);
         crouch2.SetActive(false);
      }
      else
      {
         myAgent.height = 3f;
         myAgent.speed = 6f;
         
         crouchUI.SetActive(false);
         crouch2.SetActive(true);
      }

      if (play)
      {
         _timer += Time.deltaTime;
         if (_timer >= 15)
         {
            play = false;
         }
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag("QTE1"))
      {
         quicktimeOne = true;
      }

      if (other.gameObject.CompareTag("QTE2"))
      {
         quicktimeTwo = true;
      }

      if (other.gameObject.CompareTag("Pick"))
      {
         pick = true;
      }

      if (other.gameObject.CompareTag("Tut2"))
      {
         text2.SetActive(true);
      }
      
      if (other.gameObject.CompareTag("Tut3"))
      {
         text3.SetActive(true);
      }
      
      if (other.gameObject.CompareTag("Tut4"))
      {
         text4.SetActive(true);
      }
      
      if (other.gameObject.CompareTag("Tut5"))
      {
         text5.SetActive(true);
      }
      
      if (other.gameObject.CompareTag("Tut6"))
      {
         text6.SetActive(true);
      }

      if (other.gameObject.CompareTag("Dica1"))
      {
         dica2.SetActive(true);
      }
      
      if (other.gameObject.CompareTag("Dica2"))
      {
         dica3.SetActive(true);
      }
      
      if (other.gameObject.CompareTag("Dica3"))
      {
         dica4.SetActive(true);
      }

      if (other.gameObject.CompareTag("HandAnim"))
      {
         play = true;
         Destroy(GameObject.FindWithTag("HandAnim"));
      }

      if (other.gameObject.CompareTag("End"))
      {
         finalMessage.SetActive(true);
         myAgent.enabled = false;
         hud.SetActive(false);

         Cursor.visible = true;
         Cursor.lockState = CursorLockMode.None;
         GetComponentInChildren<FirstPersonLook>().enabled = false;
      }

      if (other.gameObject.CompareTag("Bullet"))
      {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      }
      
   }

   private void OnTriggerExit(Collider other)
   {
      if (other.gameObject.CompareTag("HandAnim"))
      {
         play = false;
      }

      if (other.gameObject.CompareTag("Pick"))
      {
         pick = false;
      }
   }
}
