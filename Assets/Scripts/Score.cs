using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	public int Kills,Assist,Death;
	PhotonView PView;
	Text myKDAText;

	void Start () {
		PView=GetComponent<PhotonView>();
		myKDAText=GameObject.Find("GestorPartida").GetComponent<GestorPartida>().myKDAText;
	}

	public void CallUpdateKDA(int K,int D,int A){
		PView.RPC("UpdateKDA",RpcTarget.All,K,D,A);
	}

	[PunRPC]
	void UpdateKDA(int K,int D,int A){
		if(PView.IsMine){
		Kills+=K;
		Death+=D;
		Assist+=A;
		if(PView.IsMine){
		myKDAText.text=Kills.ToString()+"/"+Death.ToString()+"/"+A.ToString();
		}
		}
	}

}
