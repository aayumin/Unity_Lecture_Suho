using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mode1_PlayScript : MonoBehaviour
{

    //  complete words
    //  quit button

    public GameObject alphabet_prefab;
    
    Utils utilscript;

    Image word_img;

    bool[] empty_positions;  // true:  need to ,   false:  None
    string target_word;
    int target_max_length;
    int target_fill_length;
 
    float empty_ratio = 0.3f;

    int num_life = 6;

    int num_complete_word = 0;
    int status = 0;  //   0:  none,   1:  target exists

    float elapsed_time = 0f;
    float delay_after_gameover = 1f;
    float elapsed_time_after_gameover = 0f;
    bool is_gameover = false;

    List<string> wrong_alphabets, correct_alphabets;
    char[] all_alphabets;

    
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString("mode_name", "Mode 1");
        status = 0;

        int temp_i=0;
        all_alphabets = new char[26]; 
        for (char c = 'a'; c <= 'z'; c++) {
            all_alphabets[temp_i] = c;
            temp_i++;
        } 

        GameObject h_render = GameObject.Find("drawing_hangman").gameObject;
        h_render.GetComponent<SpriteRenderer>().sprite = null;

        word_img = GameObject.Find("word_img").GetComponent<Image>();
        utilscript = GameObject.Find("scriptObject").GetComponent<Utils>();


    }

    public void DoGameOver() {  
        // set word data
        PlayerPrefs.SetInt("complete_words", num_complete_word);

        // set coin data
        PlayerPrefs.SetInt("new_coin", 0);
        PlayerPrefs.SetFloat("elapsed_time", elapsed_time);

        is_gameover = true;
        //GameObject.Find("scriptObject").GetComponent<SceneScript>().GoToResultScene();
    }

    // Def_ni_e   ==>  (i, t)    //   a ~ z
    void rendering_alphabet_candidate() {
        // 26   ==>    9 * 3

        float x_interval = 0.4f;
        float y_interval = 0.5f;


        foreach(Transform child in GameObject.Find("rendering_alphabet").transform){
            Destroy(child.gameObject);
        }

        for (int i=0; i< all_alphabets.Length ; i++){
            int x_idx = i % 26;
            int y_idx = (int)(i / 26);
            GameObject temp = Instantiate(alphabet_prefab);
            temp.transform.SetParent(GameObject.Find("rendering_alphabet").transform);
            temp.transform.localPosition = new Vector3(x_idx * x_interval, y_idx * y_interval, 0f );

            temp.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("alphabet_" + all_alphabets[i].ToString() );   // target_word[i]
            temp.GetComponent<Alphabet_Info>().set_alphabet(all_alphabets[i].ToString());
        }
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
        GameObject.Find("num_words").gameObject.GetComponent<Text>().text = num_complete_word.ToString() + " words complete";

        if (num_life <= 0) DoGameOver();

        if (is_gameover) {
            elapsed_time_after_gameover += Time.deltaTime;
        }

        if(elapsed_time_after_gameover >= delay_after_gameover) {
            GameObject.Find("scriptObject").GetComponent<SceneScript>().GoToResultScene();
        }

    }
    

    public void click_alphabet(string c) {
        int is_paused = PlayerPrefs.GetInt("pause", 0);
        if (is_paused == 1) return;


        bool is_correct = false;
        for ( int i=0; i< correct_alphabets.Count; i++) {
            if (c == correct_alphabets[i]) {
                is_correct = true;
                break;
            }
        }

        if (is_correct) {
            handle_correct_alphabet(c);
        }
        else {
            num_life -= 1;
            num_life = Mathf.Max(num_life, 0);
            
            
            utilscript.play_wrong_sound_effect();

            GameObject h_render = GameObject.Find("drawing_hangman").gameObject;
            h_render.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("hangman"+ (6 - num_life).ToString());
            
        }


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


    void render_target_word() {
        GameObject temp;
        GameObject rendering_group = GameObject.Find("rendering_word").gameObject;
        
        for (int i=0; i< target_max_length; i++){
            // temp = AlphabetObjectPooling.alpha_instance.GetObj();
            temp = Instantiate(alphabet_prefab);

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
        target_word = utilscript.get_random_word();
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
        
        
        string show_target_word = "";
        for (int i=0; i<target_word.Length; i++){
            if (empty_positions[i]) show_target_word += "_";
            else show_target_word += target_word[i];
        }



        
    }
}
