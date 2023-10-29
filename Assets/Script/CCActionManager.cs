using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager {
	
	private FirstController sceneController;

	private CCMoveToAction moveToShip, moveToBank, moveShip;
	protected new void Start() {
		sceneController = (FirstController)SSDirector.getInstance ().currentSceneController;
		sceneController.actionManager = this;
	}

	// Update is called once per frame
	protected new void Update ()
	{
		base.Update ();
	}

	public void setMoveShip(GameObject gameobject, int boatstate){
		Vector3 vector;
		if(boatstate == 2)
			vector = new Vector3(4f, 0, 0);
		else
			vector = new Vector3(-4f, 0, 0);
		moveShip = CCMoveToAction.GetSSAction(vector, 10);
		this.RunAction(gameobject,moveShip);
	}
	public void setMoveToShip(GameObject gameobject,int seat,int boatstate){
		Vector3 vector;
		if(boatstate == 1)
			vector = new Vector3(-4f + (seat==1?1:-1), 0.5f, 0);
		else
			vector = new Vector3(4f + (seat==1?1:-1), 0.5f, 0);
		moveToShip = CCMoveToAction.GetSSAction(vector, 10);
		Debug.Log("movetoship,target:"+vector);
		this.RunAction(gameobject,moveToShip);
	}


	public void setMoveToBank(GameObject gameobject, int boatstate, int number){
		Vector3 vector;
		if(boatstate == 1)
			vector = new Vector3(-6.5f+(number>2?-1:0),1f,-1f+number%3);
		else
			vector = new Vector3(6.5f+(number>2?1:0),1f,-1f+number%3);
		moveToBank = CCMoveToAction.GetSSAction(vector,10);
		Debug.Log("movetobank,target:"+vector);
		this.RunAction(gameobject,moveToBank);
	}
}

