using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    [SerializeField] GameObject damageText;


    public void SpawnText(float damage)
    {
        Instantiate(damageText, gameObject.transform);
    }
}
