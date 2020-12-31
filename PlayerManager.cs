using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView pv;
    GameObject controller;
    private Transform ui_health_bar;
    private void Awake() {
        pv=GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(pv.IsMine){
            CreateController();
            ui_health_bar=GameObject.Find("hud/health/Bar").transform;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CreateController(){
        Transform spawnpoint= spawnManager.Instance.getSpawnPoint();
        controller=PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","PlayerController"),spawnpoint.position,spawnpoint.rotation,0,new object[] {pv.ViewID });
        
    }
    public void Die(){
        PhotonNetwork.Destroy(controller);
        CreateController();
        ui_health_bar.localScale = new Vector3(1,1,1);
    }
}
