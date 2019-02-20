using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class GestorPartida : MonoBehaviourPunCallbacks {

	public bool IsConnected=false;
	public int myPing;
	public Text myPingText,myKDAText;
	public Transform myPuntoCreacion;
	public string myRegion;
	public GameObject myPlayer,Seleccion;

	void Awake(){
		PhotonNetwork.AutomaticallySyncScene=true;
	}
	void OnFailedToConnectToPhoton(){
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster(){
		PhotonNetwork.JoinRoom("ER",null);
	}

	public override void OnJoinedRoom(){
	  Seleccion.SetActive(true);
    }

    public void SeleccionarFantin(){
		Seleccion.SetActive(false);
		myPlayer=PhotonNetwork.Instantiate("Fantin",myPuntoCreacion.position,Quaternion.identity,0,null);
    }

	public void SeleccionarKalani(){
		Seleccion.SetActive(false);
		myPlayer=PhotonNetwork.Instantiate("Kalani",myPuntoCreacion.position,Quaternion.identity,0,null);
    }

	public override void OnJoinRoomFailed(short returnCode, string message){
		RoomOptions opcionesRoom = new RoomOptions();
		opcionesRoom.MaxPlayers=3;
		opcionesRoom.CleanupCacheOnLeave=false;
		TypedLobby myLobby = new TypedLobby();
		PhotonNetwork.JoinOrCreateRoom("ER",opcionesRoom,myLobby,null);
    }

	void Update(){
		IsConnected=PhotonNetwork.IsConnected;
		if(PhotonNetwork.CurrentRoom!=null && IsConnected){
		myPing = PhotonNetwork.GetPing();
		myRegion=PhotonNetwork.CloudRegion;
		myPingText.text="Ping:"+myPing.ToString()+"ms";
		}else{
		myPingText.text="Desconectado";
		}
	}

	void Start(){
	PhotonNetwork.GameVersion="1";
	PhotonNetwork.SendRate=50;
	PhotonNetwork.SerializationRate=45;
	PhotonNetwork.ConnectUsingSettings();
	}

}
