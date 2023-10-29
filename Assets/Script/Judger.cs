using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Judger : ScriptableObject
{
    private FirstController sceneController;
    public int[] leftbank;
    public int[] rightbank;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(){
        sceneController = (FirstController)SSDirector.getInstance ().currentSceneController;
        leftbank = new int[6]{1,1,1,-1,-1,-1};
        rightbank = new int[6]{0,0,0,0,0,0};
    }
    public void Check_if_lose(){
        if ((leftbank.Sum() < 0  && leftbank.Take(3).Sum() > 0)|| (rightbank.Sum() < 0 && rightbank.Take(3).Sum() > 0)){
            sceneController.winflag = false;
            sceneController.GameOver();
        }
    }

    public void Check_if_win(){
        sceneController.winflag = false;
        for(int i = 0;i<6;i++){
            if(rightbank[i] == 0)
                return;
        }
        sceneController.winflag = true;
        sceneController.GameOver();
    }
    public void UpdateBank(){
        for(int i = 0;i<3;i++){
            leftbank[i] = 0;
            leftbank[i+3] = 0;
            rightbank[i] = 0;
            rightbank[i+3] = 0;
            if(sceneController.chastate[i] == 1)
                leftbank[i] = 1;
            else if(sceneController.chastate[i] == 2)
                rightbank[i] = 1;
            if(sceneController.chastate[i+3] == 1)
                leftbank[i+3] = -1;
            else if(sceneController.chastate[i+3] == 2)
                rightbank[i+3] = -1;
        }
    }
}
