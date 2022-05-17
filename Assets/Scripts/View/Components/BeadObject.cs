using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeadObject : BoardObject 
{
    Color color;
    private float angleOffset;
   
    private Vector3 originPos;
    private float originAngle;

    public void SetAngleOffset(Vector3 center, float angle)
    {
        originPos = transform.position;
        Vector3 v = originPos - center;
        //Debug.Log(originPos + " " + center + " " + v);
        originAngle = Mathf.Atan2(v.y, v.x);
        //originAngle = angle;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(int r, int g, int b)
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.color = new Color(r, g, b, 1);
    }

    public void UpdatePosition(float angle, Vector3 center)
    {
        //Debug.Log(angle + " " + originAngle + " " + center);
        //float theta = angle + angleOffset;
        float theta = angle;// + originAngle;
        Vector3 normalized = originPos - center;
        Vector3 reposition = new Vector3(
            Mathf.Cos(theta) * normalized.x - Mathf.Sin(theta) * normalized.y,
            Mathf.Sin(theta) * normalized.x + Mathf.Cos(theta) * normalized.y,
            0
        ) + center;

        transform.position = reposition;
    }
}
