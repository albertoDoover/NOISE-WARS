using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Codigo especifico del personaje Kalani
public class Kalani_Personaje : MonoBehaviour {

	public GameObject Ulti,Especial1,Especial2;
	public Transform Ulti_Point,Especial1_Point,Especial2_Point;
	public Personaje myPersonaje;
	Transform Creacion;
	public float VelocityEspecial1;

	public void LanzarUlti_Kalani(){
		Creacion=Instantiate(Ulti,Ulti_Point.position,Ulti.transform.rotation).transform;
		if(transform.localScale.x>0){
			Creacion.localScale=new Vector3(Creacion.localScale.x*-1,Creacion.localScale.y,Creacion.localScale.z);
		}
		Creacion.position=Ulti_Point.position;
		Creacion.GetComponent<AtaqueScript>().myCharacter=myPersonaje;
	}

	public void LanzarEspecial1_Kalani(){
		Creacion=Instantiate(Especial1,Especial1_Point.position,Especial1.transform.rotation).transform;
		if(transform.localScale.x<0){
			Creacion.localScale=new Vector3(Creacion.localScale.x*-1,Creacion.localScale.y,Creacion.localScale.z);
			Creacion.GetComponent<Rigidbody2D>().velocity=new Vector2(VelocityEspecial1*-1,0f);
		}else{
			Creacion.GetComponent<Rigidbody2D>().velocity=new Vector2(VelocityEspecial1,0f);
		}
		Creacion.GetComponent<AtaqueScript>().myCharacter=myPersonaje;
	}

	public void LanzarEspecial2_Kalani(){
		Creacion=Instantiate(Especial2,Especial2_Point.position,Especial2.transform.rotation).transform;
		Creacion.GetComponent<AtaqueScript>().myCharacter=myPersonaje;
	}
}
