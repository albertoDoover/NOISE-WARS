using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kalani_Personaje : MonoBehaviour {

	public GameObject Ulti;
	public Transform Ulti_Point;
	public Personaje myPersonaje;

	public void LanzarUlti_Kalani(){
		Instantiate(Ulti,Ulti_Point.position,Ulti.transform.rotation).GetComponent<AtaqueScript>().myCharacter=myPersonaje;
	}


}
