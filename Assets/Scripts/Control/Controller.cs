using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    private RingObject currentActivated;

    private Vector3 initialPos;
    private Vector3 endPos;
    private Vector3 ringPos;
    private float angleOffset;

    private GameManager gm;

    private RuntimePlatform platform;

    // Start is called before the first frame update
    void Start()
    {
        currentActivated = null;
        gm = FindObjectOfType<GameManager>();

        platform = Application.platform;
    }

    // Update is called once per frame
    void Update()
    {
        switch (platform)
        {
            case RuntimePlatform.Android:
                HandleTouch();
                GetKeyForAndroid();
                break;
            case RuntimePlatform.WindowsEditor:
                HandleClick();
                break;
        }
    }
    private void HandleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] hits = Physics2D.OverlapPointAll(mousePos);

            if (hits.Length == 1)
            {    
                currentActivated = hits[0].gameObject.GetComponent<RingObject>();
                ringPos = hits[0].gameObject.transform.position;
                initialPos = mousePos;
                Vector3 vec = initialPos - ringPos;
                angleOffset = Mathf.Atan2(vec.y, vec.x);
                gm.SetRingActivate(currentActivated.Index);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (currentActivated)
            {
                endPos = mousePos;
                currentActivated.SetActive(false);
                Rotate();
            }
        }
        //else if (Input.GetMouseButton(0))
        //{
        //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector3 currentVec = mousePos - ringPos;
        //    float angle = Mathf.Atan2(currentVec.y, currentVec.x) - angleOffset;

        //    Debug.Log(angle);
        //    if (Mathf.Abs(angle) > Mathf.PI / 3) //over 60 deg
        //    {
        //        Debug.Log("over 60 deg");
        //        initialPos = mousePos;
        //        Vector3 vec = initialPos - ringPos;
        //        angleOffset = Mathf.Atan2(vec.y, vec.x);
        //        gm.HandleOver60deg(currentActivated.Index, angle > 0);
        //    }
        //}
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
    private void GetKeyForAndroid()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gm.HandleEscapeButton();
        }
        else if (Input.GetKeyDown(KeyCode.Home))
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.Menu))
        {

        }
    }

    private void Rotate()
    {
        int _index = currentActivated.Index;
        Vector3 center = currentActivated.gameObject.transform.position;
        initialPos -= center; endPos -= center; //normalize

        float angle = Mathf.Atan2(endPos.y, endPos.x) - Mathf.Atan2(initialPos.y, initialPos.x);

        //float zeta = initialPos.x * endPos.y - initialPos.y * endPos.x ;

        int rotateUnit = Mathf.Abs((int)(angle / (Mathf.PI / 3.0f)));
        Debug.Log("end : " + Mathf.Atan2(endPos.y, endPos.x) + ", initial : " + Mathf.Atan2(initialPos.y, initialPos.x) + " angle : " + angle + ", count : " + rotateUnit);

        currentActivated = null;

        while(rotateUnit-- > 0) gm.BoardUpdate(_index, angle);
    }
}
