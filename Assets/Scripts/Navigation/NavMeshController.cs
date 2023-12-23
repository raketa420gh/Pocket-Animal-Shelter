using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshController : MonoBehaviour
{
    [SerializeField] private NavMeshSurface _navMeshSurface;

    public void UpdateNavMesh()
    {
        _navMeshSurface.BuildNavMesh();
    }
    
}