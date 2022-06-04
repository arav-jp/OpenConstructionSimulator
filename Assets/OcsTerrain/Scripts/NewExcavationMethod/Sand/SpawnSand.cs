using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSand : MonoBehaviour
{
    public int _id;
    public bool _onTerrain;

    public SandManager _sandManager;

    private MeshCollider mc;

    private void Awake()
    {
        mc = GetComponent<MeshCollider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        mc.enabled = false;
    }

    private void OnEnable()
    {
        _onTerrain = false;
        float distance = calcDistanceToNearObj();
        if(distance < 0.2)
        {
            _sandManager.Dispose(_id);
        }
        else
        {
            mc.enabled = true;
        }
    }

    private float calcDistanceToNearObj()
    {
        var targets = GameObject.FindGameObjectsWithTag(this.gameObject.tag);
        
        if (targets.Length == 1) return float.MaxValue;

        float minTargetDistance = float.MaxValue;
        foreach (var target in targets)
        {
            if (target == this.gameObject) continue;
            if (!target.activeSelf) continue;
            var targetDistance = Vector3.Distance(transform.position, target.transform.position);
            if (!(targetDistance < minTargetDistance)) continue;
            minTargetDistance = targetDistance;
        }
        return minTargetDistance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain") _onTerrain = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain") _onTerrain = false;
    }
}
