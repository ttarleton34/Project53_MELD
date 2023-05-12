using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;

public class CSVReader : MonoBehaviour
{
    public string fileName = "data.csv";
    public List<DataEntry> dataEntries;

    void Awake()
    {
        ClearCSVFile();
        LoadCSVData();
    }

    void LoadCSVData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string fileContents;

        if (File.Exists(filePath))
        {
            fileContents = File.ReadAllText(filePath);
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
            return;
        }

        string[] rows = fileContents.Split('\n');
                
        dataEntries = new List<DataEntry>();

        for (int i = 1; i < rows.Length; i++)
        {
            if (rows[i].Trim() == "") continue;
            string[] row = rows[i].Split(',');
            DataEntry entry = new DataEntry
            {
                Time = TimeSpan.Parse(row[2]),
                Temperature = float.Parse(row[4]),
                ActualTemp = float.Parse(row[27]),
                Torque = new Vector3(float.Parse(row[13]), float.Parse(row[16]), float.Parse(row[19])),
                Position = new Vector3(float.Parse(row[11]), float.Parse(row[14]), float.Parse(row[17]))
            };
            dataEntries.Add(entry);
        }
    }

    public void ReadCSV()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string fileContents;

        if (File.Exists(filePath))
        {
            fileContents = File.ReadAllText(filePath);
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
            return;
        }

        string[] rows = fileContents.Split('\n');

        dataEntries.Clear();

        for (int i = 1; i < rows.Length; i++)
        {
            if (rows[i].Trim() == "") continue;
            string[] row = rows[i].Split(',');
            DataEntry entry = new DataEntry
            {
                Time = TimeSpan.Parse(row[2]),
                Temperature = float.Parse(row[4]),
                ActualTemp = float.Parse(row[27]),
                Torque = new Vector3(float.Parse(row[14]), float.Parse(row[17]), float.Parse(row[20])),
                Position = new Vector3(float.Parse(row[12]), float.Parse(row[15]), float.Parse(row[18]))
            };
            //Debugging
            //Debug.Log("Time: " + row[2]);
            //Debug.Log("Value: " + row[4]);
            dataEntries.Add(entry);
        }

        //Debugging
        //Debug.Log("Rows read from CSV: " + rows.Length);
    }

    public void ClearCSVFile()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        
        if (File.Exists(filePath))
        {
            // Store the column titles
            string columnTitles;
            using (StreamReader reader = new StreamReader(filePath))
            {
                columnTitles = reader.ReadLine();
            }

            // Clear the file and write the column titles back
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                writer.WriteLine(columnTitles);

                // Add a dummy entry with 0s in every row
                /*
                string dummyEntry = "0";
                int columnCount = columnTitles.Split(',').Length;
                for (int i = 1; i < columnCount; i++)
                {
                    dummyEntry += ",0";
                }
                writer.WriteLine(dummyEntry);
                */
            }
        }
    }

    

    public class DataEntry
    {
        public TimeSpan Time;
        public float Temperature;
        public float ActualTemp;
        public Vector3 Torque;
        public Vector3 Position;

        /*
        public string GetValueByName(string name)
        {
            DataEntry latestEntry = dataEntries.LastOrDefault();

            if (name == "Time")
            {
                return Time.TotalSeconds.ToString();;
            }
            else if (name == "Temperature")
            {
                return Temperature.ToString();;
            }
            else if (name == "Torque")
            {
                return $"{latestEntry.Torque.x}, {latestEntry.Torque.y}, {latestEntry.Torque.z}";
            }
            else
            {
                Debug.LogWarning($"No field with the name '{name}' found in DataEntry.");
                return "N/A";
            }
        }
        */
    }

    public string[] GetValueByName(string name)
    {
        DataEntry latestEntry = dataEntries.LastOrDefault();
        if (latestEntry == null)
        {
            return new string[] { "N/A" };
        }

        if (name == "Time")
        {
            return new string[] { latestEntry.Time.TotalSeconds.ToString() };
        }
        else if (name == "Temperature")
        {
            return new string[] { latestEntry.Temperature.ToString() };
        }
        else if (name == "ActualTemp")
        {
            return new string[] { latestEntry.ActualTemp.ToString() };
        }
        else if (name == "Torque")
        {
            return new string[] { latestEntry.Torque.x.ToString(), latestEntry.Torque.y.ToString(), latestEntry.Torque.z.ToString() };
        }
        else if (name == "Position")
        {
            return new string[] { latestEntry.Position.x.ToString(), latestEntry.Position.y.ToString(), latestEntry.Position.z.ToString() };
        }
        // Add more fields as necessary
        else
        {
            Debug.LogWarning($"No field with the name '{name}' found in DataEntry.");
            return new string[] { "0" };
        }
    }

    public int GetNumberOfEntries()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        if (File.Exists(filePath))
        {
            string fileContents = File.ReadAllText(filePath);
            string[] rows = fileContents.Split('\n');
            // Subtract 1 because the first row is the header
            return rows.Length - 1;
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
            return 0;
        }
    }
}

