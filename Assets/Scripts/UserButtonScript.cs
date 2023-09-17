using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserButtonScript : MonoBehaviour
{
    // 
    public bool is_select = false;
    UserManagerScript userManagerScript;
    string name;
    string level;


    public void ClickButton() {
        userManagerScript.HideAllBorder();
 
        if (is_select == false) {
            userManagerScript.SetSelectedName(name);
            userManagerScript.SetSelectedLevel(level);
            userManagerScript.is_selected = true;
            is_select = true;
            ShowBorder();
        }
        else {
            userManagerScript.is_selected = false;
            is_select = false;
            HideBorder();
        }
    }

    public void ShowBorder() {
        gameObject.transform.Find("border").gameObject.SetActive(true);
    }

    public void HideBorder() {
        gameObject.transform.Find("border").gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start() 
    {

        userManagerScript = GameObject.Find("ScriptObject").GetComponent<UserManagerScript>();
        name = gameObject.transform.Find("nickname").gameObject.GetComponent<Text>().text;
        level = gameObject.transform.Find("level").gameObject.GetComponent<Text>().text;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
