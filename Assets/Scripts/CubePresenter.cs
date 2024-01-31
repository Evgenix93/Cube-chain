using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class CubePresenter: ICubePresenter
{
    [Inject]
    private ICubeView cubeView;

    [Inject]
    private ICubeModel cubeModel;

  

    public void OnMouseButtonHolding(Vector3 mousePos)
    {
       
        if (!cubeModel.IsPushed)
        {
            if (!cubeModel.IsStarted)
            {
               cubeModel.Offset = mousePos - cubeModel.CubePosition;
                cubeModel.IsStarted = true;
                return;
            }
            mousePos -= cubeModel.Offset;
            mousePos.x *= 2.8f;
            if (mousePos.x < -5.2f)
                mousePos.x = -5.2f;
            if (mousePos.x > 5.2f)
                mousePos.x = 5.2f;
            
            var newPos = new Vector3(mousePos.x, cubeModel.CubePosition.y, cubeModel.CubePosition.z);
            cubeView.Move(newPos);
        }
    }

    public void OnUpdate(Vector3 position)
    {
        cubeModel.CubePosition = position;
    }

    public void OnMouseButtonUp()
    {
        if (!cubeModel.IsPushed)
        {
            cubeView.Push(Vector3.forward * 30f);
            cubeModel.IsPushed = true;
        }
    }

    public void OnCollision(Collision collision)
    {
       
        if (collision.gameObject.name.Contains("Cube"))
        {
            if(!cubeModel.IsMerging)
                cubeView.Contact(collision.gameObject.GetInstanceID());
            
        }
         
    }

   public void OnTriggerEnter(Vector3 velocity)
    {
        if (velocity.z < 0)
            cubeView.LineCross();
    }

    public int GetNumber()
    {
        return cubeModel.Number;
    }

  public  bool IsMerging()
    {
        return cubeModel.IsMerging;
    }

    public void IsKinemaic(bool isKinematic)
    {
        cubeModel.IsKinematic = isKinematic;
    }

    public void SetNumber()
    {
        cubeModel.Number *= 2;
    }

    public void OnStart()
    {
        cubeView.ChangeMaterial(cubeModel.Number);
        cubeView.ChangeNumber(cubeModel.Number);
    }

    public void OnMergeFinished()
    {
       // cubeModel.Number *= 2;
        cubeView.ChangeMaterial(cubeModel.Number);
        cubeView.ChangeNumber(cubeModel.Number);
        float xForce = Random.Range(-5f, 5f);
        float yForce = Random.Range(5f, 15f);
        float zForce = Random.Range(5, 15f);
        cubeView.EnableCollider(true);
        cubeView.ThrowUp(new Vector3(xForce, yForce, zForce));
        cubeView.MergeFinished(cubeModel.Number);
        cubeModel.IsMerging = false;
    }

    public void IsMerging(bool merging)
    {
        cubeModel.IsMerging = merging;
    }
}
