using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {
    public GameObject bulletPrefab;
    public void Shoot(Word targetWord) {
        var bullet = Instantiate(bulletPrefab, new Vector2(transform.position.x, transform.position.y + 2f), Quaternion.identity);
        bullet.GetComponent<Bullet>().SetTarget(targetWord);
    }
}
