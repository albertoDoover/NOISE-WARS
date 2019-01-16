using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Botones que envan comandos a jugador
public class BotonAccion : MonoBehaviour {

	public Personaje PersonajeControlable; // Personaje activo del jugador actual
	public bool push; // El boton ha sido presionado
	public int code=0; // Codigo de comando

	public void PushButton(){
		if(!push && PersonajeControlable!=null){
			push = true;
			if(code==-1){
			PersonajeControlable.saltar();
			}else if(code==0){
			PersonajeControlable.ataquebasico ();
			}else if(code==1){
			PersonajeControlable.especial1();
			}else if(code==2){
			PersonajeControlable.especial2();
			}else if(code==3){
			PersonajeControlable.especial3();
			}
		}
	}
}
