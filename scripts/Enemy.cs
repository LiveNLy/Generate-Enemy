using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;

    public event Action<Enemy> Releasing;

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Plane plane))
            Releasing?.Invoke(this);
    }

    public void StartMoving(Vector3 direction)
    {
        StartCoroutine(Move(direction));
    }

    private IEnumerator Move(Vector3 direction)
    {
        var wait = new WaitForEndOfFrame();

        while (true)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
            yield return wait;
        }
    }
}
