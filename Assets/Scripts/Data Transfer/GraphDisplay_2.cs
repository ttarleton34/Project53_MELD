using UnityEngine;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;
using System.Linq;

public class GraphDisplay_2 : MonoBehaviour
{
    public GameObject graphContainer;
    public GameObject dataPointPrefab;
    public GameObject lineSegmentPrefab;
    public CSVReader csvReader;
    //public GameObject graphArea;

    private RectTransform graphRectTransform;
    private float graphWidth;
    private float graphHeight;
    private int maxEntries = 50;

    private List<GameObject> graphObjects;

    /*
    void Start()
    {
        graphRectTransform = graphContainer.GetComponent<RectTransform>();
        graphWidth = graphRectTransform.sizeDelta.x;
        graphHeight = graphRectTransform.sizeDelta.y;

        graphObjects = new List<GameObject>();

        UpdateGraph();
    }
    */

    void Start()
    {
        graphRectTransform = graphContainer.GetComponent<RectTransform>();
        //graphArea.GetComponent<RectTransform>().pivot = new Vector2(0, 1);

        graphObjects = new List<GameObject>();

        StartCoroutine(UpdateGraphCoroutine());
    }

    IEnumerator UpdateGraphCoroutine()
    {
        while (true)
        {
            if(csvReader.GetNumberOfEntries() > 1)
            {
                UpdateGraph();
            }
            yield return new WaitForSeconds(2f);
        }
    }

    void UpdateGraph()
    {
            graphWidth = graphRectTransform.sizeDelta.x;
            graphHeight = graphRectTransform.sizeDelta.y;

            csvReader.ReadCSV();

            ClearGraph();

            System.TimeSpan minTime = csvReader.dataEntries.Min(entry => entry.Time);
            System.TimeSpan maxTime = csvReader.dataEntries.Max(entry => entry.Time);
            float minActualTemp = csvReader.dataEntries.Min(entry => entry.ActualTemp);
            float maxActualTemp = csvReader.dataEntries.Max(entry => entry.ActualTemp);

            Vector2 prevPosition = Vector2.zero;

            int pointIndex = 0;
            int startIndex = Mathf.Max(0, csvReader.dataEntries.Count - maxEntries);

            List<CSVReader.DataEntry> lastDataEntries = csvReader.dataEntries.OrderBy(entry => entry.Time).Skip(startIndex).ToList();

            foreach (CSVReader.DataEntry dataEntry in lastDataEntries)
            {
                float xPosition = (float)((dataEntry.Time - minTime).TotalSeconds / (maxTime - minTime).TotalSeconds) * graphWidth;
                float yPosition = ((dataEntry.ActualTemp - minActualTemp) / (maxActualTemp - minActualTemp)) * graphHeight;

                //Debug.Log($"Data Entry Time: {dataEntry.Time}, Value: {dataEntry.ActualTemp}");
                //Debug.Log($"Min Time: {minTime}, Max Time: {maxTime}, Max ActualTemp: {maxActualTemp}");
                //Debug.Log($"xPosition: {xPosition}, yPosition: {yPosition}");
                //Debug.Log("Height: " + graphHeight + "Width: " + graphWidth);
                
                GameObject dataPoint = Instantiate(dataPointPrefab, graphContainer.transform);
                graphObjects.Add(dataPoint);
                RectTransform dataPointRectTransform = dataPoint.GetComponent<RectTransform>();
                dataPointRectTransform.anchoredPosition = new Vector2(xPosition, yPosition);

                //Debugging
                //Debug.Log("Data point " + pointIndex + ": xPosition = " + xPosition + ", yPosition = " + yPosition);

                if (pointIndex > 0)
                {
                    GameObject lineSegment = Instantiate(lineSegmentPrefab, graphContainer.transform);
                    graphObjects.Add(lineSegment);
                    RectTransform lineSegmentRectTransform = lineSegment.GetComponent<RectTransform>();

                    Vector2 dir = (new Vector2(xPosition, yPosition) - prevPosition).normalized;
                    float distance = Vector2.Distance(prevPosition, new Vector2(xPosition, yPosition));
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                    lineSegmentRectTransform.sizeDelta = new Vector2(distance, lineSegmentRectTransform.sizeDelta.y);
                    lineSegmentRectTransform.anchoredPosition = new Vector2(prevPosition.x, prevPosition.y);
                    lineSegmentRectTransform.localRotation = Quaternion.Euler(0, 0, angle);
                }

                //Debugging
                //Debug.Log("Data point RectTransform anchored position: " + dataPointRectTransform.anchoredPosition);

                prevPosition = new Vector2(xPosition, yPosition);
                pointIndex++;

                CSVValueDisplay[] valueDisplays = FindObjectsOfType<CSVValueDisplay>();
                foreach (CSVValueDisplay display in valueDisplays)
                {
                    display.UpdateTextField();
                }
            }
    }

    void ClearGraph()
    {
        foreach (GameObject graphObject in graphObjects)
        {
            Destroy(graphObject);
        }
        graphObjects.Clear();
    }
}