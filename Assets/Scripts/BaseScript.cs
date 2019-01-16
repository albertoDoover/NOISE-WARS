using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Script de la base aliada o enemiga

public class BaseScript : MonoBehaviour {
	public Image fillCircle; // Circulo de carga
	public int team; // ID del equipo que propietario de esta base
	public float carga=0f; // Porcentaje de carga para soltar bandera
	bool cargando,puedecargar=true,protegido; // Estados
	public Flag myFlag; // Bandera que arroja esta base
	Personaje PersonajeContacto; // Personaje que esta en contacto con esta base
	public GestorPartida Gestor; // Administrador de partida

	void FixedUpdate(){		
		if(myFlag.liberada){
			fillCircle.gameObject.SetActive (false);
			carga = 0f;
			PersonajeContacto = null;
			cargando = false;
		}else if(protegido){
			fillCircle.gameObject.SetActive (false);
		}else if(cargando && puedecargar){
			puedecargar = false;
			if(carga<100f){
				carga++;
				fillCircle.fillAmount = carga / 100f;
			}else if(PhotonNetwork.isMasterClient && PersonajeContacto!=null){
				carga = 0f;
				myFlag.liberada = true;
				PersonajeContacto.solicitarFlag ();
				fillCircle.gameObject.SetActive (false);
			}
			Invoke ("resetcargando",0.1f);
		}
	}

	void resetcargando(){
		puedecargar = true;
	}

	void OnTriggerEnter2D(Collider2D c){
		if(c.transform.tag=="Player" && !myFlag.liberada){
			if (c.transform.GetComponent<Personaje> ().teamid != team && PersonajeContacto == null) {
				PersonajeContacto = c.transform.GetComponent<Personaje> ();
				cargando = true;
				fillCircle.gameObject.SetActive (true);
			} else if (c.transform.GetComponent<Personaje> ().teamid == team) {
				protegido = true;
			}
		}
	}

	void OnTriggerStay2D(Collider2D c){
		if(c.transform.tag=="Player" && !myFlag.liberada){
			if (c.transform.GetComponent<Personaje> ().teamid != team && PersonajeContacto == null) {
				PersonajeContacto = c.transform.GetComponent<Personaje> ();
				cargando = true;
				fillCircle.gameObject.SetActive (true);
			} else if (c.transform.GetComponent<Personaje> ().teamid == team) {
				if(c.transform.GetComponent<Personaje> ().myFlag.Buscar==c.transform && c.transform.GetComponent<Personaje> ().mine){
					c.transform.GetComponent<Personaje> ().myFlag.Buscar = null;
					c.transform.GetComponent<Personaje> ().myFlag.reset ();
					c.transform.GetComponent<Personaje> ().solicitarGanar ();
				}
				protegido = true;
			}
		}
	}

	void OnTriggerExit2D(Collider2D c){
		if(c.transform.tag=="Player" && !myFlag.liberada){
			PersonajeContacto = null;
			if (c.transform.GetComponent<Personaje> ().teamid != team) {
				cargando = false;
				fillCircle.gameObject.SetActive (false);
			} else if (c.transform.GetComponent<Personaje> ().teamid == team) {
				protegido = false;
			}
		}
	}

}
