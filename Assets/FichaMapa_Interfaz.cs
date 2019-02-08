using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FichaMapa_Interfaz : MonoBehaviour {

	Mapa_Intefaz GestorMapa;
	public Transform myPersonaje;
	public float LerpSpeed;

	void Start () {
		GestorMapa=GameObject.Find("Mapa_Interfaz").GetComponent<Mapa_Intefaz>();
		transform.SetParent(GestorMapa.transform);
	}
	void Update () {
		if(myPersonaje!=null){
		transform.position=Vector3.Lerp(transform.position,GestorMapa.getPositionMap(myPersonaje),LerpSpeed);
		}
	}
}
