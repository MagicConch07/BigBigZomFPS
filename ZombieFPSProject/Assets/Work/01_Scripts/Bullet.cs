using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    void Start()
    {
        Destroy(gameObject, 2f);
    }

    void Update()
    {
        Vector3 dir = Vector3.left * speed * Time.deltaTime;
        //dir = transform.TransformDirection(dir);
        transform.Translate(dir);
    }
}
