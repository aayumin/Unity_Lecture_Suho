using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingObjScript : MonoBehaviour
{   

    float speed;
    int angle_level;
    string alphabet;

    // parameters
    float min_x = -8f, max_x = 8f;
    float ground_y, pos_x, pos_y, slope;

    bool thrown_on_left;
    float height,  y_velocity;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void initialize() {
        
        if (angle_level == 2){
            ground_y = GameObject.Find("ground").transform.position.y;
            pos_x = transform.position.x;
            pos_y = transform.position.y;
            slope = Random.Range( -1 * (pos_x - min_x)/(pos_y - ground_y)   ,   (max_x - pos_x)/(pos_y - ground_y)  );
        }
        else if (angle_level == 3){
            if (Random.Range(0, 2) == 1) thrown_on_left = true;
            else thrown_on_left = false;
            
            height = Random.Range(ground_y, 1.6f);
            y_velocity = Random.Range(0.5f, 4.0f);


            Vector3 pos = transform.position;
            pos.y = height;
            pos.x = (thrown_on_left == true) ? -10f : 10f ;
            transform.position = pos;

        }
    }

    // Update is called once per frame
    void Update()
    {
        //  level 1:

        if (angle_level == 1){
            Vector3 pos = transform.position;
            pos.y -= speed * Time.deltaTime;
            transform.position = pos;
        }
        else if (angle_level == 2){
            Vector3 pos = transform.position;
            pos.y -= speed * Time.deltaTime;
            pos.x += speed * slope * Time.deltaTime;
            transform.position = pos;


        }
        else { // angle_level == 3            
            Vector3 pos = transform.position;
            if (thrown_on_left) pos.x += speed * Time.deltaTime;
            else  pos.x -= speed * Time.deltaTime;
            pos.y += y_velocity * Time.deltaTime;
            transform.position = pos;

            y_velocity -= 0.15f;
        }

    }

    public void set_alphabet(string c) {
        alphabet = c;
    }


    public void set_speed(float v) {
        speed = v;
    }


    public void set_angle(int level) {
        angle_level = Random.Range(1, level + 1);
    }

    
    public void set_position(Vector3 pos) {
        transform.position = pos;
    }


    // collision
    //  ==>  Ground.     ==> Player
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Ground"){
            AlphabetObjectPooling.alpha_instance.PutObj(gameObject);
        }
        else if (other.tag == "Player") {

            if (gameObject.tag == "wrong")  // wrong
                GameObject.Find("scriptObject").GetComponent<Mode2_PlayScript>().handle_wrong_alphabet(alphabet);

            else if (gameObject.tag == "correct") {  // correct
                GameObject.Find("scriptObject").GetComponent<Mode2_PlayScript>().handle_correct_alphabet(alphabet);
            }
            else if (gameObject.tag == "obstacle"){
                GameObject.Find("scriptObject").GetComponent<Mode2_PlayScript>().handle_obstacle();
            }
            else if (gameObject.tag == "coin"){
                GameObject.Find("scriptObject").GetComponent<Mode2_PlayScript>().handle_coin();
            }

            AlphabetObjectPooling.alpha_instance.PutObj(gameObject);
        }
    }



}
