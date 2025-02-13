using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using System.Linq;

/// <summary>
/// マテリアルの透過を制御するスクリプト
/// </summary>
public class CameraViewFix : MonoBehaviour
{
    CinemachineFreeLook camera;
    private Transform _player;
    [SerializeField]
    private List<RaycastHit> lastRaycastHit = new List<RaycastHit>();

    void Start()
    {
        camera = GetComponent<CinemachineFreeLook>();
        //_player = camera.Follow;
    }

    void Update()
    {
        if(_player == null)
        {
            if (camera.Follow == null) return;
            _player = camera.Follow;
        }
        var playerDirection = _player.position - transform.position;
        var playerDistance = Vector3.Distance(_player.position, transform.position);
        var objects = GetObjectsInRaycast(playerDirection, playerDistance);
        foreach (var item in objects)
        {
            SkinnedMeshRenderer[] meshes = item.collider.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (item.collider.TryGetComponent(out MeshRenderer renderer))
            {
                foreach (var mat in renderer.materials)
                {
                   mat.SetFloat("_Alpha", Mathf.InverseLerp(0.2f, 3, playerDistance));
                }
            }
            else if(meshes.Length > 0)
            {
                foreach (var mesh in meshes)
                {
                    foreach (var mat in mesh.materials)
                    {
                        mat.SetFloat("_Alpha", Mathf.InverseLerp(0.2f, 1, playerDistance));
                    }
                }
            }
        }
        var resetMaterial = lastRaycastHit
            .Where(a => !objects.Any(b => b.collider.name == a.collider.name))
            .ToList();

        foreach (var item in resetMaterial)
        {
            if (item.collider.TryGetComponent(out MeshRenderer renderer))
            {
                foreach (var mat in renderer.materials)
                {
                    mat.SetFloat("_Alpha", 1);
                }
            }
        }
        lastRaycastHit = objects;
    }

    List<RaycastHit> GetObjectsInRaycast(Vector3 playerDirection, float playerDistance)
    {
        List<RaycastHit> hitObjects = new List<RaycastHit>();
        Ray ray = new Ray(transform.position, playerDirection.normalized);
        RaycastHit[] hits = Physics.RaycastAll(ray, playerDistance);

        Debug.DrawLine(ray.origin, ray.origin + (ray.direction * playerDistance), Color.red);

        foreach (var hit in hits)
        {
            hitObjects.Add(hit);
        }
        return hitObjects;
    }
}
