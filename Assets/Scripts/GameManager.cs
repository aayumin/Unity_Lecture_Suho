using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    /// nickname,   level,  exp,   money,   \

    GameObject exp_fill;

    Text level_text, exp_text, money_text, nickname_text;
    InputField nickname_input;

    public GameObject initialize_popup;

    int level, exp, money;
    string nickname;
    
    // Start is called before the first frame update
    void Start()
    {   
        initialize();
        
        if (PlayerPrefs.GetString("nickname", "-") == "-"){
            initialize2();
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
 
    }
 
    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if (scene.name == "MainScene"){
            initialize();

        }
    }

    void initialize() {
        level = PlayerPrefs.GetInt("level", 1);
        nickname = PlayerPrefs.GetString("nickname", "-");
        exp = PlayerPrefs.GetInt("exp", 0);
        money = PlayerPrefs.GetInt("total_coin", 0);


        level_text = GameObject.Find("level").transform.Find("level_text").GetComponent<Text>();
        exp_text = GameObject.Find("exp").transform.Find("text").GetComponent<Text>();
        nickname_text = GameObject.Find("nickname").transform.Find("text").GetComponent<Text>();
        money_text = GameObject.Find("money").transform.Find("text").GetComponent<Text>();


        level_text.text = level.ToString();
        exp_text.text = exp.ToString() + " / 100";
        nickname_text.text = nickname;
        money_text.text = "$ " + money.ToString();


        exp_fill = GameObject.Find("exp_fill").gameObject;
        Vector3 temp = exp_fill.transform.localScale;
        temp.x = (float)(exp) / 100;
        exp_fill.transform.localScale = temp;
    }

    void initialize2() {
            // level,   exp,   total_coin,  
        PlayerPrefs.SetInt("level", 1);
        PlayerPrefs.SetInt("exp", 0);
        PlayerPrefs.SetInt("total_coin", 0);

        level = 1;

        initialize_popup.SetActive(true);
    }

    public void confirm_nickname() {
        nickname_input = GameObject.Find("InputField").gameObject.GetComponent<InputField>();
        PlayerPrefs.SetString("nickname", nickname_input.text);
        initialize_popup.SetActive(false);

        nickname = nickname_input.text;
        nickname_text.text = nickname;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
