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
        //HandleTouch();
        HandleClick();
    }
    private void HandleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 pos = new Vector2(mousePos.x, mousePos.y);
            Collider2D[] hits = Physics2D.OverlapPointAll(pos);

            if (hits.Length == 1)
            {    
                currentActivated = hits[0].gameObject.GetComponent<RingObject>();
                initialPos = pos;
                gm.SetRingActivate(currentActivated.Index);
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
                currentActivated.SetActive(false);
                Rotate();
            }
        }
    }
    private void HandleTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                    Vector2 pos = new Vector2(touchPos.x, touchPos.y);
                    Collider2D[] hits = Physics2D.OverlapPointAll(pos);

                    if (hits.Length == 1)
                    {
                        currentActivated = hits[0].gameObject.GetComponent<RingObject>();
                        initialPos = pos;
                        gm.SetRingActivate(currentActivated.Index);
                    }
                    break;
                case TouchPhase.Moved:
                    break;
                case TouchPhase.Ended:
                    touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                    pos = new Vector2(touchPos.x, touchPos.y);

                    if (currentActivated)
                    {
                        endPos = pos;
                        currentActivated.SetAlpha(0.3f);
                        currentActivated.SetActive(false);
                        Rotate();
                    }
                    break;
            }
        }
    }

    private void Rotate()
    {
        int _index = currentActivated.Index;
        Vector2 center = currentActivated.gameObject.transform.position;
        //Vector2 center = new Vector2(currentActivated.posX, currentActivated.posY);
        initialPos -= center; endPos -= center; //normalize
        currentActivated = null;

        float zeta = initialPos.x * endPos.y - initialPos.y * endPos.x ;
        //Debug.Log(initialPos + " " + endPos + center + zeta);
        //if (Mathf.Abs(zeta) < 1f) return;

        gm.BoardUpdate(_index, zeta);
    }
}
