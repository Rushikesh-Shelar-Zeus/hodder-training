/*=============================================
Author:        -- Rushikesh Shelar
Create date:   -- 2025-08-05
Description:   Deletes a customer from the Customers table by Id.
              Returns 0 rows affected if not found.
=============================================*/
CREATE PROCEDURE [dbo].[Customer_Delete]
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
            @TodaysDate DATETIME
    /* end declaration section */
    /*******************************************************************************************************/

    /*******************************************************************************************************/
    /* begin processing section */
    BEGIN TRY
        SELECT @TodaysDate = GETUTCDATE();

        -- Validate: Customer must exist
        IF NOT EXISTS (SELECT 1 FROM Customers WHERE Id = @Id)
        BEGIN
            SET @IsCustomError = 1;
            SET @Error = 404;
            SET @ErrorMessage = 'Customer not found.';
            GOTO ErrorHandler;
        END

        /* begin transaction section */
        /****************************************/
        BEGIN TRANSACTION;

            DELETE FROM Customers
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

        SELECT @Error = ERROR_NUMBER(),
               @ErrorMessage = ERROR_MESSAGE();
        
        SET @RowsAffected = 0;

        EXEC [GetError]
            @ErrorNum = @Error,
            @IsCustomError = @IsCustomError,
            @ErrorMessage = @ErrorMessage;

        RETURN;
    END CATCH

    -- Manual error handler for custom errors
    ErrorHandler:
        IF (@@TRANCOUNT > 0)
            ROLLBACK TRANSACTION;

        SET @RowsAffected = 0;

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
DECLARE @RowsAffected INT;
EXEC Customer_Delete
    @Id = 1,
    @RowsAffected = @RowsAffected OUTPUT;

SELECT @RowsAffected AS RowsAffected;
*/
