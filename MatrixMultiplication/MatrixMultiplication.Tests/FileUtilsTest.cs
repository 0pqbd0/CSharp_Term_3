using FileUtils;
using MatrixImplementation;


namespace FileUtilsTests;

[TestClass()]
public class MultiplicationTest
{
    [TestMethod()]
    public void ParseFromFileWithCorrectMatrix()
    {
        var matrixFromFile = MatrixFile.ParseFileToMatrix("../../../TestData/CorrectMatrix.txt");

        var expectedResultMatrix = new Matrix(new int[,]
            {
                { 1, 3 },
                { 5, 7 }

            });

        for (var i = 0; i < expectedResultMatrix.ColumnsCount; ++i)
        {
            for (var j = 0; j < expectedResultMatrix.RowsCount; ++j)
            {
                Assert.AreEqual(expectedResultMatrix[i, j], matrixFromFile[i, j],
                $"Элемент [{i}, {j}] не совпадает с ожиадаемым результатом");
            }
        }
    }

    [TestMethod()]
    public void ParseFromFileWithCorrectSpacesMatrix()
    {
        var matrixFromFile = MatrixFile.ParseFileToMatrix("../../../TestData/SpacesMatrix.txt");

        var expectedResultMatrix = new Matrix(new int[,]
            {
                { 1, 17, 21 },
                { 3,  7,  1 },
                { 5,  1000, 11 }

            });

        for (var i = 0; i < expectedResultMatrix.ColumnsCount; ++i)
        {
            for (var j = 0; j < expectedResultMatrix.RowsCount; ++j)
            {
                Assert.AreEqual(expectedResultMatrix[i, j], matrixFromFile[i, j],
                $"Элемент [{i}, {j}] не совпадает с ожиадаемым результатом");
            }
        }
    }

    [TestMethod()]
    public void ParseFromFileWithEmptyMatrix()
    {
        var ex = Assert.ThrowsException<InvalidDataException>(() => MatrixFile.ParseFileToMatrix("../../../TestData/EmptyMatrix.txt"));
        Assert.AreEqual(ex.Message, "File is empty or contains only whitespace");
    }

    [TestMethod()]
    public void ParseFromFileWithIncorrectMatrix()
    {
        var ex = Assert.ThrowsException<InvalidDataException>(() => MatrixFile.ParseFileToMatrix("../../../TestData/IncorrectMatrix.txt"));
        Assert.AreEqual(ex.Message, "Line 3 does not match expected column count");
    }

}
