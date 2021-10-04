using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingCollider : MonoBehaviour {
    [SerializeField]
    private RingObject ring;
    public RingObject Ring => ring;

    void Start()
    {

    }

    void Update()
    {

    }

    public void SetRingAlpha(float f) => ring.SetAlpha(f);
}
