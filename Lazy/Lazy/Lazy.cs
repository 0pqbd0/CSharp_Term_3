using ILazy;
namespace LazyImplementation;

public class Lazy<T> : ILazy<T>
    where T : class, new()
{
    private Exception? supplierException;
    private Func<T>? supplier;
    private T? value;

    public Lazy(Func<T> supplier)
    {
        value = null;
        this.supplier = supplier;
    }

    public T Get()
    {
        if (supplierException != null)
        {
            throw supplierException;
        }
        if (supplier == null)
        {
            throw new ArgumentNullException("Supplier cannot be null");
        }

        if (value == null)
        {
            try
            {
                value = supplier();
            }
            catch (Exception ex)
            {
                supplierException = ex;
                Console.WriteLine(ex.Message);
            }
        }

        return value;
    }

}