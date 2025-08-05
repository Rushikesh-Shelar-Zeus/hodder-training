/*=============================================
Author:        -- Rushikesh Shelar
Create date:   -- 2025-08-05
Description:   Inserts a new customer into the Customers table and returns the new Id.
=============================================*/
CREATE PROCEDURE [dbo].[Customer_Create]
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @PhoneNumber NVARCHAR(10),
    @Address NVARCHAR(255),
    @NewId INT OUTPUT
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

        /* begin transaction section */
        /****************************************/
        BEGIN TRANSACTION;

            INSERT INTO Customers (Name, Email, PhoneNumber, Address)
            VALUES (@Name, @Email, @PhoneNumber, @Address);

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
-- Declare @NewId first to run it interactively:
DECLARE @NewId INT;
EXEC Customer_Create
    @Name = 'John Doe',
    @Email = 'john@example.com',
    @PhoneNumber = '1234567890',
    @Address = '123 Main St',
    @NewId = @NewId OUTPUT;

SELECT @NewId;
*/
