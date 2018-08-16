using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatch : MonoBehaviour {

    public GameObject player;
    public CatchTheFruitManager manager;

    public float sensitifity;
    public Transform xLimitRight, xLimitLeft;

    float f_lastX = 0.0f;
    float f_difX = 0.5f;
    float f_steps = 0.0f;
    int i_direction = 1;

    private Transform imageTarget;

    private void Awake() {
        if (manager == null)
            manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<CatchTheFruitManager>();
        imageTarget = GameObject.FindGameObjectWithTag("ImageTarget").transform;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.right * 0.0001f;
        if (Input.GetMouseButtonDown(0)) {
            f_difX = 0.0f;
        } else if (Input.GetMouseButton(0)) {
            f_difX = Mathf.Abs(f_lastX - Input.GetAxis("Mouse X"));
            if (f_lastX < Input.GetAxis("Mouse X") && DetectRightLimit()) {
                i_direction = -1;
                //transform.Translate(-transform.right * -f_difX * sensitifity);
                transform.position += -transform.right * -f_difX * sensitifity;
            }

            if (f_lastX > Input.GetAxis("Mouse X") && DetectLeftLimit()) {
                i_direction = 1;
                //transform.Translate(-transform.right * f_difX * sensitifity);
                transform.position += -transform.right * f_difX * sensitifity;
            }

            f_lastX = -Input.GetAxis("Mouse X");
        } else {
            if (f_difX > 0.5f) f_difX -= 0.05f;
            if (f_difX < 0.5f) f_difX += 0.05f;
        }

        if (!DetectLeftLimit())
            transform.position = xLimitLeft.position;
        else if (!DetectRightLimit())
            transform.position = xLimitRight.position;


        //xLimitRight.localEulerAngles = new Vector3(imageTarget.rotation.x, xLimitRight.rotation.y, xLimitRight.rotation.z);
        //xLimitLeft.localEulerAngles = new Vector3(imageTarget.rotation.x, xLimitLeft.rotation.y, xLimitLeft.rotation.z);
        //transform.localEulerAngles = new Vector3(imageTarget.rotation.x, imageTarget.rotation.y, transform.rotation.z);
        //Debug.DrawRay(transform.position, transform.right, Color.green);
        //Debug.Log(xLimitRight.position + " Right");
        //Debug.Log(xLimitLeft.position +  "Left");
        //Debug.Log(transform.position + "Main");
        //Debug.Log(transform.right + "RightIS");
    }

    private bool DetectRightLimit() {
        Vector3 temp = Vector3.Scale(xLimitRight.right , xLimitRight.position) - Vector3.Scale(transform.position , transform.right);

        if (temp.x < 0)
            return false;
        else if (temp.y < 0)
            return false;
        else if (temp.z < 0)
            return false;

        return true;
    }

    private bool DetectLeftLimit() {
        Vector3 temp = Vector3.Scale(xLimitLeft.right, xLimitLeft.position) - Vector3.Scale(transform.position, transform.right);
        if (temp.x > 0)
            return false;
        else if (temp.y > 0)
            return false;
        else if (temp.z > 0)
            return false;
            
        return true;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Fruit")) {
            collision.gameObject.SetActive(false);
            manager.ScoreIncrease(collision.transform.parent.name);
            Debug.Log(collision.transform.parent.name);
        }
    }
}
