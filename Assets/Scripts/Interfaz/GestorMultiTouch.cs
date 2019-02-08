using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Script para administrar los multiples toques que realiza el usuario de moviles a los botones
public class GestorMultiTouch : MonoBehaviour {

	public Transform[] Botones,Flechas; // Botones en la interfaz
	List<Transform> BotonesActivos,FlechasActivas; // Botones en la interfaz presionados
	Vector3 touchworld; // Posicion de toque
	Vector2 pos; // Posicion de toque en Vector2
	RaycastHit2D hit2d; // Raycast emitido en busqueda de colision
	public Personaje PersonajeActivo; // Personaje del jugador

	void Start () {
		if(!Application.isMobilePlatform){
			gameObject.SetActive (false);
		}
		BotonesActivos = new List<Transform>();	
		FlechasActivas = new List<Transform>();	
	}

	void Update () {
		BotonesActivos.Clear ();
		FlechasActivas.Clear ();
		int direccion = 0;
		if (Application.isMobilePlatform) {
			if (Input.touchCount > 0) {
				for (int i = 0; i < Input.touchCount; i++) {
					if (Input.GetTouch (i).phase != TouchPhase.Began) {
						touchworld = Camera.main.ScreenToWorldPoint (Input.GetTouch (i).position);
						pos = new Vector2 (touchworld.x, touchworld.y);
						hit2d = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.GetTouch (i).position), Input.GetTouch (i).position);
						if (hit2d.collider != null) {
							if (hit2d.transform.gameObject.tag == "BotonControl") {
								hit2d.transform.gameObject.GetComponent<BotonAccion> ().PushButton();
								BotonesActivos.Add (hit2d.transform);
							}else if (hit2d.transform.gameObject.name == "FlechaDerecha") {
								direccion = 1;
							} else if (hit2d.transform.gameObject.name == "FlechaIzquierda") {
								direccion = -1;
							} 
						}
					}
				}
			}
		} else {
			if (Input.GetMouseButton (0)) {
				touchworld = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				pos = new Vector2 (touchworld.x, touchworld.y);
				hit2d = Physics2D.Raycast (pos, Camera.main.transform.forward);
				if (hit2d.collider != null) {
					if (hit2d.transform.gameObject.tag == "BotonControl") {
						hit2d.transform.gameObject.GetComponent<BotonAccion> ().PushButton();
						BotonesActivos.Add (hit2d.transform);
					} else if (hit2d.transform.gameObject.name == "FlechaDerecha") {
						direccion = 1;
					} else if (hit2d.transform.gameObject.name == "FlechaIzquierda") {
						direccion = -1;
					} 
				}
			}
		}

		if(PersonajeActivo!=null){
			if (direccion == 1) {
				PersonajeActivo.MoveRight ();
			} else if (direccion == -1) {
				PersonajeActivo.MoveLeft ();
			} else {
				PersonajeActivo.MoveZero ();
			}
		}

		for(int i=0;i<Botones.Length;i++){
			if(!BotonesActivos.Contains(Botones[i])){
				Botones [i].GetComponent<BotonAccion> ().push=false;
			}
		}
	}
}
