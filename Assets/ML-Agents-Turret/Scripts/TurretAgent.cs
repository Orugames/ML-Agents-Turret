
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class TurretAgent : Agent
{
    public BulletPool bulletPool;
    public GameObject objective;
    public GameObject turret;
    public GameObject turretEndPoint;


    public Vector2 posMin = new Vector2(0, 0);
    public Vector2 posMax = new Vector2(100, 100);

    public float lastRotationValue = 0f;

    public override void Initialize()
    {
        SetResetParameters();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Get angle between targeet and turret forward
        Vector3 targetDir = objective.transform.position - turret.transform.position;
        float angle = Vector3.Angle(turret.transform.forward, targetDir);
        float normalizedAngle = (angle - 0f) / (180f - 0f);

        sensor.AddObservation(normalizedAngle);
        sensor.AddObservation(objective.transform.position.z);
        sensor.AddObservation(objective.transform.rotation.x);



        //sensor.AddObservation(ball.transform.position - gameObject.transform.position);
        //sensor.AddObservation(m_BallRb.velocity);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        //var rotateAction = Mathf.Clamp(vectorAction[0], -1f, 1f);
        var rotateAction = vectorAction[0];
        var shootAction = vectorAction[1];

        //Debug.Log(rotateAction.ToString() + ", " + shootAction.ToString());
        if (rotateAction == 0)
            turret.transform.localEulerAngles += new Vector3(0, 1, 0);
        else if (rotateAction == 1)
            turret.transform.localEulerAngles -= new Vector3(0, 1, 0);


        if (shootAction == 1)
        {
            AddReward(-0.005f);
            ShootBall();
        }

        AddReward(-0.001f);


    }

    public override void OnEpisodeBegin()
    {
        
        //Reset the parameters when the Agent is reset.
        SetResetParameters();
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = -Input.GetAxis("Horizontal");
        actionsOut[1] = 0;

        Vector3 targetDir = objective.transform.position - turret.transform.position;
        float angle = Vector3.Angle(turret.transform.forward, targetDir);

        Debug.Log(angle);

        if (angle < 5.0f)
            Debug.Log("close");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            actionsOut[1] = 1;
            //ShootBall();
        }
    }

    public void SetObjective()
    {
        objective.transform.localPosition = new Vector3(Random.Range(posMin.x, posMax.x), 3, Random.Range(posMin.y, posMax.y));
    }

    public void ObjectiveShot()
    {
        Debug.Log(GetCumulativeReward());
        AddReward(1f);
        EndEpisode();
        // Reset episode
    }


    public void ShootBall()
    {
        GameObject newBullet = bulletPool.GetBullet();

        if (newBullet == null)
            return;

        newBullet.transform.position = turretEndPoint.transform.position;
        newBullet.transform.eulerAngles = turretEndPoint.transform.eulerAngles;
        newBullet.GetComponent<Bullet>().turretAgent = this;
    }

    public void SetResetParameters()
    {
        SetObjective();
    }

}
