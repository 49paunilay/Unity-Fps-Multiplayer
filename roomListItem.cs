using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;

public class roomListItem : MonoBehaviour
{
    public TMP_Text text;
    public RoomInfo info;
    public void SetUp(RoomInfo _info){
        info=_info;
        text.text=_info.Name;


    }
    public void onClick(){
        Launcher.Instance.JoinRoom(info);

    }
}
