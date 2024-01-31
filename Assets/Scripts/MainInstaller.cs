using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField]
    CubeView cubeView;

    [SerializeField]
    GameObject cubePrefab;

    [SerializeField]
    Material[] materials;

    [SerializeField]
    GameObject coinPrefab;

    [SerializeField]
    ChestView chest;

    [SerializeField]
    TextMeshProUGUI coinsText;

    [SerializeField]
    GameObject gameOverText, restartButton;

    [SerializeField]
    LineColliderView lineCollider;

    public override void InstallBindings()
    {
        Container.Bind<ICubeView>().To<CubeView>().FromMethod(GetCubeView).AsTransient();
        Container.Bind<ICubePresenter>().To<CubePresenter>().AsTransient();
        Container.Bind<GameObject>().WithId("cube").FromInstance(cubePrefab);
        Container.Bind<GameObject>().WithId("coin").FromInstance(coinPrefab);
        Container.Bind<GameObject>().WithId("gameOver").FromInstance(gameOverText);
        Container.Bind<GameObject>().WithId("restartButton").FromInstance(restartButton);
        Container.Bind<ICubeModel>().To<CubeModel>().FromMethod(GetCubeModel).AsTransient();
        Container.Bind<Material[]>().FromInstance(materials).AsSingle();
        Container.Bind<IChestView>().To<ChestView>().FromInstance(chest).AsSingle();
        Container.Bind<TextMeshProUGUI>().FromInstance(coinsText).AsSingle();
        Container.Bind<IChestPresenter>().To<ChestPresenter>().AsSingle();
        Container.Bind<IChestModel>().To<ChestModel>().AsSingle();
        Container.Bind<ILineColliderView>().To<LineColliderView>().FromInstance(lineCollider);
        
    }

   public void InjectGameManager(GameObject cube, GameManager gameManager)
    {
        cubeView = cube.GetComponent<CubeView>();
        Container.Inject(cubeView);
        Container.Inject(gameManager);

    }

    public void InjectCube(GameObject cube)
    {
        var cubeView = GetComponent<CubeView>();
        Container.Inject(cubeView);
    }

    private CubeView GetCubeView()
    {
        return cubeView;
    }

    private CubeModel GetCubeModel()
    {
        var numbers = new List<int> { 2, 4, 8 };
        var index = Random.Range(0, numbers.Count);
        return new CubeModel(numbers[index]);
    }

}