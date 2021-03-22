using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ball種類,新增BallScript後需增加類型
public enum BallType
{
    Default
}

[RequireComponent(typeof(Rigidbody))]
public abstract class BasicBall : MonoBehaviour
{
    protected BallFactory manager;

    protected Light lightSource;
    protected Material ballMat;
    protected Rigidbody rb;
    [SerializeField]
    protected Color lightColor;

    //Ball亮度等級
    public int lightLevel = 1;
    //是否在拋出狀態
    public bool isThrow = false;
    //是否在地上
    protected bool onGrand = false;

    //拋出時的力度
    public float throwRange = 10f;

    //淡化設定
    protected Color fadeColor;
    //淡化時間
    public float fadeSec_Static = 3.0f;
    protected float fadeSec_Dynamic;

    public void SetManager(BallFactory factory)
    {
        if (manager != null)
            return;
        manager = factory;
    }

    protected void ShowLight() {
        lightSource.enabled = true;
        SetLightColor(lightColor);
    }

    protected void HideLight()
    {
        lightSource.enabled = false;
        SetLightColor(Color.black);
    }

    protected void SetLightColor(Color color)
    {
        ballMat.SetColor("_EmissionColor", color);
        lightSource.color = color;
    }

    public abstract void Throwing(Transform throwAngle);

    public abstract void Throwing(Transform throwAngle,float runSpeed, float holdTime);

    //淡化亮度
    public void FadeLight(float fadeTime)
    {
        float lightIntensity = lightSource.intensity;
        if (lightIntensity > 0)
        {
            fadeSec_Dynamic -= Time.deltaTime;
            float lightReduction = Mathf.Abs(lightIntensity * (Time.deltaTime / fadeSec_Dynamic));
            lightSource.intensity -= lightReduction;
            float t = 1 - (fadeSec_Dynamic / fadeSec_Static);
            fadeColor = Color.Lerp(lightColor, Color.black, t);
            SetLightColor(fadeColor);
        }else
        {
            FadeEnd();
        }
    }

    protected void FadeEnd() {
        //淡化最終時刪除自身
        //SelfDelete();
    }

    protected void SelfDelete()
    {
        //最暗時刪除物件
        if (manager == null)
        {
            GameObject.FindObjectOfType<BallFactory>().DeleteBall(this.gameObject);
        }
        else
        {
            manager.DeleteBall(this.gameObject);
        }
    }

    protected void Init()
    {
        rb = transform.GetComponent<Rigidbody>();
        lightSource = transform.GetComponentInChildren<Light>();
        ballMat = transform.GetComponent<Renderer>().material;
    }

    protected void StartInit()
    {
        rb.useGravity = false;
        lightSource.range = lightLevel * 5;
        lightSource.intensity = lightLevel * 2.5f;
        fadeSec_Dynamic = fadeSec_Static;
        HideLight();
    }

    protected void Updating()
    {
        if (onGrand)
        {
            FadeLight(fadeSec_Dynamic);
        }
    }

    protected void Awake()
    {
        Init();
    }


    // Start is called before the first frame update
    protected void Start()
    {
        StartInit();
    }

    // Update is called once per frame
    protected void Update()
    {
        Updating();
    }

    protected void OnCollisionEnter(Collision collision){
        if (collision.collider.CompareTag("Grand") && !onGrand)
        {
            onGrand = true;
        }

        //執波並刪除自身
        if(collision.collider.CompareTag("Player") && onGrand)
        {
            collision.gameObject.GetComponent<BallController>().IncreaseBallStore(1);
            SelfDelete();
        }
    }

    protected void OnTriggerStay(Collider collider)
    {
        //搜尋道具,範圍依Trigger大小(暫設 5 )
        if (onGrand && collider.CompareTag("Item"))
        {
            //發光
            //collider.GetComponent <?> ().functionName();

            Debug.Log(collider.name);
        }

    }
}
