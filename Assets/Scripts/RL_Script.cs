using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RL_Script : MonoBehaviour
{
    // RL parameters
    float gamma = 0.9f;
    int num_improve = 10;
    int num_eval_per_improve = 10;  // 30

    float epsilon;
    float epsilon_discount_factor = 0.99f;

    float collision_cost = -50f;

    int num_states = 4;
    public int num_state_levels = 5;
    public int num_directions;

    public float[,,,] value;
    public int[,,,] policy;
    public float[,,,] reward;
    public int[,,,] iter_cnt;

    public static RL_Script instance = null;

    string base_path;
    
    void Awake() // DontDestroyOnLoad.   Singleton
    {
        if (instance == null) 
            instance = this; 
        else if (instance != this) 
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject); 
    }


    void initialize() {
        epsilon = 0.95f;


        // initialize
        for (int i=0; i<num_state_levels; i++){
            for (int j=0; j<num_state_levels; j++){
                for (int k=0; k<num_state_levels; k++){
                    for (int l=0; l<num_state_levels; l++){
                        value[i,j,k,l] = 0f;
                        reward[i,j,k,l] = 0f;
                        policy[i,j,k,l] = Random.Range(1, num_directions + 1);
                        iter_cnt[i,j,k,l] = 0;
                    }
                }
            }
        }
    }

    void load_data() {

        string data_path;
        string temp;
        string[] value_list;
        StreamReader reader;

        // value
        //data_path = base_path + "/value.txt";
        //reader = new StreamReader(data_path);
        
        for (int i=0; i<num_state_levels; i++){
            for (int j=0; j<num_state_levels; j++){
                //temp = reader.ReadLine();
                for (int k=0; k<num_state_levels; k++){
                    
                    //temp = reader.ReadLine();
                    //value_list = temp.Split(',');

                    for (int l=0; l<num_state_levels; l++){
                        //value[i,j,k,l] = float.Parse(value_list[l]); //, CultureInfo.InvariantCulture.NumberFormat);
                        value[i,j,k,l] = PlayerPrefs.GetFloat("value" + i.ToString() + j.ToString() + k.ToString() + l.ToString(), 0f);
                    }
                }
            }
        }


        // reward
        //data_path = base_path + "/reward.txt";
        //reader = new StreamReader(data_path);
        
        for (int i=0; i<num_state_levels; i++){
            for (int j=0; j<num_state_levels; j++){
                //temp = reader.ReadLine();
                for (int k=0; k<num_state_levels; k++){
                    
                    //temp = reader.ReadLine();
                    //value_list = temp.Split(',');
                    for (int l=0; l<num_state_levels; l++){
                        //reward[i,j,k,l] = float.Parse(value_list[l]); //, CultureInfo.InvariantCulture.NumberFormat);
                        reward[i,j,k,l] = PlayerPrefs.GetFloat("reward" + i.ToString() + j.ToString() + k.ToString() + l.ToString(), 0f);
                    }
                }
            }
        }



        // policy
        //data_path = base_path + "/policy.txt";
        //reader = new StreamReader(data_path);
        
        for (int i=0; i<num_state_levels; i++){
            for (int j=0; j<num_state_levels; j++){
                //temp = reader.ReadLine();
                for (int k=0; k<num_state_levels; k++){
                    
                    //temp = reader.ReadLine();
                    //value_list = temp.Split(',');
                    for (int l=0; l<num_state_levels; l++){
                        //policy[i,j,k,l] = int.Parse(value_list[l]); //, CultureInfo.InvariantCulture.NumberFormat);
                        policy[i,j,k,l] = PlayerPrefs.GetInt("policy" + i.ToString() + j.ToString() + k.ToString() + l.ToString(), 3);
                    }
                }
            }
        }

        // iter_cnt
        //data_path = base_path + "/iter_cnt.txt";
        //reader = new StreamReader(data_path);
         
        for (int i=0; i<num_state_levels; i++){
            for (int j=0; j<num_state_levels; j++){
                //temp = reader.ReadLine();
                for (int k=0; k<num_state_levels; k++){
                    
                    //temp = reader.ReadLine();
                    //value_list = temp.Split(',');
                    for (int l=0; l<num_state_levels; l++){
                        //iter_cnt[i,j,k,l] = int.Parse(value_list[l]); //, CultureInfo.InvariantCulture.NumberFormat);
                        iter_cnt[i,j,k,l] = PlayerPrefs.GetInt("iter_cnt" + i.ToString() + j.ToString() + k.ToString() + l.ToString(), 0);
                    }
                }
            }
        }




    }

    public void save_data() {
        string data_path;
        string data = "";
        StreamWriter writer;

        
        // value
        //data_path = base_path + "/value.txt";
        //writer = File.CreateText(data_path);
        for (int i=0; i<num_state_levels; i++){
            for (int j=0; j<num_state_levels; j++){
                data += i.ToString() + "," + j.ToString() + "\n"; 

                for (int k=0; k<num_state_levels; k++){
                    for (int l=0; l<num_state_levels; l++){
                        //data += value[i,j,k,l].ToString("F1") + ",";
                        PlayerPrefs.SetFloat("value" + i.ToString() + j.ToString() + k.ToString() + l.ToString(), value[i,j,k,l]);
                    }
                    data += "\n";
                }
            }
        }
        //writer.WriteLine(data);
        //writer.Close();

        // reward
        data = "";
        //data_path = base_path + "/reward.txt";
        //writer = File.CreateText(data_path);
        for (int i=0; i<num_state_levels; i++){
            for (int j=0; j<num_state_levels; j++){
                data += i.ToString() + "," + j.ToString() + "\n"; 

                for (int k=0; k<num_state_levels; k++){
                    for (int l=0; l<num_state_levels; l++){
                        //data += reward[i,j,k,l].ToString("F1") + ",";
                        PlayerPrefs.SetFloat("reward" + i.ToString() + j.ToString() + k.ToString() + l.ToString(), reward[i,j,k,l]);
                    }
                    data += "\n";
                }
            }
        }
        //writer.WriteLine(data);
        //writer.Close();

        // policy
        data = "";
        //data_path = base_path + "/policy.txt";
        //writer = File.CreateText(data_path);
        for (int i=0; i<num_state_levels; i++){
            for (int j=0; j<num_state_levels; j++){
                data += i.ToString() + "," + j.ToString() + "\n"; 

                for (int k=0; k<num_state_levels; k++){
                    for (int l=0; l<num_state_levels; l++){
                        //data += policy[i,j,k,l].ToString() + ",";
                        PlayerPrefs.SetInt("policy" + i.ToString() + j.ToString() + k.ToString() + l.ToString(), policy[i,j,k,l]);
                    }
                    data += "\n";
                }
            }
        }
        //writer.WriteLine(data);
        //writer.Close();

        // iter_cnt
        data = "";
        //data_path = base_path + "/iter_cnt.txt";
        //writer = File.CreateText(data_path);
        for (int i=0; i<num_state_levels; i++){
            for (int j=0; j<num_state_levels; j++){
                //data += i.ToString() + "," + j.ToString() + "\n"; 

                for (int k=0; k<num_state_levels; k++){
                    for (int l=0; l<num_state_levels; l++){
                        //data += iter_cnt[i,j,k,l].ToString() + ",";
                        PlayerPrefs.SetInt("iter_cnt" + i.ToString() + j.ToString() + k.ToString() + l.ToString(), iter_cnt[i,j,k,l]);
                    }
                    data += "\n";
                }
            }
        }
        //writer.WriteLine(data);
        //writer.Close();

    }

    // Start is called before the first frame update
    void Start()
    {
        num_directions = 2 * num_states;

        value = new float[num_state_levels, num_state_levels, num_state_levels, num_state_levels];
        reward = new float[num_state_levels, num_state_levels, num_state_levels, num_state_levels];
        policy = new int[num_state_levels, num_state_levels, num_state_levels, num_state_levels];
        iter_cnt = new int[num_state_levels, num_state_levels, num_state_levels, num_state_levels];
        
        
        //base_path = Application.dataPath + "/data";
        int exist_data = PlayerPrefs.GetInt("exist_data", 0);
        //if (File.Exists(base_path + "/value.txt")) {
        if (exist_data == 1) {
            load_data();
        }
        else {
            initialize();
            save_data();
            PlayerPrefs.SetInt("exist_data", 1);
        }


    }


    // Update is called once per frame
    void Update()
    {
        //policy_iteration(policy, value, reward);
    }

    // void policy_iteration(int[,,] policy, float[,,] value, float[,,] reward) {  //  call by reference  
    public void policy_iteration() { 
        // initialize
        float[,,,] new_value = new float[num_state_levels, num_state_levels, num_state_levels, num_state_levels];
        for (int i=0; i<num_state_levels; i++){
            for (int j=0; j<num_state_levels; j++){
                for (int k=0; k<num_state_levels; k++){
                    for (int l=0; l<num_state_levels; l++){
                        new_value[i,j,k, l] = 0f;
                    }
                }
            }
        }

        // iteration
        for (int aa=0; aa<num_improve; aa++){
            for(int bb=0; bb<num_eval_per_improve; bb++){
                new_value = policy_eval(policy, value, reward);
                
                // value = new_value;
                for (int i=0; i<num_state_levels; i++){
                    for (int j=0; j<num_state_levels; j++){
                        for (int k=0; k<num_state_levels; k++){
                            for (int l=0; l<num_state_levels; l++){

                                value[i,j,k,l] = new_value[i,j,k,l];
                            }
                        }
                    }
                }
            }
            policy = policy_improve(value, reward);
        }

    }

    bool check_collision(int[] state, int direction) {
        if (direction == 1 && state[0] == num_state_levels -1) return true;
        else if (direction == 2 && state[0] == 0) return true;
        else if (direction == 3 && state[1] == num_state_levels -1) return true;
        else if (direction == 4 && state[1] == 0) return true;
        else if (direction == 5 && state[2] == num_state_levels -1) return true;
        else if (direction == 6 && state[2] == 0) return true;
        else if (direction == 7 && state[3] == num_state_levels -1) return true;
        else if (direction == 8 && state[3] == 0) return true;

        return false;
    }

    int[] update_state(int[] state, int direction) {
        int[] next_state = new int[num_states];
        
        for (int i=0; i<num_states; i++){
            next_state[i] = state[i];
        }

        switch(direction){
            case 1:
                next_state[0] += 1;
                break;
            case 2:
                next_state[0] -= 1;
                break;
            case 3:
                next_state[1] += 1;
                break;
            case 4:
                next_state[1] -= 1;
                break;
            case 5:
                next_state[2] += 1;
                break;
            case 6:
                next_state[2] -= 1;
                break;
            case 7:
                next_state[3] += 1;
                break;
            case 8:
                next_state[3] -= 1;
                break;
        }



        for (int i=0; i<num_states; i++){
            next_state[i] = Mathf.Clamp(next_state[i], 0, num_state_levels - 1);
        }
        return next_state;
    }

    public void calcul_next_state(int map_speed_level, int map_delay_level, int map_obstacle_level, int map_angle_level){ // TODO
        /*****
        Input: current state
        Method:  epsilon-greedy. balance between <explore-exploit>
        ******/

        // speed,  interval, obstacle,  angle_level

        int updated_policy = policy[map_speed_level, map_delay_level, map_obstacle_level, map_angle_level];
        int next_map_speed_level = map_speed_level;
        int next_map_delay_level = map_delay_level;
        int next_map_obstacle_level = map_obstacle_level;
        int next_map_angle_level = map_angle_level;

        float random_v = Random.Range(0.0f, 1.0f);
        epsilon = epsilon * epsilon_discount_factor;

        // remove
        Debug.Log("(initial) Policy: " + updated_policy.ToString());

       
        switch(updated_policy){
            case 1:
                next_map_speed_level += 1;
                break;
            case 2:
                next_map_speed_level -= 1;
                break;
            case 3:
                next_map_delay_level += 1;
                break;
            case 4:
                next_map_delay_level -= 1;
                break;
            case 5:
                next_map_obstacle_level += 1;
                break;
            case 6:
                next_map_obstacle_level -= 1;
                break;
            case 7:
                next_map_angle_level += 1;
                break;
            case 8:
                next_map_angle_level -= 1;
                break;
        }


        if (random_v < epsilon){
            // updated_policy = Random.Range(1, num_directions + 1);
            
            next_map_speed_level +=  -1 + 2 * Random.Range(0, 2);
            next_map_delay_level +=  -1 + 2 * Random.Range(0, 2);
            next_map_obstacle_level +=  -1 + 2 * Random.Range(0, 2);
            next_map_angle_level +=  -1 + 2 * Random.Range(0, 2);

        }



        next_map_speed_level = Mathf.Clamp(next_map_speed_level, 0, num_state_levels - 1);
        next_map_delay_level = Mathf.Clamp(next_map_delay_level, 0, num_state_levels - 1);
        next_map_obstacle_level = Mathf.Clamp(next_map_obstacle_level, 0, num_state_levels - 1);
        next_map_angle_level = Mathf.Clamp(next_map_angle_level, 0, num_state_levels - 1);

        // remove
        Debug.Log("Updated Policy: " + updated_policy.ToString());
        Debug.Log("epsilon: " + epsilon.ToString());
        Debug.Log("random_v: " + random_v.ToString());

        // remove
        //next_map_delay_level = 2;   // 이거만 일단 고정..

        // save
        PlayerPrefs.SetInt("map_speed_level", next_map_speed_level);
        PlayerPrefs.SetInt("map_delay_level", next_map_delay_level);
        PlayerPrefs.SetInt("map_obstacle_level", next_map_obstacle_level);
        PlayerPrefs.SetInt("map_angle_level", next_map_angle_level);

    }

    float[,,,] policy_eval(int[,,,] policy, float[,,,] value, float[,,,] reward){
        float[,,,] new_value = new float[num_state_levels, num_state_levels, num_state_levels, num_state_levels];
        int[] next_state = new int[num_states];

        for (int i=0; i<num_state_levels; i++){
            for (int j=0; j<num_state_levels; j++){
                for (int k=0; k<num_state_levels; k++){
                    for (int l=0; l<num_state_levels; l++){
                        next_state = update_state(new int[4]{i,j,k,l}, policy[i,j,k,l]);
                        new_value[i,j,k,l] = reward[i,j,k,l] + gamma * value[next_state[0],next_state[1],next_state[2], next_state[3]];
                    }
                }
            }
        }

        return new_value;
    }

    int[,,,] policy_improve(float[,,,] value, float[,,,] reward){
        int[] next_state;
        int[,,,] new_policy = new int[num_state_levels, num_state_levels, num_state_levels, num_state_levels];
        float max_value;
        bool is_collision;
        

        for (int i=0; i<num_state_levels; i++){
            for (int j=0; j<num_state_levels; j++){
                for (int k=0; k<num_state_levels; k++){
                    for (int l=0; l<num_state_levels; l++){ 
                        max_value = -9999999999;
                        
                        for (int u=1; u<num_directions+1; u++){ 
                            next_state = update_state(new int[4]{i, j, k, l}, u);
                            is_collision = check_collision(new int[4]{i, j, k, l}, u);
                            
                            float v =  gamma * value[next_state[0],next_state[1],next_state[2], next_state[3]];
                            if (is_collision) v += collision_cost;


                            // remove
                            int n0 = next_state[0];
                            int n1 = next_state[1];
                            int n2 = next_state[2];
                            int n3 = next_state[3];

                            if (v > max_value) {
                                max_value = v;
                                new_policy[i,j,k,l] = u;
                            }
                        }

                    }
                }
            }
        }

        return new_policy;

    }


    public void print_value(int map_delay_level){  // not complete
        Debug.Log("================= VALUE ================\n");
        string str = "";
        for (int i=0; i<num_state_levels; i++){
            for (int j=0; j<num_state_levels; j++){
                str += value[i,j,map_delay_level,0].ToString()+ ", ";
            }
            str += "\n";
        }
        Debug.Log(str);
    }
    public void print_reward(int map_delay_level){  // not complete
        Debug.Log("================= REWARD ================\n");
        string str = "";
        for (int i=0; i<num_state_levels; i++){
            for (int j=0; j<num_state_levels; j++){
                str += reward[i,j,map_delay_level,0].ToString()+ ", ";
            }
            str += "\n";
        }
        Debug.Log(str);
    }
    public void print_policy(int map_delay_level){  // not complete
        Debug.Log("================= POLICY ================\n");
        string str = "";
        for (int i=0; i<num_state_levels; i++){
            for (int j=0; j<num_state_levels; j++){
                str += policy[i,j,map_delay_level,0].ToString()+ ", ";
            }
            str += "\n";
        }
        Debug.Log(str);
    }
}