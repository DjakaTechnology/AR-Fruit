using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LevelLoader : MonoBehaviour {
    public GameObject credit;
    public GameObject about;

    public GameObject loadingScreen;
    public Slider loadingBar;
    public TextMeshProUGUI progressText;

    private bool isCreditOpen = false, isAboutOpen = false;

	public void LoadLevel(int sceneIndex){
		StartCoroutine(LoadAsyncly(sceneIndex));
	}

    private void Update() {
        if (about.activeSelf)
            isAboutOpen = true;
        if (credit.activeSelf)
            isCreditOpen = true;
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isCreditOpen || isAboutOpen) {
                if(isCreditOpen)
                    Credit();
                if(isAboutOpen)
                    About();
            } else
                Application.Quit();
        }
    }

    public void Credit() {
        if (!isCreditOpen)
            StartCoroutine(CreditOpen());
        else
            StartCoroutine(CreditClose());
    }

    IEnumerator CreditOpen() {
        isCreditOpen = true;
        credit.SetActive(true);
        credit.GetComponent<Animator>().Rebind();
        credit.GetComponent<Animator>().SetTrigger("Pop");
        yield return new WaitForSeconds(.5f);
        credit.GetComponent<Animator>().enabled = false;
    }

    IEnumerator CreditClose() {
        isCreditOpen = false;
        credit.GetComponent<Animator>().enabled = true;
        credit.GetComponent<Animator>().Rebind();
        credit.GetComponent<Animator>().SetTrigger("PopOut");
        yield return new WaitForSeconds(.5f);
        credit.SetActive(false);
    }

    IEnumerator AboutOpen() {
        isAboutOpen = true;
        about.SetActive(true);
        about.GetComponent<Animator>().Rebind();
        about.GetComponent<Animator>().SetTrigger("Pop");
        yield return new WaitForSeconds(.5f);
        about.GetComponent<Animator>().enabled = false;
    }

    IEnumerator AboutClose() {
        isAboutOpen = false;
        about.GetComponent<Animator>().enabled = true;
        about.GetComponent<Animator>().Rebind();
        about.GetComponent<Animator>().SetTrigger("PopOut");
        yield return new WaitForSeconds(.5f);
        about.SetActive(false);
    }

    IEnumerator LoadAsyncly(int sceneIndex){
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(true);

		while(!operation.isDone){
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;

            if (loadingBar.value < .2)
                progressText.SetText("Hey is this thing on ?");
            else if (loadingBar.value < .5)
                progressText.SetText("Calling the fruit buddies...");
            else if (loadingBar.value < .8)
                progressText.SetText("Hey camera, how do you do ?");
            else if (loadingBar.value < .9)
                progressText.SetText("Marker, Wake UP!");
            else
                progressText.SetText("Hello, AR World !");

			yield return null;
		}
	}

    public void About() {
        if (!isAboutOpen)
            StartCoroutine(AboutOpen());
        else
            StartCoroutine(AboutClose());
    }

    public void Exit() {
        Application.Quit();
    }
}
