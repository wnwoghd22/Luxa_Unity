using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    private RingObject currentActivated;

    private Vector2 initialPos;
    private Vector2 endPos;
    private Vector2 ringPos;
    private float angleOffset;

    private GameManager gm;

    private Camera _main;
    private RuntimePlatform platform;

    // Start is called before the first frame update
    void Start()
    {
        currentActivated = null;
        gm = FindObjectOfType<GameManager>();

        _main = Camera.main;
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
            Vector2 mousePos = _main.ScreenToWorldPoint(Input.mousePosition);
            SelectRing(mousePos);
            return;
        }
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = _main.ScreenToWorldPoint(Input.mousePosition);
            currentActivated?.HandleRotate(mousePos);
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePos = _main.ScreenToWorldPoint(Input.mousePosition);
            UnselectRing(mousePos);
            return;
        }
    }
    private void HandleTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = _main.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    SelectRing(touchPos);
                    break;
                case TouchPhase.Moved:
                    currentActivated?.HandleRotate(touchPos);
                    break;
                case TouchPhase.Ended:
                    UnselectRing(touchPos);
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
    private void SelectRing(Vector2 pos)
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(pos);

        if (hits.Length == 1)
        {
            currentActivated = hits[0].gameObject.GetComponent<RingObject>();
            ringPos = currentActivated.gameObject.transform.position;
            initialPos = pos - ringPos;
            gm.SetRingActivate(currentActivated.Index);
        }
    }
    private void UnselectRing(Vector2 pos)
    {
        if (currentActivated)
        {
            endPos = pos - ringPos;
            currentActivated.SetActive(false);
            Rotate();
        }
    }

    private void Rotate()
    {
        int _index = currentActivated.Index;
        endPos = endPos.normalized; initialPos = initialPos.normalized;

        Vector2 vec2 = endPos - initialPos;

        float angle = 2 * Mathf.Asin(vec2.magnitude / 2);

        float zeta = initialPos.x * endPos.y - initialPos.y * endPos.x;

        int rotateUnit = (int)((Mathf.Abs(angle) + Mathf.PI / 6.0f) / (Mathf.PI / 3.0f));

#if UNITY_EDITOR
        // Debug.Log(endPos + ", " + initialPos + ", " + vec2 + "\nmagnitude : " + vec2.magnitude + 
        //    "\nzeta : " + zeta + 
        //    "\nend : " + Mathf.Atan2(endPos.y, endPos.x) + ", initial : " + Mathf.Atan2(initialPos.y, initialPos.x) + 
        //    "\nangle : " + angle + ", count : " + rotateUnit);
#endif

        currentActivated = null;

        if (rotateUnit == 0) gm.UndoRotate(_index);
        while(rotateUnit-- > 0) gm.BoardUpdate(_index, zeta);
    }
}
