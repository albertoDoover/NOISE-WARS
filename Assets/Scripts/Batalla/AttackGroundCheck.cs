using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase para ataques terrestes que recorren todo el suelo
public class AttackGroundCheck : MonoBehaviour {

	public Transform Check; // Guia para saber si colisiona con suelo
	public float Radio=0.05f,LifeTime=1f; // Radio para chequear colision suelo, Tiempo para destruir
	bool Activado=true; // Chequear colision?
	public ParticleSystem[] SistemasParticulas; // Sistemas de particualas que deben desactivarse
	public LayerMask Suelo; 
	Collider2D[] Colisiones; // Colisiones detectadas

	void Update () {
		if(Activado){
			Colisiones = Physics2D.OverlapCircleAll(Check.position,Radio,Suelo);
				if(Colisiones.Length==0){
					Activado=false;
						for(int i=0;i<SistemasParticulas.Length;i++){
							SistemasParticulas[i].Stop();
							GetComponent<Collider2D>().enabled=false;
						}
					Invoke("Destruir",LifeTime);
				}
		}	
	}

	void Destruir(){ // Destruir objeto
		Destroy(gameObject);
	}

}
