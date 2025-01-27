using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EvaluationsData", menuName = "ScriptableObjects/EvaluationsDataSet", order = 1)]
public class EvaluationsDataSet : ScriptableObject
{
    [SerializeField] private string dataKey;
    [SerializeField] private string evaluationsString;
    [SerializeField] private Sprite evaluationsSprite;

    public string DataKey { get => dataKey; }
    public string EvaluationsString { get => evaluationsString; }
    public Sprite EvaluationsSprite { get => evaluationsSprite; }
}
