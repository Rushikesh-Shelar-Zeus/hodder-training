/*=============================================
Author:        -- Rushikesh Shelar
Create date:   -- 2025-08-05
Description:	Deletes an order and its related order items. Returns number of rows affected from Orders table.
=============================================*/
CREATE PROCEDURE [dbo].[Order_Delete]
(
	@Id INT,
	@RowsAffected INT OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON;

	/*******************************************************************************************************/		
	/*begin declaration section */	
	DECLARE @Error INT,
			@ErrorMessage NVARCHAR(4000),
			@IsCustomError BIT = 0,
			@TodaysDate DATETIME

	/*end declaration section*/
	/*******************************************************************************************************/

	/*******************************************************************************************************/		
	/*begin processing section */			
	BEGIN TRY
		SELECT @TodaysDate = GETUTCDATE()

		/*begin transaction section*/
		/****************************************/
		BEGIN TRANSACTION;

			-- Delete order items first due to FK constraint
			DELETE FROM OrderItems WHERE OrderId = @Id;

			-- Delete the order itself
			DELETE FROM Orders WHERE Id = @Id;

			-- Capture number of rows deleted from Orders table
			SET @RowsAffected = @@ROWCOUNT;

		COMMIT TRANSACTION;
		/*end transaction section*/
		/****************************************/

	/*end processing section */
	/*******************************************************************************************************/

	/*******************************************************************************************************/		
	/*begin result section */
		SELECT 0 AS 'success', @RowsAffected AS 'DeletedOrders'
		RETURN
	/*end result section */
	/*******************************************************************************************************/

	/*******************************************************************************************************/		
	/*begin error handler section */		  
	END TRY  
	BEGIN CATCH  
		SELECT @Error = ERROR_NUMBER(), @ErrorMessage = ERROR_MESSAGE()
		SET @IsCustomError = 0

		IF (@@TRANCOUNT > 0)
			ROLLBACK TRANSACTION;

		SET @RowsAffected = 0

		EXEC [GetError]
			@ErrorNum = @Error,           -- INT
			@IsCustomError = @IsCustomError, -- BIT
			@ErrorMessage = @ErrorMessage   -- NVARCHAR

		RETURN
	END CATCH  
	/*end error handler section */
	/*******************************************************************************************************/			
END
