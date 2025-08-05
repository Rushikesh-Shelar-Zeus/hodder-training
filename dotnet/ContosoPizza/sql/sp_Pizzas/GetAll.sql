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
