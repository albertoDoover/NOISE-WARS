using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

//Codigo del personaje Aingeru. Contiene el manejo de sus animaciones y creacion de proyectiles

public class Aingueru : MonoBehaviour {
	public UnityArmatureComponent armature;
	public bool corriendo,atacando,reflectando,tocasuelo,subiendo,combo,noqueado,volar,golpe,IA; // Variables para el animator
	public string stanimation; // Nombre animacion
	public float vel = 0f,radio,sizex,lerp; // Velocidad, radio de colision con suelo, Escala horizontal, Lerp para interpolar movimiento
	Rigidbody2D rig;
	public LayerMask mascarasuelo; // Identificador de suelo
	public UnityEngine.Transform comprobadorsuelo,ultipoint,inicio,DardoPosicion; // Posicion de los pies, Posicion para iniciar ataque especial, Posicion de reinicio
	int numeroATK=0; // Patron del ataque
	public GameObject Dardo,Ulti;
	public AudioClip SonidoGolpe,SonidoReflectar,SonidoViento;
	public AudioSource AudioComponent;
	#region VariablesSalto 
	public float SaltoUpfg,SaltoDown,FactorSalto,DuracionSalto,SaltoLerp,Salto; // Velocidad al subir, velocidad al bajar, Velocidad activa, Interpolacion de salto
	int SaltoDisp=0; // Conteo para doble salto
	float gravedad; // Fuerza con la que cae
	#endregion

	#region Start Update //Metodos basicos de Unity
	void Awake(){
		rig = GetComponent<Rigidbody2D>();
	}

	void Start () {		
		FactorSalto = SaltoDown;
		armature.AddDBEventListener (DragonBones.EventObject.FRAME_EVENT,readEvent);
		armature.AddDBEventListener (DragonBones.EventObject.COMPLETE,finishEvent);
	}

	void Update () {
		tocasuelo = Physics2D.OverlapCircle (comprobadorsuelo.position, radio, mascarasuelo);
		if (rig.velocity.y > 0.1f) {
			subiendo = true;
		} else if (rig.velocity.y <= 0f) {
			subiendo = false;
		}
		if (!IA) {
			if (tocasuelo && !subiendo) {
				if (!atacando) {	
					if (Input.GetKey (KeyCode.A)) {
						especial1 ();
					} else {
						reflectando = false;
						if (Input.GetKeyDown (KeyCode.S)) {
							ataquebasico ();
						} else if (Input.GetKeyDown (KeyCode.D)) {
							especial2 ();
						} else if (Input.GetKeyDown (KeyCode.W) && false) {
							especial1 ();
						} else {
							corriendo = false;
						}
					}
				} else {
					corriendo = false;
				}					
			}
			if (Input.GetKey (KeyCode.LeftArrow) && !atacando && !reflectando) {
				MoveLeft ();
			} else if (Input.GetKey (KeyCode.RightArrow) && !atacando && !reflectando) {
				MoveRight ();
			} else if (!Application.isMobilePlatform) {
				MoveZero ();
			}
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				saltar ();
			}	
		} else {
			especial1 ();
		}
	}
	#endregion

	#region Movimientos
	public  void MoveLeft(){
		if (!noqueado && !atacando && !reflectando) {
			corriendo = true;
			rig.velocity = new Vector2 (-vel, rig.velocity.y);
			armature.armature.flipX = true;
			if(rig.velocity.y==0){
			setDragonBonesAnimation ("ESPRINTAR",0);
			}
		} else {
			MoveZero ();
		}
	}

	public void MoveRight(){
		if (!noqueado && !atacando && !reflectando) {
			corriendo = true;
			rig.velocity = new Vector2 (vel, rig.velocity.y);
			armature.armature.flipX = false;
			if(rig.velocity.y==0){
				setDragonBonesAnimation ("ESPRINTAR",0);
			}
		} else {
			MoveZero ();
		}
	}

	void WindAttack(){
		AudioComponent.PlayOneShot (SonidoViento);
		Ulti.SetActive (true);
	}

	public void MoveZero(){
		if(tocasuelo && !atacando && !reflectando){
		setDragonBonesAnimation ("IDLE",0);
		}else if(!tocasuelo && rig.velocity.y==0f){
		setDragonBonesAnimation ("WIGGLE",0);
		}
		corriendo = false;
		rig.velocity = new Vector2 (0f, rig.velocity.y);
	}

	public void ataquebasico(){
		if(!atacando && tocasuelo && !subiendo && !reflectando){
			atacando = true;
			setDragonBonesAnimation ("ESPECIAL_2",1);
		}
	}

	public void especial1(){
		if(!reflectando && tocasuelo && !subiendo){
			reflectando = true;
			setDragonBonesAnimation ("ESPECIAL_1",1);
		}
	}

	public void especial2(){
		if(!atacando && tocasuelo && !subiendo && !reflectando){
			atacando = true;
			setDragonBonesAnimation ("ULTI",1);
		}
	}

	public void especial3(){
		if(!atacando && tocasuelo && !subiendo && !reflectando){
			atacando = true;
			setDragonBonesAnimation ("ULTI",1);
		}
	}

	public void saltar(){
		if (SaltoDisp>0 && !atacando && !noqueado && !reflectando) {
			SaltoDisp--;
			rig.velocity = new Vector2 (rig.velocity.x,0f);
			rig.AddForce (new Vector2(0f,Salto),ForceMode2D.Impulse);
			setDragonBonesAnimation ("SALTO",0);
		}
	}

	void endNoqueado(){
		noqueado = false;
	}

	void revivir(){
		volar = false;
		rig.freezeRotation = true;
		rig.velocity = Vector2.zero;
		rig.rotation = 0f;
		transform.position = inicio.position;
		rig.gravityScale = gravedad;
	}

	#endregion

	#region Colisiones
	void OnCollisionEnter2D(Collision2D c){
		if(c.transform.tag=="Suelo"){
			SaltoDisp = 2;
		}
	}

	void OnTriggerEnter2D(Collider2D c){
		if(c.tag=="Golpe" && IA){
			if(!reflectando){
				setDragonBonesAnimation ("DAÑO",1);
				AudioComponent.PlayOneShot (SonidoGolpe);
			}else{
				setDragonBonesAnimation ("REFLECTAR",1);
				AudioComponent.PlayOneShot (SonidoReflectar);
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

	public void mandarvolar(){
		Invoke ("revivir",3f);
		rig.gravityScale = 0;
		rig.freezeRotation = false;
		rig.AddTorque (200f);
		Vector2 dir = new Vector2 (((transform.localScale.x/Mathf.Abs(transform.localScale.x))*50f),200f);
		rig.AddForce (dir,ForceMode2D.Impulse);
	}

	public void spritemostrar(){
		Debug.Log ("mostrar");
	}

	void spriteocultar(){
		Debug.Log ("no mostrar");
	}

	void spritetransparencia(){
		Debug.Log ("transparente");
	}


	void CancelarSalto(){
		FactorSalto = SaltoDown;
	}

	public void finishattack(){
		Ulti.SetActive (false);
		atacando = false;
		numeroATK = -1;
	}

	void LanzarDardo(){
		if(armature.armature.flipX){
			Instantiate (Dardo,DardoPosicion.position,Quaternion.identity).GetComponent<DardoAingueru>().Shoot(-1,transform.name);
		}else{
			Instantiate (Dardo,DardoPosicion.position,Quaternion.identity).GetComponent<DardoAingueru>().Shoot(1,transform.name);
		}
	}

	public void combooff(){
		combo = false;
	}
	#endregion

	#region Dragonbones
	void readEvent(string nombre,DragonBones.EventObject c){
		Debug.Log (c.name+" activado");
		Invoke (c.name,0f);
	}

	void finishEvent(string nombre,DragonBones.EventObject c){
		if(armature.animation.lastAnimationName!="ESPECIAL_1"){
		if (tocasuelo) {
			setDragonBonesAnimation ("IDLE", 0);
		} else {
			setDragonBonesAnimation ("SALTO",0);
		}
		Ulti.SetActive (false);
		atacando = false;
		reflectando = false;
		numeroATK = -1;
		}
	}

	void setDragonBonesAnimation(string nombre,int times){
		if(armature.animation.lastAnimationName!=nombre){
			armature.animation.FadeIn (nombre, 0f, times);	
		}
	}

	#endregion

}
