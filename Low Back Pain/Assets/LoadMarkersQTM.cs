using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QTMRealTimeSDK;
using TMPro;
using UnityEngine.UI;

namespace QualisysRealTime.Unity
{
    public class LoadMarkersQTM : MonoBehaviour
    {
        public GameObject prefabToggleMarker;
        public GameObject parentContent;

        protected RTClient rtClient;
        private List<GameObject> markers;
        private List<LabeledMarker> markerData;

        private bool streaming = false;

        // Start is called before the first frame update
        void Start()
        {
            rtClient = RTClient.GetInstance();
            markers = new List<GameObject>();
        }

        // Update is called once per frame
        void Update()
        {
            if (rtClient == null) rtClient = RTClient.GetInstance();

            if (rtClient.GetStreamingStatus() && !streaming)
            {
                InitiateMarkers();
                streaming = true;
            }
            if (!rtClient.GetStreamingStatus() && streaming)
            {
                streaming = false;
                InitiateMarkers();
            }

        }

        private void InitiateMarkers()
        {
            foreach (var marker in markers)
            {
                Destroy(marker);
            }

            markers.Clear();
            markerData = rtClient.Markers;

            for (int i = 0; i < markerData.Count; i++)
            {
                GameObject newMarker = Instantiate(prefabToggleMarker, parentContent.transform);
                newMarker.transform.Find("Label").GetComponent<TextMeshProUGUI>().text = "m_"+markerData[i].Name;
                string name = markerData[i].Name;
                newMarker.name = "Toggle_"+markerData[i].Name;
                newMarker.GetComponent<Toggle>().group = parentContent.GetComponent<ToggleGroup>();
                newMarker.GetComponent<Toggle>().onValueChanged.AddListener(delegate { Settings.instance.SetMarkerID(newMarker.GetComponent<Toggle>(), name); });
                markers.Add(newMarker);
            }
        }
    }
}
