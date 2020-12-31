using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public itemInfo _iteminfo;
    public GameObject itemGameObject;

    public abstract void Use();
}
