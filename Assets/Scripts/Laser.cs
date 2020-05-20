using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8;
    [SerializeField]
    private bool _isEnemyLaser;

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser)
        {
            MoveDown();
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

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }
}
