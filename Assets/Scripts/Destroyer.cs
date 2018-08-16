using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour {
    public string[] tags;

    private Transform imageTarget;
    private CatchTheFruitManager manager;
    // Use this for initialization
	void Start () {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<CatchTheFruitManager>();
        imageTarget = GameObject.FindGameObjectWithTag("ImageTarget").transform;
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnCollisionEnter(Collision collision) {
        foreach(string tag in tags) {
            if (collision.collider.CompareTag(tag)) {
                if (tag == "Fruit") {
                    StartCoroutine(WaitForAnimation(collision.gameObject));
                    manager.DamageDealt();
                }
            }

        }
    }


    IEnumerator WaitForAnimation(GameObject target) {
        Animator anim = target.GetComponent<CatchFruit>().anim;
        anim.SetTrigger("PopOut");
        yield return new WaitForSeconds(.4f);
        target.SetActive(false);
        anim.Rebind();
    }
}
