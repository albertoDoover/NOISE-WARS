using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Scrpit para que la camara siga con la distancia apropiada al jugador

public class CamFollow : MonoBehaviour {
	public Transform player; // Jugador activo
	public float distanciax,distanciay,velocidad,shakex,shakey; //Distancia que mantiene en X & Y,Velocidad con la que sigue la jugador, Factor de alteracion enX & Y al simular Shake
	Vector3 newpos; // Posicion actualizada
	public float LimTop=28.7f,LimBot=-18f,LimLeft=-120f,LimRight=188.4f; // Limites hasta donde se puede seguir con la camara
	Animation Anim; // Animaciones

	void Start(){
		Anim=GetComponent<Animation>();
	}

	void Update () {
		if(player!=null){
			newpos = new Vector3 (player.position.x+distanciax+shakex,player.position.y+distanciay,-10f); // Posicion del jugador

			// Verificacion de limites
			if(newpos.x<LimLeft){
				newpos.x=LimLeft;
			}else if(newpos.x>LimRight){
				newpos.x=LimRight;
			}
			if(newpos.y>LimTop){
				newpos.y=LimTop;
			}else if(newpos.y<LimBot){
				newpos.y=LimBot;
			}
			transform.position = Vector3.Lerp(transform.position,newpos,velocidad); // Mover camara
		}
	}

	public void ShakeCamera(string NombreAnimacion){ // Animar 
		if(Anim.IsPlaying("CamaraLeve") && NombreAnimacion=="CamaraHard"){
			Anim.Play(NombreAnimacion);
		}else if(!Anim.isPlaying){
			Anim.Play(NombreAnimacion);
		}
	}


}
