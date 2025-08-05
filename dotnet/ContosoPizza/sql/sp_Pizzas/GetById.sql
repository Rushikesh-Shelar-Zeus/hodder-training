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
