using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FireRaising
{
    public class Fire : MonoBehaviour
    {
        [SerializeField] private float _fireBurningTime = 5f;
        [SerializeField] private float _fireSpreadingTime = 0.1f;

        [Space(5f)]

        [SerializeField] private FirePhases _firePhase = FirePhases.Start;
        [SerializeField] private FireSides _fireSide;

        [SerializeField] private TileBase _puddleTileUpBurned;
        [SerializeField] private TileBase _puddleTileDownBurned;
        [SerializeField] private TileBase _puddleTileRightBurned;
        [SerializeField] private TileBase _puddleTileLeftBurned;

        private Tilemap _puddlesUpTilemap;
        private Tilemap _puddlesDownTilemap;
        private Tilemap _puddlesRightTilemap;
        private Tilemap _puddlesLeftTilemap;
        private Tilemap _immortalPuddlesTilemap;

        private Animator _animator;

        private Vector3 _firePos = Vector3.zero;
        public Vector3 FirePos { get => _firePos; set => _firePos = value; }

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            _puddlesUpTilemap = GameObject.FindGameObjectWithTag("PuddlesUpGrid").GetComponent<Tilemap>();
            _puddlesDownTilemap = GameObject.FindGameObjectWithTag("PuddlesDownGrid").GetComponent<Tilemap>();
            _puddlesRightTilemap = GameObject.FindGameObjectWithTag("PuddlesRightGrid").GetComponent<Tilemap>();
            _puddlesLeftTilemap = GameObject.FindGameObjectWithTag("PuddlesLeftGrid").GetComponent<Tilemap>();
            _immortalPuddlesTilemap = GameObject.FindGameObjectWithTag("ImmortalPuddlesGrid").GetComponent<Tilemap>();
        }

        private void OnEnable()
        {
            StartCoroutine(FireCycle());
            StartCoroutine(Spreading());
        }

        private IEnumerator Spreading()
        {
            while (true)
            {
                if (_firePhase != FirePhases.End)
                {
                    Vector3Int rightTile = Vector3Int.FloorToInt(new Vector3(_firePos.x + 1, _firePos.y - 1, 0));
                    Vector3Int leftTile = Vector3Int.FloorToInt(new Vector3(_firePos.x - 1, _firePos.y - 1, 0));

                    Vector3Int firePos;

                    if (_fireSide == FireSides.Up)
                    {
                        if (_puddlesUpTilemap.HasTile(rightTile) == true && IsBurned(_puddlesUpTilemap.GetTile(rightTile)) == false)
                        {
                            firePos = rightTile;
                            firePos.y += 1;

                            if (!FireController.Instance.GetCellsWithFireList.Contains(_puddlesUpTilemap.GetCellCenterWorld(firePos)))
                            {
                                yield return new WaitForSeconds(_fireSpreadingTime);
                                FireController.Instance.IgniteTile(_puddlesUpTilemap.GetCellCenterWorld(firePos), FireSides.Up);
                            }
                        }

                        if (_puddlesUpTilemap.HasTile(leftTile) == true && IsBurned(_puddlesUpTilemap.GetTile(leftTile)) == false)
                        {
                            firePos = leftTile;
                            firePos.y += 1;

                            if (!FireController.Instance.GetCellsWithFireList.Contains(_puddlesUpTilemap.GetCellCenterWorld(firePos)))
                            {
                                yield return new WaitForSeconds(_fireSpreadingTime);
                                FireController.Instance.IgniteTile(_puddlesUpTilemap.GetCellCenterWorld(firePos), FireSides.Up);
                            }
                        }
                    }
                }
            }
        }

        public void FiringEnd()
        {
            Vector3Int pos = Vector3Int.zero;

            switch (_fireSide)
            {
                case FireSides.Up:
                    pos = _puddlesUpTilemap.WorldToCell(transform.position);
                    pos.y -= 1;

                    _puddlesUpTilemap.SetTile(pos, _puddleTileUpBurned);
                    break;

                case FireSides.Down:
                    pos = _puddlesDownTilemap.WorldToCell(transform.position);
                    pos.y += 1;

                    _puddlesDownTilemap.SetTile(pos, _puddleTileDownBurned);
                    break;

                case FireSides.Right:
                    pos = _puddlesRightTilemap.WorldToCell(transform.position);
                    pos.x -= 1;

                    _puddlesRightTilemap.SetTile(pos, _puddleTileRightBurned);
                    break;

                case FireSides.Left:
                    pos = _puddlesLeftTilemap.WorldToCell(transform.position);
                    pos.x += 1;

                    _puddlesLeftTilemap.SetTile(pos, _puddleTileLeftBurned);
                    break;
            }
        }

        private IEnumerator FireCycle()
        {
            _animator.Play("Start", 0, 0);
            _firePhase = FirePhases.Start;

            yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length + 0.15f);

            _animator.Play("Firing", 0, 0);
            _firePhase = FirePhases.Firing;

            yield return new WaitForSeconds(_fireBurningTime);

            _animator.Play("End", 0, 0);
            _firePhase = FirePhases.End;

            FiringEnd();

            yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length - 0.75f);

            FireController.Instance.FiringEnd(_firePos);
            gameObject.SetActive(false);
        }

        private bool IsBurned(TileBase tile)
        {
            return tile == _puddleTileUpBurned || tile == _puddleTileDownBurned || tile == _puddleTileRightBurned || tile == _puddleTileLeftBurned;
        }
    }

    public enum FireSides
    {
        Up,
        Down,
        Right,
        Left
    }

    public enum FirePhases
    {
        Start,
        Firing,
        End
    }
}
