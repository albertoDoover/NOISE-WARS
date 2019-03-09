using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;

// Script general de la partida 
public class GestorPartida : MonoBehaviourPunCallbacks {

	public bool IsConnected=false;
	public int myPing;
	public Text myPingText,myKDAText,MensajePartida;
	public Transform myPuntoCreacion,Interfaz;
	public GameObject myPlayer,Seleccion;
	public TowerScript[] Torres;
	public int EquipoVictoria=-1; 
	float DistanciaCamara=13.6f;
	public CAM CamaraPrincipal;
	string NombrePlayer;
	public InputField NombrePlayerText;
	public GestorMultiTouch MultiTouch;
	public Chat myChat;

	#region Funciones Basicas
	void Awake(){
		PhotonNetwork.AutomaticallySyncScene=true;
	}

	void Start(){
		SetNombreGenerico();
		PhotonNetwork.GameVersion="1";
		PhotonNetwork.SendRate=50;
		PhotonNetwork.SerializationRate=45;
		PhotonNetwork.ConnectUsingSettings();
	}

	void Update(){
		// Zoom al personaje
		if(Input.GetAxis("Mouse ScrollWheel")>0 && DistanciaCamara<17f){
			DistanciaCamara=DistanciaCamara+0.25f;
		}else if(Input.GetAxis("Mouse ScrollWheel")<0 && DistanciaCamara>10f){
			DistanciaCamara=DistanciaCamara-0.25f;
		}

		// Estado de conexion
		IsConnected=PhotonNetwork.IsConnected;
		if(PhotonNetwork.CurrentRoom!=null && IsConnected){
		myPing = PhotonNetwork.GetPing();
		myPingText.text="Ping:"+myPing.ToString()+"ms";
		}else{
		myPingText.text="Desconectado";
		}
	}
	#endregion

	#region Funciones Photon
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
		if(NombrePlayerText.text!=""){
		Seleccion.SetActive(false);
		myPlayer=PhotonNetwork.Instantiate("Fantin",myPuntoCreacion.position,Quaternion.identity,0,null);
		SetName();
		MultiTouch.AdaptarPersonajePrincipal(myPlayer.GetComponent<Personaje>());
		if(Application.isMobilePlatform){
		MultiTouch.gameObject.SetActive(true);
		}
		}
     }

	public void SeleccionarKalani(){
		if(NombrePlayerText.text!=""){
		Seleccion.SetActive(false);
		myPlayer=PhotonNetwork.Instantiate("Kalani",myPuntoCreacion.position,Quaternion.identity,0,null);
		SetName();
		MultiTouch.AdaptarPersonajePrincipal(myPlayer.GetComponent<Personaje>());
		if(Application.isMobilePlatform){
		MultiTouch.gameObject.SetActive(true);
		}
		}
      }

    void SetName(){
    	myPlayer.GetComponent<PersonajeOnline>().SetNombre(NombrePlayer);
    }
    
	public override void OnJoinRoomFailed(short returnCode, string message){
		RoomOptions opcionesRoom = new RoomOptions();
		opcionesRoom.MaxPlayers=3;
		opcionesRoom.CleanupCacheOnLeave=false;
		TypedLobby myLobby = new TypedLobby();
		PhotonNetwork.JoinOrCreateRoom("ER",opcionesRoom,myLobby,null);
    }
	#endregion

	#region Torres
	public void DañoTorre(int Daño,int Code){ // Reportar daño a torres y bases
		Torres[Code].ReportarDañoTorre(Daño);
	}

	void FixedUpdate(){
		// Adaptar interfaz a zoom
		Interfaz.localScale=new Vector3(DistanciaCamara/13.6f,DistanciaCamara/13.6f,1f);
		CamaraPrincipal.orthographicSize=DistanciaCamara;

		if(PhotonNetwork.IsMasterClient && EquipoVictoria==-1){ // Verificar que equipo gano
			if((Torres[0].HP)<=0){
				myPlayer.GetComponent<PhotonView>().RPC("setVictoriaOnline",RpcTarget.AllBuffered,2);
			}else if((Torres[7].HP)<=0){{
				myPlayer.GetComponent<PhotonView>().RPC("setVictoriaOnline",RpcTarget.AllBuffered,1);
			}
		}
	  }
	}

	public void setVictoria(int Team){ // Finalizar partida
	if(EquipoVictoria==-1){
		EquipoVictoria=Team;
		if(myPlayer.GetComponent<PersonajeOnline>().TeamID==Team){
			MensajePartida.text="VICTORIA";
		}else{
			MensajePartida.text="DERROTA";
		}
	}
	}
	#endregion

	#region NombreJugador
	void SetNombreGenerico(){ // Asignar nombre generico al jugador
		NombrePlayer="Player"+(Mathf.RoundToInt(9999*Random.value)).ToString();
		NombrePlayerText.text=NombrePlayer;
	}

	public void SetNombreJugador(){
		NombrePlayer=NombrePlayerText.text;
	}
	#endregion

}
