using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChestView
{
    void OpenChest(bool open);

    void RotateChestUp(float angle);

    Vector3 GetPosition();
}
