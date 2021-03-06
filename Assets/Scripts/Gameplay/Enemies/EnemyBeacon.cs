using System.Collections;
using System.Linq;
using Gameplay.Enemies.Species;
using NyarlaEssentials;
using UnityEngine;
using World;

namespace Gameplay.Enemies
{
    public class EnemyBeacon : Transformer
    {
        [SerializeField] private EnemySpecie[] _enemyRoster;
        [SerializeField] private float _delay;
        [SerializeField] private MeshRenderer _meshRenderer;

        private Room _roomIn;
        
        public void Init(Room roomIn)
        {
            _roomIn = roomIn;
            StartCoroutine(SpawnEnemy());
        }

        private IEnumerator SpawnEnemy()
        {
            EnemySpecie specie = _enemyRoster.ToList().ChooseRandomElement();
            _meshRenderer.material = specie.BeaconMaterial;
            yield return new WaitForSeconds(_delay);
            EnemyVitals enemy = Instantiate(specie.gameObject, transform.position.WithY(0),
                    Quaternion.identity, _roomIn.transform).GetComponent<EnemyVitals>();
            
            _roomIn.AddEnemyAlive(enemy);
            Destroy(gameObject);
        }
    }
}