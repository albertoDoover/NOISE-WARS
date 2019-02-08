using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Codigo del control del personaje. Incluye control de animaciones, colisiones y sincronizacion online

public class Personaje : Photon.MonoBehaviour {
	public bool corriendo,atacando,agachado,tocasuelo,subiendo,combo,noqueado,volar,golpe; // Variables para el animator
	public bool mine=false; // Objeto perteneciente al jugador activo
	Animator anim; // Animator del personaje
	AnimatorOverrideController animatorOver; // Controlador para sobreescribir animator
	public string stanimation; // Nombre animacion
	public Image BarraVida,BarraFondo,BarraRoja; // Barras vida
	public Canvas MyCanvas; // Interfaz
	public float vel = 0f,radio,sizex,lerp; // Velocidad, radio de colision con suelo, Escala horizontal, Lerp para interpolar movimiento
	Rigidbody2D rig;
	public SpriteRenderer spi; // Cuerpo del personaje
	public LayerMask mascarasuelo; // Identificador de suelo
	public Transform comprobadorsuelo,ultipoint,inicio; // Posicion de los pies, Posicion para iniciar ataque especial, Posicion de reinicio
	public int teamid; // Identificador de equipo
	public GameObject ulti,especial,comboeffect; // Prefabs de la ulti, ataque especial y combo
	int numeroATK=0; // Patron del ataque
	public Flag myFlag,EnemyFlag; // Bandera de equipo y enemiga
	GestorPartida Gestor; // Administrador de la partida (puntaje,kills y mensajes)
	#region VariablesSalto 
	public float SaltoUpfg,SaltoDown,FactorSalto,DuracionSalto,SaltoLerp,Salto; // Velocidad al subir, velocidad al bajar, Velocidad activa, Interpolacion de salto
	int SaltoDisp=0; // Conteo para doble salto
	string MyName; // Nombre de jugador
	float gravedad; // Fuerza con la que cae
	#endregion
	public AnimationClip[] animaciones; // Conjunto de animaciones para las replicas de jugadores
	#region Estadisticas
	public float HPMAX,HP; // Vida maxima, Vida actual
	#endregion

	#region Start Update //Metodos basicos de Unity
	void Awake(){
		rig = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		animatorOver = new AnimatorOverrideController (anim.runtimeAnimatorController);
		anim.runtimeAnimatorController = animatorOver;
	}

	void Start () {		
		FactorSalto = SaltoDown;
		OnlinePos = new Vector2 (transform.position.x,transform.position.y);
		MyCanvas.worldCamera = Camera.main;
		Gestor = GameObject.Find ("GestorPartida").GetComponent<GestorPartida> ();
		if (GetComponent<PhotonView> ().isMine) {
			mine = true;
			GetComponent<PhotonVoiceRecorder> ().enabled = true;
			Camera.main.GetComponent<CamFollow> ().player = transform;
			if(Application.isMobilePlatform){
			GameObject[] Botones = GameObject.FindGameObjectsWithTag ("BotonControl");
			for(int i=0;i<Botones.Length;i++){
				Botones [i].GetComponent<BotonAccion> ().PersonajeControlable = this;
			}
			GameObject.Find ("GestorMultiTouch").GetComponent<GestorMultiTouch> ().PersonajeActivo = this;
			}
		} else {
			rig.gravityScale=0f;
			GetComponent<PhotonVoiceSpeaker> ().enabled = true;
		}
		if(GetComponent<PhotonView> ().ownerId%2==0){
			teamid = 1;
			myFlag = GameObject.Find ("Flag2").GetComponent<Flag> ();
			EnemyFlag = GameObject.Find ("Flag1").GetComponent<Flag> ();
			inicio = GameObject.Find ("BasePoint1").transform;
		}else{
			teamid = 2;
			myFlag = GameObject.Find ("Flag1").GetComponent<Flag> ();
			EnemyFlag = GameObject.Find ("Flag2").GetComponent<Flag> ();
			inicio = GameObject.Find ("BasePoint2").transform;
		}
		if(GetComponent<PhotonView> ().ownerId==1){
			MyName="Blanco";
			spi.color = Color.white;
		}else if(GetComponent<PhotonView> ().ownerId==2){
			MyName="Verde";
			spi.color = Color.green;
		}else if(GetComponent<PhotonView> ().ownerId==3){
			MyName="Azul";
			spi.color = Color.blue;
		}else if(GetComponent<PhotonView> ().ownerId==4){
			MyName="Rojo";
			spi.color = Color.red;
		}
		transform.name = MyName;
	}

	void Update () {
		BarraVida.fillAmount = HP / HPMAX;
		if (mine) {
			tocasuelo = Physics2D.OverlapCircle (comprobadorsuelo.position, radio, mascarasuelo);
				if (rig.velocity.y > 0.1f) {
					subiendo = true;
				} else if (rig.velocity.y <= 0f) {
					subiendo = false;
				}
				if (tocasuelo && !subiendo) {
					if (!atacando) {					
						if (Input.GetKey(KeyCode.S)) {
						ataquebasico ();
						} else if (Input.GetKeyDown (KeyCode.A)) {
						especial3 ();
						} else if (Input.GetKeyDown (KeyCode.D)) {
						especial2 ();
						} else if (Input.GetKeyDown (KeyCode.W) && false) {
						especial1 ();
						}						
					} else {
						corriendo = false;
						agachado = false;
					}
				} else {
					corriendo = false;
					agachado = false;
				}
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				saltar ();
				}
				
			if (Input.GetKey (KeyCode.LeftArrow) && !atacando) {
			    MoveLeft ();
			} else if (Input.GetKey (KeyCode.RightArrow) && !atacando) {
				MoveRight ();
			} else if(!Application.isMobilePlatform){
				MoveZero ();
			}
			anim.SetBool ("volar", volar);
			anim.SetInteger ("NumeroAtaque", numeroATK);
			anim.SetBool ("ensuelo", tocasuelo);
			anim.SetBool ("subiendo", subiendo);
			anim.SetBool ("corriendo", corriendo);
			anim.SetBool ("atacando", atacando);
			anim.SetBool ("agachado", agachado);
			anim.SetBool ("combo", combo);
			anim.SetBool ("noqueado", noqueado);
		} else if(HP>0){
			setPositionOnline ();
		}
	}
	#endregion

	#region Movimientos
	public  void MoveLeft(){
		if (!noqueado && !atacando && HP > 0) {
			corriendo = true;
			rig.velocity = new Vector2 (-vel, rig.velocity.y);
			transform.localScale = new Vector3 (sizex, transform.localScale.y, 0f);
		} else {
			MoveZero ();
		}
	}

	public void MoveRight(){
		if (!noqueado && !atacando && HP > 0) {
			corriendo = true;
			rig.velocity = new Vector2 (vel, rig.velocity.y);
			transform.localScale = new Vector3 (-sizex, transform.localScale.y, 0f);
		} else {
			MoveZero ();
		}
	}

	public void MoveZero(){
		corriendo = false;
		rig.velocity = new Vector2 (0f, rig.velocity.y);
	}

	public void ataquebasico(){
		if(!atacando && tocasuelo && !subiendo){
			golpe = true;
			atacando = true;
			numeroATK = 0;
		}
	}

	public void especial1(){
		if(!atacando && tocasuelo && !subiendo){
			atacando = true;
			numeroATK = 1;
		}
	}

	public void especial2(){
		if(!atacando && tocasuelo && !subiendo){
			atacando = true;
			numeroATK = 2;
		}
	}

	public void especial3(){
		if(!atacando && tocasuelo && !subiendo){
			atacando = true;
			numeroATK = 3;
		}
	}

	public void saltar(){
		if (SaltoDisp>0 && !atacando&& !noqueado) {
			SaltoDisp--;
			rig.velocity = new Vector2 (rig.velocity.x,0f);
			rig.AddForce (new Vector2(0f,Salto),ForceMode2D.Impulse);
		}
	}

	void endNoqueado(){
		noqueado = false;
	}

	void revivir(){
		volar = false;
		if (mine) {
			Camera.main.GetComponent<CamFollow> ().player = transform;
		}
		rig.freezeRotation = true;
		rig.velocity = Vector2.zero;
		rig.rotation = 0f;
		transform.position = inicio.position;
		rig.gravityScale = gravedad;
		HP = HPMAX;
	}

	#endregion

	#region Colisiones
	void OnCollisionEnter2D(Collision2D c){
		if(c.transform.tag=="Suelo"){
			SaltoDisp = 2;
		}
	}

	void OnTriggerEnter2D(Collider2D c){
		if(c.transform.tag=="Escondite" && !atacando){
			if (Gestor.myteam == teamid) {
				spritetransparencia ();
			} else {
				if(spi.isVisible){
					spritetransparencia ();
				}else{
					spriteocultar ();
				}
			}

		}
		if(c.transform.tag=="Ataque"){
			spritemostrar ();
			if(c.GetComponent<AtaqueScript>().myCharacter.teamid!=teamid && HP>0 && mine){
				if (c.GetComponent<AtaqueScript> ().damage >= HP) {
				HP = 0;
				if(mine){				
				Camera.main.GetComponent<CamFollow> ().player = null;
				if (myFlag.Buscar != null) {
					if (myFlag.Buscar.transform.name == MyName) {
						Gestor.muerte ("Bandera"+teamid+" Recuperada");
						myFlag.reset();
					}
				}
				volar = true;				
				photonView.RPC ("solicitarMorirOnline", PhotonTargets.AllBufferedViaServer);

				}
				}else{
				if (c.GetComponent<AtaqueScript> ().nock && mine) {
					noqueado = true;
					Invoke ("endNoqueado", 2f);
					StartCoroutine (Camera.main.GetComponent<CamFollow> ().shakeeffect ());					
				}
					HP -= c.GetComponent<AtaqueScript> ().damage;
			}
			}
		}
	}

	void OnTriggerExit2D(Collider2D c){
		if(c.transform.tag=="Escondite"){
			spritemostrar ();
		}
	}
	#endregion

	#region efectos
	public void solicitarGanar(){
		photonView.RPC ("solicitarGanarOnline",PhotonTargets.AllBufferedViaServer);
	}

	public void solicitarFlag(){
		photonView.RPC ("solicitarFlagOnline",PhotonTargets.AllBufferedViaServer);
	}

	public void regresarFlag(){
		photonView.RPC ("solicitarRegresarOnline",PhotonTargets.AllBufferedViaServer);
	}

	public void mandarvolar(){
		Invoke ("revivir",3f);
		rig.gravityScale = 0;
		rig.freezeRotation = false;
		rig.AddTorque (200f);
		Vector2 dir = new Vector2 (((transform.localScale.x/Mathf.Abs(transform.localScale.x))*50f),200f);
		rig.AddForce (dir,ForceMode2D.Impulse);
	}

	public void spritemostrar(){
		spi.color = new Color (spi.color.r,spi.color.g,spi.color.b,1f);
		BarraFondo.color = new Color (BarraFondo.color.r,BarraFondo.color.g,BarraFondo.color.b,1f);
		BarraVida.color = new Color (BarraVida.color.r,BarraVida.color.g,BarraVida.color.b,1f);
		BarraRoja.color = new Color (BarraRoja.color.r,BarraRoja.color.g,BarraRoja.color.b,1f);
	}

	void spriteocultar(){
		spi.color = new Color (spi.color.r,spi.color.g,spi.color.b,0f);
		BarraFondo.color = new Color (BarraFondo.color.r,BarraFondo.color.g,BarraFondo.color.b,0f);
		BarraVida.color = new Color (BarraVida.color.r,BarraVida.color.g,BarraVida.color.b,0f);
		BarraRoja.color = new Color (BarraRoja.color.r,BarraRoja.color.g,BarraRoja.color.b,0f);
	}

	void spritetransparencia(){
		spi.color = new Color (spi.color.r,spi.color.g,spi.color.b,55f/255f);
	}


	void CancelarSalto(){
		FactorSalto = SaltoDown;
	}

	public void finishattack(){
		atacando = false;
		numeroATK = -1;
	}

	public void combooff(){
		combo = false;
	}

	public void ultiattack(){
		Transform creacion = Instantiate (ulti, ultipoint.position, Quaternion.identity).transform;
		creacion.GetComponent<AtaqueScript>().myCharacter=this;
		creacion.GetComponent<Collider2D> ().enabled = true;
	}
	public void especialattack(){
		Transform creacion = Instantiate (especial,ultipoint.position,Quaternion.identity).transform;
		creacion.GetComponent<AtaqueScript>().myCharacter=this;
		creacion.GetComponent<Collider2D> ().enabled = true;
		creacion.GetComponent<Animator> ().enabled = true;
	}
	public void comboattack(){
		Transform creacion = Instantiate (comboeffect,ultipoint.position,Quaternion.identity).transform;
		creacion.GetComponent<AtaqueScript>().myCharacter=this;
		creacion.GetComponent<Collider2D> ().enabled = true;
	}
	#endregion

	#region PhotonOnline // Region de metodos Online
	Vector2 OnlinePos;
	float sentido;
	string LastAnimation="";
	bool canChangeAnim=true;

	void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info){
		if(stream.isWriting){			
			stream.SendNext (rig.position);
			stream.SendNext (transform.localScale.x);
			stream.SendNext (HP);
			//animaciones
			stream.SendNext (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name);
		}else{
			OnlinePos = (Vector2)stream.ReceiveNext (); 
			sentido = (float)stream.ReceiveNext ();
			transform.localScale = new Vector3(sentido,transform.localScale.y,0f);
			HP = (float)stream.ReceiveNext ();
			stanimation=(string)stream.ReceiveNext ();
			if(LastAnimation!=stanimation && canChangeAnim){
				LastAnimation = stanimation;
				for(int i=0;i<animaciones.Length;i++){
					if(LastAnimation==animaciones[i].name){
							if(LastAnimation!="idle" && LastAnimation!="correr" && LastAnimation!="noqueo" && LastAnimation!="salto"){
								canChangeAnim = false;
								Invoke ("ActchangeAnim",animaciones [i].length);								
							}
							animatorOver ["idle"] = animaciones [i];
						anim.Play ("idle",-1,0f);
					} 
				}
			}
			}
	}

	void ActchangeAnim(){
		canChangeAnim = true;
	}

	void setPositionOnline(){
		Vector2 interpolatePosition = Vector2.Lerp (rig.position,OnlinePos,lerp);
		if(Vector2.Distance(rig.position,OnlinePos)>3f){
			interpolatePosition = OnlinePos;
		}
		rig.position = interpolatePosition;
	}

	[PunRPC]
	void solicitarFlagOnline(){
		Gestor.showmensajebandera (teamid);
		myFlag.setup (transform);
	}

	[PunRPC]
	void solicitarRegresarOnline(){
		Gestor.muerte ("Bandera"+teamid+" Recuperada");
		EnemyFlag.reset();
	}

	[PunRPC]
	void solicitarSoltarOnline(){
		myFlag.Soltar();
	}

	[PunRPC]
	void solicitarGanarOnline(){
		myFlag.reset ();
		Gestor.ganar (teamid);
	}

	[PunRPC]
	void solicitarMorirOnline(){
	if (myFlag.Buscar != null) {
		if (myFlag.Buscar.transform.name == MyName) {
			Gestor.muerte ("Bandera"+teamid+" Recuperada");
			myFlag.reset();
		}
	}
		if (Gestor.myteam == teamid) {
			Gestor.deaths++;
		} else {
			Gestor.kills++;
		}
	if(mine){
	  Gestor.muerte ("Te han matado");
	}else{
	  Gestor.muerte (MyName+" ha muerto");
	}

	}
	#endregion
}
