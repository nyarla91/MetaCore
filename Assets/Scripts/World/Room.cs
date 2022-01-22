using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using NyarlaEssentials;
using Player;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace World
{
    public class Room : Transformer
    {
        [SerializeField] private WallPiece[] _exitWalls;
        [SerializeField] private float _raiseSpeed;
        [SerializeField] private Collider _enemyBounds;
        [SerializeField] private GameObject _enemyBeaconPrefab;
        
        private float _targetY;
        private bool _competed = true;
        private List<Vector3>[] _enemyWaves;
        public PlayerMarker _playerMarker { get; set; }
        private List<EnemyStatus> _enemiesAlive = new List<EnemyStatus>();
        
        public WallPiece GetExitWall(int index) => _exitWalls[index];

        public void Generate(bool isFinalAtWing)
        {
            _competed = false;
            int waves = 2 + Random.Range(-1, 1);

            _enemyWaves = new List<Vector3>[waves];
            for (int wave = 0; wave < waves; wave++)
            {
                _enemyWaves[wave] = new List<Vector3>();
                int enemies = 5 - waves + Random.Range(-1, 1);
                for (int enemy = 0; enemy < enemies; enemy++)
                {
                    Vector3 relativePosition = _enemyBounds.bounds.RandomPointInBounds() - transform.position;
                    relativePosition.y = 0;
                    _enemyWaves[wave].Add(relativePosition);
                }
            }
        }

        public void AddEnemyAlive(EnemyStatus enemyStatus) => _enemiesAlive.Add(enemyStatus);

        public void Show()
        {
            _targetY = 0;
            _playerMarker.CurrentRoom = this;
            _playerMarker.Core.ReturnCore();
            if (!_competed)
                StartCoroutine(Combat());
        }

        public void Hide()
        {
            _targetY = -200;
        }

        private IEnumerator Combat()
        {
            foreach (var wave in _enemyWaves)
            {
                yield return new WaitForSeconds(1);
                foreach (var beacon in wave)
                {
                    Vector3 position = transform.position + beacon;
                    EnemyBeacon enemyBeacon = Instantiate(_enemyBeaconPrefab, position, Quaternion.identity, transform)
                        .GetComponent<EnemyBeacon>();
                    enemyBeacon.Init(this);
                }
                yield return new WaitUntil(() => _enemiesAlive.Count == wave.Count);
                foreach (var enemyAlive in _enemiesAlive)
                {
                    enemyAlive.OnDeath += () => { _enemiesAlive.Remove(enemyAlive); };
                }
                yield return new WaitUntil(() => _enemiesAlive.Count == 0);
            }
            _competed = true;
        }
        
        private void FixedUpdate()
        {
            float newY = Mathf.Lerp(transform.position.y, _targetY, Time.fixedDeltaTime * _raiseSpeed);
            transform.position = transform.position.WithY(newY);
        }
    }
}