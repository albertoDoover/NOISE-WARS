using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestorPartida : Photon.MonoBehaviour {

	public Transform pos1,pos2;
	PhotonView myplayer;
	public Text Mensaje,MensajeSala;
	public Text killsaliadas,killsenemigas,pointsaliados,pointsenemigos;
	public int myteam,kills,deaths,points,loss;

	void Start () {
		PhotonNetwork.sendRate = 100;
		PhotonNetwork.sendRateOnSerialize = 100;
		PhotonNetwork.autoCleanUpPlayerObjects=false;
		PhotonNetwork.ConnectUsingSettings ("v1.0");
	}

	void OnJoinedLobby(){
		RoomOptions roomop = new RoomOptions () {
			IsVisible = false, MaxPlayers = 4,
		};
		PhotonNetwork.JoinOrCreateRoom ("sala",roomop,TypedLobby.Default);
	}

	void OnJoinedRoom(){
		Mensaje.text = "";
		MensajeSala.text = "";
		myplayer = PhotonNetwork.Instantiate ("Fantin",transform.position,Quaternion.identity,0).GetComponent<PhotonView>();
		if(myplayer.ownerId%2==0){
			myplayer.transform.position = pos1.position;
			myteam = 1;
		}else{
			myplayer.transform.position = pos2.position;
			myteam = 2;
		}
	}

	void ClearMensaje(){
		Mensaje.text = "";
		Mensaje.gameObject.SetActive (false);
	}

	public void showmensajebandera(int code){
		Mensaje.gameObject.SetActive (true);
		CancelInvoke ();
		if(code==myteam){
			Mensaje.text = "Tu equipo tiene la bandera";
		}else{
			Mensaje.text = "El enemigo tiene la bandera";
		}
		Invoke ("ClearMensaje",2f);
	}

	public void muerte(string texto){
		Mensaje.gameObject.SetActive (true);
		CancelInvoke ();
		Mensaje.text = texto;
		killsaliadas.text = "Kills: " + kills;
		killsenemigas.text = "Kills: " + deaths;
		Invoke ("ClearMensaje",2f);

	}

	public void ganar(int code){
		Mensaje.gameObject.SetActive (true);
		CancelInvoke ();
		if(code==myteam){
			points++;
			Mensaje.text = "¡Punto para tu equipo!";
		}else{
			loss++;
			Mensaje.text = "Punto enemigo...";
		}
		pointsaliados.text = "Points: " + points;
		pointsenemigos.text = "Points: " + loss;
		Invoke ("ClearMensaje",2f);
	}

	void OnDisconnectedFromPhoton(){
		Mensaje.gameObject.SetActive (true);
		CancelInvoke ();
		Mensaje.text = "Reconectando";
		Invoke ("ClearMensaje",5f);
		PhotonNetwork.ReconnectAndRejoin ();
	}

	void OnFailedToConnectToPhoton(){
		MensajeSala.text = "Error al conectar";
	}

	void OnPhotonJoinRoomFailed(){
		MensajeSala.text = "Error al conectar";
	}
	void OnPhotonRandomJoinFailed(){
		MensajeSala.text = "Error al conectar";
	}
}
