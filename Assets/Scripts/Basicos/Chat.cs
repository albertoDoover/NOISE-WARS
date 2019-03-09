using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Chat para room
public class Chat : MonoBehaviour {

	public Text TextoChat; // Chat ubicado a la izquierda de lña pantalla
	public GestorPartida Gestor; // Gestor partida
	List<string> Mensajes; // Mensajes (Maximo 6)
	public InputField BarraTexto; // Barra para introducir mensaje
	public GameObject BotonEnviar; 


	void Start(){
	Mensajes=new List<string>();
	}

	void FixedUpdate(){
	if(Input.GetKeyDown(KeyCode.Return)){ // Iniciar envio de mensaje al presionar Enter
		ActivarTexto();
	}
	}

	public void ActivarTexto(){ // Activar barra de envio de mensaje
	BotonEnviar.SetActive(false);
	BarraTexto.gameObject.SetActive(true);
	BarraTexto.Select();
	}

	public void EnviarTexto(){ // Enviar mensaje
	BotonEnviar.SetActive(true);
	BarraTexto.gameObject.SetActive(false);
	if(Gestor.myPlayer!=null && BarraTexto.text.Trim()!=""){ // Filtrar mensajes nulos
		Gestor.myPlayer.GetComponent<PersonajeOnline>().EnviarMensaje(BarraTexto.text.Trim()); // RPC enviar mensajes
	}
	BarraTexto.text="";
	}

	public void AgregarMensaje(string NuevoMensaje){
		if(Mensajes.Count==6){ // Limitar maximo de mensajes
			Mensajes.RemoveAt(0);
		}
			Mensajes.Add(NuevoMensaje);
			Adaptar();
	}

	void Adaptar(){ // Añadir nuevo mensaje
		string NuevoTextoChat="";
		foreach (string Mensaje in Mensajes){
			NuevoTextoChat=NuevoTextoChat+Mensaje+"\n";
		} 
			TextoChat.text=NuevoTextoChat;
	}

}
