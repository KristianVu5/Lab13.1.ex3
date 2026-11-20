using System;
using System.IO;

class ParseFileException : ApplicationException
{
    public string FileName { get; }
    public long LineNumber { get; }

    public ParseFileException(string aMassage, string aFileName, long aLineNumber,
        Exception aCauseException) : base (aMassage, aCauseException)
    {
        FileName = aFileName;
        LineNumber = aLineNumber;
    }

    public ParseFileException(string aMassage, string aFileName, Exception aCauseException) :
        this(aMassage, aFileName, 0, aCauseException)
    { }

    public ParseFileException(string aMassage, string aFileName):
        this(aMassage, aFileName, null)
    { }
    
}

class Program
{
    static void Main()
    {
        try
        {
            long sumOfLines = CalculateSumOfLines(@"C:\New folder\test.txt");
            Console.WriteLine("The sum of lines = {0}", sumOfLines);
        }
        catch (ParseFileException pfe)
        {
            Console.WriteLine("File name: {0}", pfe.FileName);
            Console.WriteLine("Line number: {0}", pfe.LineNumber);
            Console.WriteLine("Exception: {0}", pfe);
        }
        Console.ReadLine();
    }

    static long CalculateSumOfLines(string aFileName)
    {
        StreamReader inF;
        try
        {
            inF = File.OpenText(aFileName);
        }
        catch (IOException ioe)
        {
            throw new ParseFileException(String.Format("Can not open the file {0} for reading.", aFileName), aFileName, ioe);
        }

        try
        {
            long sum = 0;
            long lineNumber = 0;
            while (true)
            {
                lineNumber++;
                string line;
                try
                {
                    line = inF.ReadLine();
                }
                catch (IOException ioe)
                {
                    throw new ParseFileException(
                        "Error reading from file.", aFileName, lineNumber, ioe);
                }

                if (line == null)
                    break;

                try
                {
                    sum += Int32.Parse(line);
                }
                catch (SystemException se)
                {
                    throw new ParseFileException(String.Format("Error parsing line '{0}'.", line), aFileName, lineNumber, se);
                }
            }
            return sum;
        }
        finally
        {
            inF.Close();
        }
    }
}