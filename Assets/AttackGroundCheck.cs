using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGroundCheck : MonoBehaviour {

	public Transform Check;
	public float Radio=0.05f,LifeTime=1f;
	bool Activado=true;
	public ParticleSystem[] SistemasParticulas;
	public LayerMask Suelo;

	void Update () {
	if(Activado){
		if(!Physics2D.OverlapCircle(Check.position,Radio,Suelo)){
			Activado=false;
				for(int i=0;i<SistemasParticulas.Length;i++){
					SistemasParticulas[i].Stop();
					GetComponent<Collider2D>().enabled=false;
				}
			Invoke("Destruir",LifeTime);
			}
	}	
	}

	void Destruir(){
		Destroy(gameObject);
	}

}
