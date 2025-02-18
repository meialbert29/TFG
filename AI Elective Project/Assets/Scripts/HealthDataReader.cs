using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

public class HealthDataReader : MonoBehaviour
{
    private XmlDocument healthData = new XmlDocument();
    private int i;
    private string textFirstDate;
    private string textLastDate;
    private int value;
    public object healthValues;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            // Load the XML file first
            healthData.Load("E:\\AI Project\\Pruebas\\Files\\test2.xml");
            Debug.Log("XML loaded.");

            // Call returnData to get the values
            healthValues = returnData();
        }
        catch (System.IO.FileNotFoundException)
        {
            Debug.LogError("XML File not found.");
        }
        catch (XmlException xmlEx)
        {
            Debug.LogError("Error loading the XML: " + xmlEx.Message);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Unknown error: " + ex.Message);
        }
    }

    public object[,] returnData()
    {
        XmlNodeList recordNodes = healthData.DocumentElement.SelectNodes("/HealthData/Record");

        object[,] values = new object[recordNodes.Count, 3];

        int r = 0;

        foreach (XmlNode recordNode in recordNodes)
        {
            if (r >= recordNodes.Count)
            {
                Debug.LogError("Attempt to access an array row out of bounds.");
                break; // Avoid out-of-bounds access
            }

            string startDate = recordNode.Attributes["startDate"].Value;
            string endDate = recordNode.Attributes["endDate"].Value;

            textFirstDate = "";
            textLastDate = "";

            i = 0;

            while (i < startDate.Length)
            {
                if (startDate[i] != ' ' && endDate[i] != ' ')
                {
                    textFirstDate += startDate[i];
                    textLastDate += endDate[i];
                    i++;
                }
                else
                {
                    i = startDate.Length;
                }
            }

            int value = int.Parse(recordNode.Attributes["value"].Value);

            values[r, 0] = textFirstDate;
            values[r, 1] = textLastDate;
            values[r, 2] = value;

            r++;
        }

        return values;
    }
}
