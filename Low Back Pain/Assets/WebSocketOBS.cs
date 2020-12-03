using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OBSWebsocketDotNet;

public class WebSocketOBS : MonoBehaviour
{
    OBSWebsocket oBSWebsocketFirstView;
    OBSWebsocket oBSWebsocketThirdView;
    OBSWebsocketDotNet.Types.StreamingService service1;
    OBSWebsocketDotNet.Types.StreamingService service2;

    // Start is called before the first frame update
    void Start()
    {
        service1 = new OBSWebsocketDotNet.Types.StreamingService();
        service1.Settings.Server = "ws://127.0.0.1:4444";

        oBSWebsocketFirstView = new OBSWebsocket();
        oBSWebsocketFirstView.SetStreamSettings(service1, true);
        oBSWebsocketFirstView.Connect("ws://127.0.0.1:4444", "");
        oBSWebsocketFirstView.SetCurrentProfile("FirstView");
        oBSWebsocketFirstView.SetRecordingFolder("‪D:\\FirstView");


        service1 = new OBSWebsocketDotNet.Types.StreamingService();
        service1.Settings.Server = "ws://127.0.0.1:4445";

        oBSWebsocketThirdView = new OBSWebsocket();
        oBSWebsocketThirdView.SetStreamSettings(service2, true);
        oBSWebsocketThirdView.Connect("ws://127.0.0.1:4445", "");
        oBSWebsocketThirdView.SetCurrentProfile("ThirdView");
        oBSWebsocketThirdView.SetRecordingFolder("‪D:\\ThirdView");




    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(oBSWebsocketFirstView.IsConnected);
        Debug.Log(oBSWebsocketThirdView.IsConnected);
    }
}
