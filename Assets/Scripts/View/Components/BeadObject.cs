using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeadObject : BoardObject 
{
    Color color;

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
        Vector3 normalized = transform.position - center;
        Vector3 reposition = new Vector3(
            Mathf.Cos(angle) * normalized.x - Mathf.Sin(angle) * normalized.y,
            Mathf.Sin(angle) * normalized.x + Mathf.Cos(angle) * normalized.y,
            0
        ) + center;

        transform.position = reposition;
    }
}
