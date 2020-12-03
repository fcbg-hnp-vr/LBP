using UnityEngine;
using UnityEngine.UI;
using QTMRealTimeSDK.Data;
using QualisysRealTime.Unity;
using QTMRealTimeSDK;
using TMPro;

[System.Serializable]
public class ServerButton : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI HostText;
    public TextMeshProUGUI IpAddressText;
    public TextMeshProUGUI InfoText;
    public DiscoveryResponse response;
    void Start()
    {
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(new UnityEngine.Events.UnityAction(Connect));
    }
    void Connect()
    {
        if (response.isConnected)
        {
           //RTClient.GetInstance().mProtocol.ReleaseControl();
            RTClient.GetInstance().Dispose();
            response.isConnected = false;
            InfoText.color = Color.red;
            InfoText.text = "Disconnected";
        }
        else
        {
            if (!RTClient.GetInstance().Connect(response, response.Port, true, true, false, true, false, true))
            {
                InfoText.color = Color.red;
                InfoText.text = "Could not connect to this server";
                response.isConnected = false;
            }
            else
            {
                //SendMessageUpwards("Disable");
                InfoText.color = Color.green;
                InfoText.text = "Connected";
                response.isConnected = true;
                /*bool master = RTClient.GetInstance().mProtocol.TakeControl();
                Debug.Log(master);
                bool capture = RTClient.GetInstance().mProtocol.StartCapture();
                Debug.Log(capture);
                bool trigger = RTClient.GetInstance().mProtocol.SendTrigger();
                Debug.Log(trigger);*/
                //RTClient.GetInstance().mProtocol.SaveFile("test", true, ref test);
            }

           
        }
    }

    
}
