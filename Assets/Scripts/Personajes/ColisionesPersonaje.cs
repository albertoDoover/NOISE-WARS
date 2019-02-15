using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ColisionesPersonaje : MonoBehaviour {

	Personaje myPersonaje;
	PersonajeOnline myPersonajeOnline;
	public SpriteRenderer spi; // Cuerpo del personaje
	public BoxCollider2D CajaColision;
	public Transform EjeColision;
	public LayerMask Mascara;
	Collider2D[] Colisiones;

	void Start(){
		myPersonaje=GetComponent<Personaje>();
		myPersonajeOnline=GetComponent<PersonajeOnline>();
	}

	void Update(){
		Colisiones=new Collider2D[0];
		Colisiones=Physics2D.OverlapBoxAll(EjeColision.position,new Vector2(CajaColision.size.x,CajaColision.size.y),0f,Mascara.value);
			for(int i=0;i<Colisiones.Length;i++){
				if(Colisiones[i].GetComponent<AtaqueScript>().myCharacter!=null){
					if(Colisiones[i].GetComponent<AtaqueScript>().myCharacter.name!=name){
						if(!Colisiones[i].GetComponent<AtaqueScript>().ColisionesDetectadas.Contains(transform.name) || Colisiones[i].GetComponent<AtaqueScript>().Continuo){
						Colisiones[i].GetComponent<AtaqueScript>().ColisionesDetectadas.Add(transform.name);
						myPersonaje.HitOn();
						if(myPersonajeOnline.isMine){
							Colisiones[i].GetComponent<AtaqueScript>().AttackEffect(transform);
						}
						if(PhotonNetwork.IsMasterClient){
							myPersonajeOnline.enviarDaño(Colisiones[i].GetComponent<AtaqueScript>().damage*myPersonaje.FactorDaño,Colisiones[i].GetComponent<AtaqueScript>().tipo);
						}
				}
			  }
			}
		}
	}


	#region Colisiones
	void OnTriggerEnter2D(Collider2D c){
		if(c.transform.tag=="Escondite" && !myPersonaje.atacando){
			if(myPersonajeOnline.isMine){
				myPersonaje.spritetransparencia();
			}else{
				myPersonaje.spriteocultar();
			}
		}
	}

	void OnTriggerExit2D(Collider2D c){
		if(c.transform.tag=="Escondite"){
			myPersonaje.spritemostrar ();
		}
	}
	#endregion


}
