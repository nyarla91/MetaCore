using Player;
using UnityEngine;

namespace Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyProjectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private MeshRenderer _mesh;
        [SerializeField] private float _speed;
        [SerializeField] private float _damage;

        private Vector3 _direction;

        public static void CreateProjectile(GameObject prefab, Transform origin, Vector3 direction)
        {
            direction.Normalize();
            EnemyProjectile newProjectile = GameObject.Instantiate(prefab, origin.position, Quaternion.identity)
                .GetComponent<EnemyProjectile>();
            newProjectile._direction = direction;
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = _direction * _speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerStatus playerStatus))
            {
                playerStatus.TakeDamage(_damage);
                Destroy(gameObject);
            }
            else if (other.gameObject.layer == 10)
            {
                Destroy(gameObject);
            }
        }
    }   
}
