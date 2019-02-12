using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PersonajeOnline : MonoBehaviour,IPunObservable {

	public bool con=false;
	public int myPing;
	PhotonView myPhotonView;
	public bool isMine;
	Rigidbody2D myRig;
	Personaje myPersonaje;
	public float Lerp=0.3f,MaximaDistancia=5f;
	public Text myPing2;
	public PaquetePhoton Pack;
	Vector2 PositionOnline;

	#region Metodos Basicos
	void Awake(){
		Pack = new PaquetePhoton();
		myPhotonView=GetComponent<PhotonView>();
		isMine=myPhotonView.IsMine;
		myRig=GetComponent<Rigidbody2D>();
		myPersonaje=GetComponent<Personaje>();
		if(isMine){			
			Camera.main.GetComponent<CamFollow>().player=transform;
		}else{
			myRig.gravityScale=0f;
		}
		ActualizarPaquetePhoton();
	}

	void Update(){
		if(!isMine){		
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

	public void enviarDaño(int daño,int tipo){
		myPhotonView.RPC("recibirDaño",RpcTarget.All,daño,tipo);
	}

	[PunRPC]
	public void recibirDaño(int daño,int tipo){
		myPersonaje.ResolverDaño(daño,tipo);
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
			if(Pack.ConteoAtk!=myPersonaje.ConteoAtk){
				myPersonaje.ConteoAtk=Pack.ConteoAtk;
				myPersonaje.atacando=Pack.atacando;
			}
		} 
		myPersonaje.numeroATK=Pack.numeroAtaque;
		myPersonaje.corriendo=Pack.corriendo;
		//myPersonaje.HP=Pack.HP;
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
