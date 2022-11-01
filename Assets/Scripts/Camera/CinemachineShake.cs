using Cinemachine;
using UnityEngine;


public class CinemachineShake : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    [SerializeField]
    private float intensity, time;

    private float startingIntensity,
                  shakeTimer,
                  shakeTimerTotal;
    private void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin camPerlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        camPerlin.m_AmplitudeGain = intensity;

        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if(shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin camPerlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                camPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 2);
            }
        }
    }


}
