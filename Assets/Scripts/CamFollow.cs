using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour {
	public Transform player;
	public float distanciax,distanciay,velocidad,shakex,shakey;
	Vector3 newpos;


	void Update () {
		if(player!=null){
			newpos = new Vector3 (player.position.x+distanciax+shakex,player.position.y+distanciay,-10f);
			transform.position = Vector3.Lerp(transform.position,newpos,velocidad);
		}
	}

	public IEnumerator shakeeffect(){
		shakex = -1;
		yield return new WaitForSeconds (0.1f);
		shakex = 2;
		yield return new WaitForSeconds (0.1f);
		shakex = -2;
		yield return new WaitForSeconds (0.1f);
		shakex = 1;
		yield return new WaitForSeconds (0.1f);
		shakex = -1;
		yield return new WaitForSeconds (0.1f);
		shakex = 2;
		yield return new WaitForSeconds (0.1f);
		shakex = -2;
		yield return new WaitForSeconds (0.1f);
		shakex = 1;
		yield return new WaitForSeconds (0.1f);
		shakex = 0;
	}


}
