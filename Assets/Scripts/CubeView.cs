using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;

public class CubeView : MonoBehaviour, ICubeView
{
    [Inject]
    private ICubePresenter cubePresenter;
    private Rigidbody cubeRigidbody;
    public event Action OnCubePushed;
    public event Action<ICubeView, int> OnContact;
    public event Action<Vector3, int> OnMergeFinished;
    public event Action OnLineCross;
    public event Action<ICubeView> OnDestroyCube;
    
    public bool ToDestroy { get; set; } = false;

    private TextMeshProUGUI[] numberTexts;

    // Start is called before the first frame update
    void Start()
    {
        cubeRigidbody = GetComponent<Rigidbody>();
        numberTexts = GetComponentsInChildren<TextMeshProUGUI>();
        cubePresenter.OnStart();
       
    }

    // Update is called once per frame
    void Update()
    {
        cubePresenter.OnUpdate(transform.position);

        if (Input.GetMouseButton(0))
        {
            cubePresenter.OnMouseButtonHolding(Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 9f));
        }

        if (Input.GetMouseButtonUp(0))
        {
           
            cubePresenter.OnMouseButtonUp();
        }

    }

    public void Move(Vector3 newPos)
    {
        /* var position = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 9f);
         if (!isStarted)
         {
             offset = position - transform.position;
             isStarted = true;

             return;
         }


         position -= offset;

         transform.position = new Vector3(position.x, transform.position.y, transform.position.z);*/

        transform.position = newPos;

    }

    public bool IsMerging()
    {
        return cubePresenter.IsMerging();
    }

    public void LineCross()
    {
        OnLineCross.Invoke();
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Quaternion GetRotation()
    {
        return transform.rotation;
    }

    public void Push(Vector3 velocity)
    {
        cubeRigidbody.velocity = velocity;
        OnCubePushed?.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        cubePresenter.OnCollision(collision);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        cubePresenter.OnTriggerEnter(cubeRigidbody.velocity);
    }

    

    public void Merge(Vector3 position)
    {
        
        
        cubePresenter.IsMerging(true);
      transform.DOMove(position, 0.5f).OnComplete(() => cubePresenter.OnMergeFinished()).SetEase(Ease.Linear);
        cubePresenter.SetNumber();
        
    }

  

    public void EnableCollider(bool enable)
    {

        // GetComponent<Collider>().enabled = enable;
        cubePresenter.IsMerging(!enable);
        cubeRigidbody.isKinematic = !enable;
    }

   public void IsKinematic(bool isKinematic)
    {
        cubeRigidbody.velocity = Vector3.zero;
        cubeRigidbody.isKinematic = isKinematic;
        
    }

    public void Contact(int instanceId)
    {
        //Debug.Log("Contact number =" + cubePresenter.GetNumber());
        OnContact?.Invoke(this, instanceId);
    }

  public void ThrowUp(Vector3 force)
    {
        cubeRigidbody.isKinematic = false;
        cubeRigidbody.velocity = force;
    }

   private IEnumerator CheckPosition(Vector3 pos)
    {
        if (transform.position == pos)
        {
            Destroy(gameObject);

        }
        else
            yield return null;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        //OnDestroyCube.Invoke(this);
    }

   public int GetInstanceId()
    {
        return gameObject.GetInstanceID();
    }

    public void ChangeMaterial(int number)
    {
       
        switch (number)
        {
            case 2:
                {
                    GetComponent<MeshRenderer>().material.color = new Color(0f, 1f, 0f);

                    break;
                }

            case 4: {
                    GetComponent<MeshRenderer>().material.color = new Color(0f, 0f, 1f);

                    break;
                }
            case 8:
                {
                    GetComponent<MeshRenderer>().material.color = new Color(0.8f, 0.8f, 0f);
                    break;
                }
            case 16:
                {
                    GetComponent<MeshRenderer>().material.color = new Color(1f, 0f, 0f);
                    break;
                }
            case 32:
                {
                    GetComponent<MeshRenderer>().material.color = new Color(1f, 0f, 1f);
                    break;
                }
            case 64:
                {
                    GetComponent<MeshRenderer>().material.color = new Color(0f, 1f, 1f);
                    break;
                }

            case 128:
                {
                    GetComponent<MeshRenderer>().material.color = new Color(0f, 0.5f, 0.8f);
                    break;
                }

            case 256:
                {
                    GetComponent<MeshRenderer>().material.color = new Color(0.75f, 0.2f, 0.46f);
                    break;
                }

            default:
                {
                    GetComponent<MeshRenderer>().material.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
                    break;
                }
            
        }
    }

    public void MergeFinished(int number)
    {

        OnMergeFinished?.Invoke(transform.position, number);
    }

    public GameObject GameObject()
    {
        return gameObject;
    }

    public int GetNumber()
    {
        return cubePresenter.GetNumber();
    }

   public void SetNumber(int number)
    {
        cubePresenter.SetNumber();
    }

    public void Rotate(Transform target)
    {
         StartCoroutine(RotateCoroutine(target));
        
    }


    private IEnumerator RotateCoroutine(Transform target)
    {
        float step = 440f * Time.deltaTime;
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        while (transform.rotation != targetRotation)
        {
            
           transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
            yield return null;
        }
    }

    public void ChangeNumber(int number)
    {
        foreach(var numberText in numberTexts)
        {
            numberText.text = number.ToString();
        }
    }
     
}
