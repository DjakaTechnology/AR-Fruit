using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EZObjectPools;
using TMPro;

public class CatchTheFruitManager : MonoBehaviour {
    public Transform[] spawnPos;
    public EZObjectPool[] fruits;
    public float fruitSpeed;
    public float maxSpeed;
    public float frequent;
    public TextMeshPro text;
    public GameObject[] healthGUI;
    public GameObject set;
    public MeshRenderer detector;
    public string[] vitAFruit, vitCFruit, vitEFruit;
    public GameObject gameOverCanvas;

    public Slider vitaminA, vitaminC, vitaminE;

    public TextMeshProUGUI gameoverText, vitAText, vitCText, vitEText;

    private bool isOver = false;
    private float vitA, vitC, vitE;
    private int score;
    private float spawnCooldown = 0;
    private int randomValue;
    private bool canSpawn = true;
    private float health = 3;
    private GameObject obj;
    // Use this for initialization
    void Start() {
        GameObject[] tempPool = GameObject.FindGameObjectsWithTag("FruitPool");
        int index = 0;
        foreach(GameObject i in tempPool) {
            fruits[index] = i.GetComponent<EZObjectPool>();
            index++;
        }
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
        }
        if (isOver)
            return;
        if (!detector.enabled) {
            set.SetActive(false);
            return;
        } else
            set.SetActive(true);
        if (canSpawn) {
            Spawn();
        } else {
            spawnCooldown += Time.deltaTime;
            if (spawnCooldown >= frequent)
                canSpawn = true;
        }
    }

    void Spawn() {
        if (isOver)
            return;

        canSpawn = false;
        spawnCooldown = 0;
        randomValue = Random.Range(0, fruits.Length);
        fruits[randomValue].TryGetNextObject(spawnPos[Random.Range(0, spawnPos.Length)].position, Quaternion.identity, out obj);
        obj.GetComponent<CatchFruit>().anim.SetTrigger("Pop");
        obj.transform.localScale = Vector3.one * 1.5f;
        ScoreIncrease(null);
    }

    public void ScoreIncrease(string name) {
        if (isOver)
            return;

        if (name != null) {
            foreach(string i in vitAFruit) {
                if (name.ToLower() == i.ToLower()) {
                    vitA += 3;
                    CalculateAverage();
                }
            }

            foreach (string i in vitCFruit) {
                if (name.ToLower() == i.ToLower()) {
                    vitC += 2;
                    CalculateAverage();
                }
            }

            foreach (string i in vitEFruit) {
                if (name.ToLower() == i.ToLower()) {
                    vitE+= 4.6f;
                    CalculateAverage();
                }
            }
        }
        score++;
        text.SetText(score.ToString());

        if (score > 10 && score % 2 == 0) {
            if (frequent >= 1.5f)
                frequent -= .5f;
            if (fruitSpeed < maxSpeed)
                fruitSpeed += .5f;
        }

        if (score % 20 == 0) {
            DamageHeal();
        }
    }

    public void CalculateAverage() {
        if (isOver)
            return;

        float total = vitA + vitC + vitE;

        vitaminA.value = vitA / total;
        vitaminC.value = vitC / total;
        vitaminE.value = vitE / total;
    }

    public void GameOver() {
        if (isOver)
            return;
        isOver = true;

        StartCoroutine(GameOverAnim());
        int highScore = PlayerPrefs.GetInt("HighScore");
        if (highScore < score) {
            PlayerPrefs.SetInt("HighScore", score);
            gameoverText.SetText("Score: " + score + "\nHigh Score : " + highScore + "\nNew High Score!!!" +"\nVitamin A : "+vitA+" grams\nvitaminC : "+vitC+" grams\nvitaminE : "+vitE+" grams");
        }else {
            gameoverText.SetText("Score: " + score + "\nHigh Score : " + highScore + "\nVitamin A : " + vitA + " grams\nvitaminC : " + vitC + " grams\nvitaminE : " + vitE + " grams");
        }

        vitAText.SetText(Mathf.Round(vitaminA.value * 100) + " %");
        vitCText.SetText(Mathf.Round(vitaminC.value * 100) + " %");
        vitEText.SetText(Mathf.Round(vitaminE.value * 100) + " %");

    }

    IEnumerator GameOverAnim() {
        gameOverCanvas.SetActive(true);
        gameOverCanvas.GetComponent<Animator>().Rebind();
        gameOverCanvas.GetComponent<Animator>().enabled = true;
        gameOverCanvas.GetComponent<Animator>().SetTrigger("Pop");
        yield return new WaitForSeconds(.4f);
        gameOverCanvas.GetComponent<Animator>().enabled = false;
    }

    IEnumerator GameRestartAnim() {
        gameOverCanvas.GetComponent<Animator>().Rebind();
        gameOverCanvas.GetComponent<Animator>().enabled = true;
        gameOverCanvas.GetComponent<Animator>().SetTrigger("PopOut");
        yield return new WaitForSeconds(.4f);
        gameOverCanvas.GetComponent<Animator>().enabled = false;
        gameOverCanvas.SetActive(false);

    }

    public void RestartScene() {
        foreach (EZObjectPool i in fruits)
            i.ClearPool();
        StartCoroutine(GameRestartAnim());
        isOver = false;

        Spawn();

        health = 3;
        UpdateHealthGUI();

        score = 0;
        text.SetText(score.ToString());

        vitA = 0;
        vitC = 0;
        vitE = 0;
        CalculateAverage();

        vitAText.SetText("A");
        vitCText.SetText("C");
        vitEText.SetText("E");

    }


    public void DamageHeal() {
        if (isOver)
            return;

        if (health <= 6) {
            health++;
            UpdateHealthGUI();
        }
    }
    public void DamageDealt() {
        if (isOver)
            return;

        if (health > 0) {
            health--;
            UpdateHealthGUI();
        }

        if (health <= 0)
            GameOver();
    }

    void UpdateHealthGUI() {
        for(int i = 0;i < healthGUI.Length; i++) {
            if (i <= health - 1) {
                if (!healthGUI[i].activeSelf)
                    StartCoroutine(HealthGUI(healthGUI[i], true));
            } else {
                StartCoroutine(HealthGUI(healthGUI[i], false));
            }
        }
    }

    IEnumerator HealthGUI(GameObject obj, bool value) {
        obj.GetComponent<Animator>().Rebind();
        if (value) {
            obj.SetActive(true);
            obj.GetComponent<Animator>().SetTrigger("Pop");
        } else
            obj.GetComponent<Animator>().SetTrigger("PopOut");

        yield return new WaitForSeconds(.4f);

        if (!value)
            obj.SetActive(false);
    }
}
