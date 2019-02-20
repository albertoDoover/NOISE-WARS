using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Scrpit para que la camara siga con la distancia apropiada al jugador

public class CamFollow : MonoBehaviour {
	public Transform player; // Jugador activo
	public float distanciax,distanciay,velocidad,shakex,shakey; //Distancia que mantiene en X & Y,Velocidad con la que sigue la jugador, Factor de alteracion enX & Y al simular Shake
	Vector3 newpos; // Posicion actualizada
	public float LimTop=28.7f,LimBot=-18f,LimLeft=-120f,LimRight=188.4f;

	void Update () {
		if(player!=null){
			newpos = new Vector3 (player.position.x+distanciax+shakex,player.position.y+distanciay,-10f);
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
			transform.position = Vector3.Lerp(transform.position,newpos,velocidad);
		}
	}

	public IEnumerator shakeeffect(){
		shakex = -1;
		yield return new WaitForSeconds (0.1f);
		shakex = 2;
		yield return new WaitForSeconds (0.1f);
		shakex = -2;
		yield return new WaitForSeconds (0.1f);
		shakex = 1;
		yield return new WaitForSeconds (0.1f);
		shakex = -1;
		yield return new WaitForSeconds (0.1f);
		shakex = 2;
		yield return new WaitForSeconds (0.1f);
		shakex = -2;
		yield return new WaitForSeconds (0.1f);
		shakex = 1;
		yield return new WaitForSeconds (0.1f);
		shakex = 0;
	}


}
