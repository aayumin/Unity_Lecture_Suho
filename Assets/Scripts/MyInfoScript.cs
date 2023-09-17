using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyInfoScript : MonoBehaviour
{
    Text avg1, avg2, avg3, cnt1, cnt2, cnt3;

    int[] cnt;
    float[] avg;

    // Start is called before the first frame update
    void Start()
    {
        cnt = new int[3];
        avg = new float[3];

        for (int i=0; i<3; i++) {
            cnt[i] = PlayerPrefs.GetInt("score_cnt_" + i.ToString(), 0);
            avg[i] = PlayerPrefs.GetFloat("score_avg_" + i.ToString(), 0);
        }


        avg1 = GameObject.Find("mode1_avg").GetComponent<Text>();
        avg2 = GameObject.Find("mode2_avg").GetComponent<Text>();
        avg3 = GameObject.Find("mode3_avg").GetComponent<Text>();

        
        cnt1 = GameObject.Find("mode1_cnt").GetComponent<Text>();
        cnt2 = GameObject.Find("mode2_cnt").GetComponent<Text>();
        cnt3 = GameObject.Find("mode3_cnt").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cnt[0] == 0) avg1.text = "-";
        else avg1.text = Mathf.Round(avg[0]).ToString();
        
        if (cnt[1] == 0) avg2.text = "-";
        else avg2.text = Mathf.Round(avg[1]).ToString();

        if (cnt[2] == 0) avg3.text = "-";
        else avg3.text = Mathf.Round(avg[2]).ToString();

        cnt1.text = cnt[0].ToString();
        cnt2.text = cnt[1].ToString();
        cnt3.text = cnt[2].ToString();

    }
}
