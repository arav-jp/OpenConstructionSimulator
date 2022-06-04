using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Excavator : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private LayerMask _terrainLayer;

    [Header("Informations(No need to input)")]
    [SerializeField] private DeformableTerrain _deformableTerrain;
    [SerializeField] private SandManager _sandManager;

    [SerializeField] private Rigidbody _rigidbody;

    [Header("Debug")]
    [SerializeField] private bool _showForceArrow;

    private void Awake()
    {
        _deformableTerrain = Object.FindObjectOfType<DeformableTerrain>();
        _sandManager = Object.FindObjectOfType<SandManager>();

        _rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag != "Terrain") return;

        Vector3 force = -collision.impulse.normalized;
        force.y = 0.0f;
        force = force.normalized;

        foreach (ContactPoint cp in collision.contacts)
        {
            Vector3 pos = cp.point;
            float height = _deformableTerrain.GetHeight(pos);
            _deformableTerrain.SetHeight(pos, pos.y-0.1f);
            _sandManager.Spawn(pos + Vector3.up*_sandManager._sandRadius*3);
#if UNITY_EDITOR
            if (_showForceArrow)
            {
                Debug.DrawLine(pos, pos - collision.impulse.normalized, Color.white);
            }
#endif
        }

        _deformableTerrain.OnHeightmapChanged();
    }
}
