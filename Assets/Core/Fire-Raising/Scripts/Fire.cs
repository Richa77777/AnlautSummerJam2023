using PlayerSpace;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FireSpace
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
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<Player>() && _firePhase != FirePhases.End)
            {
                StopAllCoroutines();
                StartCoroutine(FiringEnd());
            }
        }

        private void Update()
        {
            if (_firePhase != FirePhases.End)
            {
                StartCoroutine(Spreading());
            }
        }

        private IEnumerator Spreading()
        {
            Vector3Int rightTile = Vector3Int.FloorToInt(new Vector3(_firePos.x + 1, _firePos.y, 0)); ;
            Vector3Int leftTile = Vector3Int.FloorToInt(new Vector3(_firePos.x - 1, _firePos.y, 0)); ;
            Vector3Int upTile = Vector3Int.FloorToInt(new Vector3(_firePos.x, _firePos.y + 1, 0)); ;
            Vector3Int downTile = Vector3Int.FloorToInt(new Vector3(_firePos.x, _firePos.y - 1, 0)); ;

            if (_fireSide == FireSides.Up)
            {
                if (_puddlesUpTilemap.HasTile(rightTile) == true && IsBurned(_puddlesUpTilemap.GetTile(rightTile)) == false)
                {
                    yield return new WaitForSeconds(_fireSpreadingTime);

                    FireController.Instance.TryIgniteTile(_puddlesUpTilemap.GetCellCenterWorld(new Vector3Int(rightTile.x, rightTile.y + 1, 0)), FireSides.Up);
                }

                if (_puddlesUpTilemap.HasTile(leftTile) == true && IsBurned(_puddlesUpTilemap.GetTile(leftTile)) == false)
                {
                    yield return new WaitForSeconds(_fireSpreadingTime);

                    FireController.Instance.TryIgniteTile(_puddlesUpTilemap.GetCellCenterWorld(new Vector3Int(leftTile.x, leftTile.y + 1, 0)), FireSides.Up);
                }

                if (_puddlesRightTilemap.HasTile(new Vector3Int(leftTile.x, leftTile.y + 1)) == true && IsBurned(_puddlesRightTilemap.GetTile(new Vector3Int(leftTile.x, leftTile.y + 1))) == false)
                {
                    yield return new WaitForSeconds(_fireSpreadingTime);

                    FireController.Instance.TryIgniteTile(_puddlesUpTilemap.GetCellCenterWorld(new Vector3Int(leftTile.x + 1, leftTile.y + 1)), FireSides.Right);
                }

                if (_puddlesLeftTilemap.HasTile(new Vector3Int(rightTile.x, rightTile.y + 1)) == true && IsBurned(_puddlesLeftTilemap.GetTile(new Vector3Int(rightTile.x, rightTile.y + 1))) == false)
                {
                    yield return new WaitForSeconds(_fireSpreadingTime);

                    FireController.Instance.TryIgniteTile(_puddlesUpTilemap.GetCellCenterWorld(new Vector3Int(rightTile.x - 1, rightTile.y + 1)), FireSides.Left);
                }
            }

            if (_fireSide == FireSides.Down)
            {
                if (_puddlesDownTilemap.HasTile(rightTile) == true && IsBurned(_puddlesDownTilemap.GetTile(rightTile)) == false)
                {
                    yield return new WaitForSeconds(_fireSpreadingTime);

                    FireController.Instance.TryIgniteTile(_puddlesDownTilemap.GetCellCenterWorld(new Vector3Int(rightTile.x, rightTile.y - 1, 0)), FireSides.Down);
                }

                if (_puddlesDownTilemap.HasTile(leftTile) == true && IsBurned(_puddlesDownTilemap.GetTile(leftTile)) == false)
                {
                    yield return new WaitForSeconds(_fireSpreadingTime);

                    FireController.Instance.TryIgniteTile(_puddlesDownTilemap.GetCellCenterWorld(new Vector3Int(leftTile.x, leftTile.y - 1, 0)), FireSides.Down);
                }

                if (_puddlesRightTilemap.HasTile(new Vector3Int(leftTile.x, leftTile.y - 1)) == true && IsBurned(_puddlesRightTilemap.GetTile(new Vector3Int(leftTile.x, leftTile.y - 1))) == false)
                {
                    yield return new WaitForSeconds(_fireSpreadingTime);

                    FireController.Instance.TryIgniteTile(_puddlesDownTilemap.GetCellCenterWorld(new Vector3Int(leftTile.x + 1, leftTile.y - 1)), FireSides.Right);
                }

                if (_puddlesLeftTilemap.HasTile(new Vector3Int(rightTile.x, rightTile.y - 1)) == true && IsBurned(_puddlesLeftTilemap.GetTile(new Vector3Int(rightTile.x, rightTile.y - 1))) == false)
                {
                    yield return new WaitForSeconds(_fireSpreadingTime);

                    FireController.Instance.TryIgniteTile(_puddlesDownTilemap.GetCellCenterWorld(new Vector3Int(rightTile.x - 1, rightTile.y - 1)), FireSides.Left);
                }
            }

            if (_fireSide == FireSides.Right)
            {
                if (_puddlesRightTilemap.HasTile(upTile) == true && IsBurned(_puddlesRightTilemap.GetTile(upTile)) == false)
                {
                    yield return new WaitForSeconds(_fireSpreadingTime);

                    FireController.Instance.TryIgniteTile(_puddlesRightTilemap.GetCellCenterWorld(new Vector3Int(upTile.x + 1, upTile.y, 0)), FireSides.Right);
                }

                if (_puddlesRightTilemap.HasTile(downTile) == true && IsBurned(_puddlesRightTilemap.GetTile(downTile)) == false)
                {
                    yield return new WaitForSeconds(_fireSpreadingTime);

                    FireController.Instance.TryIgniteTile(_puddlesRightTilemap.GetCellCenterWorld(new Vector3Int(downTile.x + 1, downTile.y, 0)), FireSides.Right);
                }

                if (_puddlesUpTilemap.HasTile(new Vector3Int(downTile.x + 1, downTile.y - 1)) == true && IsBurned(_puddlesUpTilemap.GetTile(new Vector3Int(downTile.x + 1, downTile.y - 1))) == false)
                {
                    yield return new WaitForSeconds(_fireSpreadingTime);

                    FireController.Instance.TryIgniteTile(_puddlesRightTilemap.GetCellCenterWorld(new Vector3Int(downTile.x + 1, downTile.y)), FireSides.Up);
                }

                if (_puddlesDownTilemap.HasTile(new Vector3Int(upTile.x + 1, upTile.y + 1)) == true && IsBurned(_puddlesDownTilemap.GetTile(new Vector3Int(upTile.x + 1, upTile.y + 1))) == false)
                {
                    yield return new WaitForSeconds(_fireSpreadingTime);

                    FireController.Instance.TryIgniteTile(_puddlesRightTilemap.GetCellCenterWorld(new Vector3Int(upTile.x + 1, upTile.y)), FireSides.Down);
                }
            }

            if (_fireSide == FireSides.Left)
            {
                if (_puddlesLeftTilemap.HasTile(upTile) == true && IsBurned(_puddlesLeftTilemap.GetTile(upTile)) == false)
                {
                    yield return new WaitForSeconds(_fireSpreadingTime);

                    FireController.Instance.TryIgniteTile(_puddlesLeftTilemap.GetCellCenterWorld(new Vector3Int(upTile.x - 1, upTile.y, 0)), FireSides.Left);
                }

                if (_puddlesLeftTilemap.HasTile(downTile) == true && IsBurned(_puddlesLeftTilemap.GetTile(downTile)) == false)
                {
                    yield return new WaitForSeconds(_fireSpreadingTime);

                    FireController.Instance.TryIgniteTile(_puddlesLeftTilemap.GetCellCenterWorld(new Vector3Int(downTile.x - 1, downTile.y, 0)), FireSides.Left);
                }

                if (_puddlesUpTilemap.HasTile(new Vector3Int(downTile.x - 1, downTile.y)) == true && IsBurned(_puddlesUpTilemap.GetTile(new Vector3Int(downTile.x - 1, downTile.y))) == false)
                {
                    yield return new WaitForSeconds(_fireSpreadingTime);

                    FireController.Instance.TryIgniteTile(_puddlesLeftTilemap.GetCellCenterWorld(new Vector3Int(downTile.x - 1, downTile.y + 1)), FireSides.Up);
                }

                if (_puddlesDownTilemap.HasTile(new Vector3Int(upTile.x - 1, upTile.y)) == true && IsBurned(_puddlesDownTilemap.GetTile(new Vector3Int(upTile.x - 1, upTile.y))) == false)
                {
                    yield return new WaitForSeconds(_fireSpreadingTime);

                    FireController.Instance.TryIgniteTile(_puddlesLeftTilemap.GetCellCenterWorld(new Vector3Int(upTile.x - 1, upTile.y - 1)), FireSides.Down);
                }
            }
        }

        private IEnumerator FiringEnd()
        {
            _animator.Play("End", 0, 0);
            _firePhase = FirePhases.End;

            Vector3Int pos = Vector3Int.zero;

            switch (_fireSide)
            {
                case FireSides.Up:
                    pos = _puddlesUpTilemap.WorldToCell(transform.position);
                    pos.y -= 1;

                    _puddlesUpTilemap.SetTile(pos, _puddleTileUpBurned);
                    BurnedTilesController.Instance.BurnedTileLiftCycleStart(pos, _puddlesUpTilemap);
                    break;

                case FireSides.Down:
                    pos = _puddlesDownTilemap.WorldToCell(transform.position);
                    pos.y += 1;

                    _puddlesDownTilemap.SetTile(pos, _puddleTileDownBurned);
                    BurnedTilesController.Instance.BurnedTileLiftCycleStart(pos, _puddlesDownTilemap);
                    break;

                case FireSides.Right:
                    pos = _puddlesRightTilemap.WorldToCell(transform.position);
                    pos.x -= 1;

                    _puddlesRightTilemap.SetTile(pos, _puddleTileRightBurned);
                    BurnedTilesController.Instance.BurnedTileLiftCycleStart(pos, _puddlesRightTilemap);
                    break;

                case FireSides.Left:
                    pos = _puddlesLeftTilemap.WorldToCell(transform.position);
                    pos.x += 1;

                    _puddlesLeftTilemap.SetTile(pos, _puddleTileLeftBurned);
                    BurnedTilesController.Instance.BurnedTileLiftCycleStart(pos, _puddlesLeftTilemap);
                    break;
            }

            yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length - 0.75f);

            FireController.Instance.FiringEnd(_firePos);
            gameObject.SetActive(false);
        }

        private IEnumerator FireCycle()
        {
            _animator.Play("Start", 0, 0);
            _firePhase = FirePhases.Start;

            yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length + 0.15f);

            _animator.Play("Firing", 0, 0);
            _firePhase = FirePhases.Firing;

            yield return new WaitForSeconds(_fireBurningTime);

            yield return StartCoroutine(FiringEnd());
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
