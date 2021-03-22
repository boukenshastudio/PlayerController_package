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
    [SerializeField]
    [Min(0)]private int ballStore = 10;
    //手持波的位置
    public Transform throwPos;
    //所持波的種類
    public GameObject holdBall;
    //是否持有波
    public bool belongBall = false;
    //冷卻時間設定(sec)
    public float coldTimeSet = 5.0f;
    //冷卻時間計時
    private float coldTime = 0.0f;
    //額外淡化時間
    public float fadeTime = 0.0f;
    //每次可丟上限
    [Min(1)]public int existBall;

    //按鍵時長記錄
    private float holdKeyTime = 0.0f;
    //預備
    private bool preThrow = false;

    //可回復波數量
    public bool refillMode = false;
    //X秒後回復波的數量
    [SerializeField]
    private float refillTime = 5.0f;
    private float recordTime = 0.0f;
    private int curMaxBall = 0;

    private PlayerController pController;

    private void recordTiming()
    {
        if (recordTime < refillTime)
        {
            recordTime += Time.deltaTime;
        }
    }

    private void refillBall()
    {

        if(ballStore < curMaxBall)
        {
            recordTiming();
            if (recordTime > refillTime)
            {
                recordTime = 0;
                ballStore++;
            }
        }
    }

    public void SetBall(BallType ballType)
    {
        if (ballStore > 0)
        {
            pController = GetComponent<PlayerController>();
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
        ballStore += count;
        if(ballStore > curMaxBall)
        {
            ballStore = curMaxBall;
        }
        if (holdBall == null)
        {
            //創造球
            SetBall(ballType);
        }
    }

    public void SetupBallStore(int count)
    {
        ballStore = count;
        curMaxBall = ballStore;
        if (holdBall == null)
        {
            //創造球
            SetBall(ballType);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        curMaxBall = ballStore;
        SetBall(ballType);
    }

    // Update is called once per frame
    void Update()
    {
        if (holdBall != null)
        {
            if(refillMode)
                refillBall();
            ThrowBall();
        }
        coldDown();
    }

    void coldDown()
    {
        if (coldTime > 0.0f)
        {
            coldTime -= Time.deltaTime;
            //Debug.Log(coldTime);
        }
    }

    void ThrowBall()
    {
        //ballFactory.ScenceBallCounter()向工廠查詢場上波的總數
        //if (ballStore > 0 && ballFactory.ScenceBallCounter() < (1 + existBall))
        if (ballStore > 0 && coldTime <= 0.0f) {
            if (Input.GetKeyDown("space") && !preThrow && !ball.isThrow)
            {
                preThrow = true;
            } else if (preThrow && holdKeyTime < 1.5f && Input.GetKey("space"))
            {
                //最張按鍵時間為1.5
                holdKeyTime += Time.deltaTime * 1.2f;
            }
            if (preThrow && Input.GetKeyUp("space"))
            {
                //人物是否在向前移動
                float runSpeed = pController.GetMoveVelocity() > 0.0f ? pController.speed : 1;
                //按鍵未夠0.5皆以0.5計算
                holdKeyTime = holdKeyTime < 0.5f ? 0.5f : holdKeyTime;
                ballStore--;
                ball.Throwing(throwPos, runSpeed, 0.5f + holdKeyTime);
                SetBall(ballType);
                holdKeyTime = 0.0f;
                coldTime = coldTimeSet;
            }
        }
    }
}
