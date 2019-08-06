using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {
    public GameObject bulletPrefab;
    Transform shootingPoint;
    Word targetWord;
    void Start() {
        shootingPoint = transform.Find("ShootingPoint");
    }
    public void Shoot(Word targetWord) {
        this.targetWord = targetWord;
        var bullet = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetTarget(targetWord);

        var direction = (targetWord.GetPosition() - (Vector2)transform.position).normalized;
        transform.up = direction;
    }
}
