using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class OsoPrueba : MonoBehaviour {

	Rigidbody2D rig;
	public float velocidad;
	public bool ia;
	UnityArmatureComponent armarture;
	bool atacando=false;
	public GameObject[] partes;
	public Collider2D ataque;

	// Use this for initialization
	void Start () {
		rig = GetComponent<Rigidbody2D> ();	
		armarture = GetComponent<UnityArmatureComponent> ();
	}
	
	void OnTriggerEnter2D(Collider2D c){
		if(ia && c.tag=="Golpe"){
			rig.AddForce (new Vector2( 50f,50f),ForceMode2D.Impulse);
		}
	}
	void apagar(){
		ataque.enabled= false;
	}

	void encender(){
		ataque.enabled=true;
		Invoke ("apagar",0.5f);
	}

	IEnumerator parpadear(){
		for(int i=0;i<3;i++){
			for(int j=0;j<partes.Length;j++){
				partes[j].SetActive(false);
			}
			yield return new WaitForSeconds(0.5f);
			for(int j=0;j<partes.Length;j++){
				partes[j].SetActive(true);
			}
		}
		yield return null;
	}

	void Update () {
		if(!ia){
		if(!armarture.animation.isPlaying && armarture.animation.lastAnimationName!="atc"){
			armarture.animation.FadeIn ("stand", 0.25f,-1);
		}else if (Input.GetKey (KeyCode.LeftArrow)) {
				rig.velocity = new Vector2 (-1,rig.velocity.y);
			if(armarture.animation.lastAnimationName!="walk"){
				armarture.animation.FadeIn ("walk", 0.25f,-1);				
				}
				armarture.armature.flipX = false;
			} else if (Input.GetKey (KeyCode.RightArrow)) {
				rig.velocity = new Vector2 (1,rig.velocity.y);
			if(armarture.animation.lastAnimationName!="walk"){
					armarture.animation.FadeIn ("walk", 0.25f,-1);				
				}
				armarture.armature.flipX = true;
		} else if (Input.GetKey (KeyCode.A) && armarture.animation.lastAnimationName!="atc") {
				armarture.animation.FadeIn ("atc", 0.25f,1);
				Invoke ("encender",1f);
		}else if(armarture.animation.lastAnimationName!="stand" && armarture.animation.lastAnimationName!="atc"){
				armarture.animation.FadeIn ("stand", 0.25f,-1);
			}
		}else if(armarture.animation.lastAnimationName!="stand" && armarture.animation.lastAnimationName!="atc"){
			rig.velocity = new Vector2 (0f,rig.velocity.y);
			armarture.animation.FadeIn ("stand", 0.25f,-1);
		}
	}
}
