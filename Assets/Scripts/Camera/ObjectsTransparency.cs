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

        private Dictionary<MeshRenderer, Material> materialMap = new Dictionary<MeshRenderer, Material>(); 

        void Update()
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward);
            List<MeshRenderer> renderers = new List<MeshRenderer>();
            foreach (RaycastHit hit in hits)
            {
                MeshRenderer meshRenderer = hit.transform.GetComponent<MeshRenderer>();
                if (meshRenderer == null) continue;
                renderers.Add(meshRenderer);
                if (materialMap.ContainsKey(meshRenderer)) continue; 
                SetTransparentMaterial(meshRenderer);
            }

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