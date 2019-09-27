using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform launchPosition;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsInvoking("fireBullet"))
            {
                InvokeRepeating("fireBullet", 0f, 0.1f);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            CancelInvoke("fireBullet");
        }
    }

    void fireBullet()
    {
        // Instantiate creates an instance of the bulletPrefab, of type Object, so we must cast is as GameObject
        GameObject bullet = Instantiate(bulletPrefab) as GameObject;
        // the bullets position is set to the launchers position
        bullet.transform.position = launchPosition.position;
        // bullet will be attached to marine (parent) so it will travel in same forward direction as marine faces
        bullet.GetComponent<Rigidbody>().velocity = transform.parent.forward * 100;

        audioSource.PlayOneShot(SoundManager.Instance.gunFire);
    }
}
