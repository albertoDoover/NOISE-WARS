using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Esta clase se utiliza para colocar las especificaciones de un ataque
public class AtaqueScript : MonoBehaviour {

	public Personaje myCharacter; // Personaje que lanza el ataque
	public bool Nock,Continuo; // Stun
	public int damage,tipo; // Cantidad de daño que otorga
	public List<string> ColisionesDetectadas; // Lista de objetos con los que ha colisionado para no repetir
	public float LifeTime,StartTime;

	void Start(){
		ColisionesDetectadas=new List<string>();
	}

	void OnEnable(){
	GetComponent<Collider2D>().enabled=true;
	Invoke("TurnOn",StartTime);
	Invoke("TurnOff",LifeTime);
	}

	void TurnOn(){
	GetComponent<Collider2D>().enabled=true;
	}

	void TurnOff(){
	GetComponent<Collider2D>().enabled=false;
	}

	void OnDisable(){
		ColisionesDetectadas.Clear();
	}
}
