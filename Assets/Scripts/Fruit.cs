using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour {
    public GameObject main;
    public GameManager gameManager;
    public GameObject sliced;

	private bool isActive;
    private MeshRenderer parent;

    private MeshRenderer mainMesh;
    private Collider mainCollider;
    private Animator mainAnimator;

    // Use this for initialization
    void Start () {
        parent = GetComponent<MeshRenderer>();
		isActive = false;
        mainMesh = main.GetComponent<MeshRenderer>();
        mainAnimator = main.GetComponent<Animator>();
        mainCollider = main.GetComponent<SphereCollider>();
        mainMesh.enabled = false;
        main.transform.localScale = Vector3.zero;
        sliced.GetComponent<MeshRenderer>().enabled = false;

        if (gameManager == null)
            gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
		if(parent.enabled){
            if (!isActive) {
                SetMainEnabled(true);
            }
			isActive = true;
		}else{
            if (isActive) {
                SetMainEnabled(false);
            }
			isActive = false;
		}
	}

    void SetMainEnabled(bool value) {
        if (!value) {
            StartCoroutine(PopOut());
        } else {
            mainAnimator.Rebind();
            mainMesh.enabled = value;
            mainAnimator.enabled = value;
            mainCollider.enabled = value;
            mainAnimator.SetTrigger("Pop");
            gameManager.SetActiveObject(mainMesh.gameObject, mainAnimator);
        }
        gameManager.isActive = value;
    }

    public void ResetSliced() {
        mainMesh.enabled = true;
        sliced.GetComponent<MeshRenderer>().enabled = false;
    }

    IEnumerator PopOut() {
        mainAnimator.SetTrigger("PopOut");
        yield return new WaitForSeconds(.4f);
        main.transform.localScale = Vector3.zero;
        gameManager.SetActiveObject(null, null);
        mainMesh.enabled = false;
        mainAnimator.enabled = false;
        mainCollider.enabled = false;
    }
}
