using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script ayuda a mantener una escala contante del objeto referente a otro
public class EscalaConstante : MonoBehaviour {

	public Transform Personaje;
	float DistanciaX,DistanciaY,EscalaX,EscalaY;

	void Awake(){
		DistanciaX=transform.position.x-Personaje.position.x;
		DistanciaY=transform.position.y-Personaje.position.y;
		transform.SetParent(null);
	}

	void Update(){
		transform.position=new Vector3(Personaje.position.x+DistanciaX,Personaje.position.y+DistanciaY,transform.position.z);
	}
}
