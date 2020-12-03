using UnityEngine;
using System.Collections.Generic;
using QualisysRealTime.Unity;
using QTMRealTimeSDK.Data;
using QTMRealTimeSDK;
using QTMRealTimeSDK.Network;

public class CreateScrollList : MonoBehaviour {

    public GameObject serverButtonPrefab; 
    public Transform contentPanel;
    private List<DiscoveryResponse> discoveryResponses;

    void Update()
    {
        if (RTClient.GetInstance().GetStreamingStatus())
        {
            //Disable();
        }

        if (contentPanel.childCount < 1)
        {
            UpdateList();
        }
    }
    public void UpdateList()
    {
        for (int i = 0; i < contentPanel.childCount; i++)
        {
            Destroy(contentPanel.GetChild(i).gameObject);
        }
        discoveryResponses = RTClient.GetInstance().GetServers();
        foreach (DiscoveryResponse server in discoveryResponses)
        {
            GameObject newServer = Instantiate(serverButtonPrefab) as GameObject;
            ServerButton serverButton = newServer.GetComponent<ServerButton>();
            serverButton.HostText.text = server.HostName;
            serverButton.IpAddressText.text = server.IpAddress + ":" + server.Port;
            serverButton.InfoText.text = server.InfoText;
            serverButton.response = server;
            newServer.transform.SetParent(contentPanel);

           

            // Send discovery packet
            List<DiscoveryResponse> list = new List<DiscoveryResponse>();

                if (RTClient.GetInstance().mProtocol.DiscoveryResponses.Count > 0)
                {
                    //Get list of all servers from protocol
                    foreach (var discoveryResponse in RTClient.GetInstance().mProtocol.DiscoveryResponses)
                    {
                        if (discoveryResponse.isConnected)
                        {
                            serverButton.InfoText.color = Color.green;
                            serverButton.InfoText.text = "Connected";
                        }
                        else
                        {
                            serverButton.InfoText.color = Color.white;
                            serverButton.InfoText.text = " ";
                        }
                    }
                }
            

        }
    }
    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
