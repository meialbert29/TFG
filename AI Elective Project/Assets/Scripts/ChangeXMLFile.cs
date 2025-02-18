using System;
using System.IO;

class XMLProcessor
{
    public void ReadAndWriteXML(string inputFilePath, string outputFilePath)
    {
        try
        {
            // Read data from the input XML file
            string[] lines = File.ReadAllLines(inputFilePath);

            // Create the output XML file and write the content
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                // Write XML file header
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                writer.WriteLine("<root>");

                bool isRootTag = false;

                // Process and copy data to the output file
                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();

                    // Skip header or duplicate root lines
                    if (trimmedLine.StartsWith("<?xml") || (trimmedLine.StartsWith("<root") && isRootTag))
                        continue;

                    // Mark when we find the initial root tag
                    if (trimmedLine.StartsWith("<root"))
                        isRootTag = true;

                    // Copy content to the new XML file
                    writer.WriteLine(trimmedLine);
                }

                // Close the root tag
                writer.WriteLine("</root>");
            }

            Console.WriteLine("Data processed and saved to " + outputFilePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error processing the XML file: " + ex.Message);
        }
    }

    static void Main()
    {
        XMLProcessor processor = new XMLProcessor();
        string inputFilePath = "E:\\AI Project\\Pruebas\\Files\\originFile.xml";
        string outputFilePath = "E:\\AI Project\\Pruebas\\Files\\destinyFile.xml";

        processor.ReadAndWriteXML(inputFilePath, outputFilePath);
    }
}
