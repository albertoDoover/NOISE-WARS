using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DardoAingueru : MonoBehaviour {

	public float Speed;
	string Propietario;

	public void Shoot(int sentido,string myChar){
		transform.localScale = new Vector3 (transform.localScale.x*sentido,transform.localScale.y,transform.localScale.z);
		GetComponent<Rigidbody2D> ().velocity = new Vector2(Speed*sentido,0f);
		Propietario = myChar;
		GetComponent<BoxCollider2D> ().enabled = true;
	}

}
