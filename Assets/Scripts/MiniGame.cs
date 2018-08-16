using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZObjectPools;

public class MiniGame : MonoBehaviour {
    public float freqObject = 3;
    public float freqTime = 5f;
    public Transform[] spawnPos;
    public GameObject[] healthObj;
    public GameObject player;

    private GameObject obj;
    private int health = 3;
    private EZObjectPool pool;
    private float currentTime;
	// Use this for initialization
	void Start () {
        pool = GetComponent<EZObjectPool>();
        if (pool.TryGetNextObject(spawnPos[Mathf.RoundToInt(Random.Range(0, 8))].position, Quaternion.identity, out obj)) {
            obj.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
        currentTime += Time.deltaTime;
        if(currentTime > freqTime) {
            for (int i = 0; i <= freqObject; i++) {
                if (pool.TryGetNextObject(spawnPos[Mathf.RoundToInt(Random.Range(0, 8))].position, Quaternion.identity, out obj)) {
                    obj.SetActive(true);
                }
            }
            currentTime = 0;
        }

        
	}

    public void LoseHealth() {
        health--;
        Debug.Log(health);
        if (health <= 0)
            Dead();
        for (int i = 0; i < healthObj.Length; i++) {
            if (i >= health)
                healthObj[i].SetActive(false);
            else
                healthObj[i].SetActive(true);
        }
    }

    void Dead() {
        //
    }
}
