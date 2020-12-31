using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPoint : MonoBehaviour
{
    public GameObject grapics;
    private void Awake() {
        grapics.SetActive(false);
    }
}
