using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Esta clase se utiliza para colocar las especificaciones de un ataque
public class AtaqueScript : MonoBehaviour {

	public Personaje myCharacter; // Personaje que lanza el ataque
	public bool Nock,Continuo,Push,Pull,Hard=false; // Stun
	public int damage,tipo; // Cantidad de daño que otorga
	public List<string> ColisionesDetectadas; // Lista de objetos con los que ha colisionado para no repetir
	public float LifeTime,StartTime; // Tiempo para finalizar colision, Tiempo para iniciar colision
	public float Tiempo,Distancia; // Velocidad Lerp àra Push, Distancia para Push

	void Start(){
		ColisionesDetectadas=new List<string>();
	}

	void OnEnable(){
	GetComponent<Collider2D>().enabled=true;
	Invoke("TurnOn",StartTime);
	Invoke("TurnOff",LifeTime);
	}

	void TurnOn(){ // Iniciar Colision
		GetComponent<Collider2D>().enabled=true;
	}

	void TurnOff(){ // Desactivar colision
		GetComponent<Collider2D>().enabled=false;
	}

	void OnDisable(){
		ColisionesDetectadas.Clear();
	}

	public void AttackEffect(Transform Personaje){ // Transmitir efecto del ataque
		if(Pull){
			StartCoroutine(Personaje.GetComponent<Personaje>().Pulling(Tiempo,myCharacter.transform.position));
		}else if(Push){
			Personaje.GetComponent<Personaje>().Pulsing((Personaje.transform.position-myCharacter.transform.position).normalized*Distancia,Tiempo);
		}
	}
}
