using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Controller : MonoBehaviour {
    public Camera cam;
    public float sensitivity;
    public GameObject pressThePaper;
    public TextMeshProUGUI notifText;

    private GameObject target;
    private bool isMouseDrag;
    private GameManager manager;
    private SoundManager soundManager;
    private string defaultNotifText;
    float f_lastX = 0.0f;
    float f_difX = 0.5f;
    float f_steps = 0.0f;
    int i_direction = 1;
    int clickedCount = 0;
    private void Awake() {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        soundManager = manager.gameObject.GetComponent<SoundManager>();
    }

    private void Start() {
        defaultNotifText = notifText.text;
    }

    GameObject ReturnClickedObject(out RaycastHit hit) {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit)) {
            target = hit.collider.gameObject;
        }

        if (target.CompareTag("Fruit"))
            return target;

        if (target.CompareTag("VirtualButton")) {
            StartCoroutine(PressThePaper());
        }
        return null;
    }

    IEnumerator DoubleTabToSlice() {
        soundManager.Play("Notif");
        pressThePaper.SetActive(true);
        pressThePaper.GetComponent<Animator>().Rebind();
        pressThePaper.GetComponent<Animator>().SetTrigger("Pop");
        yield return new WaitForSeconds(3);
        pressThePaper.GetComponent<Animator>().SetTrigger("PopOut");
        yield return new WaitForSeconds(.4f);
        pressThePaper.SetActive(false);
        pressThePaper.GetComponent<Animator>().Rebind();
    }

    IEnumerator PressThePaper() {
        soundManager.Play("Notif");
        pressThePaper.SetActive(true);
        pressThePaper.GetComponent<Animator>().Rebind();
        pressThePaper.GetComponent<Animator>().SetTrigger("Pop");
        yield return new WaitForSeconds(3);
        pressThePaper.GetComponent<Animator>().SetTrigger("PopOut");
        yield return new WaitForSeconds(.4f);
        pressThePaper.SetActive(false);
        pressThePaper.GetComponent<Animator>().Rebind();
    }

    IEnumerator WaitForAnotherInput() {
        yield return new WaitForSeconds(1);
        clickedCount = 0;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            clickedCount += 1;
            StartCoroutine(WaitForAnotherInput());
            RaycastHit hitInfo;
            target = ReturnClickedObject(out hitInfo);
            if (target != null) {
                isMouseDrag = true;
            }
        }

        if(clickedCount == 2) {
            SliceFruit();
            clickedCount = 0;
        }

        if (Input.GetMouseButtonUp(0)) {
            isMouseDrag = false;
        }

        if (isMouseDrag) {
            if (Input.GetMouseButtonDown(0)) {
                f_difX = 0.0f;
            } else if (Input.GetMouseButton(0)) {
                f_difX = Mathf.Abs(f_lastX - Input.GetAxis("Mouse X"));

                if (f_lastX < Input.GetAxis("Mouse X")) {
                    i_direction = -1;
                    target.transform.Rotate(Vector3.forward, -f_difX);
                }

                if (f_lastX > Input.GetAxis("Mouse X")) {
                    i_direction = 1;
                    target.transform.Rotate(Vector3.forward, f_difX);
                }

                f_lastX = -Input.GetAxis("Mouse X");
            } else {
                if (f_difX > 0.5f) f_difX -= 0.05f;
                if (f_difX < 0.5f) f_difX += 0.05f;

                target.transform.Rotate(Vector3.forward, f_difX * i_direction);
            }
        }
    }

    void SliceFruit() {
        if (target.GetComponent<MeshRenderer>().enabled) {
            target.transform.parent.GetComponent<Fruit>().sliced.GetComponent<MeshRenderer>().enabled = true;
            target.GetComponent<MeshRenderer>().enabled = false;
        } else {
            target.transform.parent.GetComponent<Fruit>().sliced.GetComponent<MeshRenderer>().enabled = false;
            target.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
