using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = player.transform.position;
        transform.position = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);
/*        transform.rotation = Quaternion.Euler(90f, player.transform.eulerAngles.y, 0f);*/
    }
}
