using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ResultScript : MonoBehaviour
{
    Text mode, time, words, total_money, new_money, exp, level;

    int map_speed_level, map_delay_level, map_obstacle_level, map_angle_level;

    int difficulty_level = -1;

    int timev, wordsv, newmoneyv, totalmoneyv, expv, levelv;
    string mode_name;

    // Start is called before the first frame update
    void Start()
    {


        mode_name = PlayerPrefs.GetString("mode_name", "Mode 1");
        wordsv = PlayerPrefs.GetInt("complete_words", 0);
        newmoneyv = PlayerPrefs.GetInt("new_coin", 0);
        totalmoneyv = PlayerPrefs.GetInt("total_coin", 0);
        levelv = PlayerPrefs.GetInt("level", 1);
        timev = (int)(PlayerPrefs.GetFloat("elapsed_time", 0f));


        expv = PlayerPrefs.GetInt("exp", 0);
        expv += calculate_exp(wordsv, timev);   //   0 ~ 99
        if (expv >= 100){
            levelv +=  (int)(expv/100);
            expv = (expv % 100);
        }

        PlayerPrefs.SetInt("level", levelv);
        PlayerPrefs.SetInt("exp", expv);


        mode = GameObject.Find("mode").GetComponent<Text>();
        time = GameObject.Find("time").GetComponent<Text>();
        words = GameObject.Find("words").GetComponent<Text>();
        total_money = GameObject.Find("total_money").GetComponent<Text>();
        new_money = GameObject.Find("new_money").GetComponent<Text>();
        
        exp = GameObject.Find("exp").GetComponent<Text>();
        level = GameObject.Find("level").GetComponent<Text>();

        update_total_coin();

        // save data
        //UserManagerScript.user_instance.UpdateCurrentData();
        //UserManagerScript.user_instance.SaveJsonData();


        if (mode_name != "Mode 2") return;

        //////////////////////////
        //  RL Script           //
        //////////////////////////

        RL_Script _RLScript = GameObject.Find("RL_manager").GetComponent<RL_Script>();


        // 게임 결과에 따라 RL reward 업데이트.
        map_speed_level = PlayerPrefs.GetInt("map_speed_level", 2);  //  0,1,2,3,4
        map_delay_level = PlayerPrefs.GetInt("map_delay_level", 2);
        map_obstacle_level = PlayerPrefs.GetInt("map_obstacle_level", 2);
        map_angle_level = PlayerPrefs.GetInt("map_angle_level", 2);

        // 게임통계 업데이트
        // 0 ~ 16
        difficulty_level = (int)(((float)(map_speed_level + map_delay_level + map_obstacle_level + map_angle_level) / 16 ) * 3  );
        difficulty_level = Mathf.Clamp(difficulty_level, 0, 2);

        int temp_cnt = PlayerPrefs.GetInt("score_cnt_" + difficulty_level.ToString(), 0);
        float temp_avg = PlayerPrefs.GetFloat("score_avg_" + difficulty_level.ToString(), 0);

        PlayerPrefs.SetInt("score_cnt_" + difficulty_level.ToString(), temp_cnt + 1);
        PlayerPrefs.SetFloat("score_avg_" + difficulty_level.ToString(),  ((temp_avg * temp_cnt) +  calculate_exp(wordsv, timev) ) / (temp_cnt + 1)  );



        float prev_reward, next_reward;
        int total_simul_cnt; 
        
        // reward 계산
        float reward_from_score = 1000f / (expv + (float)1e-7);
        if (expv < 10) {
            reward_from_score = 0f;
        }
        reward_from_score = Mathf.Clamp(reward_from_score, 0f, 100f);


        total_simul_cnt = _RLScript.iter_cnt[map_speed_level, map_delay_level, map_obstacle_level, map_angle_level];
        prev_reward = _RLScript.reward[map_speed_level, map_delay_level, map_obstacle_level, map_angle_level];
        next_reward = (prev_reward * total_simul_cnt + reward_from_score) / (total_simul_cnt + 1);

        _RLScript.reward[map_speed_level, map_delay_level, map_obstacle_level, map_angle_level] = next_reward;
        _RLScript.iter_cnt[map_speed_level, map_delay_level, map_obstacle_level, map_angle_level] += 1;


        // policy iteration (policy eval + policy improve)
        _RLScript.policy_iteration();


        // new map parameter for next stage
        _RLScript.calcul_next_state(map_speed_level, map_delay_level, map_obstacle_level, map_angle_level);
        _RLScript.save_data();

        
        // save data
        //UserManagerScript.user_instance.UpdateCurrentData();
        //UserManagerScript.user_instance.SaveJsonData();


    } 

    public void update_total_coin() { 
        PlayerPrefs.SetInt("total_coin", newmoneyv + totalmoneyv);
    }

    int calculate_exp(int word_cnt, int elapsed_time) {
        return elapsed_time * 1 + word_cnt * 5;
    }

    // exp

    // Update is called once per frame
    void Update()
    {
        mode.text = mode_name;

        time.text = "Time: " + ((int)(timev/60)).ToString()  + "m " + (timev % 60).ToString() + "s";
        total_money.text = "$" + totalmoneyv.ToString();
        new_money.text = "+ " + newmoneyv.ToString();
        words.text = "Complete words: " + wordsv.ToString();
        
        exp.text = "exp : " + expv.ToString();
        level.text = "level: " + levelv.ToString();

    }
}
