using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICubeModel
{
    bool IsPushed { get; set; }

    int Number { get; set; }

    bool IsMerging { get; set; }

    public Vector3 Offset { get; set; }

    public bool IsStarted { get; set; }

    public Vector3 CubePosition { get; set; }

    public bool IsKinematic { get; set; }
}
