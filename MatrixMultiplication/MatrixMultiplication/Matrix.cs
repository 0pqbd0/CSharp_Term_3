namespace MatrixImplementation;

/// <summary>
/// The class representing the matrix
/// </summary>
public class Matrix
{
    /*public const int GeneratedMatrixMaxCellValue = 1000;*/
    private readonly int[,] dataMatrix;

    public int ColumnsCount
    {
        get { return dataMatrix.GetLength(0); }
    }

    public int RowsCount
    {
        get { return dataMatrix.GetLength(1); }
    }

    public Matrix(int[,] dataMatrix)
    {
        this.dataMatrix = (int[,])dataMatrix.Clone();
    }

    public int this[int rowsIndex, int colomnIndex]
    {
        get { return dataMatrix[rowsIndex, colomnIndex]; }
    }

    /*private static readonly Random rand = new Random();*/
    /*public static Matrix GenerateMatrix(int rows, int columns)
    {
        var newMatrixData = new int[rows, columns];
        for (var i = 0; i < rows; ++i)
        {
            for (var j = 0; j < columns; ++j)
            {
                newMatrixData[i, j] = rand.Next(-GeneratedMatrixMaxCellValue, GeneratedMatrixMaxCellValue + 1);
            }
        }

        return new Matrix(newMatrixData);
    }*/

}

