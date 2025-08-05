-- GET ALL
CREATE PROCEDURE Customer_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * 
    FROM Customers;
END;
GO

-- GET By Id

CREATE PROCEDURE Customer_GetById
    @Id INT
AS
BEGIN  
    SET NOCOUNT ON;
    
    SELECT * 
    FROM Customers
    WHERE Id = @Id;
END;
GO

-- CREATE 

CREATE PROCEDURE Customer_Create
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @PhoneNumber NVARCHAR(10),
    @Address NVARCHAR(255),
    @NewId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Customers (Name, Email, PhoneNumber, Address)
        VALUES (@Name, @Email,@PhoneNumber, @Address);

        SET @NewId = SCOPE_IDENTITY();

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        
        SET @NewId = -1;
    END CATCH
END;
GO
    

-- UPDATE

CREATE PROCEDURE Customer_Update
    @Id INT,
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @PhoneNumber NVARCHAR(10),
    @Address NVARCHAR(255),
    @RowsAffected  INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE Customers
        SET Name = @Name,
            Email = @Email,
            PhoneNumber = @PhoneNumber,
            Address = @Address
        WHERE Id = @Id;

        SET @RowsAffected = @@ROWCOUNT;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        
        SET @RowsAffected = 0;
    END CATCH
END;
GO

-- DELETE

CREATE PROCEDURE Customer_Delete
    @Id INT,
    @RowsAffected INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM Customers
        WHERE Id = @Id;

        SET @RowsAffected = @@ROWCOUNT;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        SET @RowsAffected = 0;
    END CATCH
END;
GO