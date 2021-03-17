using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody rb;
    public float impulseForce = 3f;

    private bool ignoreNextCollision;

    private Vector3 startPosition;


    [HideInInspector]
    public int perfectPass;

    public float superSpeed = 10;

    private bool isSuperSpeedActive;

    public int perfectPassCount = 3;


    private void Start()
    {
        startPosition = transform.position;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ignoreNextCollision)
        {
            return;
        }
        DeathPart deathPart = collision.transform.GetComponent<DeathPart>();
        if (deathPart)
        {
            GameManager.singleton.RestartLevel();
        }

        //if (isSuperSpeedActive && !collision.transform.GetComponent<GameObject>())
        //{
        //    Destroy(collision.transform.parent.gameObject, 0.2f);
        //}
        //else
        //{
        //    DeathPart deathPart = collision.transform.GetComponent<DeathPart>();
        //    if (deathPart)
        //    {
        //        GameManager.singleton.RestartLevel();
        //    }
        //}



        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);

        ignoreNextCollision = true;
        Invoke("AllowNextCollision", 0.2f);

        perfectPass = 0;
        isSuperSpeedActive = false;
    }

    private void Update()
    {
        if(perfectPass >= perfectPassCount && !isSuperSpeedActive)
        {
            isSuperSpeedActive = true;

            rb.AddForce(Vector3.down * superSpeed, ForceMode.Impulse);
        }
    }

    private void AllowNextCollision()
    {
        ignoreNextCollision = false;
    }

    public void ResetBall()
    {
        transform.position = startPosition;
    }

    
}
