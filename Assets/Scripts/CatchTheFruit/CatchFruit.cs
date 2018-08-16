using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchFruit : MonoBehaviour {

    private float speed;
    public CatchTheFruitManager manager;
    public Animator anim;
    private Transform imageTarget;
    void Awake() {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<CatchTheFruitManager>();
        imageTarget = GameObject.FindGameObjectWithTag("ImageTarget").transform;
    }

    // Update is called once per frame
    void Update () {
        speed = manager.fruitSpeed;
        transform.Translate(Vector3.back * speed * Time.deltaTime);
        transform.rotation = imageTarget.rotation;
	}
}
