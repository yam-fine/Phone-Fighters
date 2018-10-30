using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    InputManager player;
    NetworkManager nm;
    [Tooltip ("cam1_obj is for the right side, cam2_obj for the left")]
    [SerializeField]
    GameObject cam1_obj, cam2_obj;
    CinemachineVirtualCamera cam1, cam2;
    static CameraScript instance;

    private void Start()
    {
        nm = NetworkManager.Instance;
        player = nm.MyPlayer.GetComponent<InputManager>();
        cam1 = cam1_obj.GetComponent<CinemachineVirtualCamera>();
        cam2 = cam2_obj.GetComponent<CinemachineVirtualCamera>();
        cam1.Follow = player.transform;
        cam2.Follow = player.transform;
    }

    private void Update()
    {
        if (player.IsFacingRight)
        {
            if (!cam1_obj.activeInHierarchy)
                cam1_obj.SetActive(true);
        }
        else
        {
            if (cam1_obj.activeInHierarchy)
            cam1_obj.SetActive(false);
        }
    }

    public static CameraScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
            }
            return instance;
        }
    }

    /// <summary>
    /// amplitude = range in which the camera will shake in the X axis |
    /// frequency = speed of shake
    /// </summary>
    /// <param name="amplitude"></param>
    /// <param name="frequency"></param>
    public void CameraShake(float amplitude, float frequency, float time)
    {
        StartCoroutine(Shake(amplitude, frequency, time));
    }

    IEnumerator Shake(float amplitude, float frequency, float time)
    {
        Noise(amplitude, frequency);

        yield return new WaitForSeconds(time);

        Noise(0, 0);
    }

    void Noise(float amplitude, float frequency)
    {
        cam1.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        cam1.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;

        cam2.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        cam2.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;
    }
}
