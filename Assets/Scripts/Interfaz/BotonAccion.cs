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
			PersonajeControlable.EspecialUlti(code);
			}else if(code==1){
			PersonajeControlable.EspecialUlti(code);
			}else if(code==2){
			PersonajeControlable.EspecialUlti(code);
			}else if(code==3){
			PersonajeControlable.EspecialUlti(code);
			}
		}
	}
}
