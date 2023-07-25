using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void _windApply(Vector3 wind, float degree, float strengthXZ);
public class WindMaker : MonoBehaviour
{
    //바람의 각도를 구해야됨
    //N, W, S, E, NW, NE, SW, SE
    // 물체가 움직이는 과정 aircraft.AddForce((direction * speed) + wind)

    //백터 3개  wind, wind.y=0인 wind, 영점

    [SerializeField]
    public Vector3 main_Wind;

    [SerializeField]
    public Vector3 now_Wind;
    [SerializeField]
    public Vector3 new_Wind;
    [SerializeField]
    public Vector2 normalVec;
    [SerializeField]
    public float xzTheta;
    [SerializeField]
    public float yTheta;
    [SerializeField]
    public float windStrength;


    //파티클을 위한 WindZone
    public WindZone windZone;
    public float LerpTime = 1f;
    
    [Header("방위와 각도")]
    public int cardinal_Sign = 0000;
    public string cardinal = "";


    [Header("Material Property")]
    public Vector2 xzTimeMultiplier = new Vector2();


    //액션 영역

    private List<_windApply> windApplys;

    #region WindApply쪽
    public void WindReturn(ref _windApply windApply)
    {
        //주소 연결

    }
    public void WindApply()
    {
        for(int i = 0; i < windApplys.Count; i++)
        {
           // windApplys[i](wind, degree, );
        }
    }
    #endregion


    private void Awake()
    {
        now_Wind = new Vector3(0, 0, 0);
    }
    //업데이트
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CheckCardinal();
        }
    }

    IEnumerator WindRoutine()
    {
        //메인 바람 랜덤 생성

        //현재 방위에 각 양 옆의 방위까지만

        //메인 바람의 강도

        //메인 바람의 수명도 결정 (분단위)

        //새로운 메인 바람의 방위 계산



        //방위를 기반으로 하위 바람 계산( 현재방위 범위만)

        //하위 바람의 수명 결정 (초단위)

        //하위 바람의 강도의 최대는 메인 바람의 강도
        
        //바람의 강도의 증가 = AnimationCurve로
        //바람의 강도 하락또한 AnimationCurve


        //바람 러프

        //주의 : 바람이 항상 강하게 부는 것이 아님

        /*바람이 죽는 시간 만드는 방법 Clamp(x, 0, 바람의 최대세기)이용
         * 생성된 바람의 세기가 0~12로 두고
         * -2를하면 0~2까지는 바람이 죽음
         * 바람이 강해지는 과정은 SLerp
         * 
         * 0~12로 만들고
         * 2~4 부분이면 /2f하면 특정 구간만 더 자주 등장
         * 
         * 바람이 불때 최대점 유지시간 (0.2f~0.6f) 랜덤생성
         * 최대 유지시간이 끝나면 
         * 현재 바람의 최대의 1/2까지 위력 하락
         * 하락 후 새로운 하위바람 생성
         */

        yield return null;
    }

    public void CalculateMaterialProperty()
    {
        xzTimeMultiplier.x = now_Wind.x;
        xzTimeMultiplier.y = now_Wind.z;

        Vector2 windFromXZ = normalVec * windStrength;
        
        windStrength = Vector2.Dot(Vector2.zero, windFromXZ);

        float WindStrengthFromx = 0.01f * normalVec.x * windStrength * 0.3f;
        float WindStrengthFromZ = 0.01f * normalVec.y * windStrength * 0.3f;

        float windDensity = 0f;
 
    }


    public void RandomMainWind()
    {

        Vector4 max_min = new Vector4(5.0f, 5.0f, -5.0f, -5.0f);

        float minX, minZ, maxX, maxZ;



        //반지름이 5인 원에 위치하는 좌표를 구해야됨
        if(cardinal_Sign == 10)
        {
            //N
            minX = -5f;
            maxX = 5f;
            minZ = 0f;

        }
        if (cardinal_Sign == 13)
        {
            //NW
        }
        if (cardinal_Sign == 14)
        {
            //NE
        }

        //랜덤 바람 생성 메인
        new_Wind.x = UnityEngine.Random.Range(-5.0f, 5.0f);
        new_Wind.y = UnityEngine.Random.Range(0f, 3.0f);
        new_Wind.z = UnityEngine.Random.Range(-5.0f, 5.0f);

        windStrength = UnityEngine.Random.Range(0f, 10f);

        /*
         * 바람의 주기 
         * RandomWind로 방향 및 유지 시간 생성
         * 방향 및 유지 시간 내에 소형 바람 생성
         * 방향은 매우 작은 차이로만
         * 시간은 Sin또는 Cos로 영향을 주어 바람이 강해졌다가 약해짐
         * 문제1. 바람이 불다가 도중에 강해짐
         * 문제2. 최고속도가 유지안됨
         */

    }
    IEnumerator SubWindLerp()
    {
        float lerptime = 1f;
        float t = 0;
        while(t < 1)
        {
            yield return new WaitForEndOfFrame();
            now_Wind = Vector3.Slerp(now_Wind, new_Wind, lerptime * Time.deltaTime);

            t += Mathf.Lerp(t, 1f, lerptime * Time.deltaTime);
        }
    }

    public void CheckCardinal()
    {
        #region 계산
        //Acos     0 ~ 180
        //Atan     -180 ~ 180
        //Quaternion.FromToRotation    0 ~ 360
        #endregion
        //법선벡터 = 실시간
        normalVec = new Vector2(now_Wind.x, now_Wind.z).normalized;

        //노말벡터의 xz값으로 구하면 안됨
        // 45도일때 0.701,0.701 => NE로 계산하면 부정확함

        // 1,0 기준으로 xz의 Line과의 각도


        //이상하게 한바퀴 도는거 방지
        float temp_xz = xzTheta;

        xzTheta = Quaternion.FromToRotation(Vector2.right, normalVec).eulerAngles.z;

        float adjust = 0f;

        if(temp_xz > 180 && (xzTheta < 90 && xzTheta > 0))
        {
            adjust = xzTheta - 360f;
        }
        

        windStrength = Vector2.Distance(Vector2.zero, new Vector2(now_Wind.x, now_Wind.z));




        if(xzTheta > 360f-22.5f && xzTheta <= 22.5)
        {

        }

    }


    public void CalculateTheta()
    {
        //지상과 바람의 각도 계산 
        //0~90만 출력됨 => 1,2,3,4 평면 기준이라
        //Vector3.zero가 기준점 임으로 뺄셈 x

        #region 계산식
        //Vector3 referencePoint = Vector3.zero;
        //Vector3 v1 = wind - referencePoint;
        //Vector3 v2 = new Vector3(wind.x, 0, wind.z) - referencePoint;
        //float dot = Vector3.Dot(v1, v2);
        //float magnitude = Vector3.Magnitude(v1) * Vector3.Magnitude(v2);
        //float radian = Mathf.Acos(dot / magnitude);
        #endregion

        Vector3 temp = now_Wind;
        temp.y = 0;

        if(now_Wind.x == 0 && now_Wind.z == 0)
        {
            yTheta = 0f;
        }
        else
        {
            #region 계산식
            //Dot 내적 연산 => 스칼라곱
            //AㆍB = ||A||*||B||cosθ
            // 
            // ||v1|| => v1(a1, a2, a3)  Magnitude(백터의 크기)
            // sqrt(a1^2 + a2^2 + a3^3)
            // 따라서
            //cosθ = AㆍB / (||A||*||B||)
            // θ = Arccos(AㆍB / (||A||*||B||))
            // θ(라디안) => Degree = Arccos(AㆍB / (||A||*||B||)) * (180/π)
            #endregion
            yTheta = Mathf.Acos(Vector3.Dot(now_Wind, temp) / Vector3.Magnitude(now_Wind) / Vector3.Magnitude(temp)) * Mathf.Rad2Deg;
        }

        //Windzone 회전
        StartCoroutine(WindZoneMove());
    }

    IEnumerator WindZoneMove()
    {
        float LerpTime = 0.5f;
        yield return new WaitForFixedUpdate();

        windZone.transform.rotation = Quaternion.Slerp(transform.rotation, 
            Quaternion.Euler(new Vector3(yTheta, xzTheta, 0f)), LerpTime * Time.fixedDeltaTime);
    }


    public void CalculateVectorLength()
    {
        //바람 벡터의 길이(강도) 계산

        //Material에서 쓰는 uv_offset 값
        //uv에서 1f는 너무 큰 값 => 보정필요
        Vector2 resizedWind = new Vector2(now_Wind.x * 0.001f, now_Wind.z * 0.001f);

        //바람의 강도
        float xzLength = new Vector2(now_Wind.x, now_Wind.z).magnitude;

        //위에서 아래로의 강도를 위한 길이
        float xyzLength = new Vector3(now_Wind.x, now_Wind.y, now_Wind.z).magnitude;

        float yPower = xyzLength / xzLength;//???보정
    }
}
