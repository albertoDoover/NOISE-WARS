using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PersonajeOnline : MonoBehaviour,IPunObservable {

	public bool con=false;
	public PhotonView myPhotonView;
	public bool isMine;
	Rigidbody2D myRig;
	Personaje myPersonaje;
	public float Lerp=0.3f,MaximaDistancia=5f;
	public Text NombreTexto;
	public PaquetePhoton Pack;
	Vector2 PositionOnline;
	public int TeamID;
	string MyName; // Nombre de jugador
	GestorPartida Gestor;

	#region Metodos Basicos
	void Awake(){
		Pack = new PaquetePhoton();
		myPhotonView=GetComponent<PhotonView>();
		isMine=myPhotonView.IsMine;
		myRig=GetComponent<Rigidbody2D>();
		myPersonaje=GetComponent<Personaje>();
		Gestor=GameObject.Find("GestorPartida").GetComponent<GestorPartida>();
		if(myPhotonView.OwnerActorNr%2==0){
			TeamID=1;
			GetComponent<Personaje>().teamid=1;
			transform.position=GameObject.Find("Posicion1").transform.position;
		}else{
			TeamID=2;
			GetComponent<Personaje>().teamid=2;
			transform.position=GameObject.Find("Posicion2").transform.position;
		}
		if(isMine){			
			Camera.main.GetComponent<CamFollow>().player=transform;
			myPersonaje.spi.sortingOrder=10;
			myPersonaje.mine=isMine;
			NombreTexto.GetComponent<Outline>().effectColor=Color.white;
		}else{
			myRig.gravityScale=0f;
		}

		ActualizarPaquetePhoton();
	}

	void FixedUpdate(){
		if(isMine){
		NombreTexto.GetComponent<Outline>().effectColor=Color.white;
		}else if(Gestor.myPlayer!=null){
			if(TeamID==Gestor.myPlayer.GetComponent<PersonajeOnline>().TeamID){
				NombreTexto.GetComponent<Outline>().effectColor=Color.blue;
			}else{
				NombreTexto.GetComponent<Outline>().effectColor=Color.red;
			}
		}
	}

	void Update(){
		if(!isMine && myPersonaje.HP>0){		
			setPositionOnline();
		}
	}
	#endregion

	#region Metodos Online
	public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info){
		if(stream.IsWriting){
			ActualizarPaquetePhoton();			
			stream.SendNext (JsonUtility.ToJson(Pack));
		}else{
			Pack = JsonUtility.FromJson<PaquetePhoton>((string)stream.ReceiveNext ()); 
			RecibirPaquetePhoton();
		}
	}

	public void enviarDaño(float daño,int tipo,string Atacante){
		myPhotonView.RPC("recibirDaño",RpcTarget.AllBuffered,daño,tipo,Atacante);
	}

	[PunRPC]
	public void recibirDaño(float daño,int tipo,string Atacante){
		myPersonaje.ResolverDaño(daño,tipo,Atacante);
	}

	public void enviarDañoTorre(int daño, int code){
		myPhotonView.RPC("recibirDañoTorre",RpcTarget.AllBuffered,daño,code);
	}

	[PunRPC]
	public void recibirDañoTorre(int daño, int code){
		Gestor.DañoTorre(daño,code);
	}

	[PunRPC]
	public void setVictoriaOnline(int Team){
		Gestor.setVictoria(Team);
	}

	public void SetNombre(string Nombre){
		myPhotonView.RPC("setNombreOnline",RpcTarget.AllBuffered,Nombre);
	}

	[PunRPC]
	void setNombreOnline(string Nombre){
		MyName=Nombre;
		NombreTexto.text=MyName;
	}

	#endregion

	#region Clases auxiliares
	public class PaquetePhoton { // Datos para ser enviados en OnPhotonSerializeView con todos los datos relevantes del personaje
		public Vector2 myPos;
		public float myScale;
		public bool atacando,corriendo;
		public int numeroAtaque,HP,TipoMuerte,ConteoAtk,Subiendo;
	}

	void ActualizarPaquetePhoton(){
		Pack.myPos=myRig.position;
		Pack.myScale = transform.localScale.x;
		Pack.atacando=myPersonaje.atacando;
		Pack.numeroAtaque=myPersonaje.numeroATK;
		Pack.HP=Mathf.CeilToInt(myPersonaje.HP);
		Pack.ConteoAtk=myPersonaje.ConteoAtk;
		Pack.TipoMuerte=myPersonaje.TipoMuerte;
		Pack.corriendo=myPersonaje.corriendo;
	}

	void RecibirPaquetePhoton(){
		transform.localScale=new Vector3(Pack.myScale,transform.localScale.y,transform.localScale.z);
		if(myRig.position.y>=PositionOnline.y){
			myPersonaje.subiendo=false;
		}else{
			myPersonaje.subiendo=true;
		}
		if(!myPersonaje.atacando){
			PositionOnline=Pack.myPos;
			if(Pack.ConteoAtk>myPersonaje.ConteoAtk && Pack.atacando){
				myPersonaje.ConteoAtk=Pack.ConteoAtk;
				myPersonaje.atacando=Pack.atacando;
			}else if(!Pack.atacando){
				myPersonaje.atacando=Pack.atacando;
			}
		} 
		myPersonaje.numeroATK=Pack.numeroAtaque;
		myPersonaje.corriendo=Pack.corriendo;
		myPersonaje.TipoMuerte=Pack.TipoMuerte;
	}
	#endregion

	#region Gestion de datos Online

	void setPositionOnline(){
		Vector2 interpolatePosition = Vector2.Lerp (myRig.position,PositionOnline,Lerp);
		if(Vector2.Distance(myRig.position,PositionOnline)>MaximaDistancia){
			interpolatePosition = PositionOnline;
		}
		myRig.position = interpolatePosition;
	}
	#endregion



}
