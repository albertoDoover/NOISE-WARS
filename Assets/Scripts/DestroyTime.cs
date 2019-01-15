using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTime : MonoBehaviour {

	public float tiempo;

	void Start () {
		Invoke ("destruir",tiempo);
	}
	
	void destruir(){
		Destroy (gameObject);
	}
}
