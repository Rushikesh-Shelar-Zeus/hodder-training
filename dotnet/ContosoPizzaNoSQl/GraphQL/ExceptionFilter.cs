namespace ContosoPizzaNoSQl.GraphQL;

public class ExceptionFilter : IErrorFilter
{

    public IError OnError(IError error)
    {
        if (error.Exception is GraphQLException graphQLException)
        {
            return error.WithMessage(graphQLException.Message);
        }

        return error
          .WithMessage("Something went wrong. Please try again later.")
          .RemoveExtension("extensions");
          
    }
}