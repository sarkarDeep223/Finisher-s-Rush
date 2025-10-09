using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    [SerializeField] private float impulseForce = 1f;




    public static CameraShake instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public void cameraEffect(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(impulseForce);
    } 



}
