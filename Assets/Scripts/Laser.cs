using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8;
    private bool _isEnemyLaser;

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser)
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            if (transform.position.y <= -8)
            {
                if (this.transform.parent != null)
                    Destroy(this.transform.parent.gameObject);
                Destroy(this.gameObject);
            }
        }
        else
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);

            if (transform.position.y >= 8)
            {
                if (this.transform.parent != null)
                    Destroy(this.transform.parent.gameObject);
                Destroy(this.gameObject);
            }
        }

    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }
}
