using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class DragonPrueba : MonoBehaviour {
	UnityArmatureComponent armature;

	// Use this for initialization
	void Start () {
		armature = GetComponent<UnityArmatureComponent> ();	
		armature.AddDBEventListener (DragonBones.EventObject.FRAME_EVENT,anim);
	}

	void anim(string nombre,DragonBones.EventObject c){
		Debug.Log (c.name+" y "+nombre);

	}

	void saludar(){
		Debug.Log ("holaaa");
	}

	// Update is called once per frame
	void Update () {
		
	}
}
