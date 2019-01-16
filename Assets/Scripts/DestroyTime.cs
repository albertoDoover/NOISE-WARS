using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Script para destruir objeto despues de cierto tiempo
public class DestroyTime : MonoBehaviour {

	public float tiempo; // Tiempo de vida del objeto

	void Start () {
		Invoke ("destruir",tiempo);
	}
	
	void destruir(){
		Destroy (gameObject);
	}
}
