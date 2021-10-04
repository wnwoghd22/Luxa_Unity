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
}
