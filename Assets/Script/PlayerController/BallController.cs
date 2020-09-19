using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]BallFactory ballFactory;
    [SerializeField]BasicBall ball;
    //當前玩家Ball的類型
    private BallType ballType = BallType.Default;
    //可丟球的數量
    private int ballStore = 5;
    public Transform throwPos;
    public GameObject holdBall;
    public bool belongBall = false;
    //額外淡化時間
    public float fadeTime = 0.0f;

    public void SetBall(BallType ballType)
    {
        if (ballStore > 0)
        {
            holdBall = ballFactory.CreateBall(ballType, this, fadeTime);
            holdBall.transform.SetParent(this.transform);
            ball = holdBall.GetComponent<BasicBall>();
        }
    }

    //改變Ball的類型
    public void ChangeBallType(BallType ballType)
    {
        this.ballType = ballType;
        if (holdBall != null)
        {
            Destroy(holdBall);
            SetBall(ballType);
        }
    }

    public void IncreaseBallStore(int count)
    {
        ballStore = count;
        if (holdBall == null)
        {
            //創造球
            SetBall(ballType);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetBall(ballType);
    }

    // Update is called once per frame
    void Update()
    {
        if (holdBall != null)
        {
            ThrowBall();
        }
    }

    void ThrowBall()
    {
        if (Input.GetKeyDown("space") && !ball.isThrow && ballStore > 0)
        {
            ballStore--;
            ball.Throwing(throwPos);
            SetBall(ballType);
        }
    }
}
