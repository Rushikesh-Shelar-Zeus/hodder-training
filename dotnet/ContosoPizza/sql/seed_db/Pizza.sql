DECLARE @i INT = 1;

DECLARE @Adjectives TABLE (Word NVARCHAR(50));
DECLARE @Types TABLE (Word NVARCHAR(50));

INSERT INTO @Adjectives VALUES ('Spicy'), ('Cheesy'), ('Tangy'), ('Crunchy'), ('Hot'), ('Deluxe'), ('Classic'), ('Smokey'), ('Sweet'), ('Loaded');
INSERT INTO @Types VALUES ('Margherita'), ('Pepperoni'), ('Paneer Tikka'), ('Veggie'), ('BBQ Chicken'), ('Hawaiian'), ('Farmhouse'), ('Supreme'), ('Four Cheese'), ('Mexican');

WHILE @i <= 100
BEGIN
    DECLARE @Adj NVARCHAR(50) = (SELECT TOP 1 Word FROM @Adjectives ORDER BY NEWID());
    DECLARE @Type NVARCHAR(50) = (SELECT TOP 1 Word FROM @Types ORDER BY NEWID());
    DECLARE @PizzaName NVARCHAR(100) = @Adj + ' ' + @Type;
    DECLARE @Price DECIMAL(10,2) = ROUND(RAND() * 300 + 100, 2); -- ₹100–₹400
    DECLARE @IsGlutenFree BIT = CAST(RAND() * 2 AS BIT);

    INSERT INTO Pizzas (Name, Price, IsGlutenFree)
    VALUES (@PizzaName, @Price, @IsGlutenFree);

    SET @i = @i + 1;
END;
