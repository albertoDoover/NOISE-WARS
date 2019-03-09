using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Codigo especifico para personaje Fantin. Se incluye generacion de especial1 y ultimate (El especial 2 se incluye en la animacion de Fantin)
public class Fantin_Personaje : MonoBehaviour {

	public GameObject Ulti,Basico,Especial;
	public Transform UltiPoint,BasicoPoint,EspecialPoint;
	public float VelocidadEspecial;
	public Personaje myPersonaje;

	public void CrearFlama(){ // Ultimate
		Instantiate(Ulti,UltiPoint.position,Ulti.transform.rotation).GetComponent<AtaqueScript>().myCharacter=GetComponent<Personaje>();
	}

	public void CrearBasico(){ // Ataque basico (Explosion)
		Instantiate(Basico,BasicoPoint.position,Basico.transform.rotation).GetComponent<AtaqueScript>().myCharacter=GetComponent<Personaje>();
	}

	public void CrearEspecial1(){
		Transform creacion = Instantiate(Especial,EspecialPoint.position,Especial.transform.rotation).transform;
		float Velocidad=VelocidadEspecial;
			if(transform.localScale.x<=0){
				creacion.localScale=new Vector3(creacion.localScale.x*-1,creacion.localScale.y,1f);
				Velocidad=VelocidadEspecial*-1f;
			}
		creacion.position=EspecialPoint.position;
		creacion.GetComponent<Rigidbody2D>().velocity= new Vector2(Velocidad,0f);
		creacion.GetComponent<AtaqueScript>().myCharacter=GetComponent<Personaje>();
	}

}
