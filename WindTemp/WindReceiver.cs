using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindReceiver : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    WindMaker zone;
    [SerializeField]
    bool isWind = false;


    //float sqrt = 면적 = A
    // 속도와 압력의 관계(베르누이식) =>  P = (1/2)* p *V^2 * A
    // p =  공기의 밀도 (1.2kg/m^3라 가정) ,V = 풍속
    // F = P * A = (1/2) * p * V^2 * A
    // 풍력계수 C(1)를 도입하면
    //F = C * (1/2) * p * V^2 * A


    private void Awake()
    {
        if(rb == null)
        {
            rb =this.gameObject.GetComponent<Rigidbody>();
        }
    }

    private void FixedUpdate()
    {
        if(isWind == true)
        {
            rb.AddForce(zone.normalVec * zone.windStrength);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Windable")
        {
            zone = other.gameObject.GetComponent<WindMaker>();
            isWind = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Windable")
        {
            isWind = false;
        }
    }
}
