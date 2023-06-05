namespace Order.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name,object key):base($"Enitiy \"{name}\" ({key}) is not found") 
        { 
                    
        }
    }
}
