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
    Light lightSource;
    Material ballMat;
    Rigidbody rb;
    [SerializeField] Color lightColor;

    //Ball亮度等級
    public int lightLevel = 1;
    //是否在拋出狀態
    public bool isThrow = false;
    //是否在地上
    bool onGrand = false;

    //拋出時的力度
    public float throwRange = 10f;

    //淡化設定
    Color fadeColor;
    //淡化時間
    public float fadeSec_Static = 3.0f;
    private float fadeSec_Dynamic;

    void ShowLight() {
        lightSource.enabled = true;
        SetLightColor(lightColor);
    }

    void HideLight()
    {
        lightSource.enabled = false;
        SetLightColor(Color.black);
    }

    void SetLightColor(Color color)
    {
        ballMat.SetColor("_EmissionColor", color);
        lightSource.color = color;
    }

    public void Throwing(Transform throwAngle)
    {
        isThrow = true;
        GetComponent<SphereCollider>().isTrigger = false;
        this.transform.parent = null;
        ShowLight();
        rb.isKinematic = false;
        rb.useGravity = true;
        GetComponent<Rigidbody>().AddForce(throwAngle.forward * throwRange, ForceMode.Impulse);
    }

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
            //最暗時刪除物件
            Destroy(this.gameObject);
        }
    }

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();
        lightSource = transform.GetComponentInChildren<Light>();
        ballMat = transform.GetComponent<Renderer>().material;
    }


    // Start is called before the first frame update
    void Start()
    {
        rb.useGravity = false;
        lightSource.range = lightLevel * 5;
        lightSource.intensity = lightLevel * 2.5f;
        fadeSec_Dynamic = fadeSec_Static;
        HideLight();
    }

    // Update is called once per frame
    void Update()
    {
        if (onGrand)
        {
            FadeLight(fadeSec_Dynamic);
        }
    }

    void OnCollisionEnter(Collision collision){
        if (collision.collider.CompareTag("Grand") && !onGrand)
        {
            onGrand = true;
        }
    }
}
