using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Codigo del mapa en la interfaz. Devuelve la posicion de la ficha dentro del minimapa en relacion al personaje en el escenario
public class Mapa_Intefaz : MonoBehaviour {

	public Transform TopMapa,BotMapa,LeftMapa,RightMapa,TopArena,BotArena,LeftArena,RightArena;

	public Vector3 getPositionMap(Transform Personaje){ // Metodo para ubicar ficha
		float xArena=(RightArena.position.x-Personaje.position.x)/(RightArena.position.x-LeftArena.position.x),yArena=(TopArena.position.y-Personaje.position.y)/(TopArena.position.y-BotArena.position.y);
		Vector3 PosicionEnMapa = new Vector3(RightMapa.position.x-(RightMapa.position.x-LeftMapa.position.x)*xArena,TopMapa.position.y-(TopMapa.position.y-BotMapa.position.y)*yArena,0f);
		return PosicionEnMapa;
	}
}
