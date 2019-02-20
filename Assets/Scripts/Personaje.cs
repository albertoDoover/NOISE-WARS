﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Codigo del control del personaje. Incluye control de animaciones, colisiones y sincronizacion online

public class Personaje : MonoBehaviour {
	public bool corriendo,atacando,tocasuelo,subiendo,noqueado,golpe,wiggle,hit; // Variables para el animator
	public bool mine=false; // Objeto perteneciente al jugador activo
	Animator anim; // Animator del personaje
	public Image BarraVida,BarraFondo,BarraRoja; // Barras vida
	public Canvas MyCanvas; // Interfaz
	public float vel = 0f,RadioSuelo,RadioWiggle,sizex,lerp; // Velocidad, radio de colision con suelo, Escala horizontal, Lerp para interpolar movimiento
	Rigidbody2D rig;
	public SpriteRenderer spi; // Cuerpo del personaje
	public LayerMask mascarasuelo; // Identificador de suelo
	public Transform comprobadorsuelo,ComprobadorSueloWiggle; // Posicion de los pies, Posicion para iniciar ataque especial, Posicion de reinicio
	public int teamid; // Identificador de equipo
	public int numeroATK=0,TipoMuerte=0,ConteoAtk=0; // Patron del ataque, Ultimo tipo de herida, Numero de ataque
	public float Salto; // Velocidad al subir
	int SaltoDisp=0; // Conteo para doble salto
	string MyName; // Nombre de jugador
	float gravedad; // Fuerza con la que cae
	public float HPMAX,HP; // Vida maxima, Vida actual
	public GameObject myFicha;
	public Vector2 OffSetImpulse;
	public float FactorDaño=1f;
	public Vector3 PosicionInicial;
	GestorPartida myGestorPartida;

	#region Start Update //Metodos basicos de Unity
	void Awake(){
		rig = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		PosicionInicial=transform.position;
	}

	void Start () {		
		MyCanvas.worldCamera = Camera.main;
		transform.name = GetComponent<PersonajeOnline>().myPhotonView.ViewID.ToString();
		Instantiate(myFicha).GetComponent<FichaMapa_Interfaz>().myPersonaje=transform;
		gravedad=rig.gravityScale;
		myGestorPartida=GameObject.Find("GestorPartida").GetComponent<GestorPartida>();
		ListaAtacantes = new List<string>();
	}

	void Update () {
		BarraVida.fillAmount = HP / HPMAX;
		tocasuelo=Physics2D.OverlapBox (comprobadorsuelo.position, new Vector2(RadioSuelo,0.05f),0f, mascarasuelo);
		wiggle=Physics2D.OverlapCircle (ComprobadorSueloWiggle.position, RadioWiggle, mascarasuelo);

		if (rig.velocity.y > 0.1f) {
			subiendo = true;
			gameObject.layer=11;
		} else if (rig.velocity.y <= 0f) {
			subiendo = false;
			gameObject.layer=9;
		}

		if (GetComponent<PersonajeOnline>().isMine && HP>0 && myGestorPartida.IsConnected) {			
			if (!subiendo && tocasuelo) {
				SaltoDisp=2;
					if (!atacando) {					
						if (Input.GetKey(KeyCode.S)) {
						EspecialUlti (0);
						} else if (Input.GetKeyDown (KeyCode.A)) {
						EspecialUlti (1);
						} else if (Input.GetKeyDown (KeyCode.D)) {
						EspecialUlti (2);
						} else if (Input.GetKeyDown (KeyCode.W)) {
						EspecialUlti (3);
						}						
					} else {
						corriendo = false;
					}				
				} else {
				corriendo = false;
			}
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				saltar ();
				}
				
			if (Input.GetKey (KeyCode.LeftArrow) && !atacando && OffSetImpulse==Vector2.zero) {
			    MoveLeft ();
			} else if (Input.GetKey (KeyCode.RightArrow) && !atacando && OffSetImpulse==Vector2.zero) {
				MoveRight ();
			} else if(!Application.isMobilePlatform){
				MoveZero ();
			}
		}
			anim.SetBool ("Hit", hit);
			anim.SetInteger ("NumeroAtaque", numeroATK);
			anim.SetBool ("ensuelo", tocasuelo);
			anim.SetBool ("subiendo", subiendo);
			anim.SetBool ("corriendo", corriendo);
			anim.SetBool ("atacando", atacando);
			anim.SetBool ("Wiggle", !wiggle);
			anim.SetInteger("HP",Mathf.RoundToInt(HP));
			anim.SetInteger("TipoMuerte",TipoMuerte);
		} 
	#endregion

	#region Movimientos
	public  void MoveLeft(){
		if (!noqueado && !atacando && HP > 0) {
			corriendo = true;
			rig.velocity = (new Vector2 (-vel, rig.velocity.y))+OffSetImpulse;
			transform.localScale = new Vector3 (-sizex, transform.localScale.y, 0f);
		} else {
			MoveZero ();
		}
	}

	public void MoveRight(){
		if (!noqueado && !atacando && HP > 0) {
			corriendo = true;
			rig.velocity = (new Vector2 (vel, rig.velocity.y))+OffSetImpulse;
			transform.localScale = new Vector3 (sizex, transform.localScale.y, 0f);
		} else {
			MoveZero ();
		}
	}

	public void MoveZero(){
		corriendo = false;
		rig.velocity = (new Vector2 (0f, rig.velocity.y))+OffSetImpulse;
	}

	public void EspecialUlti(int code){
		if(!atacando && tocasuelo && !subiendo){
			ConteoAtk++;
			numeroATK = code;
			atacando = true;
		}
	}

	public void saltar(){
		if (SaltoDisp>0 && !atacando && HP>0) {
			SaltoDisp--;
			rig.velocity = new Vector2 (rig.velocity.x,0f);
			rig.AddForce (new Vector2(0f,Salto),ForceMode2D.Impulse);
		}
	}

	void endNoqueado(){
		noqueado = false;
	}

	void Revivir(){
		if (mine) {
			Camera.main.GetComponent<CamFollow> ().player = transform;
		}
		atacando=false;
		hit=false;
		rig.freezeRotation = true;
		rig.velocity = Vector2.zero;
		rig.rotation = 0f;
		rig.gravityScale = gravedad;
		HP = HPMAX;
		anim.Play("Jump");
		transform.position=PosicionInicial;
	}
	#endregion

	#region Efectos
	public void DeathSetup(){
		Invoke ("Revivir",3f);
	}

	public void spritemostrar(){
		spi.color = new Color (spi.color.r,spi.color.g,spi.color.b,1f);
		BarraFondo.color = new Color (BarraFondo.color.r,BarraFondo.color.g,BarraFondo.color.b,1f);
		BarraVida.color = new Color (BarraVida.color.r,BarraVida.color.g,BarraVida.color.b,1f);
		BarraRoja.color = new Color (BarraRoja.color.r,BarraRoja.color.g,BarraRoja.color.b,1f);
	}

	public void spriteocultar(){
		spi.color = new Color (spi.color.r,spi.color.g,spi.color.b,0f);
		BarraFondo.color = new Color (BarraFondo.color.r,BarraFondo.color.g,BarraFondo.color.b,0f);
		BarraVida.color = new Color (BarraVida.color.r,BarraVida.color.g,BarraVida.color.b,0f);
		BarraRoja.color = new Color (BarraRoja.color.r,BarraRoja.color.g,BarraRoja.color.b,0f);
	}

	public void spritetransparencia(){
		spi.color = new Color (spi.color.r,spi.color.g,spi.color.b,55f/255f);
	}

	public void finishattack(){
		atacando = false;
		numeroATK = -1;
	}

	public void HitOn(){
		if(!hit){
			hit=true;
			Invoke("HitOff",0.05f);
		}
			spi.color= new Color(248f/255f,162f/255f,107f/255f,1f);
	}

	void HitOff(){
		spi.color= new Color(1f,1f,1f,1f);
		hit=false;
	}

	public void MuerteVolar(){
		GetComponent<BoxCollider2D>().enabled=false;
		if(GetComponent<PersonajeOnline>().isMine){
			Camera.main.GetComponent<CamFollow>().player=null;
		}
		GetComponent<BoxCollider2D>().enabled=false;
		rig.gravityScale=0f;
		if(transform.localScale.z>=0){
			rig.AddForce(new Vector2(-30f,100f),ForceMode2D.Impulse);
		}else{
			rig.AddForce(new Vector2(30f,100f),ForceMode2D.Impulse);
		}
	}

	public void StopAnimation(){
		if(GetComponent<PersonajeOnline>().isMine){
			anim.speed=0f;
			Invoke("Continuar",Photon.Pun.PhotonNetwork.GetPing()/2000f);
		}
	}

	void Continuar(){
		anim.speed=1f;
	}

	public void ActivarImpulso(float tiempo,Vector2 Offset){
		OffSetImpulse = Offset;
		Invoke("DesactivarImpulso",tiempo);
	}

	public void DesactivarImpulso(){
		OffSetImpulse= new Vector2(0f,0f);
	}
	#endregion

	#region ResolucionDaño
	public List<string> ListaAtacantes;
	string Ejecutor="";

	public void ResolverDaño(float daño, int tipo,string NombreAtacante){
		if(!tocasuelo){
			TipoMuerte=-1;
		}else{
			TipoMuerte=tipo;
		}

		if(HP>0){			
			if(daño>=HP){
				HP=0;
				Invoke("Revivir",3f);
				Ejecutor=NombreAtacante;
				if(Photon.Pun.PhotonNetwork.IsMasterClient){
				GameObject.Find(Ejecutor).GetComponent<Score>().CallUpdateKDA(1,0,0);
				GetComponent<Score>().CallUpdateKDA(0,1,0);
				ReportarAsistencias();
				}
			}else{
				HP-=daño;
				ListaAtacantes.Add(NombreAtacante);
				Invoke("RemoverAsistencia",5f);
			}
		}
	}

	void ReportarAsistencias(){
	List<string> Asistencias= new List<string>();
		foreach(string value in ListaAtacantes){
			if(!Asistencias.Contains(value) && value!=Ejecutor){
				Asistencias.Add(value);
			}
		}
		foreach(string value in Asistencias){
			if(Photon.Pun.PhotonNetwork.IsMasterClient){
			GameObject.Find(value).GetComponent<Score>().CallUpdateKDA(0,0,1);
			}
		}
		ListaAtacantes.Clear();
	}

	void RemoverAsistencia(){
	if(ListaAtacantes.Count>0){
	ListaAtacantes.RemoveAt(0);
	}
	}
	#endregion
}
