using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Fruit", menuName = "Fruit")]
public class FruitData:ScriptableObject {

    public string name, detailContent, nutritionContent, benefitContent, riskContent;

    [Range(0.0f, 1f)]
    public float vitA, vitC, vitE;
}
