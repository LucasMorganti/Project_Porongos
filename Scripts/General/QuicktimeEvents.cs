using System;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace General
{
    public class QuicktimeEvents : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        public GameObject hud;
        [SerializeField] private GameObject mesh;
        [SerializeField] private PlayerControl state;
        [SerializeField] private OffMeshLink link1;
        [SerializeField] private OffMeshLink link2;

        #region QTE1 Variables

        [Header("Press Quicktime")] 
        [SerializeField] private GameObject mouseIcon;
        [SerializeField] private int totalPresses;

        /*private SerialPort _arduinoPort;
        private int _status;*/
        private float _timer;

        #endregion

        #region QTE2 Variables

        [Header("Random Quicktime")] 
        [SerializeField] private GameObject randomPoints;
        [SerializeField] private GameObject[] buttons;

        private int _currentActiveButton = 0;
        private int _lastActiveButton;
        private float _timer2;

        #endregion
        
        private int _numberOfFails;

        private void Start()
        {
            /*_arduinoPort = new SerialPort("COM3", 9600);
            _arduinoPort.Open();
            _arduinoPort.ReadTimeout = 1;*/
            
            mouseIcon.SetActive(false);
            randomPoints.SetActive(false);
            
            state.quicktimeOne = false;
            state.quicktimeTwo = false;

            foreach (var t in buttons)
            {
                t.SetActive(false);
            }
            buttons[0].SetActive(true);

            link1.activated = false;
            link2.activated = false;
        }

        private void Update()
        {
            if (_numberOfFails >= 3)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            
            if (state.quicktimeOne)
            {
                mouseIcon.SetActive(true);
                player.GetComponentInChildren<Raycast>().enabled = false;

                Time.timeScale = 0;
                
                ClickQuicktime();
                //ArduinoControl();

                state.GetComponentInChildren<FirstPersonLook>().enabled = false;
                hud.SetActive(false);
            }
            else
            {
                Time.timeScale = 1;
                
                mouseIcon.SetActive(false);
                player.GetComponentInChildren<Raycast>().enabled = true;
                
                state.GetComponentInChildren<FirstPersonLook>().enabled = true;
                hud.SetActive(true);
            }

            if (state.quicktimeTwo)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                Time.timeScale = 1;

                randomPoints.SetActive(true);
                player.GetComponentInChildren<Raycast>().enabled = false;

                _timer2 += Time.deltaTime;
                if (_timer2 >= 2)
                {
                    state.quicktimeTwo = false;
                    _currentActiveButton = 0;
                    
                    foreach (var t in buttons)
                    {
                        t.SetActive(false);
                    }
                    buttons[0].SetActive(true);

                    _numberOfFails++;
                }
                
                state.GetComponentInChildren<FirstPersonLook>().enabled = false;
                hud.SetActive(false);
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                Time.timeScale = 1;
            
                randomPoints.SetActive(false);
                player.GetComponentInChildren<Raycast>().enabled = true;
                _timer2 = 0;
                
                state.GetComponentInChildren<FirstPersonLook>().enabled = true;
                hud.SetActive(true);
            }
        }

        private void ClickQuicktime()
        {
            _timer += Time.deltaTime;
            if (_timer >= 7)
            {
                _timer = 0;

                state.quicktimeOne = false;
                mouseIcon.SetActive(false);
                player.GetComponentInChildren<Raycast>().enabled = true;
            }

            if (Input.GetMouseButtonDown(0))
            {
                totalPresses++;
                if (totalPresses >= 10)
                {
                    link1.activated = true;

                    state.quicktimeOne = false;
                    mouseIcon.SetActive(false);
                    player.GetComponentInChildren<Raycast>().enabled = true;
                    
                    state.GetComponentInChildren<FirstPersonLook>().enabled = true;
                    hud.SetActive(true);

                    Destroy(GameObject.FindWithTag("QTE1"));
                }
            }
        }

        /*private void ArduinoControl()
        {
            if (_arduinoPort.IsOpen)
            {
                try
                {
                    _status = _arduinoPort.ReadByte();
                    if (_status == 1)
                    {
                        totalPresses++;
                        if (totalPresses >= 10)
                        {
                            link1.activated = true;

                            state.quicktimeOne = false;
                            mouseIcon.SetActive(false);
                            player.GetComponentInChildren<Raycast>().enabled = true;
                            
                            state.GetComponentInChildren<FirstPersonLook>().enabled = true;
                            hud.SetActive(true);

                            Destroy(GameObject.FindWithTag("QTE1"));
                        }
                        else
                        {
                            //anim
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }*/

        public void OnClick()
        {
            _timer2 = 0;
            
            buttons[_currentActiveButton].SetActive(false);

            _currentActiveButton++;
            
            buttons[_currentActiveButton].SetActive(true);
        }

        public void Exit()
        {
            link2.activated = true;

            state.quicktimeTwo = false;
            Time.timeScale = 1;
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            randomPoints.SetActive(false);
            player.GetComponentInChildren<Raycast>().enabled = true;
            
            state.GetComponentInChildren<FirstPersonLook>().enabled = true;
            hud.SetActive(true);
            
            Destroy(this.gameObject);
        }
    }
}
