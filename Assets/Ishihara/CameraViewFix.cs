using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.XR;
using static UnityEditor.Progress;
using UnityEngine.UIElements;
using UnityEngine.InputSystem.iOS;
using System.Diagnostics;

/// <summary>
/// マテリアルの透過を制御するスクリプト
/// </summary>
public class CameraViewFix : MonoBehaviour
{
    Camera camera;
    private Transform _player;
    [SerializeField]
    private List<Collider> lastRaycastHit = new List<Collider>();

    void Start()
    {
        camera = Camera.main;
        //_player = camera.Follow;
    }

    void Update()
    {
        if(_player == null)
        {
            _player = CharacterManager.instance.player.transform;
        }
        Vector3 position = camera.transform.position;
        Vector3 playerDirection = _player.position - position;
        float playerDistance =  Vector3.Distance(_player.position, position);
        List<RaycastHit> objects = GetObjectsInRaycast(position, playerDirection, playerDistance);
        foreach (var item in objects)
        {
            float distance = Vector3.Distance(item.point, position) / playerDistance;
            SkinnedMeshRenderer[] meshes = item.collider.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (item.collider.TryGetComponent(out MeshRenderer renderer))
            {
                foreach (var mat in renderer.materials)
                {
                   mat.SetFloat("_Alpha", Mathf.Lerp(0.3f, 1.2f, distance));
                }
            }
            else if(meshes.Length > 0)
            {
                foreach (var mesh in meshes)
                {
                    foreach (var mat in mesh.materials)
                    {
                        mat.SetFloat("_Alpha", Mathf.Lerp(0.3f, 1.2f, distance));
                    }
                }
            }
        }
        List<Collider> resetMaterial = lastRaycastHit
            .Where(a => !objects.Any(b => b.collider.name == a.name))
            .ToList();

        foreach (var item in resetMaterial)
        {
            SkinnedMeshRenderer[] meshes = item.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (item.TryGetComponent(out MeshRenderer renderer))
            {
                foreach (var mat in renderer.materials)
                {
                    mat.SetFloat("_Alpha", 1);
                }
            }
            else if (meshes.Length > 0)
            {
                foreach (var mesh in meshes)
                {
                    foreach (var mat in mesh.materials)
                    {
                        mat.SetFloat("_Alpha", 1);
                    }
                }
            }
        }
        // 一番最後の要素をコピーしてから一番最後の要素を消す
        for (int i = 0; i < resetMaterial.Count; i++)
        {
            int targetIndex = lastRaycastHit.IndexOf(resetMaterial[i]);
            if(targetIndex == -1) continue;
            // 消したいindexに一番最後の要素を移す
            lastRaycastHit[targetIndex] = lastRaycastHit[lastRaycastHit.Count - 1];
            // 一番最後の要素を削除
            lastRaycastHit.RemoveAt(lastRaycastHit.Count - 1);
        }
        foreach (var item in objects)
        {
            if (!lastRaycastHit.Contains(item.collider))
                lastRaycastHit.Add(item.collider);
        }
    }

    List<RaycastHit> GetObjectsInRaycast(Vector3 position, Vector3 playerDirection, float playerDistance)
    {
        List<RaycastHit> hitObjects = new List<RaycastHit>();
        Ray ray = new Ray(position, playerDirection.normalized);
        RaycastHit[] hits = Physics.RaycastAll(ray, playerDistance);


        foreach (var hit in hits)
        {
            hitObjects.Add(hit);
        }
        return hitObjects;
    }

    private void OnTriggerStay(Collider other)
    {
        SkinnedMeshRenderer[] meshes = other.GetComponentsInChildren<SkinnedMeshRenderer>();
        if (other.TryGetComponent(out MeshRenderer renderer))
        {
            foreach (var mat in renderer.materials)
            {
                mat.SetFloat("_Alpha", 0.3f);
            }
        }
        else if (meshes.Length > 0)
        {
            foreach (var mesh in meshes)
            {
                foreach (var mat in mesh.materials)
                {
                    mat.SetFloat("_Alpha", 0.3f);
                }
            }
        }

        //lastRaycastHit.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        SkinnedMeshRenderer[] meshes = other.GetComponentsInChildren<SkinnedMeshRenderer>();
        if (other.TryGetComponent(out MeshRenderer renderer))
        {
            foreach (var mat in renderer.materials)
            {
                mat.SetFloat("_Alpha", 1);
            }
        }
        else if (meshes.Length > 0)
        {
            foreach (var mesh in meshes)
            {
                foreach (var mat in mesh.materials)
                {
                    mat.SetFloat("_Alpha", 1);
                }
            }
        }
    }
}
