using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class GestorPartida : MonoBehaviourPunCallbacks {

	public bool con=false;
	public int myPing;
	public Text myPingText;
	public Transform myPuntoCreacion;
	public string myRegion;

	void Awake(){
		PhotonNetwork.AutomaticallySyncScene=true;
	}
	void OnFailedToConnectToPhoton(){
	Debug.Log("fdsfsdsf");
	}

	public override void OnConnectedToMaster(){
		PhotonNetwork.JoinRoom("ER",null);
	}

	public override void OnJoinedRoom(){
	  if(PhotonNetwork.PlayerList.Length%2==0){
		PhotonNetwork.Instantiate("Fantin",myPuntoCreacion.position,Quaternion.identity,0,null);
     }else{
		PhotonNetwork.Instantiate("Kalani",myPuntoCreacion.position,Quaternion.identity,0,null);
     }
    }

	public override void OnJoinRoomFailed(short returnCode, string message){
		RoomOptions opcionesRoom = new RoomOptions();
		opcionesRoom.MaxPlayers=3;
		opcionesRoom.CleanupCacheOnLeave=false;
		TypedLobby myLobby = new TypedLobby();
		PhotonNetwork.JoinOrCreateRoom("ER",opcionesRoom,myLobby,null);
    }

	void Update(){
		con=PhotonNetwork.IsConnected;
		if(PhotonNetwork.CurrentRoom!=null && con){
		myPing = PhotonNetwork.GetPing();
		myRegion=PhotonNetwork.CloudRegion;
		myPingText.text=myRegion.ToString()+" : "+myPing.ToString();
		}else{
		myPingText.text="Desconectado";
		}

	}

	void Start(){
	if(PhotonNetwork.IsConnected){

	}else{
	PhotonNetwork.GameVersion="1";
	PhotonNetwork.SendRate=50;
	PhotonNetwork.SerializationRate=45;
	PhotonNetwork.ConnectUsingSettings();
	}
	}

}
