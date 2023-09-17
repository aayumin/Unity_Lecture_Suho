using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphabetObjectPooling : MonoBehaviour
{

    int max_num_objs = 30;
    Queue<GameObject> q;

    public GameObject alphabet_prefab;

    public static AlphabetObjectPooling alpha_instance = null;

    void Awake() {
        if (alpha_instance == null){
            alpha_instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        q = new Queue<GameObject>();
        for (int i=0; i< max_num_objs; i++){
            GameObject obj = Instantiate(alphabet_prefab);
            PutObj(obj);

        }

    }



    public void PutObj(GameObject obj) {
        
        obj.SetActive(false);
        obj.transform.SetParent(gameObject.transform);
        q.Enqueue(obj);

    }

    public GameObject GetObj() {
        GameObject obj = q.Dequeue();
        obj.SetActive(true);
        obj.transform.SetParent(null);

        return obj;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
