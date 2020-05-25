using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8;
    [SerializeField]
    private bool _isEnemyLaser;
    private bool _isEnemySmartLaser;

    private SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn_Manager is null");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser)
        {
            MoveDown();
        }
        else if (_isEnemySmartLaser)
        {
            MoveUp();
        }
        else
        {
            switch (this.name)
            {
                case "Left_Laser":
                    Debug.Log("Left Laser");
                    MoveLeft();
                    break;
                case "Right_Laser":
                    Debug.Log("Right Laser");
                    MoveRight();
                    break;
                case "UpLeft_Laser":
                    MoveUpLeft();
                    break;
                case "UpRight_Laser":
                    MoveUpRight();
                    break;
                case "Homing":
                    HomeInToClosestEnemy();
                    break;
                default:
                    MoveUp();
                    break;
            }
        }

    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y >= 8)
        {
            if (this.transform.parent != null)
                Destroy(this.transform.parent.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -8)
        {
            if (this.transform.parent != null)
                Destroy(this.transform.parent.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void MoveLeft()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.x <= -11)
        {
            if (this.transform.parent != null)
                Destroy(this.transform.parent.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void MoveRight()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.x >= 11)
        {
            if (this.transform.parent != null)
                Destroy(this.transform.parent.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void MoveUpLeft()
    {
        var vector = Quaternion.Euler(0, 30, 0) * Vector3.up;
        transform.Translate(vector * _speed * Time.deltaTime);

        if (transform.position.y >= 8)
        {
            if (this.transform.parent != null)
                Destroy(this.transform.parent.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void MoveUpRight()
    {
        var vector = Quaternion.Euler(0, -30, 0) * Vector3.up;
        transform.Translate(vector * _speed * Time.deltaTime);

        if (transform.position.y >= 8)
        {
            if (this.transform.parent != null)
                Destroy(this.transform.parent.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void HomeInToClosestEnemy()
    {
        var closestEnemyPosition = _spawnManager.GetClosestEnemy(this.transform.position);

        if (closestEnemyPosition != null)
        {
            var rigidBody = GetComponent<Rigidbody2D>();
            Vector3 direction = closestEnemyPosition.position - this.transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotateToTarget = Quaternion.AngleAxis(angle, Vector3.forward);
            this.transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, Time.deltaTime * 0.5f);
            rigidBody.velocity = new Vector2(direction.x * _speed, direction.y * _speed);
        }
        else
        {
            MoveUp();
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    public void AssignEnemySmartLaser()
    {
        _isEnemySmartLaser = true;
    }
}
