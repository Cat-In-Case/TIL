using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WindTemp : MonoBehaviour
{
    Vector4 minmaxXZ = new Vector4(-5f, -5f, 5f, 5f);
    float MaxY = 3f;

    //메인
    Vector3 mainWind;
    float mainStrength;

    //바람의 방향과 각도
    Vector2 normalized_XZ;

    float yTheta;
    float xzTheta;
    float cardinalSign = 0000f;
    string cardianl;

    //생성된 서브 바람
    Vector3 subWind;
    float subStrength;

    //현재 바람 상태
    Vector3 nowWind;
    float nowStrength;

    //계산된 Material Property


    //Cashing
    Vector3 yZeroWind;


    /*
     * 메인 바람의 변화는 매우 김
     * 서브 바람은 6번 사용?
     * 바람을 스택?
     */

    /*
     * 메인 바람의 생성
     * -> 
     */

    IEnumerator Wind()
    {
        //메인 바람 생성
        mainWind.x = Random.Range(minmaxXZ.x, minmaxXZ.z);
        mainWind.z = Random.Range(minmaxXZ.y, minmaxXZ.w);

        mainWind.y = Random.Range(0.1f, MaxY);

        mainStrength = new Vector2(mainWind.x, mainWind.z).magnitude;

        Vector2 mainNormal = new Vector2(mainWind.x, mainWind.z);

        //xzTheta : 메인 바람이 부는 방향을 알아야됨
        xzTheta = Quaternion.FromToRotation(Vector2.right, mainNormal).eulerAngles.z;

        //시그널 계산
        if (xzTheta > 22.5f && xzTheta <= 67.5f)
        {
            //북동
        }
        else if (xzTheta > 360f - 22.5f || xzTheta <= 22.5f)
        {
            //동쪽
        }
        else if (xzTheta > 67.5f && xzTheta <= 112.5f)
        {
            //북쪽
        }
        else if (xzTheta > 112.5f && xzTheta <= 157.5f)
        {


        }

        //바람이 새로 불때마다 서브 바람 생성됨

        //서브 바람 생성
        subWind.x = Random.Range(0, mainWind.x);
        subWind.z = Random.Range(0, mainWind.z);

        float subWindLife = Random.Range(3, 10);


        //yTheta : windZone 각도
        yZeroWind = subWind;
        yZeroWind.y = 0;
        if (subWind.x == 0 && subWind.z == 0)
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
            yTheta = Mathf.Acos(Vector3.Dot(subWind, yZeroWind) / (Vector3.Magnitude(subWind) * Vector3.Magnitude(yZeroWind))) * Mathf.Rad2Deg;
        }

        //바람이 부는 시간 
        float windingTime = Random.Range(2f, 5f);

        //현재 부는 바람
        float restTime = Random.Range(Random.Range(-1, 0), Random.Range(5, 10));    //바람이 멈추는 시간
        float adjustTime = 0f;
        if(restTime < 0)
        {
            adjustTime = restTime;
        }

        //바람 변화
        nowWind = Vector3.Slerp(nowWind, subWind, windingTime * 3f * Time.deltaTime);

        //바람의 세기
        float t = 0f;
        bool isEnd = false;

        if(isEnd == false)
        {         
            t += Time.deltaTime * windingTime * 18;
            nowStrength = subStrength * Mathf.Sin(t * Mathf.Deg2Rad);
            if (t >= 180f - ((windingTime - adjustTime) * 18f))
            {
                nowStrength = 0f;
                t = 0f;
                isEnd = true;
            }
        }
        else
        {
            //바람 끝남
            t += Time.deltaTime;

            if(t >= restTime)
            {
                //바람 휴식 종료
                //다시 바람 만들어야됨
            }
        }

        //현재 바람에 대한 normalized
        normalized_XZ = new Vector2(subWind.x, subWind.z).normalized;






        yield return null;
    }

}
