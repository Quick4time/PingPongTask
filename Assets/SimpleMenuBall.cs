using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMenuBall : MonoBehaviour {

    private float startForce = 5.0f;
    private Rigidbody2D myRigidBody;


    // Use this for initialization
    void Start ()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        if (myRigidBody != null)
        {
            myRigidBody.gravityScale = 0.0f;
            myRigidBody.sharedMaterial = Resources.Load("PhysicsMaterials/Bouncy") as PhysicsMaterial2D;
        }
        PullBall();
    }

    void PullBall()
    {
        var ramdom = new[]
        {
            ProportionValue.Create(0.5f, new Vector2(-startForce/2, startForce)),
            ProportionValue.Create(0.5f, new Vector2(startForce/2, startForce)),
        };
        myRigidBody.velocity = ramdom.ChoseByRandom();
    }
}
