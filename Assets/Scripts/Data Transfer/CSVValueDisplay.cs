using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CSVValueDisplay : MonoBehaviour
{
    public CSVReader csvReader;
    public string fieldName;
    public TMPro.TextMeshProUGUI textComponent;
    public float lowThreshold;
    public float highThreshold;

    void Start()
    {
        if (csvReader == null)
        {
            Debug.LogError("CSVReader not found in the scene.");
        }

        if (textComponent == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on this GameObject.");
        }
        textComponent = GetComponent<TextMeshProUGUI>();
        UpdateTextField();
    }

    public void UpdateTextField()
    {
        if (csvReader.dataEntries.Count > 0)
        {
            CSVReader.DataEntry latestEntry = csvReader.dataEntries[csvReader.dataEntries.Count - 1];
            
            if(fieldName == "Temperature")
            {
                float value = float.Parse(csvReader.GetValueByName("ActualTemp")[0]);
                //textComponent.text = "Temperature\n" + csvReader.GetValueByName("ActualTemp")[0];
                textComponent.text = $"Temperature\n<color={GetColorForValue(value)}>{value}</color>";
                //UpdateTextColor(value);
            }
            else if(fieldName == "Position")
            {
                float posX = float.Parse(csvReader.GetValueByName(fieldName)[0]);
                float posY = float.Parse(csvReader.GetValueByName(fieldName)[1]);
                float posZ = float.Parse(csvReader.GetValueByName(fieldName)[2]);
                //textComponent.text = "Position\n" + "X "+ csvReader.GetValueByName(fieldName)[0] + "\nY " + csvReader.GetValueByName(fieldName)[1] + "\nZ " + csvReader.GetValueByName(fieldName)[2];
                textComponent.text = $"Torque\nX: <color={GetColorForValue(posX)}>{posX}</color>\nY: <color={GetColorForValue(posY)}>{posY}</color>\nZ: <color={GetColorForValue(posZ)}>{posZ}</color>";
            }
            else if(fieldName == "Torque")
            {
                float torqueX = float.Parse(csvReader.GetValueByName(fieldName)[0]);
                float torqueY = float.Parse(csvReader.GetValueByName(fieldName)[1]);
                float torqueZ = float.Parse(csvReader.GetValueByName(fieldName)[2]);
                //textComponent.text = "Torque\n" + "X "+ csvReader.GetValueByName(fieldName)[0] + "\nY " + csvReader.GetValueByName(fieldName)[1] + "\nZ " + csvReader.GetValueByName(fieldName)[2];
                textComponent.text = $"Torque\nX: <color={GetColorForValue(torqueX)}>{torqueX}</color>\nY: <color={GetColorForValue(torqueY)}>{torqueY}</color>\nZ: <color={GetColorForValue(torqueZ)}>{torqueZ}</color>";
            }
            else if(fieldName == "Spindle")
            {
                float value = float.Parse(csvReader.GetValueByName("Temperature")[0]);
                UpdateTextColor(value);
            }
            else if(fieldName == "Actuator")
            {
                float value = float.Parse(csvReader.GetValueByName("ActualTemp")[0]);
                UpdateTextColor(value);
            }
        }
        else
        {
            textComponent.text = "N/A";
        }
    }

    private string GetColorForValue(float value)
    {
        if (value > highThreshold)
        {
            return "red";
        }
        else if (value < lowThreshold)
        {
            return "yellow";
        }
        else
        {
            return "white";
        }
    }

    void UpdateTextColor(float value)
    {
        if (value < lowThreshold)
        {
            textComponent.color = Color.yellow;
        }
        else if (value > highThreshold)
        {
            textComponent.color = Color.red;
        }
        else
        {
            textComponent.color = Color.white;
        }
    }
}
