using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class BtnController : MonoBehaviour, IVirtualButtonEventHandler {
    public int canvasIndex;

    private VirtualButtonBehaviour btnVirtual;
    private Animator anim;
    private GameManager manager;
	// Use this for initialization
	void Start () {
        btnVirtual = GetComponent<VirtualButtonBehaviour>();
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        btnVirtual.RegisterEventHandler(this);
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnButtonPressed(VirtualButtonBehaviour vb) {
        manager.ChangeActiveCanvas(canvasIndex);
        anim.SetBool("ButtonPressed", true);
        Debug.Log("Button released");
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb) {
        anim.SetBool("ButtonPressed", false);
        Debug.Log("Button released");
    }

}
