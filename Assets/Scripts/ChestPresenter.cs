using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ChestPresenter: IChestPresenter
{
    [Inject]
    private IChestModel chestModel;
    [Inject]
    private IChestView chestView;

    public void OpenChest(bool open)
    {

        if(!chestModel.IsOpened && open)
        {
            chestView.RotateChestUp(45f);
            chestModel.IsOpened = true;
        }

        if (!open && chestModel.IsOpened)
        {
            chestView.RotateChestUp(-45f);
            chestModel.IsOpened = false;
        }
    }
}
