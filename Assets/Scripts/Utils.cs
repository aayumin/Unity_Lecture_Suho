using System.Collections;
using System.Collections.Generic;
using System.IO; 
using UnityEngine;
using UnityEngine.UI;

public class Utils : MonoBehaviour
{   

    public AudioSource wrong_sound_effect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void play_wrong_sound_effect() {
        int will_play = PlayerPrefs.GetInt("soundeffect_play", 1);
        float volume = PlayerPrefs.GetFloat("soundeffect_volume", 1.0f);

        if (will_play == 1){
            wrong_sound_effect.Play();
            wrong_sound_effect.volume = volume;
        }
    }


    public string get_random_word() {

        //string path = Application.dataPath + "/Resources/words/words.txt";
        //StreamReader reader = new StreamReader(path);

        TextAsset textFile = Resources.Load("words/words") as TextAsset;
        StringReader reader = new StringReader(textFile.text);

        
        List<string> word_list = new List<string>();
        string temp;

        while ( (temp = reader.ReadLine()) != null) {
        //for (int i=0; i<lines.Length; i++){
            word_list.Add( temp.Trim() );
           // word_list.Add( lines[i].Trim() );
        }

        int idx = Random.Range(0,  word_list.Count);
        return word_list[idx];
    }

    public Vector3 resize_image(Sprite sprite, Image img) {
        float w = sprite.rect.size[0];
        float h = sprite.rect.size[1];

        if (w > h){
            h = h/w;
            w = 1.0f;
        }
        else {
            w = w/h;
            h = 1.0f;
        }

        return new Vector3(w, h, 1.0f);
    }

    public void Set_Alphabet_Image(GameObject obj, string alpha) {
        Sprite sp = Resources.Load<Sprite>("alphabet_" + alpha);
        obj.GetComponent<SpriteRenderer>().sprite = sp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
