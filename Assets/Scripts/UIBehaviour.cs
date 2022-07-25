using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI triesValue;

    public void SetTriesValue(int value)
    {
        triesValue.text = value.ToString();
    }
}
