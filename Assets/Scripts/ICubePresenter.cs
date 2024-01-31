using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICubePresenter
{
    void OnMouseButtonHolding(Vector3 mousePos);
    void OnMouseButtonUp();

    void OnCollision(Collision collision);

    void OnTriggerEnter(Vector3 velocity);

    int GetNumber();

    void SetNumber();

    void OnStart();

    void OnMergeFinished();

    void IsMerging(bool merging);

    bool IsMerging();

    void OnUpdate(Vector3 position);

    
}
