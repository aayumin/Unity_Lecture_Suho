using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mode2_PlayScript : MonoBehaviour
{
    Utils utilscript;
    Image word_img;

    public GameObject rendered_alphabet_prefab;
    public GameObject life_prefab;

    bool[] empty_positions;  // true:  need to ,   false:  None
    string target_word;
    int target_max_length;
    int target_fill_length;
 
    float empty_ratio = 0.3f;

    int num_life = 3;
    int coin_amount = 100;

    int num_complete_word = 0;
    int collected_coin = 0;

    int status = 0;  //   0:  none,   1:  target exists

    float elapsed_time = 0f;
    float next_obj_elapsed_time = 0f;


    List<string> wrong_alphabets, correct_alphabets;
    char[] all_alphabets;

    // map difficulty
    float throwing_speed;
    float throwing_interval;
    float obstacle_throwing_rate, coin_throwing_rate;
    float wrong_alphabet_throwing_rate;
    int throwing_angle_level;

    



    public void DoGameOver() {  
        // set word data
        PlayerPrefs.SetInt("complete_words", num_complete_word);

        // set coin data
        //int total_coin = PlayerPrefs.GetInt("total_coin", 0);
        //PlayerPrefs.SetInt("total_coin", total_coin + collected_coin);
        PlayerPrefs.SetInt("new_coin", collected_coin);

        PlayerPrefs.SetFloat("elapsed_time", elapsed_time);


        GameObject.Find("scriptObject").GetComponent<SceneScript>().GoToResultScene();
    }

    // Start is called before the first frame update
    void Start()
    {   

        PlayerPrefs.SetString("mode_name", "Mode 2");

        utilscript = GameObject.Find("scriptObject").GetComponent<Utils>();
        word_img = GameObject.Find("word_img").GetComponent<Image>();
        status = 0;

        int temp_i=0;
        all_alphabets = new char[26]; 
        for (char c = 'a'; c <= 'z'; c++) {
            all_alphabets[temp_i] = c;
            temp_i++;
        } 
        
        show_life();


        int speed_idx = PlayerPrefs.GetInt("map_speed_level", 2);
        int delay_idx = PlayerPrefs.GetInt("map_delay_level", 2);
        int obstacle_idx = PlayerPrefs.GetInt("map_obstacle_level", 2);
        int angle_idx = PlayerPrefs.GetInt("map_angle_level", 2);

        float[] arr_throwing_speed = {3.0f, 3.5f, 4.5f, 5.5f, 6.0f};
        float[] arr_throwing_interval = {2.5f, 2.0f, 1.5f, 1.0f, 0.5f};
        float[] arr_obstacle_throwing_rate = {0.05f, 0.1f, 0.2f, 0.3f, 0.4f};
        int[] arr_throwing_angle_level = {1, 2, 2, 3, 3};


        // parameter initialize
        throwing_speed = arr_throwing_speed[speed_idx];
        throwing_interval = arr_throwing_interval[delay_idx];
        obstacle_throwing_rate = arr_obstacle_throwing_rate[obstacle_idx];
        throwing_angle_level = arr_throwing_angle_level[angle_idx]; // 1: straight from upper , 2: (pre)+various angles from upper, 3: (pre) + arc-shape thrown from side
        coin_throwing_rate = 0.25f;
        wrong_alphabet_throwing_rate = 0.5f;   //  0f ~ 1f

        Debug.Log(throwing_speed.ToString() + ", " + throwing_interval.ToString() + ", " + obstacle_throwing_rate.ToString() + ", " + throwing_angle_level.ToString());

    }

    // Update is called once per frame
    void Update()
    {
        if (status == 0 ){
            assign_new_word();
            destroy_rendered_word();
            render_target_word();
            status = 1;
        }

        elapsed_time += Time.deltaTime;
        next_obj_elapsed_time += Time.deltaTime;
        if (next_obj_elapsed_time >= throwing_interval) {
            next_obj_elapsed_time = 0f;
            throwing_obj();
        }

        GameObject.Find("coin").gameObject.GetComponent<Text>().text = collected_coin.ToString();
        GameObject.Find("num_words").gameObject.GetComponent<Text>().text = num_complete_word.ToString() + " words complete";


        if (num_life <= 0) DoGameOver();

    }


    void throwing_obj() {
        // what obj   (alphabet,   coin,  obstacle)
        // TAG
        GameObject obj = AlphabetObjectPooling.alpha_instance.GetObj();

        float select_obj_rate = Random.Range(0f, 1f);
        if (select_obj_rate < obstacle_throwing_rate) {  // obstacle
            obj.gameObject.tag = "obstacle";
            obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("obstacle");
            
        }
        else if (select_obj_rate < obstacle_throwing_rate + coin_throwing_rate) {  // coin
            obj.gameObject.tag = "coin";
            obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("coin");
        }
        else { // alphabet
            select_obj_rate = Random.Range(0f, 1f);
            if (select_obj_rate < wrong_alphabet_throwing_rate) {   // wrong
                int idx = Random.Range(0, wrong_alphabets.Count);
                obj.gameObject.tag = "wrong";
                obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("alphabet_" + wrong_alphabets[idx]);
                obj.GetComponent<ThrowingObjScript>().set_alphabet(wrong_alphabets[idx]);
            }
            else { // correct

                int idx = Random.Range(0, correct_alphabets.Count);
                obj.gameObject.tag = "correct";
                obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("alphabet_" + correct_alphabets[idx]);
                obj.GetComponent<ThrowingObjScript>().set_alphabet(correct_alphabets[idx]);
            }
        }

        // angle.
        float xpos = Random.Range(-8.5f, 8.5f);
        //obj.transform.position = new Vector3(xpos, 7f, 0f);
        obj.GetComponent<ThrowingObjScript>().set_position(new Vector3(xpos, 7f, 0f));
        obj.GetComponent<ThrowingObjScript>().set_angle(throwing_angle_level);
        obj.GetComponent<ThrowingObjScript>().set_speed(throwing_speed);
        obj.GetComponent<ThrowingObjScript>().initialize();
     
    }

    void show_life(){
        // 3개 ..    처음 위치.   간격.
        
        float x_interval = 0.5f;

        foreach(Transform child in GameObject.Find("rendering_life").transform){
            Destroy(child.gameObject);
        }

        for (int i=0; i< num_life; i++){
            GameObject temp = Instantiate(life_prefab);
            temp.transform.SetParent(GameObject.Find("rendering_life").transform);
            temp.transform.localPosition = new Vector3(i * x_interval, 0f, 0f );
        }

    }

    public void handle_coin(){
        collected_coin += coin_amount;   
    }

    public void handle_obstacle() {
        num_life -= 1;
        show_life();
    }


    public void handle_wrong_alphabet(string c) {
        num_life -= 1;
        show_life();
    }

    public void handle_correct_alphabet(string c) {
        if (correct_alphabets.Contains(c.ToString()))
            correct_alphabets.Remove(c.ToString());
        if (wrong_alphabets.Contains(c.ToString()) == false)
            wrong_alphabets.Add(c.ToString());

        // rendering
        for (int i=0; i<target_word.Length; i++){
            if (target_word[i].ToString() == c) {
                empty_positions[i] = false;
            }
        }


        
        if (check_word_completion()){
            assign_new_word();
            num_complete_word += 1;
        }

        destroy_rendered_word();
        render_target_word();

    }

    bool check_word_completion(){
        bool complete = true;
        for (int i=0; i<target_word.Length; i++){
            if (empty_positions[i]) {
                complete = false;
                break;
            }
        }
        
        return complete;
    }

    void destroy_rendered_word() {
        foreach(Transform child in GameObject.Find("rendering_word").transform){
            Destroy(child.gameObject);
        }
    }

    void render_word_idx(int idx) {
        // TODO
    }

    void render_target_word() {
        GameObject temp;
        GameObject rendering_group = GameObject.Find("rendering_word").gameObject;
        
        for (int i=0; i< target_max_length; i++){
            // temp = AlphabetObjectPooling.alpha_instance.GetObj();
            temp = Instantiate(rendered_alphabet_prefab);
            

            if (empty_positions[i] == true){
                temp.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("empty_alphabet");;
            }
            else {
                temp.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("alphabet_" + target_word[i].ToString() );   // target_word[i]
            }
            


            temp.transform.SetParent(rendering_group.transform);
            temp.transform.localPosition = new Vector3(i * 1.0f, 0f, 0f);

        }

        word_img.sprite = Resources.Load<Sprite>("words/" + target_word);
        Vector3 temp_scale = utilscript.resize_image(Resources.Load<Sprite>("words/" + target_word), word_img);
        word_img.transform.localScale = temp_scale;

    }

    void assign_new_word() {
        target_word = GameObject.Find("scriptObject").GetComponent<Utils>().get_random_word();
        target_max_length = target_word.Length;


        // initialize
        empty_positions = new bool[target_max_length];
        for (int i=0; i<target_max_length; i++){
            empty_positions[i] = false;
        }
        
        // empty positions
        target_fill_length = 0;

        while(target_fill_length < target_max_length * empty_ratio){
            int idx;
            
            while(true) {
                idx = Random.Range(0, target_max_length);
                if (empty_positions[idx] == false) {
                    empty_positions[idx] = true;
                    break;
                }
            }            
            target_fill_length += 1;
        }


        // alphabet list initialize
        wrong_alphabets = new List<string>();
        correct_alphabets = new List<string>();
        for (int i=0; i< all_alphabets.Length; i++){

            bool empty_alpha = false;
            for (int j = 0; j<target_word.Length; j++){
                if (target_word[j] == all_alphabets[i] && empty_positions[j] == true) {
                    empty_alpha = true;
                    correct_alphabets.Add(all_alphabets[i].ToString());
                    break;
                }
            }

            if (empty_alpha == false) {
                wrong_alphabets.Add(all_alphabets[i].ToString());
            }
        }
        


        
    }


}
