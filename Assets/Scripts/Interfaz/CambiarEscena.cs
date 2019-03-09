using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Script para cargar Escena utilizar con animacion de transiccion
public class CambiarEscena : MonoBehaviour {

	// Metodo para cargar la escena
	public void Cambiar(){
	if(Application.isMobilePlatform){
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
		SceneManager.LoadScene("TestRoom");
	}
}
