using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class playerController : MonoBehaviourPunCallbacks , Damagabe
{
    private Rigidbody mybody;
    public GameObject cameraHolder;
    public float mouseSensitivity,sprintSpeed,JumpSpeed,walkSpeed,smoothTime;
    bool grounded;
    Vector3 smoothVelocity;
    Vector3 moveAmount;
    float verticalLookRotation;
    PhotonView pv;
    public Item[] items;
    int itemIndex;
    int previousItemIndex=-1;
    const float maxhealth=100f;
    float currenthealth=maxhealth;
    PlayerManager playermanager;
    public AudioSource gunAudio;

    private Transform ui_health_bar;

    private void Awake() {
        mybody=GetComponent<Rigidbody>();
        pv=GetComponent<PhotonView>();
        playermanager = PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<PlayerManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(pv.IsMine){
            EquipItems(0);

        }
        else{
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(mybody);

        }
        ui_health_bar=GameObject.Find("hud/health/Bar").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!pv.IsMine){
            return;
        }
        LookAround();
        Movement();
        playerJump();

        for(int i=0;i<items.Length;i++){
            if(Input.GetKeyDown((i+1).ToString())){
                EquipItems(i);
            }
        }
        if(Input.GetAxisRaw("Mouse ScrollWheel")>0f){
            if(itemIndex>=items.Length - 1){
                EquipItems(0);
            }else{
                EquipItems(itemIndex+1);
            }
            
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel")<0f){
            if(itemIndex <=0){
                EquipItems(items.Length-1);
            }else{
                EquipItems(itemIndex-1);
            }
            
        }
        if(Input.GetMouseButtonDown(0)){
            items[itemIndex].Use();
            gunAudio.Play();
        }
        if(transform.position.y<-25f){
            Die();
        }

        
    }
    void LookAround(){
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X")*mouseSensitivity);
        verticalLookRotation+=Input.GetAxisRaw("Mouse Y")*mouseSensitivity;
        verticalLookRotation=Mathf.Clamp(verticalLookRotation,-80f,80f);
        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }
    void Movement(){
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"),0f,Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount,moveDir*(Input.GetKey(KeyCode.LeftShift)? sprintSpeed:walkSpeed),ref smoothVelocity,smoothTime);
    }
    void playerJump(){
        if(Input.GetKeyDown(KeyCode.Space) && grounded){
            mybody.AddForce(transform.up * JumpSpeed);
        }

    }
    public void SetGroundedState(bool _grounded){
        grounded=_grounded;

    }
    private void FixedUpdate() {
        if(!pv.IsMine){
            return;
        }
        mybody.MovePosition(mybody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
    void EquipItems(int _index){
        if(_index==previousItemIndex){
            return;
        }
        itemIndex=_index;
        items[itemIndex].itemGameObject.SetActive(true);
        if(previousItemIndex!=-1){
            items[previousItemIndex].itemGameObject.SetActive(false);
        }
        previousItemIndex=itemIndex;

        if(pv.IsMine){
            Hashtable hash=new Hashtable();
            hash.Add("itemIndex",itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer,Hashtable changeProps){
        if(!pv.IsMine && targetPlayer==pv.Owner){
            EquipItems((int)changeProps["itemIndex"]);
        }
    }
    public void TakeDamage(float damage){
        pv.RPC("RPC_TakeDamage",RpcTarget.All,damage);
        
    }
    [PunRPC]
    void RPC_TakeDamage(float damage){
        if(!pv.IsMine){
            return;
        }
        currenthealth-=damage;
        RefreshHealth();
        if(currenthealth<=0){

            Die();
        }
    }

    void Die(){
        playermanager.Die();

    }
    void RefreshHealth(){
        float t_health_ratio= (float)currenthealth/(float)maxhealth;
        ui_health_bar.localScale = new Vector3(t_health_ratio,1,1);
    }
}

