using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FireRaising
{
    public class Canister : MonoBehaviour
    {
        [SerializeField] private GameObject _fireUpPrefab;
        [SerializeField] private GameObject _fireDownPrefab;
        [SerializeField] private GameObject _fireRightPrefab;
        [SerializeField] private GameObject _fireLeftPrefab;

        private Tilemap _puddlesUpTilemap;
        private Tilemap _puddlesDownTilemap;
        private Tilemap _puddlesRightTilemap;
        private Tilemap _puddlesLeftTilemap;
        private Tilemap _immortalPuddlesTilemap;

        private void Start()
        {
            _puddlesUpTilemap = GameObject.FindGameObjectWithTag("PuddlesUpGrid").GetComponent<Tilemap>();
            _puddlesDownTilemap = GameObject.FindGameObjectWithTag("PuddlesDownGrid").GetComponent<Tilemap>();
            _puddlesRightTilemap = GameObject.FindGameObjectWithTag("PuddlesRightGrid").GetComponent<Tilemap>();
            _puddlesLeftTilemap = GameObject.FindGameObjectWithTag("PuddlesLeftGrid").GetComponent<Tilemap>();
            _immortalPuddlesTilemap = GameObject.FindGameObjectWithTag("ImmortalPuddlesGrid").GetComponent<Tilemap>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

        }
    }
}