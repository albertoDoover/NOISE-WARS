using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fantin_Personaje : MonoBehaviour {

	public GameObject Ulti,Basico,Especial;
	public Transform UltiPoint,BasicoPoint,EspecialPoint;
	public float VelocidadEspecial;
	public Personaje myPersonaje;

	public void CrearFlama(){
		Instantiate(Ulti,UltiPoint.position,Ulti.transform.rotation).GetComponent<AtaqueScript>().myCharacter=GetComponent<Personaje>();
	}

	public void CrearBasico(){
		Instantiate(Basico,BasicoPoint.position,Basico.transform.rotation).GetComponent<AtaqueScript>().myCharacter=GetComponent<Personaje>();
	}

	public void AumentarDefensa(){
		CancelInvoke();
		myPersonaje.FactorDaño=0.5f;
		myPersonaje.spi.color=Color.yellow;
		Invoke("RestaurarDefensa",5f);
	}

	void RestaurarDefensa(){
	myPersonaje.FactorDaño=1f;
	myPersonaje.spi.color=Color.white;
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
