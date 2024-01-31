using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ChestView : MonoBehaviour, IChestView
{
    private Transform chestUp;
    [Inject]
    private IChestPresenter chestPresenter;

    // Start is called before the first frame update
    void Start()
    {
        var transforms = GetComponentsInChildren<Transform>();
       
        chestUp = Array.Find(transforms, (transform) => transform.name.Contains("Up"));

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenChest(bool open)
    {
        chestPresenter.OpenChest(open);
    }

    public void RotateChestUp(float angle)
    {
        chestUp.transform.Rotate(angle, 0f, 0f, Space.Self);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
