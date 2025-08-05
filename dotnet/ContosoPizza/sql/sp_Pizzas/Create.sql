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
