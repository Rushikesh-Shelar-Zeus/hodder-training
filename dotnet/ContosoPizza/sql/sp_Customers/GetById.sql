/*=============================================
Author:        -- Rushikesh Shelar
Create date:   -- 2025-08-05
Description:   Retrieves a customer by Id from the Customers table.
=============================================*/
CREATE PROCEDURE [dbo].[Customer_GetById]
    @Id INT
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

        /* No transaction needed for SELECT */
        /* end processing section */
    /*******************************************************************************************************/

    /*******************************************************************************************************/
    /* begin result section */
        SELECT * 
        FROM Customers
        WHERE Id = @Id;

        RETURN;
    /* end result section */
    /*******************************************************************************************************/

    END TRY
    BEGIN CATCH
        SELECT @Error = ERROR_NUMBER(),
               @ErrorMessage = ERROR_MESSAGE();
        GOTO ErrorHandler;
    END CATCH;

    /*******************************************************************************************************/
    /* begin error handler section */
    ErrorHandler:
        EXEC [GetError]
            @ErrorNum = @Error,
            @IsCustomError = @IsCustomError,
            @ErrorMessage = @ErrorMessage;

        IF (@@TRANCOUNT > 0)
            ROLLBACK TRANSACTION;

        RETURN;
    /* end error handler section */
    /*******************************************************************************************************/
END
GO

/*
EXEC Customer_GetById
    @Id = 1;
*/
