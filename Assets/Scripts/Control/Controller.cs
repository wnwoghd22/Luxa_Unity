using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    //public float rotatespeed = 10f;
    //private float _startingPosition;

    private bool onTouch = false;

    private RingObject currentActivated;

    private Vector2 initialPos;
    private Vector2 endPos;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        currentActivated = null;
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleTouch();
        HandleClick();
    }
    private void HandleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 pos = new Vector2(mousePos.x, mousePos.y);
            Collider2D[] hits = Physics2D.OverlapPointAll(pos);

            //foreach (Collider2D hit in hits)
            //    Debug.Log(hit);

            if (hits.Length == 1)
            {
                currentActivated = hits[0].gameObject.GetComponent<RingCollider>().Ring;
                Debug.Log(currentActivated.Index);
                currentActivated.SetAlpha(0.7f);
                initialPos = pos;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 pos = new Vector2(mousePos.x, mousePos.y);

            if (currentActivated)
            {
                endPos = pos;
                currentActivated.SetAlpha(0.3f);
                Rotate();
            }
        }
    }
    private void HandleTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
            {

            }

            switch (touch.phase)
            {
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

    private void Rotate()
    {
        int _index = currentActivated.Index;
        Vector2 center = new Vector2(currentActivated.posX, currentActivated.posY);
        initialPos -= center; endPos -= center; //normalize
        currentActivated = null;

        float zeta = initialPos.x * endPos.y - initialPos.y * endPos.y;
        Debug.Log(zeta);
        if (Mathf.Abs(zeta) < 1f) return;

        gm.ViewerUpdate(_index, zeta < 0);
    }
}
