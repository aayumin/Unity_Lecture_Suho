using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingScript : MonoBehaviour
{

    Toggle toggle_bgm, toggle_effect;
    Slider slider_bgm, slider_effect;

    public GameObject settingBoard;
    AudioSource bgm;

    // Start is called before the first frame update
    void Start()   
    {
        // settingBoard = GameObject.Find("setting_board").gameObject;
        toggle_bgm = settingBoard.transform.Find("Toggle_bgm").gameObject.GetComponent<Toggle>();
        slider_bgm = settingBoard.transform.Find("Slider_bgm").gameObject.GetComponent<Slider>();
        toggle_effect = settingBoard.transform.Find("Toggle_effect").gameObject.GetComponent<Toggle>();
        slider_effect = settingBoard.transform.Find("Slider_effect").gameObject.GetComponent<Slider>();
        bgm = GameObject.Find("BGM_audio").gameObject.GetComponent<AudioSource>();


        toggle_bgm.onValueChanged.AddListener(Pause_bgm);
        slider_bgm.onValueChanged.AddListener(set_bgm_volume);
        
        toggle_effect.onValueChanged.AddListener(Pause_effect);
        slider_effect.onValueChanged.AddListener(set_effect_volume);

        if (PlayerPrefs.GetInt("bgm_play", 1) == 1)
            toggle_bgm.isOn = true;
        else toggle_bgm.isOn = false;
        slider_bgm.value = PlayerPrefs.GetFloat("bgm_volume", 1.0f);

        
        if (PlayerPrefs.GetInt("soundeffect_play", 1) == 1)
            toggle_effect.isOn = true;
        else toggle_effect.isOn = false;
        slider_effect.value = PlayerPrefs.GetFloat("soundeffect_volume", 1.0f);


    }

    void Pause_effect(bool flag) {
        if (flag) {
            PlayerPrefs.SetInt("soundeffect_play", 1);
        }
        else {
            PlayerPrefs.SetInt("soundeffect_play", 0);
        }   
    }

    void set_effect_volume(float v) {
        // v : 0.0 ~ 1.0
        PlayerPrefs.SetFloat("soundeffect_volume", v);
    }
    

    void Pause_bgm(bool flag) {
        if (flag) {
            bgm.Play();
            PlayerPrefs.SetInt("bgm_play", 1);
        }
        else {
            bgm.Pause();
            PlayerPrefs.SetInt("bgm_play", 0);
        }

        
    }

    void set_bgm_volume(float v) {
        // v : 0.0 ~ 1.0
        bgm.volume = v;
        PlayerPrefs.SetFloat("bgm_volume", v);
    }
    
    public void Quit_Completely() {
        Application.Quit();
    }

    public void Open_Setting_Board() {
        settingBoard.SetActive(true);  // open board
        Time.timeScale = 0;  // pause

        PlayerPrefs.SetInt("pause", 1);
    }

    public void Close_Setting_Board() {
        settingBoard.SetActive(false); 
        Time.timeScale = 1; 

        PlayerPrefs.SetInt("pause", 0);
    }

    public void Quit_gotoMain() {
        Time.timeScale = 1; 
        SceneManager.LoadScene("MainScene");
        
        PlayerPrefs.SetInt("pause", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
