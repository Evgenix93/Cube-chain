using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineColliderView : MonoBehaviour, ILineColliderView
{

    public event Action OnLineCross;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        OnLineCross.Invoke();
    }
}
