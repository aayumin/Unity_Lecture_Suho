using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TEST_Script : MonoBehaviour
{
    public Toggle toggle;
    public Slider slider;
    AudioSource audioSource1, audioSource2;

    
    public Scrollbar bar;
    GameObject item_board;

    // Start is called before the first frame update
    void Start()
    {
        audioSource1 = GameObject.Find("sound_effect").GetComponent<AudioSource>();
        audioSource2 = GameObject.Find("bgm").GetComponent<AudioSource>();


        // audioSource2.Play();
        // audioSource2.loop = true;


        toggle.onValueChanged.AddListener(Play_bgm);
        slider.onValueChanged.AddListener(set_volume);

    }
    
    public void set_volume(float v){
        audioSource2.volume = v;
    }

    public void Play_bgm(bool flag) {
        if (flag)
            audioSource2.Play();
        else {
            audioSource2.Pause();
        }
    }


    public void Play_Effect() {
        audioSource1.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
