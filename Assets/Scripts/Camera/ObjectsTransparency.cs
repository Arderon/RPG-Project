using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngineInternal;

namespace RPG.Camera
{
    public class ObjectsTransparency : MonoBehaviour
    {
        [SerializeField] Material transparentMaterial;
        [SerializeField] float overlapSphereRadius = 3f;

        private float cameraDistance;
        private float layerMask = 3;

        private Dictionary<MeshRenderer, Material> materialMap = new Dictionary<MeshRenderer, Material>();


        private void Start()
        {
            cameraDistance = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) - 0.2f;
        }

        void Update()
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, cameraDistance);
            List<MeshRenderer> renderers = new List<MeshRenderer>();
            foreach (RaycastHit hit in hits)
            {
                MeshRenderer meshRenderer = hit.transform.GetComponent<MeshRenderer>();
                if (meshRenderer == null) continue;
                AddToTransparent(renderers, meshRenderer);
            }

            //if camera inside object it become transparent
            //for this we can first get all object nearby and than check if camera inside in any of them
            ProcessOutsideObjects(renderers);

            List<MeshRenderer> renderersToDelete = new List<MeshRenderer>();
            foreach (MeshRenderer renderer in materialMap.Keys)
            {
                //inside this loop, you cannot add or remove elements from materialMap because it will cause an error
                //so I decide to create list lenderers to delete for deleting annececary renderers but I need to find better way for doing this
                if (!renderers.Contains(renderer))
                {
                    SetPrimaryMaterial(renderer);
                    renderersToDelete.Add(renderer);
                }
            }

            foreach(MeshRenderer renderer in renderersToDelete)
            {
                materialMap.Remove(renderer);
            }
        }

        private void ProcessOutsideObjects(List<MeshRenderer> renderers)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, overlapSphereRadius);
            foreach (Collider collider in hitColliders)
            {
                if(collider == gameObject.GetComponent<Collider>()) continue;

                if (collider.bounds.Contains(transform.position))
                {
                    AddToTransparent(renderers, collider.GetComponent<MeshRenderer>());
                }
            }
        }

        private void AddToTransparent(List<MeshRenderer> renderers, MeshRenderer meshRenderer)
        {
            if (meshRenderer.gameObject.layer != layerMask) return;
            renderers.Add(meshRenderer);
            if (materialMap.ContainsKey(meshRenderer)) return;
            SetTransparentMaterial(meshRenderer);
        }

        private void SetTransparentMaterial(MeshRenderer meshRenderer)
        {
            materialMap.Add(meshRenderer, meshRenderer.material);
            meshRenderer.material = transparentMaterial;
        }

        private void SetPrimaryMaterial(MeshRenderer renderer)
        {
            if (renderer == null) return;
            renderer.material = materialMap[renderer];
        }
    }

}