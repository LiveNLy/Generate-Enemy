using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private float _speed;

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Plane plane))
            _spawner.ReleaseEnemy(this);
    }

    private void Update()
    {
        transform.position += transform.forward * _speed;
    }
}
