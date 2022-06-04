using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
[RequireComponent(typeof(TerrainCollider))]
public class DeformableTerrain : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private Terrain _terrain;

    [Header("Informations(No need to input)")]
    [SerializeField] private SandManager _sandManager;

    [SerializeField] private TerrainData _terrainData;

    [SerializeField] private Vector3 _terrainSize;

    [SerializeField] private Vector3 _dimensionRatio;

    [SerializeField] private int _terrainHeightmapResolution;

    [SerializeField] private bool _isHeightmapChanged;

    [Header("Debug")]
    [SerializeField] private bool _noDeform;

    private float[,] _heightmap;
    private float[,] _originalHeightmap;

    private void Awake()
    {
        _sandManager = Object.FindObjectOfType<SandManager>();

        _terrain = GetComponent<Terrain>();
        _terrainData = _terrain.terrainData;
        _terrainSize = _terrain.terrainData.size;
        _terrainHeightmapResolution = this._terrain.terrainData.heightmapResolution;
        _heightmap = _terrain.terrainData.GetHeights(0, 0, _terrainHeightmapResolution, _terrainHeightmapResolution);
        _originalHeightmap = _terrain.terrainData.GetHeights(0, 0, _terrainHeightmapResolution, _terrainHeightmapResolution);
        _dimensionRatio = new Vector3(_terrainHeightmapResolution / _terrainSize.x,
                                           1 / _terrainSize.y,
                                           _terrainHeightmapResolution / _terrainSize.z);
    }
    
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        StartCoroutine(this.UpdateTerrainCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHeight(Vector3 pos, float height)
    {
        if (_noDeform) return;
        int pos_z = (int)(pos.z * (_terrainHeightmapResolution - 1) / _terrainSize.z);
        int pos_x = (int)(pos.x * (_terrainHeightmapResolution - 1) / _terrainSize.x);
        _heightmap[pos_z, pos_x] = height / _terrainSize.y;
        _heightmap[pos_z+1, pos_x] = height / _terrainSize.y;
        _heightmap[pos_z -1, pos_x] = height / _terrainSize.y;
        _heightmap[pos_z, pos_x+1] = height / _terrainSize.y;
        _heightmap[pos_z, pos_x-1] = height / _terrainSize.y;
        _heightmap[pos_z + 1, pos_x + 1] = height / _terrainSize.y;
        _heightmap[pos_z + 1, pos_x - 1] = height / _terrainSize.y;
        _heightmap[pos_z - 1, pos_x + 1] = height / _terrainSize.y;
        _heightmap[pos_z - 1, pos_x - 1] = height / _terrainSize.y;
    }

    public float GetHeight(Vector3 pos)
    {
        int pos_z = (int)(pos.z * (_terrainHeightmapResolution - 1) / _terrainSize.z);
        int pos_x = (int)(pos.x * (_terrainHeightmapResolution - 1) / _terrainSize.x);
        return _heightmap[pos_z, pos_x] * _terrainSize.y;
    }


    private IEnumerator UpdateTerrainCoroutine()
    {
        for (; ; )
        {
            if (_isHeightmapChanged)
            {
                _terrain.terrainData.SetHeightsDelayLOD(0, 0, _heightmap);
                _isHeightmapChanged = false;
                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
        }
        //yield break;
    }

    public void OnHeightmapChanged()
    {
        _isHeightmapChanged = true;
    }

    private void OnApplicationQuit()
    {
        RestoreTerrain();
    }

    public void RestoreTerrain()
    {
        _terrain.terrainData.SetHeightsDelayLOD(0, 0, _originalHeightmap);
    }
}
