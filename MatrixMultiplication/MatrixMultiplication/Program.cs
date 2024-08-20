using FileUtils;
using MatrixImplementation;

public class Program
{
    public static int Main(string[] args)
    {
        if (args.Length == 0 || args[0] == "--help" || args[0] == "-h")
        {
            Console.WriteLine("""

              This program for matrix multiplication using parallel computing.

              To multiply matrices from files:

              dotnet run [first matrix file path] [second matrix file path] [result file path] 

              """);
            return 0;
        }

        if (args.Length == 4)
        {
            try
            {

                var firstMatrix = MatrixFile.ParseFileToMatrix(args[0]);
                var secondMatrix = MatrixFile.ParseFileToMatrix(args[1]);
                var resultMatrix = Multiplication.MultiplicationParallel(firstMatrix, secondMatrix);
                MatrixFile.SaveMatrixToAFile(args[2], resultMatrix);
                return 0;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("The wrong size of the matrix");
                return 1;
            }
            catch (InvalidDataException)
            {
                Console.WriteLine("The wrong file");
                return 1;
            }

        }

        else
        {
            Console.WriteLine("Invalid number of arguments.");
            Console.WriteLine("For help use: dotnet run --help or -h");
            return 1;
        }
    }
}