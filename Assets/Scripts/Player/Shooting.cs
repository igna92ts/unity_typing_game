using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {
    public GameObject bulletPrefab;
    Transform shootingPoint;
    Vector2 rotationDest;
    void Start() {
        shootingPoint = transform.Find("ShootingPoint");
    }
    public void Shoot(Word targetWord) {
        rotationDest = targetWord.GetPosition().normalized;
        var bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetTarget(targetWord);
    }
    void Update() {
        transform.up = Vector2.MoveTowards(transform.up, new Vector2(rotationDest.x, rotationDest.y), Time.deltaTime * 10f);
    }
}
