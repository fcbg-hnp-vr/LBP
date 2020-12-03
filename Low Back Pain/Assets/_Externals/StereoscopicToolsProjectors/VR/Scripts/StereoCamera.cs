using UnityEngine;

public class StereoCamera : MonoBehaviour
{
    public GameObject LeftCamera;
    public GameObject RightCamera;
    public float IPD = 0.065f;

    void Awake()
    {
        UpdateIPD();
    }

    public void SetIPD(float ipd)
    {
        IPD = ipd;
        UpdateIPD();    
    }

    public void UpdateIPD()
    {
        LeftCamera.transform.localPosition = new Vector3(IPD * -0.5f, 0.0f, 0.0f);
        RightCamera.transform.localPosition = new Vector3(IPD * 0.5f, 0.0f, 0.0f);
    }

	public void set_stereo(bool b_active)
	{
		if (b_active)
		{
			LeftCamera.transform.localPosition = new Vector3(IPD * -0.5f, 0.0f, 0.0f);
			RightCamera.transform.localPosition = new Vector3(IPD * 0.5f, 0.0f, 0.0f);
		}
		else
		{
			LeftCamera.transform.localPosition = new Vector3(0, 0.0f, 0.0f);
			RightCamera.transform.localPosition = new Vector3(0, 0.0f, 0.0f);
		}
	}

	void Update()
	{
		/*if (Input.GetKey(KeyCode.M))
		{
			IPD += 0.001f;
			UpdateIPD();
		}
		if (Input.GetKey(KeyCode.N))
		{
			IPD -= 0.001f;
			UpdateIPD();
		}
		if (Input.GetKey(KeyCode.B))
		{
			IPD = 0;
			UpdateIPD();
		}*/
	}
}
