using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class TowerScript : MonoBehaviour {

	public float HP=1500,HPMAX=1500;
	public Image BarraVida;
	public int NTower,TeamID;
	public GestorPartida Gestor;
	public BoxCollider2D CajaColision;
	public Transform EjeColision;
	public LayerMask Mascara;
	Collider2D[] Colisiones;
	SpriteRenderer mySprite;
	public Sprite[] EvolucionTorre;
	Animation Anim;

	void Start(){
		mySprite=GetComponent<SpriteRenderer>();
		Anim=GetComponent<Animation>();
	}

	void Update(){
		if(Gestor.myPlayer!=null){
		if(Gestor.myPlayer.GetComponent<PersonajeOnline>().TeamID!=TeamID){
			BarraVida.color=Color.magenta;
		}else{
			BarraVida.color=Color.blue;
		}
	}
		BarraVida.fillAmount=HP/HPMAX;
		Colisiones=new Collider2D[0];
		Colisiones=Physics2D.OverlapBoxAll(EjeColision.position,new Vector2(CajaColision.size.x,CajaColision.size.y),0f,Mascara.value);
			for(int i=0;i<Colisiones.Length;i++){
				if(Colisiones[i].GetComponent<AtaqueScript>().myCharacter!=null){
				if(Colisiones[i].GetComponent<AtaqueScript>().myCharacter.GetComponent<PersonajeOnline>().TeamID!=TeamID && !Colisiones[i].GetComponent<AtaqueScript>().ColisionesDetectadas.Contains(transform.name) || Colisiones[i].GetComponent<AtaqueScript>().myCharacter.GetComponent<PersonajeOnline>().TeamID!=TeamID && Colisiones[i].GetComponent<AtaqueScript>().Continuo){
						Colisiones[i].GetComponent<AtaqueScript>().ColisionesDetectadas.Add(transform.name);
						if(PhotonNetwork.IsMasterClient && Gestor.EquipoVictoria==-1){
							Colisiones[i].GetComponent<AtaqueScript>().myCharacter.GetComponent<PersonajeOnline>().enviarDañoTorre(Colisiones[i].GetComponent<AtaqueScript>().damage,NTower);
						}
						Anim.Play();
				}
			}
		 }
	  }

	 public void ReportarDañoTorre(int Daño){
	  	if(HP>0){
			HP-=Daño;
			if(HP<=0){
				mySprite.sprite=EvolucionTorre[0];
			}else if(HP<(HPMAX*0.3f)){
				mySprite.sprite=EvolucionTorre[1];
			}else if(HP<HPMAX*0.6f){
				mySprite.sprite=EvolucionTorre[2];
			}
	 	 }		
	  }



}
