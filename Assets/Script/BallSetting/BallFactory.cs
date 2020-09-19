using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BallFactory:MonoBehaviour
{
    //儲存Ball prefab的列表
    [SerializeField]List<GameObject> ballList;


    public GameObject CreateBall(BallType ballType, Vector3 createPos)
    {
        GameObject target = null;

        switch (ballType)
        {
            case BallType.Default:
                target = ballList.Find(ball => ball.GetComponents<DefaultBall>() != null);
                break;
                //case BallType.newType:
                //target = ballList.Find(ball => ball.GetComponents<newTypeBall>() != null);
                //break;
        }

        if (target == null)
        {
            return target;
        }
        else
        {
            target.GetComponent<Rigidbody>().isKinematic = true;
            target.GetComponent<Collider>().isTrigger = true;
            //該地點生成BAll
            return Instantiate(target, createPos, Quaternion.identity);
        }
    }

    public GameObject CreateBall(BallType ballType,BallController player)
    {
        if (player.belongBall)
        {
            return null;
        }
        //玩家手上生成BAll
        return CreateBall(ballType,player.throwPos.position);
    }

    //能設定光淡的時間
    public GameObject CreateBall(BallType ballType, BallController player,float fadeTime)
    {
        GameObject target = CreateBall(ballType, player);
        target.GetComponent<BasicBall>().fadeSec_Static += fadeTime;
        return target;
    }

}
