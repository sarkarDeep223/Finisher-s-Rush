using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Obsticel : MonoBehaviour
{
    private CinemachineImpulseSource impulseManager;

    private void Awake()
    {
        impulseManager = GetComponent<CinemachineImpulseSource>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            GameOverSequence();
        }
    }


    private void GameOverSequence()
    {
        CameraShake.instance.cameraEffect(impulseManager);
        StartCoroutine(PauseAfterShake());
    }



    private IEnumerator PauseAfterShake()
    {
        yield return new WaitForSecondsRealtime(0.1f); // real time, not affected by timeScale
        Time.timeScale = 0f;
    }

}
