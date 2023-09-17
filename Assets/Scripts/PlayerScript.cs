using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerScript : MonoBehaviour
{

    float speed = 10.0f;
    float jump_power = 10.0f;

    bool is_on_ground = false;


    void Start() {



    }




    void Update() {

        // keyboard
        if (Input.GetKey(KeyCode.LeftArrow)) {
            Vector3 pos = transform.position;
            pos.x -= speed * Time.deltaTime;
            transform.position = pos;

            // flip X
        }
        else if (Input.GetKey(KeyCode.RightArrow)) {
            Vector3 pos = transform.position;
            pos.x += speed * Time.deltaTime;
            transform.position = pos;
        }


        if (Input.GetKey(KeyCode.Space) && is_on_ground) {
            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector3(0, 1, 0) * jump_power, ForceMode2D.Impulse);
            is_on_ground = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Ground") {
            is_on_ground = true;
        }
    }


}