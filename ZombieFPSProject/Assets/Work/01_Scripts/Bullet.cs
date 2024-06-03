using System.Collections;
using System.Collections.Generic;
using ObjectPooling;
using UnityEngine;

public class Bullet : PoolableMono
{
    public float speed = 10f;

    void Start()
    {
        StartCoroutine(DisableBullet());
    }

    void Update()
    {
        Vector3 dir =  speed * Time.deltaTime * Vector3.left;
        //dir = transform.TransformDirection(dir);
        transform.Translate(dir);
    }

    private IEnumerator DisableBullet()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        PoolManager.Instance.Push(this);
    }

    public override void ResetItem()
    {
        gameObject.SetActive(true);
    }
}
