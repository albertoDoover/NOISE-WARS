using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script de bandera que se utiliza para obtener puntos
public class Flag : MonoBehaviour {

	public bool liberada; // La bandera esta liberada o sigue en la base
	public Transform Buscar; // Posicion que buscara
	public Vector3 IniPos; // Posicion inicial (para resetear)
	public int teamid; // ID de equipo 
	public BaseScript myBase; // Base de su equipo

	void Start(){
		IniPos = transform.position;
	}

	void Update () {
		if(Buscar!=null && liberada){
			transform.position = new Vector3 (Buscar.position.x,Buscar.position.y+0.5f,Buscar.position.z);
		}else if(!liberada){
			transform.position = IniPos;
		}
	}

	public void reset(){
		Buscar = null;
		liberada = false;
		transform.position = IniPos;
	}

	public void setup(Transform personaje){
		Buscar = personaje;
		liberada = true;
		myBase.carga = 0f;
	}

	public void Soltar(){
		Buscar = null;
	}

}
