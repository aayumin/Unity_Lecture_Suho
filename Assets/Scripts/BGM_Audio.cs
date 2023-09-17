using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Audio : MonoBehaviour
{
    // Start is called before the first frame update

    public static BGM_Audio bgm_instance;

    void Awake() {
        if (bgm_instance == null) {
            bgm_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
