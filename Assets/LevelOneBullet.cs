using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneBullet : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSecondsRealtime(15);

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!collision.gameObject.name.Contains("Demo"))
            {
                collision.gameObject.GetComponent<PlayerMovement>().KillPlayer();
            }
            else
            {
                Instantiate(collision.gameObject, collision.gameObject.GetComponent<PlayerDemo>().startLocation.transform.position, Quaternion.identity);

                Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.tag != "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
