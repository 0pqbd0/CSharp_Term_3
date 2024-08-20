using MatrixImplementation;

namespace FileUtils;

public static class MatrixFile
{
    /// <summary>
    /// Creates a matrix from the input file
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    public static Matrix ParseFileToMatrix(string filePath)
    {
        var lines = File.ReadAllLines(filePath);

        var nonEmptyLines = lines
        .Select(line => line.Trim())
        .Where(line => !string.IsNullOrWhiteSpace(line))
        .ToList();

        if (nonEmptyLines.Count == 0)
        {
            throw new InvalidDataException("File is empty or contains only whitespace");
        }

        int[,] dataMatrix = new int[nonEmptyLines.Count, lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length];

        for (var i = 0; i < nonEmptyLines.Count; ++i)
        {
            var line = nonEmptyLines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                         .Select(value =>
                         {
                             if (int.TryParse(value, out int result))
                             {
                                 return result;
                             }
                             else
                             {
                                 throw new InvalidDataException($"Invalid number format in line {i + 1}: '{value}'");
                             }
                         })
                         .ToArray();
            if (line.Length != dataMatrix.GetLength(1))
            {
                throw new InvalidDataException($"Line {i + 1} does not match expected column count");
            }

            for (var j = 0; j < line.Length; ++j)
            {
                dataMatrix[i, j] = line[j];
            }
        }
        return new Matrix(dataMatrix);
    }

    /// <summary>
    /// Saves the matrix to a file
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="matrix"></param>
    public static void SaveMatrixToAFile(string filePath, Matrix matrix)
    {
        using StreamWriter writer = new StreamWriter(filePath);

        for (var i = 0; i < matrix.RowsCount; ++i)
        {
            for (var j = 0; j < matrix.ColumnsCount; ++j)
            {
                writer.Write($"{matrix[i, j]} ");
            }

            writer.Write(Environment.NewLine);
        }
    }
}
