using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//默認Ball,直接繼承BasicBall
public class DefaultBall : BasicBall
{
    //最高點距離(即鏡頭前方多遠為目標點)
    [SerializeField]
    private float DisHighpoint = 10.0f;
    //手持波的位置與鏡頭方向的計算
    private Transform throwDirection;

    public void SetHighpointDistance(float dis)
    {
        DisHighpoint = dis;
    }

    public override void Throwing(Transform throwAngle)
    {
        Throwing(throwAngle,1.0f, 1.0f);
    }

    public override void Throwing(Transform throwAngle, float runSpeed, float holdTime)
    {
        isThrow = true;
        GetComponent<SphereCollider>().isTrigger = false;
        this.transform.parent = null;
        ShowLight();
        rb.isKinematic = false;
        rb.useGravity = true;
        throwDirection = GameObject.FindGameObjectWithTag("MainCamera").transform;
        //Debug.Log(throwDirection.position);
        //Debug.DrawLine(throwDirection.position, throwDirection.position + throwDirection.forward,Color.red,10f);
        if (throwDirection == null)
        {
            GetComponent<Rigidbody>().AddForce(throwAngle.forward * throwRange * (float)Math.Pow(holdTime, 2) * 0.5f, ForceMode.Impulse);
        }
        else
        {
            float runForce = runSpeed > 1 ? runSpeed * 0.5f * (1-(holdTime/4.25f)) : 1;
            Vector3 throwPosDir = ((throwDirection.position + (throwDirection.forward * DisHighpoint)) - throwAngle.position).normalized;
            //方向 * 力度 * 按鍵時間^2 * 1/2 * 移動力
            GetComponent<Rigidbody>().AddForce(throwPosDir * throwRange * (float)Math.Pow(holdTime, 2) * 0.5f * runForce, ForceMode.Impulse);
            //按鍵時間^2 * 1/2 * 移動力 的影響
            Debug.Log("holdTime: "+ holdTime+" | "+ (float)Math.Pow(holdTime, 2) * 0.5f * runForce) ;
        }
    }
}
