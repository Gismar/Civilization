using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCreator : MonoBehaviour {

    [SerializeField] private TileBase[] _terrainTiles;
    [SerializeField] private TileBase _village;
    [SerializeField] private Tilemap _map;
    [SerializeField] private Rect _mapSize;

    private void Awake()
    {
        float scale = 0.25f;
        for (int x = 0; x < _mapSize.width; x++)
            for (int y = 0; y < _mapSize.height; y++)
                if (x == Mathf.FloorToInt(_mapSize.width / 2f) && y == Mathf.FloorToInt(_mapSize.height / 2f))
                    _map.SetTile(new Vector3Int(x, y, 0), SetVillage(new Vector3Int(x, y, 0)));
                else
                    _map.SetTile(new Vector3Int(x, y, 0), GetTile(Mathf.PerlinNoise(x * scale, y * scale)));
        _map.CompressBounds();
    }

    TileBase SetVillage(Vector3Int pos)
    {
        GameObject.FindGameObjectWithTag("Master").GetComponent<GameMaster>().VillageSystem.FirstVillagePos(pos);
        return _village;
    }

    TileBase GetTile(float noise)
    {
        if (noise > 0.7f)
            return _terrainTiles[0];
        else if (noise > 0.55f)
            return _terrainTiles[1];
        else if (noise > 0.3f)
            return _terrainTiles[2];
        else
            return _terrainTiles[3];
    }
}
