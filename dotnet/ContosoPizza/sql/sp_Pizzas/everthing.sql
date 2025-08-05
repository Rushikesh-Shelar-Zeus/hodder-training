/*=============================================
Author:        -- Rushikesh Shelar
Create date:   -- 2025-08-05
Description:   Inserts a new pizza into the Pizzas table and returns the new Id.
=============================================*/
CREATE PROCEDURE [dbo].[Pizza_Create]
    @Name NVARCHAR(100),
    @Price DECIMAL(10,2),
    @IsGlutenFree BIT,
    @NewId INT OUTPUT
AS
BEGIN 
    SET NOCOUNT ON;

    /*******************************************************************************************************/
    /* begin declaration section */
    DECLARE @Error INT,
            @ErrorMessage NVARCHAR(4000),
            @IsCustomError BIT = 0,
            @TodaysDate DATETIME;
    /* end declaration section */
    /*******************************************************************************************************/

    /*******************************************************************************************************/
    /* begin processing section */
    BEGIN TRY
        SELECT @TodaysDate = GETUTCDATE();

        /* begin transaction section */
        /****************************************/
        BEGIN TRANSACTION;

            INSERT INTO Pizzas (Name, Price, IsGlutenFree)
            VALUES (@Name, @Price, @IsGlutenFree);

            SET @NewId = SCOPE_IDENTITY();

        COMMIT TRANSACTION;
        /****************************************/
        /* end transaction section */
    END TRY
    /* end processing section */
    /*******************************************************************************************************/

    /*******************************************************************************************************/
    /* begin error handler section */
    BEGIN CATCH
        IF (@@TRANCOUNT > 0)
            ROLLBACK TRANSACTION;

        SELECT @Error = ERROR_NUMBER(),
               @ErrorMessage = ERROR_MESSAGE();

        SET @NewId = -1;

        EXEC [GetError]
            @ErrorNum = @Error,
            @IsCustomError = @IsCustomError,
            @ErrorMessage = @ErrorMessage;

        RETURN;
    END CATCH
    /* end error handler section */
    /*******************************************************************************************************/
END
GO

/*
-- Example usage:
DECLARE @NewId INT;
EXEC Pizza_Create
    @Name = 'Pepperoni',
    @Price = 12.99,
    @IsGlutenFree = 0,
    @NewId = @NewId OUTPUT;

SELECT @NewId AS NewPizzaId;
*/


/*=============================================
Author:        -- Rushikesh Shelar
Create date:   -- 2025-08-05
Description:   Deletes a pizza from the Pizzas table by Id.
=============================================*/
CREATE PROCEDURE [dbo].[Pizza_Delete]
    @Id INT,
    @RowsAffected INT OUTPUT
AS
BEGIN 
    SET NOCOUNT ON;

    /*******************************************************************************************************/
    /* begin declaration section */
    DECLARE @Error INT,
            @ErrorMessage NVARCHAR(4000),
            @IsCustomError BIT = 0,
            @TodaysDate DATETIME;
    /* end declaration section */
    /*******************************************************************************************************/

    /*******************************************************************************************************/
    /* begin processing section */
    BEGIN TRY
        SELECT @TodaysDate = GETUTCDATE();

        -- Validate: Pizza must exist
        IF NOT EXISTS (SELECT 1 FROM Pizzas WHERE Id = @Id)
        BEGIN
            SET @IsCustomError = 1;
            SET @Error = 404;
            SET @ErrorMessage = 'Pizza not found.';
            RAISERROR(@ErrorMessage, 16, 1);  -- control goes to CATCH
        END

        /* begin transaction section */
        /****************************************/
        BEGIN TRANSACTION;

            DELETE FROM Pizzas 
            WHERE Id = @Id;

            SET @RowsAffected = @@ROWCOUNT;

        COMMIT TRANSACTION;
        /****************************************/
        /* end transaction section */
    END TRY
    /* end processing section */
    /*******************************************************************************************************/

    /*******************************************************************************************************/
    /* begin error handler section */
    BEGIN CATCH
        IF (@@TRANCOUNT > 0)
            ROLLBACK TRANSACTION;

        SELECT 
            @Error = ERROR_NUMBER(),
            @ErrorMessage = ERROR_MESSAGE();

        SET @RowsAffected = 0;

        EXEC [GetError]
            @ErrorNum = @Error,
            @IsCustomError = @IsCustomError,
            @ErrorMessage = @ErrorMessage;

        RETURN;
    END CATCH
    /* end error handler section */
    /*******************************************************************************************************/
END
GO

/*
-- Example usage:
DECLARE @RowsAffected INT;
EXEC Pizza_Delete
    @Id = 1,
    @RowsAffected = @RowsAffected OUTPUT;

SELECT @RowsAffected AS RowsAffected;
*/


/*=============================================
Author:        -- Rushikesh Shelar
Create date:   -- 2025-08-05
Description:   Retrieves all pizzas from the Pizzas table.
=============================================*/
CREATE PROCEDURE [dbo].[Pizza_GetAll]
AS
BEGIN 
    SET NOCOUNT ON;

    /*******************************************************************************************************/
    /* begin declaration section */
    DECLARE @Error INT,
            @ErrorMessage NVARCHAR(4000),
            @IsCustomError BIT = 0,
            @TodaysDate DATETIME;
    /* end declaration section */
    /*******************************************************************************************************/

    /*******************************************************************************************************/
    /* begin processing section */
    BEGIN TRY
        SELECT @TodaysDate = GETUTCDATE();

        /* No transaction needed for simple SELECT */
    END TRY
    /* end processing section */
    /*******************************************************************************************************/

    /*******************************************************************************************************/
    /* begin result section */
        SELECT * 
        FROM Pizzas;

        RETURN;
    /* end result section */
    /*******************************************************************************************************/

    /*******************************************************************************************************/
    /* begin error handler section */
    BEGIN CATCH
        SELECT @Error = ERROR_NUMBER(),
               @ErrorMessage = ERROR_MESSAGE();

        EXEC [GetError]
            @ErrorNum = @Error,
            @IsCustomError = @IsCustomError,
            @ErrorMessage = @ErrorMessage;

        RETURN;
    END CATCH
    /* end error handler section */
    /*******************************************************************************************************/
END
GO

/*
-- Example usage:
EXEC Pizza_GetAll;
*/

/*=============================================
Author:        -- Rushikesh Shelar
Create date:   -- 2025-08-05
Description:   Retrieves a pizza by Id from the Pizzas table.
=============================================*/
CREATE PROCEDURE [dbo].[Pizza_GetById]
    @Id INT
AS
BEGIN 
    SET NOCOUNT ON;

    /*******************************************************************************************************/
    /* begin declaration section */
    DECLARE @Error INT,
            @ErrorMessage NVARCHAR(4000),
            @IsCustomError BIT = 0,
            @TodaysDate DATETIME;
    /* end declaration section */
    /*******************************************************************************************************/

    /*******************************************************************************************************/
    /* begin processing section */
    BEGIN TRY
        SELECT @TodaysDate = GETUTCDATE();

        -- Optional: check if Pizza exists
        IF NOT EXISTS (SELECT 1 FROM Pizzas WHERE Id = @Id)
        BEGIN
            SET @IsCustomError = 1;
            SET @Error = 404;
            SET @ErrorMessage = 'Pizza not found.';
            GOTO ErrorHandler;
        END
    END TRY
    /* end processing section */
    /*******************************************************************************************************/

    /*******************************************************************************************************/
    /* begin result section */
        SELECT * 
        FROM Pizzas 
        WHERE Id = @Id;

        RETURN;
    /* end result section */
    /*******************************************************************************************************/

    /*******************************************************************************************************/
    /* begin error handler section */
    BEGIN CATCH
        SELECT @Error = ERROR_NUMBER(),
               @ErrorMessage = ERROR_MESSAGE();

        EXEC [GetError]
            @ErrorNum = @Error,
            @IsCustomError = @IsCustomError,
            @ErrorMessage = @ErrorMessage;

        RETURN;
    END CATCH

    -- Manual handler for custom validation failures
    ErrorHandler:
        EXEC [GetError]
            @ErrorNum = @Error,
            @IsCustomError = @IsCustomError,
            @ErrorMessage = @ErrorMessage;

        RETURN;
    /* end error handler section */
    /*******************************************************************************************************/
END
GO

/*
-- Example usage:
EXEC Pizza_GetById @Id = 1;
*/

/*=============================================
Author:        -- Rushikesh Shelar
Create date:   -- 2025-08-05
Description:   Updates an existing pizza in the Pizzas table.
=============================================*/
CREATE PROCEDURE [dbo].[Pizza_Update]
    @Id INT,
    @Name NVARCHAR(100),
    @Price DECIMAL(10,2),
    @IsGlutenFree BIT,
    @RowsAffected INT OUTPUT
AS
BEGIN 
    SET NOCOUNT ON;

    /*******************************************************************************************************/
    /* begin declaration section */
    DECLARE @Error INT,
            @ErrorMessage NVARCHAR(4000),
            @IsCustomError BIT = 0,
            @TodaysDate DATETIME;
    /* end declaration section */
    /*******************************************************************************************************/

    /*******************************************************************************************************/
    /* begin processing section */
    BEGIN TRY
        SELECT @TodaysDate = GETUTCDATE();

        -- Validate: Pizza must exist
        IF NOT EXISTS (SELECT 1 FROM Pizzas WHERE Id = @Id)
        BEGIN
            SET @IsCustomError = 1;
            SET @Error = 404;
            SET @ErrorMessage = 'Pizza not found.';
            RAISERROR(@ErrorMessage, 16, 1);  -- control flows to CATCH
        END

        /* begin transaction section */
        /****************************************/
        BEGIN TRANSACTION;

            UPDATE Pizzas 
            SET Name = @Name,
                Price = @Price,
                IsGlutenFree = @IsGlutenFree
            WHERE Id = @Id;

            SET @RowsAffected = @@ROWCOUNT;

        COMMIT TRANSACTION;
        /****************************************/
        /* end transaction section */
    END TRY
    /* end processing section */
    /*******************************************************************************************************/

    /*******************************************************************************************************/
    /* begin error handler section */
    BEGIN CATCH
        IF (@@TRANCOUNT > 0)
            ROLLBACK TRANSACTION;

        SELECT 
            @Error = ERROR_NUMBER(),
            @ErrorMessage = ERROR_MESSAGE();

        SET @RowsAffected = 0;

        EXEC [GetError]
            @ErrorNum = @Error,
            @IsCustomError = @IsCustomError,
            @ErrorMessage = @ErrorMessage;

        RETURN;
    END CATCH
    /* end error handler section */
    /*******************************************************************************************************/
END
GO

/*
-- Example usage:
DECLARE @RowsAffected INT;
EXEC Pizza_Update
    @Id = 1,
    @Name = 'Margherita',
    @Price = 9.99,
    @IsGlutenFree = 0,
    @RowsAffected = @RowsAffected OUTPUT;

SELECT @RowsAffected AS RowsAffected;
*/