using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class UserManagerScript : MonoBehaviour
{

    public static UserManagerScript user_instance;

    public GameObject userRegisterWindow;
    public GameObject deleteUserWindow;
    public GameObject showEmpty;

    List<string> user_name_list;
    UserDataList user_data_list;

    public GameObject userObj;
    float userListHeight = 128f;

    GameObject scrollViewContent;


    public string selected_username;
    public string selected_userlevel;
    public bool is_selected = false;

    string jsonpath;

    void Awake() {
        if (user_instance == null) {
            user_instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        user_name_list = new List<string>();


        // test
        // for (int i=0; i<10; i++){
        //     AddUser("user_" + (i+1).ToString(), i+1);
        // }


        user_data_list = new UserDataList();
        jsonpath = Application.dataPath + "/data/userdata.json";

        //scrollViewContent = GameObject.Find("Content");
        Initialize();

        
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if (scene.name == "UserScene"){
            //scrollViewContent = GameObject.Find("Content");
            //Debug.Log(scrollViewContent);

            Initialize();
        }
    }

    void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void Initialize(){
        
        scrollViewContent = GameObject.Find("Content");
        Debug.Log(scrollViewContent);


        if (File.Exists(jsonpath)){
            LoadJsonData();
            ShowUserList();
        }
        else SaveJsonData();

        if (user_name_list.Count == 0) ShowEmptyText();
    }

    void ShowUserList() { 

        foreach(UserData user in user_data_list.users){

            GameObject obj = Instantiate(userObj);
            obj.transform.SetParent(scrollViewContent.transform);

            Text name_text = obj.transform.Find("nickname").gameObject.GetComponent<Text>();
            Text level_text = obj.transform.Find("level").gameObject.GetComponent<Text>();

            name_text.text = user.nickname;
            level_text.text = "LV." + user.level.ToString();

            // scroll view height
            Vector2 v = scrollViewContent.GetComponent<RectTransform>().sizeDelta;
            scrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(v[0], v[1] + userListHeight);

            // add to user_name_list
            user_name_list.Add(user.nickname);
        }

    }
 


    public void ShowEmptyText() {
        showEmpty.SetActive(true);
    }

    
    public void HideEmptyText() {
        showEmpty.SetActive(false);
    }


    public void PlaywithSelectedUser() { 
        if (is_selected) {
            PlayerPrefs.SetString("nickname", selected_username);


            UserData user = new UserData();  // TODO
            foreach(UserData tempuser in user_data_list.users){
                if (tempuser.nickname == selected_username){
                    user = tempuser;
                }
            }


            PlayerPrefs.SetInt("level", user.level);
            PlayerPrefs.SetInt("total_coin", user.coin);
            PlayerPrefs.SetInt("exp", user.exp);
            for (int i=0; i<3; i++){
                PlayerPrefs.SetInt("score_cnt_" + i.ToString(), user.score_cnt[i]);
                PlayerPrefs.SetFloat("score_avg_" + i.ToString(), user.score_avg[i]);
            }

            SceneManager.LoadScene("MainScene");
            // TODO
            // RL array
            //
        }

    }

    public void AddUser(string name, int level) {
        GameObject obj = Instantiate(userObj);
        obj.transform.SetParent(scrollViewContent.transform);

        Text name_text = obj.transform.Find("nickname").gameObject.GetComponent<Text>();
        Text level_text = obj.transform.Find("level").gameObject.GetComponent<Text>();

        name_text.text = name;
        level_text.text = "LV." + level.ToString();

        // scroll view height
        Vector2 v = scrollViewContent.GetComponent<RectTransform>().sizeDelta;
        scrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(v[0], v[1] + userListHeight);

        // add to user_name_list
        user_name_list.Add(name);

        // set Data
        user_data_list.AddUser(name, level);

        HideEmptyText();

        SaveJsonData();
    }


    public void LoadUserList() {
        // user_name_list
        // instantiate
    }


    public void OpenRegisterWindow() {
        userRegisterWindow.SetActive(true);
        userRegisterWindow.transform.Find("InputField").GetComponent<InputField>().text = "";

    }

    public void OpenDeleteWindow() {
        if (is_selected){
            deleteUserWindow.transform.Find("nickname").gameObject.GetComponent<Text>().text = selected_username;
            deleteUserWindow.transform.Find("level").gameObject.GetComponent<Text>().text = selected_userlevel;
            deleteUserWindow.SetActive(true);
        }
    }
    public void CloseDeleteWindow() {
        deleteUserWindow.SetActive(false);
    }
    public void CloseRegisterWindow() {
        userRegisterWindow.SetActive(false);
    }

    public void HideAllBorder() {
        foreach(Transform child in scrollViewContent.transform) {
            child.gameObject.GetComponent<UserButtonScript>().HideBorder();
        }
    }

    public void SetSelectedName(string name) {
        selected_username = name;
    }
    public void SetSelectedLevel(string level) {
        selected_userlevel = level;
    }


    public void HandleNameConfirm() {
        string name = userRegisterWindow.transform.Find("InputField").Find("text").GetComponent<Text>().text;
        if (name == "") return;

        if (isUniqueName(name)) {
            AddUser(name, 1);
            CloseRegisterWindow();
        }
        else {
            userRegisterWindow.transform.Find("duplicatedName").gameObject.SetActive(true);
            // fade-out   // TODO
        }
    }


    public bool isUniqueName(string name){
        if (user_name_list.Contains(name))
        return false;

        else return true;
    }

    // User.   nickname,  level, exp, coin.    
    // value,  reward.  //  ==>  json.

    public void UpdateCurrentData() {
        // current play user

        string nickname = PlayerPrefs.GetString("nickname", "");


        foreach(UserData user in user_data_list.users){
            if (user.nickname == nickname){
                user.coin = PlayerPrefs.GetInt("total_coin", user.coin);
                user.exp = PlayerPrefs.GetInt("exp", user.exp);
                user.level = PlayerPrefs.GetInt("level", user.level);
                for (int i=0; i<3; i++){
                    user.score_cnt[i] = PlayerPrefs.GetInt("score_cnt_" + i.ToString(), user.score_cnt[i]);
                    user.score_avg[i] = PlayerPrefs.GetFloat("score_avg_" + i.ToString(), user.score_avg[i]);
                }


                user.SetRLData(RL_Script.instance.value, RL_Script.instance.reward, RL_Script.instance.policy, RL_Script.instance.iter_cnt);

            }
        }   
    }


    // TODO
    // RL data save.
    // RL script initialize.  
    // password login.

    public void SaveJsonData() {
        string jsontext = JsonUtility.ToJson(user_data_list);
        File.WriteAllText(jsonpath, jsontext);
    }

    public void LoadJsonData() {
        string jsontext = File.ReadAllText(jsonpath);
        user_data_list = JsonUtility.FromJson<UserDataList>(jsontext);

        foreach(UserData user in user_data_list.users) {
            Debug.Log(user.nickname + ", " + user.level.ToString());
        }
    }

    public void DeleteUser() {
        foreach(Transform child in scrollViewContent.transform){
            Text nickname_text = child.Find("nickname").gameObject.GetComponent<Text>();
            if (nickname_text.text == selected_username) {
                
                Vector2 v = scrollViewContent.GetComponent<RectTransform>().sizeDelta;
                scrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(v[0], v[1] - userListHeight);


                child.transform.SetParent(null);
                Destroy(child.gameObject);
                break;
            }
        }

        CloseDeleteWindow();

        user_data_list.DeleteUser(selected_username);
        user_name_list.Remove(selected_username);
        if (user_name_list.Count == 0) ShowEmptyText();

        SaveJsonData();

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
