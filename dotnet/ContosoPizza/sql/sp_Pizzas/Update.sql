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