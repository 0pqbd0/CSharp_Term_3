namespace MatrixImplementation;

public static class Multiplication
{
    /// <summary>
    /// Checks wheather the number of columns in the first matrix matches the number of rows in the second matrix
    /// </summary>
    /// <param name="firstMatrix"></param>
    /// <param name="secondMatrix"></param>
    /// <returns></returns>
    public static bool IsMultiplied(Matrix firstMatrix, Matrix secondMatrix)
    {
        return firstMatrix.ColumnsCount == secondMatrix.RowsCount;
    }

    /// <summary>
    /// Standard single-threaded multiplication of two matrices
    /// </summary>
    /// <param name="firstMatrix"></param>
    /// <param name="secondMatrix"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static Matrix MultiplicationStandard(Matrix firstMatrix, Matrix secondMatrix)
    {
        if (!IsMultiplied(firstMatrix, secondMatrix))
        {
            throw new ArgumentException("The dimension of the matrices does not allow to multiply them");
        }

        int[,] result = new int[firstMatrix.RowsCount, secondMatrix.ColumnsCount];
        for (var i = 0; i < firstMatrix.RowsCount; ++i)
        {
            for (var j = 0; j < secondMatrix.ColumnsCount; ++j)
            {
                result[i, j] = Enumerable.Range(0, firstMatrix.ColumnsCount)
                    .Sum(k => firstMatrix[i, k] * secondMatrix[k, j]);
            }
        }
        return new Matrix(result);
    }

    /// <summary>
    /// Parallel multiplication of two matrices
    /// </summary>
    /// <param name="firstMatrix"></param>
    /// <param name="secondMatrix"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static Matrix MultiplicationParallel(Matrix firstMatrix, Matrix secondMatrix)
    {
        if (!IsMultiplied(firstMatrix, secondMatrix))
        {
            throw new ArgumentException("The dimension of the matrices does not allow to multiply them");
        }

        int[,] result = new int[firstMatrix.RowsCount, secondMatrix.ColumnsCount];
        var threadsCount = Math.Min(Environment.ProcessorCount, firstMatrix.RowsCount);
        var threads = new Thread[threadsCount];
        var rowsPerThreads = (firstMatrix.RowsCount / threadsCount) + 1;

        for (var i = 0; i < threadsCount; ++i)
        {
            var currentThredNumber = i;
            threads[currentThredNumber] = new Thread(() =>
            {
                for (var rowNumber = currentThredNumber * rowsPerThreads;
                    rowNumber < (currentThredNumber + 1) * rowsPerThreads && rowNumber < firstMatrix.RowsCount;
                    ++rowNumber)
                {
                    for (var columnNumber = 0; columnNumber < secondMatrix.ColumnsCount; ++columnNumber)
                    {
                        result[rowNumber, columnNumber] = Enumerable.Range(0, firstMatrix.ColumnsCount)
                    .Sum(k => firstMatrix[rowNumber, k] * secondMatrix[k, columnNumber]);
                    }
                }

            });
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }
        foreach (var thread in threads)
        {
            thread.Join();
        }
        return new Matrix(result);
    }
}
