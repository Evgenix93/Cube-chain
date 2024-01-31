using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICubeView
{
    void Move(Vector3 newPos);

    void Push(Vector3 velocity);

    event Action OnCubePushed;

    void Merge(Vector3 position);

    event Action<ICubeView, int> OnContact;


    void ChangeMaterial(int number);

    GameObject GameObject();

    int GetNumber();

    void SetNumber(int number);

    void Contact(int instanceId);

    Vector3 GetPosition();

    void EnableCollider(bool enable);

    void ThrowUp(Vector3 force);

    event Action<Vector3, int> OnMergeFinished;

    void MergeFinished(int number);

    void Rotate(Transform target);

    void ChangeNumber(int number);

    Quaternion GetRotation();

    Transform GetTransform();

    event Action OnLineCross;

    void LineCross();

    int GetInstanceId();

    bool ToDestroy { get; set; }

    event Action<ICubeView> OnDestroyCube;

    bool IsMerging();

    void IsKinematic(bool isKinematic);
}
