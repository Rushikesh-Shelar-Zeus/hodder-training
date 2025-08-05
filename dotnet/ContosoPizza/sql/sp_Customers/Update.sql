/*=============================================
Author:        -- Rushikesh Shelar
Create date:   -- 2025-08-05
Description:   Updates an existing customer in the Customers table.
=============================================*/
CREATE PROCEDURE [dbo].[Customer_Update]
    @Id INT,
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @PhoneNumber NVARCHAR(10),
    @Address NVARCHAR(255),
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

        /* begin transaction section */
        /****************************************/
        BEGIN TRANSACTION;

            UPDATE Customers
            SET Name = @Name,
                Email = @Email,
                PhoneNumber = @PhoneNumber,
                Address = @Address
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
    /* end error handler section */
    /*******************************************************************************************************/
END
GO

/*
-- Example usage:
DECLARE @RowsAffected INT;
EXEC Customer_Update
    @Id = 1,
    @Name = 'Updated Name',
    @Email = 'updated@example.com',
    @PhoneNumber = '9876543210',
    @Address = 'Updated Address',
    @RowsAffected = @RowsAffected OUTPUT;

SELECT @RowsAffected AS RowsAffected;
*/
