using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GuiHandler : MonoBehaviour {
    public GameObject Player = null;
    Text guiX;
    Text guiY;
    Text guiZ;
    Text ballVel;
    Text ballSum;
    Canvas mainMenu;
    public bool isVRScene = false;
    Button mainMenuBtn;
    Button cancelBtn;

    bool isMainMenuActive = false;

    throwProjectile throwProjectile;

    // Use this for initialization
    void Start () {
        //set time scale to be sure game is not paused;
        Time.timeScale = 1;
        
       if(isVRScene)
        {
            //todo setup VR menu

        } else
        {
            setupFPSMenu();
            Player.SetActive(true);
        }

    }

      // Update is called once per frame
    void FixedUpdate()
    {
        if(isVRScene)
        {
            //todo add vr scene stats
        } else
        {
            drawPlayerStats();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                toggleMenu();
            }

            checkMenuClose();
        }
    }

    private void checkMenuClose()
    {
        //Debug.Log(PlayerPrefs.GetInt("isMenu"));
        if (PlayerPrefs.GetInt("isMenu") == 0)
        {
            //closed menu, restore player controller
            Player.SetActive(true);
        }
    }


    #region Methods
    private void toggleMenu()
    {
        Debug.Log("Open menu");
        if (isVRScene)
        {
            //todo: ad vr scene menu
        } else
        {
            pause();
        }
    }

    /**
     * draw player Stats for FPS
    */
    private void drawPlayerStats()
    {
        if(isVRScene)
        {
            //TODO: add vr scene stats
        } else
        {
            GameObject UIP = GameObject.Find("UI_Player");
            if (UIP != null && UIP.activeInHierarchy)
            {
                if (guiX != null && guiY != null && guiZ != null)
                {
                    guiX.text = Player.transform.position.x.ToString();
                    guiY.text = Player.transform.position.y.ToString();
                    guiZ.text = Player.transform.position.z.ToString();
                }

                if (throwProjectile != null && ballVel != null)
                {
                    string speed = string.Format("{0:0.0}", throwProjectile.ThrowSpeed);
                    ballVel.text = speed;
                }

                if (ballSum != null)
                {
                    string sum = string.Format("{0:0.0}", throwProjectile.ballIDs.Length);
                    ballSum.text = sum;
                }
            }
        }
    }

    private void setupFPSMenu()
    {
        Debug.Log("setup FPS menu");
        try
        {
            guiX = GameObject.Find("guiX").GetComponent<Text>();
            guiY = GameObject.Find("guiY").GetComponent<Text>();
            guiZ = GameObject.Find("guiZ").GetComponent<Text>();
            ballVel = GameObject.Find("ballVelText").GetComponent<Text>();
            ballSum = GameObject.Find("ballSum").GetComponent<Text>();

            GameObject.Find("UI_Player").SetActive(true);
        } catch (Exception e)
        {
            Debug.LogWarning("FPS Menu is inactive - " + e.Message.ToString());
            //GameObject.Find("UI_Player").SetActive(false);
        }


        try
        {
            GameObject player = GameObject.Find("FPSController");
            if (player != null)
            {
                Player = player;
                throwProjectile = player.GetComponent<throwProjectile>();
            }
        } catch (Exception error)
        {
            Debug.LogWarning("Cant find FPS Player Controller " + error.StackTrace.ToString());
        }
    }

    public void pause()
    {
        Debug.Log("Open menu");
        Player.SetActive(false);
        Time.timeScale = 0;
        SceneManager.LoadSceneAsync("FPSMenu", LoadSceneMode.Additive);
        PlayerPrefs.SetInt("isMenu", 1);
        PlayerPrefs.Save();
        //Debug.Log(PlayerPrefs.GetInt("isMenu"));
    }

    public void resume()
    {
        Player.SetActive(true);
        Time.timeScale = 1;
        PlayerPrefs.SetInt("isMenu", 0);
        PlayerPrefs.Save();
    }
    #endregion

}
