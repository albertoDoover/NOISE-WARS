using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fantin_Personaje : MonoBehaviour {

	public GameObject Ulti;
	public Transform UltiPoint;

	public void CrearFlama(){
		Instantiate(Ulti,UltiPoint.position,Ulti.transform.rotation).GetComponent<AtaqueScript>().myCharacter=GetComponent<Personaje>();
	}

}
