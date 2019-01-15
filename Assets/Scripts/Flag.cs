using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {

	public bool liberada;
	public Transform Buscar;
	public Vector3 IniPos;
	public int teamid;
	public BaseScript myBase;

	void Start(){
		IniPos = transform.position;
	}

	void Update () {
		if(Buscar!=null && liberada){
			transform.position = new Vector3 (Buscar.position.x,Buscar.position.y+0.5f,Buscar.position.z);
		}else if(!liberada){
			transform.position = IniPos;
		}
	}

	public void reset(){
		Buscar = null;
		liberada = false;
		transform.position = IniPos;
	}

	public void setup(Transform personaje){
		Buscar = personaje;
		liberada = true;
		myBase.carga = 0f;
	}

	public void Soltar(){
		Buscar = null;
	}

}
