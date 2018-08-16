using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Vuforia;

public class GameManager : MonoBehaviour {
    public GameObject scanImage;
    public TextMeshProUGUI objectName;

    public VirtualButtonBehaviour btnNutrition;
    public GameObject btnNutritionChild;
    public Canvas[] listCanvas;
    //public Vector3[] canvasOffset;
    public FruitData[] fruit;
    public Camera camera;

    public TextMeshProUGUI detailContent, nutritionContent, benefitContent, riskContent;
    public Slider vitA, vitC, vitE;

    public float canvasWidth;
    public float canvasHeight;
    public float canvasSpeed;
    public bool isFlashActive = false;

    public GameObject activeFruit;
    public Animator activeAnimator;
    public bool isActive = false;

    private SoundManager soundManager;
    private int activeIndex = 0;
    private Vector3[] canvasOriginalSize;
    private Vector3[] canvasRotation;

    void Start () {
        btnNutritionChild = btnNutrition.gameObject.transform.GetChild(0).gameObject;
        soundManager = GetComponent<SoundManager>();

        canvasOriginalSize = new Vector3[listCanvas.Length];
        canvasRotation = new Vector3[listCanvas.Length];

        int i = 0;
        foreach(Canvas index in listCanvas) {
            canvasOriginalSize[i] = index.transform.localScale;
            canvasRotation[i] = index.transform.localEulerAngles;

            i++;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
        }

        if (activeFruit != null) {
            listCanvas[activeIndex].transform.parent.localPosition = Vector3.zero;
            listCanvas[activeIndex].transform.localEulerAngles = canvasRotation[activeIndex];
            listCanvas[activeIndex].transform.parent.localEulerAngles = Vector3.zero - activeFruit.transform.parent.localEulerAngles;
            scanImage.gameObject.SetActive(false);

            //listCanvas[activeIndex].transform.parent.localPosition = canvasOffset[activeIndex];
            //listCanvas[activeIndex].transform.parent.position = canvasOffset[activeIndex];
            //listCanvas[activeIndex].transform.localEulerAngles = canvasRotation[activeIndex] + activeFruit.transform.parent.transform.parent.localEulerAngles;
        } else if (activeFruit == null) {
            listCanvas[activeIndex].transform.localScale = Vector3.Lerp(listCanvas[activeIndex].transform.localScale, Vector3.zero, canvasSpeed);
            scanImage.gameObject.SetActive(true);
        }

        //Debug.Log(listCanvas[activeIndex].transform.parent.localPosition);
        //Debug.Log(Vector3.Scale(activeFruit.transform.localPosition, activeFruit.transform.right) - listCanvas[activeIndex].transform.parent.localPosition);
        //Debug.Log(listCanvas[activeIndex].transform.parent.position - activeFruit.transform.position);
        //Debug.Log(listCanvas[activeIndex].transform.localPosition);
        //Debug.Log(activeFruit.transform.parent.parent.transform.position);
        //Debug.Log(activeFruit.transform.parent.parent.name);
    }

    public void ChangeActiveCanvas(int index) {
        if (index == activeIndex)
            return;
        StartCoroutine(CanvasPopOutAnimation(listCanvas[activeIndex]));
        activeIndex = index;
        listCanvas[activeIndex].transform.parent.parent = activeFruit.transform.parent;
        StartCoroutine(CanvasPopInAnimation(listCanvas[activeIndex]));
    }

    IEnumerator ScanImagePopInAnimation() {
        scanImage.GetComponent<Animator>().Rebind();
        scanImage.gameObject.SetActive(true);
        scanImage.GetComponent<Animator>().enabled = true;
        scanImage.GetComponent<Animator>().SetTrigger("Pop");
        yield return new WaitForSeconds(.5f);
        scanImage.GetComponent<Animator>().enabled = false;
    }

    IEnumerator ScanImagePopOutAnimation() {
        scanImage.GetComponent<Animator>().Rebind();
        scanImage.GetComponent<Animator>().enabled = true;
        scanImage.GetComponent<Animator>().SetTrigger("PopOut");
        yield return new WaitForSeconds(.5f);
        scanImage.GetComponent<Animator>().enabled = false;
        scanImage.gameObject.SetActive(false);
    }

    IEnumerator CanvasPopInAnimation(Canvas target) {
        target.GetComponent<Animator>().Rebind();
        target.gameObject.SetActive(true);
        target.GetComponent<Animator>().enabled = true;
        target.GetComponent<Animator>().SetTrigger("Pop");
        yield return new WaitForSeconds(.5f);
        target.GetComponent<Animator>().enabled = false;
    }

    IEnumerator CanvasPopOutAnimation(Canvas target) {
        target.GetComponent<Animator>().Rebind();
        target.GetComponent<Animator>().enabled = true;
        target.GetComponent<Animator>().SetTrigger("PopOut");
        yield return new WaitForSeconds(.5f);
        target.GetComponent<Animator>().enabled = false;
        target.gameObject.SetActive(false);
    }

    public void SetActiveObject(GameObject obj, Animator anim) {
        activeAnimator = anim;
        activeFruit = obj;
        soundManager.Play("Pop");

        if (obj == null) {
            StartCoroutine(CanvasPopOutAnimation(listCanvas[activeIndex]));
            StartCoroutine(ScanImagePopOutAnimation());
            objectName.SetText("No Active Object Detected");
            return;
        }

        obj.transform.parent.GetComponent<Fruit>().ResetSliced();


        StartCoroutine(ScanImagePopInAnimation());

        objectName.SetText(obj.transform.parent.name);

        listCanvas[activeIndex].transform.parent.parent = activeFruit.transform.parent;
        StartCoroutine(CanvasPopInAnimation(listCanvas[activeIndex]));

        foreach (FruitData i in fruit) {
            if(i.name == obj.transform.parent.name) {
                vitA.value = i.vitA;
                vitC.value = i.vitC;
                vitE.value = i.vitE;

                
                detailContent.SetText(System.Text.RegularExpressions.Regex.Unescape(i.detailContent));
                nutritionContent.SetText(System.Text.RegularExpressions.Regex.Unescape(i.nutritionContent));
                benefitContent.SetText(System.Text.RegularExpressions.Regex.Unescape(i.benefitContent));
                riskContent.SetText(System.Text.RegularExpressions.Regex.Unescape(i.riskContent));

                break;
            }
        }
    }

    public void SetCanvasSize() {
        listCanvas[activeIndex].GetComponent<RectTransform>().sizeDelta = new Vector2(canvasWidth, canvasHeight);
    }

    public void SetFlash() {
        isFlashActive = !isFlashActive;
        CameraDevice.Instance.SetFlashTorchMode(isFlashActive);
    }
}﻿