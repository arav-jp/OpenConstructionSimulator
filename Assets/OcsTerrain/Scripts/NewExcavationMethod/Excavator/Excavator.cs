using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Excavator : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float _minForceToExcavate;
    [SerializeField] private LayerMask _terrainLayer;

    [Header("Informations(No need to input)")]
    [SerializeField] private DeformableTerrain _deformableTerrain;
    [SerializeField] private SandManager _sandManager;

    [SerializeField] private Rigidbody _rigidbody;

    [Header("Debug")]
    [SerializeField] private bool _showForceArrow;
    [SerializeField] private bool _isExcavating;

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

        if (collision.impulse.magnitude == 0) return;

        Vector3 force = -collision.impulse / Time.fixedDeltaTime / (float)collision.contacts.Length;
        force.y = 0;
        
        if (force.magnitude < _minForceToExcavate) return;

        foreach (ContactPoint cp in collision.contacts)
        {
            Vector3 pos_cp = cp.point;
            Vector3 pos_sp = cp.point + force.normalized*_sandManager._maxSandRadius*2;

            _deformableTerrain.SetHeight(pos_cp, pos_sp, pos_cp.y - _sandManager._maxSandRadius);

            RaycastHit hit;
            if (Physics.Raycast(new Vector3(pos_sp.x, _deformableTerrain._terrainSize.y + 0.1f, pos_sp.z), Vector3.down, out hit, _deformableTerrain._terrainSize.y + 0.2f, _terrainLayer))
            {
                if (hit.collider.tag == "Terrain")
                {
                    float height_sp = _deformableTerrain._terrainSize.y - hit.distance;
                    while (height_sp > pos_cp.y)
                    {
                        float radius = Random.Range(_sandManager._minSandRadius, _sandManager._maxSandRadius);
                        if (radius*2 > height_sp - pos_cp.y) radius = (height_sp -  pos_cp.y)/0.5f;
                        _sandManager.Spawn(new Vector3(pos_sp.x, height_sp, pos_sp.z));
                        height_sp -= radius*2;
                    }
                    
                }
            }

#if UNITY_EDITOR
            if (_showForceArrow)
            {
                Debug.DrawLine(pos_cp, pos_sp, Color.white);
            }
#endif
        }
        

        _deformableTerrain.OnHeightmapChanged();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag != "Terrain") return;
        _isExcavating = false;
    }
}
