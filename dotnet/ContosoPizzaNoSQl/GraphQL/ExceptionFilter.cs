namespace ContosoPizzaNoSQl.GraphQL;

public class ExceptionFilter : IErrorFilter
{

    public IError OnError(IError error)
    {
         if (error.Exception is GraphQLException ex)
        {
            return ex.Errors.First();
        }
        return error;;
          
    }
}