﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    //ID for Powerups
    //0 = Triple Shot
    //1 = Speed
    //2 = Shields
    [SerializeField]
    private int _powerupID;

    private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start()
    {
        _audioManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.6)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            var player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.RefillAmmo();
                        break;
                    case 4:
                        player.HealPlayer();
                        break;
                    case 5:
                        player.SlowDownActive();
                        break;
                    case 6:
                        player.SprayShotActive();
                        break;
                    case 7:
                        player.HomingShotActive();
                        break;
                }
            }
            _audioManager.PlayPowerupSound();
            Destroy(this.gameObject);
        }
        else if (other.tag == "Laser")
        {
            var laser = other.gameObject.GetComponent<Laser>();
            if (laser != null)
            {
                if (laser._isEnemyLaser)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
