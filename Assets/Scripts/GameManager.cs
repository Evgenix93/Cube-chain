using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject(Id ="cube")]
    private GameObject cubePrefab;

    [SerializeField]
    private MainInstaller mainInstaller;

    [Inject]
    private ICubeView cubeView;

    [Inject(Id = "coin")]
    private GameObject coin;

    [Inject]
    private TextMeshProUGUI coinsAmountText;

    private int coinsAmount;

    
    private ICubeView mergeCubeView1;
    private int instanceId;

    [Inject]
    private IChestView chestView;

    
    [Inject(Id ="gameOver")]
    private GameObject gameOverText;

    [Inject(Id = "restartButton")]
    private GameObject restartButton;

    private List<GameObject> coins = new List<GameObject>();

    private List<ICubeView> cubeViews = new List<ICubeView>(); 

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60; 
        cubeView.OnCubePushed += CreateCube;
        cubeView.OnContact += Merge;
        cubeView.OnMergeFinished += CreateCoins;
        cubeView.OnLineCross += GameOver;
      //  cubeView.OnDestroyCube += RemoveCubeView;
        coinsAmountText.text = "0";
        cubeViews.Add(cubeView);
    }

    // Update is called once per frame
    void Update()
    {
        if (coins.Count == 0)
            chestView.OpenChest(false);
        else
            chestView.OpenChest(true);

    }

    private void GameOver()
    {
        gameOverText.SetActive(true);
        restartButton.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    void CreateCube()
    {
        StartCoroutine(CreateCubeCoroutine());
    }

    private IEnumerator CreateCubeCoroutine()
    {
       
        yield return new WaitForSeconds(1);
        
        var cube = Instantiate(cubePrefab, new Vector3(0f, 2f, -13.5f), Quaternion.identity);
        mainInstaller.InjectGameManager(cube, this);
        cubeView.OnCubePushed += CreateCube;
        cubeView.OnContact += Merge;
        cubeView.OnMergeFinished += CreateCoins;
        cubeView.OnLineCross += GameOver;
       // cubeView.OnDestroyCube += RemoveCubeView;
        cubeViews.Add(cubeView);
    }

    private void RemoveCubeView(ICubeView cubeView)
    {
        cubeViews.Remove(cubeView);
    }

    private void Merge(ICubeView mergeCubeView1, int instanceId)
    {
        if (mergeCubeView1.ToDestroy)
            return;

        var mergeCubeView2 = cubeViews.Find( (cubeView) => cubeView.GetInstanceId() == instanceId);
        if (mergeCubeView1.GetNumber() == mergeCubeView2?.GetNumber() && mergeCubeView2?.IsMerging() == false)
        {
            mergeCubeView2.ToDestroy = true;
            Destroy(mergeCubeView2.GameObject(), 0.5f);
            cubeViews.Remove(mergeCubeView2);
            mergeCubeView2.IsKinematic(true);
            mergeCubeView1.IsKinematic(true);

            mergeCubeView2.Rotate(mergeCubeView1.GetTransform());
            mergeCubeView1.Rotate(mergeCubeView2.GetTransform());
            mergeCubeView1.Merge(mergeCubeView2.GetPosition());
        }
        
    }


    private void MergeDebug(ICubeView mergeCubeView2, int instanceId2)
    {
       // Debug.Log("Merge");
        if (mergeCubeView1 == null) {
            mergeCubeView1 = mergeCubeView2;
            instanceId = instanceId2;
            return;
        }

        if (mergeCubeView1.GetNumber() == mergeCubeView2.GetNumber() && instanceId == mergeCubeView2.GetInstanceId() && instanceId != mergeCubeView1.GetInstanceId())
        {
            mergeCubeView2.EnableCollider(false);
            mergeCubeView1.EnableCollider(false);
            Destroy(mergeCubeView2.GameObject(), 0.9f);
            mergeCubeView2.Rotate(mergeCubeView1.GetTransform());
            mergeCubeView1.Rotate(mergeCubeView2.GetTransform());
            mergeCubeView1.Merge(mergeCubeView2.GetPosition());
            
        }

        mergeCubeView1 = null;
        instanceId = -1;
        
    }

   void CreateCoins(Vector3 position, int number)
    {
        
        StartCoroutine(CreateCoinsCoroutine(position, number));

    }



    IEnumerator CreateCoinsCoroutine(Vector3 position, int number)
    {

        var localCoins = new List<GameObject>();
        int coinAmount = number / 4;
        for (int i = 0; i < coinAmount; i++)
        {
            var coinInst = Instantiate(coin, position, Quaternion.identity);
            coins.Add(coinInst);
            localCoins.Add(coinInst);
            var coinRigidbody = coinInst.GetComponent<Rigidbody>();
            coinRigidbody.velocity = new Vector3(Random.Range(-5f, 5f), Random.Range(8f, 15f), Random.Range(-5f, 5f));
            coinRigidbody.angularVelocity = new Vector3(Random.Range(-5f, 5f), Random.Range(8f, 15f), Random.Range(-5f, 5f));
        }


        yield return new WaitForSeconds(0.5f);
        foreach(var coinObj in localCoins)
        {
            coinObj.GetComponent<Collider>().enabled = true;
        }
        yield return new WaitForSeconds(2f);

        foreach(var coinObj in localCoins)
        {
            var coinRigidbody = coinObj.GetComponentInChildren<Rigidbody>();
            coinRigidbody.useGravity = false;
            coinObj.GetComponent<Collider>().enabled = false;
            coinRigidbody.velocity = new Vector3(Random.Range(-5f, 5f), 8f, Random.Range(-5f, 5f));
            coinRigidbody.angularVelocity = new Vector3(Random.Range(-5f, 5f), Random.Range(8f, 15f), Random.Range(-5f, 5f));
        }

        yield return new WaitForSeconds(1f);

        
        foreach (var coinObj in localCoins)
        {
            chestView.OpenChest(true);
            coinObj.transform.DOMove(chestView.GetPosition(), 2f).OnComplete(() => { 
                coinsAmount++;
                coinsAmountText.text = coinsAmount.ToString();

                //if (coinObj == coins.Last())
                   // chestView.OpenChest(false);

                coins.Remove(coinObj);
                Destroy(coinObj);
            });
        }
    }

}
