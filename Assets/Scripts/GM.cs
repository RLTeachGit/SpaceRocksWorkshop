﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//As the game relies on this its a good idea to make this script's
//Execution order make it run before other scripts
//Check Project Settings - Execution order 

public class GM : MonoBehaviour {

    public int MyScore = 0; //Score

    public int MyRockcount = 0;

    int HowManyRocks = 5;

    [SerializeField]
    GameObject PlayerShipPrefab; //Link to player ship prefab

    [SerializeField]
    GameObject RockPrefab; //Link to Rock prefab

    GameObject mPlayer; //Keep link to player

    private static GM sSingletonRef = null;  //Static variable, hidden

    public static GM singleton {   //Expose public getter
        get {
            Debug.Assert(sSingletonRef != null, "Calling on GM before its ready");
            return sSingletonRef;
        }
    }

    private void Awake() { //Runs as soon as object is instantiated, before Start()
        if (sSingletonRef == null) { //first time we are started this is null
            sSingletonRef = this; //Now set it to ourself
            DontDestroyOnLoad(gameObject); //Make sure GO is not unloaded 
            Debug.Assert(PlayerShipPrefab != null, "Please assign PlayerShip in IDE");
            Debug.Assert(RockPrefab != null, "Please assign Rock in IDE");
        } else if (sSingletonRef != this) { //If we are run again then make sure we are the same object
            Destroy(gameObject); //If not destroy the imposter (duplicate) to make sure there is only one
        }
    }

    //Helper function to generate a valid screen position
    public static Vector2 RandomPosition() {
        float tHeight = Camera.main.orthographicSize; //Work out screen bounds
        float tWidth = tHeight * Camera.main.aspect;

        return new Vector2(Random.Range(-tWidth, tWidth), Random.Range(-tHeight, tHeight)); //Make Vector inside them
    }

    private void Start() {
        mPlayer = Instantiate(PlayerShipPrefab, RandomPosition(), Quaternion.identity); //Place player onscreen when game starts
        StartCoroutine(StartNewLevel());
    }

    void MoreRocks(int vCount) {
        for (int i = 0; i < vCount; i++) {
            Instantiate(RockPrefab, RandomPosition(), Quaternion.identity); //Place Rocks onscreen when game starts
        }
    }

    IEnumerator StartNewLevel() {

        for(; ; ) {
            while (MyRockcount > 0) {
                yield return new WaitForSeconds(1);
            }
            yield return new WaitForSeconds(5);
            MoreRocks(HowManyRocks++);
        }
    }
}

