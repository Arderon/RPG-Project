using RPG.Control;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IReycastable
    {
        [SerializeField] Weapon weaponPrefab;
        [SerializeField] float respawnTime = 5;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.GetComponent<Fighter>().GetCurrentWeapon() == weaponPrefab) return;
                PickupWeapon(other.GetComponent<Fighter>());
            }
        }

        private void PickupWeapon(Fighter fighter)
        {
            fighter.GetComponent<Fighter>().EquipWeapon(weaponPrefab);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float time)
        {
            HidePickup();
            yield return new WaitForSeconds(time);
            ShowPickup();
        }

        private void HidePickup()
        {
            GetComponent<Collider>().enabled = false;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        private void ShowPickup()
        {
            GetComponent<Collider>().enabled = true;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        public bool HandleRaycast(PlayerController playerController)
        {
            if (Input.GetMouseButtonDown(1))
            {
                playerController.GetComponent<Mover>().MoveTo(transform.position);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Item;
        }
    }

}