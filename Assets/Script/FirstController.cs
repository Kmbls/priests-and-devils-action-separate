using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction {

	public CCActionManager actionManager { get; set;}
	public Judger judge;

    public GameObject[] Chalist;
    public GameObject boat;
    public GameObject background;
    public GameObject cam;
    public int[] boatlist;
	public int[] chastate;
	public int boatstate;
    public int remainseat;
    public bool winflag;
	public bool isgameover;
	// the first scripts
	void Awake () {
		SSDirector director = SSDirector.getInstance ();
		director.setFPS (60);
		director.currentSceneController = this;
		director.currentSceneController.LoadResources ();
		Debug.Log ("awake FirstController!");
		actionManager = gameObject.AddComponent<CCActionManager>();
		judge = ScriptableObject.CreateInstance<Judger>();
		judge.Initialize();
	}
	 
	// loading resources for first scence
	public void LoadResources () {
        background = Instantiate<GameObject>(
            Resources.Load<GameObject> ("prefabs/background"),
            Vector3.zero, Quaternion.identity);
        background.name = "background";


        boat = Instantiate<GameObject>(
            Resources.Load<GameObject> ("prefabs/boat"),
            new Vector3(-4f,0,0), Quaternion.identity); 
        boat.name = "boat";

        Chalist = new GameObject[6];
        for (int i = 0;i<3;i++){
            Chalist[i] = Instantiate<GameObject>(
                Resources.Load<GameObject> ("prefabs/priest"),
                new Vector3(-6.5f,1f,-1f+i), Quaternion.identity);
			Chalist[i].name = ""+i;
            Chalist[i+3] = Instantiate<GameObject>(
                Resources.Load<GameObject> ("prefabs/devil"),
                new Vector3(-7.5f,1f,-1f+i), Quaternion.identity);
			Chalist[i+3].name = ""+(i+3);
		}
		chastate = new int[6]{1,1,1,1,1,1};
		boatstate = 1;
        isgameover = false;
        boatlist = new int[2]{-1,-1};
        remainseat = 2;
	}
	
	public void Pause ()
	{
		throw new System.NotImplementedException ();
	}

	public void Resume ()
	{
		throw new System.NotImplementedException ();
	}

	#region IUserAction implementation
	public void GameOver ()
	{
        if(winflag)
            Debug.Log("you win!");
        else
            Debug.Log("you lose!");
        isgameover = true;
	}
	#endregion


	// Use this for initialization
	void Start () {
		//give advice first
	}
	
	// Update is called once per frame
	void Update () {
		//give advice first
		if (!isgameover && Input.GetButtonDown("Fire1")) {
			Vector3 mp = Input.mousePosition;
			Camera ca;
			if (cam != null ) ca = cam.GetComponent<Camera> (); 
			else ca = Camera.main;
			Ray ray = ca.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
                if(hit.collider.gameObject != null){
                    string name = hit.collider.gameObject.name;
                    int number;
                    if (name == "boat" && remainseat < 2){
						boatstate = boatstate%2 + 1;
						actionManager.setMoveShip(boat,boatstate);
                        for(int i = 0;i<2;i++){
                            if(boatlist[i] != -1){
								Debug.Log("boatstate:"+boatstate);
                                actionManager.setMoveToShip(Chalist[boatlist[i]],i,boatstate);
								chastate[boatlist[i]] = 0;
                            }
                        }
                        judge.Check_if_lose();
                        return;
                    }
                    if(name != "background" && name != "boat"){
                        number = int.Parse(name);
                        int seat = (boatlist[0] == -1) ? 0 : 1;
                        if(chastate[number] != boatstate && chastate[number] != 0)
                            return;
                        if(remainseat <=0 && chastate[number] != 0)
                            return;
                        if (chastate[number] == 1 || chastate[number] == 2){
							actionManager.setMoveToShip(Chalist[number],seat,boatstate);
                            boatlist[boatlist[0] == -1 ? 0 : 1] = number;
                            remainseat--;
							chastate[number] = 0;
							Debug.Log(chastate);
                        }
                        else{
							actionManager.setMoveToBank(Chalist[number],boatstate,number);
                            boatlist[boatlist[0] == number ? 0 : 1] = -1;
                            remainseat++;
							chastate[number] = boatstate;
							Debug.Log(chastate);
                        }
                        judge.UpdateBank();
                        //judge.Check_if_lose();
                        judge.Check_if_win();
                    }
                }
			}
		}
	}

}
