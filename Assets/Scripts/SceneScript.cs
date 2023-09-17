using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{   

    // public static SceneScript scene_instance = null;

    // void Awake() {
    //     if (scene_instance == null) {
    //         scene_instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else {
    //         Destroy(gameObject);
    //     }

    // }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }  


    public void GoToMainScene() {
        SceneManager.LoadScene("MainScene");
    }

    
    public void GoToUsercene() {
        SceneManager.LoadScene("UserScene"); 
    }
    
    public void GoToModeSelectScene() {
        SceneManager.LoadScene("ModeSelectScene");
    }
    
    public void GoToMode1_Scene() {
        SceneManager.LoadScene("Mode1_Scene");
    }
    public void GoToMode2_Scene() {
        SceneManager.LoadScene("Mode2_Scene");
    }
    public void GoToMode3_Scene() {
        SceneManager.LoadScene("Mode3_Scene");
    }
    
    public void GoToResultScene() {
        SceneManager.LoadScene("ResultScene");
    }
    public void GoToMyInfoScene() {
        SceneManager.LoadScene("MyInfoScene");
    }
}
