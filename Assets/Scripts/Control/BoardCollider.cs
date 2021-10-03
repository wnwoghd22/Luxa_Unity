using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCollider : MonoBehaviour {
    //public float rotatespeed = 10f;
    //private float _startingPosition;
    private bool onTouch = false;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
           
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos)) {
                
            }
            
            switch (touch.phase) {
            case TouchPhase.Began:
                //_startingPosition = touch.position.x;
                break;
            case TouchPhase.Moved:
                // if (startingPosition > touch.position.x)
                // {
                //     transform.Rotate(Vector3.back, -turnspeed * Time.deltaTime);
                // }
                // else if (startingPosition < touch.position.x)
                // {
                //     transform.Rotate(Vector3.back, rotatespeed * Time.deltaTime);
                // }
                break;
            case TouchPhase.Ended:
                Debug.Log("Touch Phase Ended.");
                break;
            }
        }  
    }
}
