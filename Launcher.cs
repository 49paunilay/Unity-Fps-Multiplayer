using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;
    public TMP_InputField roomnameInputFoeld;
    public TMP_Text errormenuerrormessage;
    public TMP_Text roomNameText;
    public Transform roomListContent;
    public GameObject roomListPrefab;
    public GameObject playerListitemPrefab;
    public Transform playerListContent;
    public GameObject startButton;
    // Start is called before the first frame update
    private void Awake() {
        Instance=this;
    }
    void Start()
    {
        // using settings connect to master server
        Debug.Log("Connecting");
        PhotonNetwork.ConnectUsingSettings();
        
    }

    public void StartGame(){
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnConnectedToMaster(){
        //connect to lobby
        PhotonNetwork.JoinLobby();
        Debug.Log("Joined! master");
        PhotonNetwork.AutomaticallySyncScene=true;
    }
    public override void OnJoinedLobby(){
        ManageMenus.Instance.OpenMenu("title");
        Debug.Log("Joined lobby!");
        PhotonNetwork.NickName = "Player "+Random.Range(0,100).ToString("000");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateRoom(){
        if(string.IsNullOrEmpty(roomnameInputFoeld.text)){
            return;
        }
        PhotonNetwork.CreateRoom(roomnameInputFoeld.text);
        ManageMenus.Instance.OpenMenu("loading");

    }
    public override void OnJoinedRoom(){
        ManageMenus.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        Player[] players = PhotonNetwork.PlayerList;
        foreach(Transform child in playerListContent){
            Destroy(child.gameObject);
        }
        for(int i=0;i<players.Count();i++){
            Instantiate(playerListitemPrefab,playerListContent).GetComponent<playerListitem>().SetUp(players[i]);


        }
        startButton.SetActive(PhotonNetwork.IsMasterClient);

    }
    public override void OnMasterClientSwitched(Player newMasterClient){
        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnCreateRoomFailed(short returnCode,string message){
        errormenuerrormessage.text = "Failed to create room "+message;
        ManageMenus.Instance.OpenMenu("error");


    }
    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
        ManageMenus.Instance.OpenMenu("loading");
    }
    public override void OnLeftRoom(){
        ManageMenus.Instance.OpenMenu("title");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList){
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
            
        }
        for(int i=0;i<roomList.Count;i++){
            if(roomList[i].RemovedFromList){
                continue;
            }
            Instantiate(roomListPrefab,roomListContent).GetComponent<roomListItem>().SetUp(roomList[i]);
        }
        
    }
    public void JoinRoom(RoomInfo info){
        PhotonNetwork.JoinRoom(info.Name);
        ManageMenus.Instance.OpenMenu("loading");

    }
    public override void OnPlayerEnteredRoom(Player newPlayer){
        Instantiate(playerListitemPrefab,playerListContent).GetComponent<playerListitem>().SetUp(newPlayer);

    }
}
