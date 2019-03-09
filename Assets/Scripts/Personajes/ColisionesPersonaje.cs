using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Deteccion de colisiones mediante Physics2D para una deteccion especifica
public class ColisionesPersonaje : MonoBehaviour {

	Personaje myPersonaje;
	PersonajeOnline myPersonajeOnline;
	public SpriteRenderer spi; // Cuerpo del personaje
	public BoxCollider2D CajaColision; // Collider referencial para chequeo de colision
	public Transform EjeColision; // Ubicacion del chequeo de collider
	public LayerMask Mascara;
	Collider2D[] Colisiones; // Colisiones detectadas

	void Start(){
		myPersonaje=GetComponent<Personaje>();
		myPersonajeOnline=GetComponent<PersonajeOnline>();
	}

	void Update(){
		Colisiones=new Collider2D[0]; 
		Colisiones=Physics2D.OverlapBoxAll(EjeColision.position,new Vector2(CajaColision.size.x,CajaColision.size.y),0f,Mascara.value); // Detectar colisiones
			for(int i=0;i<Colisiones.Length;i++){
				if(Colisiones[i].GetComponent<AtaqueScript>().myCharacter!=null){
				if(Colisiones[i].GetComponent<AtaqueScript>().myCharacter.name!=name && Colisiones[i].GetComponent<AtaqueScript>().myCharacter.teamid!=GetComponent<Personaje>().teamid){
						if(!Colisiones[i].GetComponent<AtaqueScript>().ColisionesDetectadas.Contains(transform.name) || Colisiones[i].GetComponent<AtaqueScript>().Continuo){
						Colisiones[i].GetComponent<AtaqueScript>().ColisionesDetectadas.Add(transform.name);
						myPersonaje.HitOn(); // Hit Player
						if(myPersonajeOnline.isMine){ // Aplicacion de efectos
							Colisiones[i].GetComponent<AtaqueScript>().AttackEffect(transform);
						}
						if(PhotonNetwork.IsMasterClient){ // Enviar daño
							myPersonajeOnline.enviarDaño(Colisiones[i].GetComponent<AtaqueScript>().damage*myPersonaje.FactorDaño,Colisiones[i].GetComponent<AtaqueScript>().tipo,Colisiones[i].GetComponent<AtaqueScript>().myCharacter.name);
						}
				}
			  }
			}
		}
	}

	#region Colisiones
	void OnTriggerEnter2D(Collider2D c){ // Entrar en contacto con escondite
		if(c.transform.tag=="Escondite" && !myPersonaje.atacando){
			if(myPersonajeOnline.isMine){
				myPersonaje.spritetransparencia();
			}else{
				myPersonaje.spriteocultar();
			}
		}
	}

	void OnTriggerExit2D(Collider2D c){ // Salir de contacto con escondite
		if(c.transform.tag=="Escondite"){
			myPersonaje.spritemostrar ();
		}
	}
	#endregion
}
