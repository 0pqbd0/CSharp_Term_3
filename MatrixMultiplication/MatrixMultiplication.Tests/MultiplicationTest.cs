using MatrixImplementation;

namespace MatrixImplementationTests;

[TestClass()]
public class MultiplicationTest
{
    [TestMethod()]
    public void MultiplicationWitCorrectDataTest()
    {
        var firstMatrix = new Matrix(new int[,]
            {
                { 1, 11 },
                { 1,  5 }

            });

        var secondMatrix = new Matrix(new int[,]
            {
                { 1, 1 },
                { 7, 3 }

            });

        var expectedResultMatrix = new Matrix(new int[,]
            {
                { 78, 34 },
                { 36, 16 }

            });

        var resultMatrixUsingStandardMultiplication = Multiplication.MultiplicationStandard(firstMatrix, secondMatrix);
        var resultMatrixUsingParallelMultiplication = Multiplication.MultiplicationParallel(firstMatrix, secondMatrix);

        for (var i = 0; i < expectedResultMatrix.ColumnsCount; ++i)
        {
            for (var j = 0; j < expectedResultMatrix.RowsCount; ++j)
            {
                Assert.AreEqual(expectedResultMatrix[i, j], resultMatrixUsingStandardMultiplication[i, j],
                $"Элемент [{i}, {j}] не совпадает с ожидаймым результатом");

                Assert.AreEqual(resultMatrixUsingStandardMultiplication[i, j], resultMatrixUsingParallelMultiplication[i, j],
                $"Элемент [{i}, {j}] не совпадает между стандартной и параллельной реализациями");
            }
        }
    }

    [TestMethod()]
    public void MultiplicationStandardWitIncorrectDataTest()
    {
        var firstMatrix = new Matrix(new int[,]
            {
                { 1, 11 },
                { 1,  5 },
                { 1, 19 }

            });

        var secondMatrix = new Matrix(new int[,]
            {
                { 1, 1 },
                { 7, 3 }
            });

        var ex = Assert.ThrowsException<ArgumentException>(() => Multiplication.MultiplicationStandard(firstMatrix, secondMatrix));
        Assert.AreEqual(ex.Message, "The dimension of the matrices does not allow to multiply them");
    }

    [TestMethod()]
    public void MultiplicationParallelWitIncorrectDataTest()
    {
        var firstMatrix = new Matrix(new int[,]
            {
                { 1, 11 },
                { 1,  5 },
                { 1, 19 }

            });

        var secondMatrix = new Matrix(new int[,]
            {
                { 1, 1 },
                { 7, 3 }
            });

        var ex = Assert.ThrowsException<ArgumentException>(() => Multiplication.MultiplicationParallel(firstMatrix, secondMatrix));
        Assert.AreEqual(ex.Message, "The dimension of the matrices does not allow to multiply them");
    }
}