using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Watermelon
{
    [DisallowMultipleComponent]
    public class SimpleOutline : MonoBehaviour
    {
        private static HashSet<Mesh> alreadyUsedMeshes = new HashSet<Mesh>();

        public enum Mode
        {
            OutlineAll,
            OutlineVisible,
            OutlineHidden,
            OutlineAndSilhouette,
            SilhouetteOnly
        }

        public Mode OutlineMode
        {
            get { return outlineMode; }
            set
            {
                outlineMode = value;
                updateRequired = true;
            }
        }

        public float OutlineWidth
        {
            get { return outlineWidth; }
            set
            {
                outlineWidth = value;
                updateRequired = true;
            }
        }

        public Color OutlineColor
        {
            get { return outlineColor; }
            set
            {
                outlineColor = value;
                updateRequired = true;
            }
        }

        [SerializeField] Mode outlineMode;


        [SerializeField] Color outlineColor = Color.white;

        [SerializeField, Range(0f, 10f)] float outlineWidth = 2f;

        private Renderer[] renderers;
        private Material outlineMaskMaterial;
        private Material outlineFillMaterial;

        private bool updateRequired;

        void Awake()
        {
            renderers = GetComponentsInChildren<Renderer>();

            outlineMaskMaterial = Instantiate(Resources.Load<Material>(@"Materials/SimpleOutlineMask"));
            outlineFillMaterial = Instantiate(Resources.Load<Material>(@"Materials/SimpleOutlineFill"));

            outlineMaskMaterial.name = "SimpleOutlineMask (Instance)";
            outlineFillMaterial.name = "SimpleOutlineFill (Instance)";

            LoadSmoothNormals();
            updateRequired = true;
        }

        void OnEnable()
        {
            foreach (var renderer in renderers)
            {
                var materials = renderer.sharedMaterials.ToList();

                materials.Add(outlineMaskMaterial);
                materials.Add(outlineFillMaterial);

                renderer.materials = materials.ToArray();
            }
        }

        void Update()
        {
            if (updateRequired)
            {
                updateRequired = false;

                UpdateMaterialProperties();
            }
        }

        void OnDisable()
        {
            foreach (var renderer in renderers)
            {
                var materials = renderer.sharedMaterials.ToList();

                materials.Remove(outlineMaskMaterial);
                materials.Remove(outlineFillMaterial);

                renderer.materials = materials.ToArray();
            }
        }

        void OnDestroy()
        {
            Destroy(outlineMaskMaterial);
            Destroy(outlineFillMaterial);
        }

        void UpdateMaterialProperties()
        {
            outlineFillMaterial.SetColor("_OutlineColor", outlineColor);

            switch (outlineMode)
            {
                case Mode.OutlineAll:
                    outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                    break;

                case Mode.OutlineVisible:
                    outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                    outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                    break;

                case Mode.OutlineHidden:
                    outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                    outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                    break;

                case Mode.OutlineAndSilhouette:
                    outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                    outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                    break;

                case Mode.SilhouetteOnly:
                    outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                    outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                    outlineFillMaterial.SetFloat("_OutlineWidth", 0);
                    break;
            }
        }

        void LoadSmoothNormals()
        {
            foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
            {
                if (!alreadyUsedMeshes.Add(meshFilter.sharedMesh))
                    continue;

                var smoothNormals = SmoothNormals(meshFilter.sharedMesh);

                meshFilter.sharedMesh.SetUVs(3, smoothNormals);
            }

            foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                if (alreadyUsedMeshes.Add(skinnedMeshRenderer.sharedMesh))
                    skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];
            }
        }

        List<Vector3> SmoothNormals(Mesh mesh)
        {
            var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);
            var smoothNormals = new List<Vector3>(mesh.normals);

            foreach (var group in groups)
            {
                if (group.Count() == 1)
                    continue;

                var smoothedNormals = Vector3.zero;

                foreach (var pair in group)
                {
                    smoothedNormals += mesh.normals[pair.Value];
                }

                smoothedNormals.Normalize();

                foreach (var pair in group)
                {
                    smoothNormals[pair.Value] = smoothedNormals;
                }
            }

            return smoothNormals;
        }

    }
}