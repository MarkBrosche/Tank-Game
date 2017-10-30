using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    // This script controls the properties of the shells fired from the tank
    // destroys on collision with a rigid body
    // create explosion and smoke on collision

    public float speed = 20f;
    public Transform bulletSpawn;
    public GameObject bulletPrefab, hitParticle, muzFlashParticle;
    public AudioClip shot;
    private AudioSource _shot;

    // Use this for initialization
    void Start()
    {
        _shot.PlayOneShot(shot);
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Fire is called to create, apply trajectory, and play the sound of a bullet
    void Fire()
    {
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * speed;
        Instantiate(muzFlashParticle);
        _shot.PlayOneShot(shot);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Shot Destroyer") || other.gameObject.CompareTag("Player"))
        {
            Instantiate(hitParticle, bulletPrefab.transform.position, bulletPrefab.transform.rotation);
            Destroy(bulletPrefab);
        }
    }
    private void Awake()
    {
        _shot = GetComponent<AudioSource>();
    }
}
