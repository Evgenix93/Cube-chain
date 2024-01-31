using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CubeModel: ICubeModel
{
    public bool IsPushed { get; set; }

    public bool IsMerging { get; set; }

    public int Number { get; set; }

    public Vector3 Offset { get; set; }

    public bool IsStarted { get; set; }

    public Vector3 CubePosition { get; set; } 

    public bool IsKinematic { get; set; }

    public CubeModel(int number)
    {
        Number = number;
        IsPushed = false;
        IsMerging = false;
        IsStarted = false;
        Offset = Vector3.zero;
        CubePosition = Vector3.zero;
        IsKinematic = false;
    }
}
