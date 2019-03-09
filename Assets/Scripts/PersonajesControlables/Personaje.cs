using System.Collections;
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
	float gravedad; // Fuerza con la que cae
	public float HPMAX,HP; // Vida maxima, Vida actual
	public GameObject myFicha;
	bool BlockMove;
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
		BarraVida.fillAmount = HP / HPMAX; // Mostrar vida
		tocasuelo=Physics2D.OverlapBox (comprobadorsuelo.position, new Vector2(RadioSuelo,0.05f),0f, mascarasuelo); // Verificar suelo
		wiggle=Physics2D.OverlapCircle (ComprobadorSueloWiggle.position, RadioWiggle, mascarasuelo); // Verificar si posee ambos pies en plataforma

		if (rig.velocity.y > 0.1f) { // Verificar si el personaje sube o baja
			subiendo = true;
			gameObject.layer=11;
		} else if (rig.velocity.y <= 0f) {
			subiendo = false;
			gameObject.layer=9;
		}

		if (GetComponent<PersonajeOnline>().isMine && HP>0 && !BlockMove) {	// Deteccion de botones (PC)		
			if (!subiendo && tocasuelo ) {
				SaltoDisp=2;
					if (!atacando) {					
						if (Input.GetKey(KeyCode.S) && !myGestorPartida.myChat.BarraTexto.gameObject.activeInHierarchy) {
						EspecialUlti (0);
					} else if (Input.GetKeyDown (KeyCode.A)  && !myGestorPartida.myChat.BarraTexto.gameObject.activeInHierarchy) {
						EspecialUlti (1);
					} else if (Input.GetKeyDown (KeyCode.D)  && !myGestorPartida.myChat.BarraTexto.gameObject.activeInHierarchy) {
						EspecialUlti (2);
					} else if (Input.GetKeyDown (KeyCode.W)  && !myGestorPartida.myChat.BarraTexto.gameObject.activeInHierarchy) {
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
				
			if (Input.GetKey (KeyCode.LeftArrow) && !atacando) {
			    MoveLeft ();
			} else if (Input.GetKey (KeyCode.RightArrow) && !atacando) {
				MoveRight ();
			} else if(!Application.isMobilePlatform){
				MoveZero ();
			}
		}
			// Actualizar animator
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
	public  void MoveLeft(){ // Mover personaje a izquierda
		if (!noqueado && !atacando && HP > 0) {
			corriendo = true;
			rig.velocity = (new Vector2 (-vel, rig.velocity.y));
			transform.localScale = new Vector3 (-sizex, transform.localScale.y, 0f);
		} else {
			MoveZero ();
		}
	}

	public void MoveRight(){ // Mover personaje a la derecha
		if (!noqueado && !atacando && HP > 0) {
			corriendo = true;
			rig.velocity = (new Vector2 (vel, rig.velocity.y));
			transform.localScale = new Vector3 (sizex, transform.localScale.y, 0f);
		} else {
			MoveZero ();
		}
	}

	public void MoveZero(){ // Frenar personaje
		corriendo = false;
		rig.velocity = (new Vector2 (0f, rig.velocity.y));
	}

	public void EspecialUlti(int code){ // Realizar basico, moviemiento especial o ultimate
		if(!atacando && tocasuelo && !subiendo){
			ConteoAtk++;
			numeroATK = code;
			atacando = true;
		}
	}

	public void saltar(){ // Salto xd
		if (SaltoDisp>0 && !atacando && HP>0) {
			SaltoDisp--;
			rig.velocity = new Vector2 (rig.velocity.x,0f);
			rig.AddForce (new Vector2(0f,Salto),ForceMode2D.Impulse);
		}
	}

	void endNoqueado(){ // Finalizar Stun
		noqueado = false;
	}

	void Revivir(){ // Reubicar personaje a posicion inicial
		if (GetComponent<PersonajeOnline>().isMine) {
			Camera.main.GetComponent<CamFollow> ().player = transform;
		}
		GetComponent<BoxCollider2D>().enabled=true;
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
	public void DeathSetup(){ // Iniciar conteo para respawn
		rig.velocity=Vector2.zero;
		Invoke ("Revivir",10f);
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

	public void finishattack(){ // Finalizar ataque
		atacando = false;
		numeroATK = -1;
	}

	public void HitOn(){ // Activar color rojizo que indique Hit
		if(!hit){
			hit=true;
			Invoke("HitOff",0.05f);
		}
			spi.color= new Color(248f/255f,162f/255f,107f/255f,1f);
	}

	void HitOff(){ // Desactivar hit
		spi.color= new Color(1f,1f,1f,1f);
		hit=false;
	}

	public void MuerteVolar(){ // Muerte en el aire
		GetComponent<BoxCollider2D>().enabled=false;
		if(GetComponent<PersonajeOnline>().isMine){ // Detener seguimiento de camara
			Camera.main.GetComponent<CamFollow>().player=null;
		}
		GetComponent<BoxCollider2D>().enabled=false; // Desactivar colisiones

		// Impular personaje
		rig.gravityScale=0f;
		if(transform.localScale.z>=0){
			rig.AddForce(new Vector2(-30f,100f),ForceMode2D.Impulse);
		}else{
			rig.AddForce(new Vector2(30f,100f),ForceMode2D.Impulse);
		}
	}

	public void StopAnimation(){ // Detener animacion brevemente de acuerdo al ping
		if(GetComponent<PersonajeOnline>().isMine){
			anim.speed=0f;
			Invoke("Continuar",Photon.Pun.PhotonNetwork.GetPing()/2000f);
		}
	}

	void Continuar(){ // Reanudar animacion
		anim.speed=1f;
	}
	#endregion 

	#region EfectosEnemigos
	public IEnumerator Pulling(float PulseSpeed,Vector2 Impulse){ // Atraer personaje a punto
		BlockMove=true;
		rig.velocity=Vector2.zero;
		rig.gravityScale=0f;
		while(Vector2.Distance(rig.position,Impulse)>1f){
			rig.position=Vector2.Lerp(rig.position,Impulse,PulseSpeed);
			yield return null;
		}
		yield return null;
		ResetBlockMove();
	}

	public void Pulsing(Vector2 Force,float Time){ // Repeler personaje
		BlockMove=true;
		rig.velocity=Vector2.zero;
		rig.gravityScale=0f;
		rig.velocity=Force;
		Invoke("ResetBlockMove",Time);
	}

	void ResetBlockMove(){ // Restaurar habilidad de movimiento del personaje
		rig.gravityScale=gravedad;
		BlockMove=false;
	}
	#endregion

	#region ResolucionDaño
	public List<string> ListaAtacantes;
	string Ejecutor="";

	public void ResolverDaño(float daño, int tipo,string NombreAtacante){ // Restar HP al personaje y tomar registro de quien lo ataca y como
		if(!tocasuelo){
			TipoMuerte=-1;
		}else{
			TipoMuerte=tipo;
		}

		if(HP>0){			
			if(daño>=HP){
				HP=0;
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

	void ReportarAsistencias(){ // Añadir asistencias a jugadores correspondientes
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

	void RemoverAsistencia(){ // Remover posibilidad de asistencia luego de cierto tiempo
	if(ListaAtacantes.Count>0){
	ListaAtacantes.RemoveAt(0);
	}
	}
	#endregion
}
