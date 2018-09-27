using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwProjectile : MonoBehaviour {
    public String ballPrefabName = "newBall";

    GameObject Ball;
    public int ThrowSpeed = 5;
    public int projectileMass = 1;
    GameObject currentBall = null;
    public int forceMultiplyer = 10;
    public int maxBalls = 5;
    public string[] ballIDs = null;
    public float playBallDistance = 2f;


	// Use this for initialization
	void Start () {
        Ball = Resources.Load(ballPrefabName) as GameObject;
        if(Ball == null)
        {
            Debug.LogError("Cant find prefab in Resource Folder with name: " + ballPrefabName);
        }
        
        //
        ballIDs = new string[0];
}
	
	// Update is called once per frame
	void FixedUpdate () {

        //on left click throw a ball
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newBall = this.makeBall();
            throwBall(newBall);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if(currentBall != null)
            {
                resetBall(this.currentBall);
            }
        }

        //change ThrowSpeed
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            //increase
            ThrowSpeed = ThrowSpeed+forceMultiplyer;
        } else if(Input.GetAxis("Mouse ScrollWheel") < 0f )
        {
            //decrease
            if(ThrowSpeed > 0)
            {
                ThrowSpeed = ThrowSpeed - forceMultiplyer;
            }
        }
	}

    public void resetBall(GameObject currentBall)
    {
        if(currentBall == null)
        {
            return;
        }

        Debug.Log("R Key pressed");
        //reset ball to camer position
        TxSoftBody txSoft = currentBall.GetComponent<TxSoftBody>();
        txSoft.enabled = false;
        Rigidbody rb = currentBall.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.transform.position = Camera.main.transform.position + Camera.main.transform.forward * playBallDistance;
        Debug.Log("Camera LocalPos: " + Camera.main.transform.localPosition);
        Debug.Log("Camera Pos: " + Camera.main.transform.position);
    }

    public GameObject makeBall()
    {
        try
        {
            GameObject newBall = Instantiate(Ball) as GameObject;
            newBall.transform.parent = null;
            currentBall = newBall;
            currentBall.transform.position = Camera.main.transform.position + Camera.main.transform.forward * playBallDistance;

            //add ballName to list
            if (ballIDs.Length >= maxBalls)
            {
                this.gcBalls();
            }
            else
            {
                ballIDs = new string[ballIDs.Length + 1];
                ballIDs[ballIDs.Length - 1] = newBall.name;
            }

            return currentBall;
        }
        catch
        {
            Debug.LogError("can not create Ball. Maybe prefab is missing.");
            return null;
        }
    }

    public void destroyAllBalls()
    {
        for(int i = 1; i < ballIDs.Length; i++)
        {
            GameObject tmp = GameObject.Find(ballIDs[i]);
            if(tmp != null)
            {
                Destroy(tmp);
            }
        }
    }

    public bool destroyBallWithName(string name)
    {
        try
        {
            for (int i = 1; i < ballIDs.Length; i++)
            {
                if (ballIDs[i] == name)
                {
                    GameObject tmp = GameObject.Find(ballIDs[i]);
                    if (tmp != null)
                    {
                        Destroy(tmp);
                        return true;
                    }
                }
            }
            return true;
        } catch
        {
            return false;
        }
    }

    public void gcBalls()
    {
        try
        {
            if (ballIDs.Length >= maxBalls)
            {
                int lengthBalls = ballIDs.Length;
                if (lengthBalls > 1)
                {
                    for (int i = lengthBalls; i >= lengthBalls - 5; i--)
                    {
                        this.destroyBallWithName(ballIDs[i-1]);
                    }
                }
            }
        } catch (Exception error)
        {
            //
            Debug.LogWarning("garbage collector error - " + error.Message);
        }
        
    }

    public void throwBall(GameObject throwableBall)
    {
        TxSoftBody tSoft = throwableBall.GetComponent<TxSoftBody>();
        try
        {
            tSoft.enabled = false;
            tSoft.transform.position = Camera.main.transform.position + Camera.main.transform.forward * playBallDistance;
            tSoft.enabled = true;
            tSoft.ApplyImpulse(Camera.main.transform.forward * ThrowSpeed);
        }
        catch
        {
            //
        }
    }

   

    #region public Helper Methods
    /**
    * Deprecated - can be removed
    */
    public bool setWorldAnchorToPosition(Vector3 worldPosition, Transform anchorObject)
    {
        if(anchorObject != null)
        {
            anchorObject.position = worldPosition;
            return true;
        } 
        return false;
    }
    #endregion
}
