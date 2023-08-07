using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    [SerializeField] GameObject damageText;

    public void SpawnText(float damage)
    {
        GameObject textParent = Instantiate(damageText, gameObject.transform);
        for (int i = 0; i < textParent.transform.childCount; i++)
        {
            Transform child = textParent.transform.GetChild(i);
            TextMeshProUGUI text = child.GetComponent<TextMeshProUGUI>();
            if (text == null) return;
            text.text = Math.Round(damage).ToString();
        }
    }
}
