using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// A StringBuilder with a maximum number of lines.
/// Older lines are removed as new lines are added.
/// </summary>
public class LimitedStringBuilder
{
    private StringBuilder StringBuilder { get; } = new StringBuilder();
    private Queue<string> LineQueue { get; } = new Queue<string>();
    private int MaxLines { get; }

    /// <summary>Creates a new LimitedStringBuilder with the given max size</summary>
    /// <param name="maxLines">The maximum number of lines to hold</param>
    public LimitedStringBuilder(int maxLines)
    {
        MaxLines = maxLines;
    }

    /// <summary>Append a new line</summary>
    /// <param name="line">A string to add as a new line</param>
    public void AppendLine(string line)
    {
        LineQueue.Enqueue(line);
        StringBuilder.AppendLine(line);

        if (LineQueue.Count > MaxLines)
        {
            var oldestLine = LineQueue.Dequeue();
            var length = oldestLine.Length + Environment.NewLine.Length;
            StringBuilder.Remove(0, length);
        }
    }

    /// <summary>Returns the contents of the string builder</summary>
    /// <returns>A string</returns>
    public override string ToString() => StringBuilder.ToString();
}
