using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using VRTK;
using UnityEngine.Events;

public class MainMenu : MonoBehaviour
{
    public Button playFPS;
    public Button playVR;
    public Button credits;
    public Button exitBtn;
    public Text version;
    VRTK_SDKSetup sdk;
    Canvas menu;
    GameObject cam;

    // Use this for initialization
    void Start()
    {
        //mainMenu = GameObject.Find("MainMenu").GetComponent<Canvas>();
        playVR = GameObject.Find("playVR").GetComponent<Button>();
        playFPS = GameObject.Find("playFPS").GetComponent<Button>();
        credits = GameObject.Find("credits").GetComponent<Button>();
        exitBtn = GameObject.Find("exitBtn").GetComponent<Button>();
        version = GameObject.Find("version").GetComponent<Text>();


        playVR.onClick.AddListener(onPlayVRClicked);
        playFPS.onClick.AddListener(onPlayFPSButtonClicked);
        credits.onClick.AddListener(onCreditsButtonClicked);
        exitBtn.onClick.AddListener(onExitButtonClicked);

        sdk = GameObject.Find("VRTKSDK").GetComponentInChildren<VRTK_SDKSetup>();
        menu  = gameObject.GetComponent<Canvas>();
        cam = GameObject.Find("fallbackCam");

        version.text = Application.version;
        setupMenu(new Scene(), LoadSceneMode.Single);
        SceneManager.sceneLoaded += setupMenu;
    }

    private void onPlayVRClicked()
    {
        SceneManager.LoadScene("VR_fh_projekt", LoadSceneMode.Single);
    }
    public void onPlayFPSButtonClicked()
    {
        //
        SceneManager.LoadSceneAsync("FPS_fh_projekt");
        //SceneManager.UnloadSceneAsync("MainMenu");
    }


    public void onExitButtonClicked()
    {
        Application.Quit();
    }

    public void onCreditsButtonClicked()
    {
        //

    }


    // Update is called once per frame
    void Update()
    {
        if(!Cursor.visible)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void OnDestroy()
    {
        playVR.onClick.RemoveAllListeners();
        playFPS.onClick.RemoveAllListeners();
        credits.onClick.RemoveAllListeners();
        exitBtn.onClick.RemoveAllListeners();
    }

    void setupMenu(Scene scene, LoadSceneMode mode)
    {

        if(cam == null || menu == null || sdk == null)
        {
            return;
        }

        Debug.LogWarning("Scene Loaded");

        if (sdk != null)
        {
            Debug.LogWarning("VR Found");
            PlayerPrefs.SetInt("isVR", 1);
            PlayerPrefs.Save();
            menu.renderMode = RenderMode.WorldSpace;
            menu.transform.position = new Vector3(261.77f, 2, 224.43f);
            menu.transform.SetGlobalScale(new Vector3(0.01f, 0.01f, 0.01f));
            cam.SetActive(false);
            playFPS.enabled = false;
            playVR.enabled = true;
        }
        else
        {
            Debug.LogWarning("No VR Found");
            PlayerPrefs.SetInt("isVR", 0);
            PlayerPrefs.Save();
            Cursor.visible = true;
            cam.SetActive(true);
            menu.renderMode = RenderMode.ScreenSpaceOverlay;
            playFPS.enabled = true;
            playVR.enabled = false;
        }

    }
       
    
}
