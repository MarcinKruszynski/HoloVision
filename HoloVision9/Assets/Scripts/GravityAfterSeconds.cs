using System.Collections;
using UnityEngine;

public class GravityAfterSeconds: MonoBehaviour
{

    void Start()
    {
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {

        yield return new WaitForSeconds(10);
        OnShoot();
    }

    public void OnShoot()
    {
        if (!this.GetComponent<Rigidbody>())
        {
            var rigidbody = this.gameObject.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }
}
