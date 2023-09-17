using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutScript : MonoBehaviour
{

    bool start_fadeout = false;
    float fadeout_time = 0.7f;
    float elapsed_time = 0f;

    Color color;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable() {
        start_fadeout = true;
        elapsed_time = 0f;
        if (gameObject.GetComponent<Image>() != null) {
            color = gameObject.GetComponent<Image>().color;
        }
        else if (gameObject.GetComponent<Text>() != null) {
            color = gameObject.GetComponent<Text>().color;
        }
    }

    // Update is called once per frame
    void Update()
    {   // Image
        // Text

        if (start_fadeout) {
            elapsed_time += Time.deltaTime;
            color.a = Mathf.Clamp((fadeout_time - elapsed_time) / fadeout_time, 0f, 1f);
            
            if (gameObject.GetComponent<Image>() != null) {
                gameObject.GetComponent<Image>().color = color;
            }
            else if (gameObject.GetComponent<Text>() != null) {
                gameObject.GetComponent<Text>().color = color;
            }
        }

        if (elapsed_time >= fadeout_time) {
            start_fadeout = false;
            gameObject.SetActive(false);
        }
        
    }
}
