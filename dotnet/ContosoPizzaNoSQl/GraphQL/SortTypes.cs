
namespace ContosoPizzaNoSQl.GraphQL.SortTypes;
public class PizzaPagedInput {
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public SortField SortBy { get; set; } = SortField.Name;
    public SortOrder Order { get; set; } = SortOrder.ASC;
}


public class OrderPagedInput {
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public OrderSortField SortBy { get; set; } = OrderSortField.CreatedAt;
    public SortOrder Order { get; set; } = SortOrder.ASC;
}

public enum SortField
{
    Name,
    Price,
}

public enum OrderSortField
{
    CustomerName,
    TotalAmount,
    CreatedAt,
}

public enum SortOrder
{
    ASC,
    DESC
}