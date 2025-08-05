/*=============================================
Author:        -- Rushikesh Shelar
Create date:   -- 2025-08-05
Description:	Creates a new order record and returns the new Order ID
=============================================*/
CREATE PROCEDURE [dbo].[Order_Create]
(
	@CustomerId INT,
	@OrderDate DATETIME,
	@TotalAmount DECIMAL(10, 2),
	@NewOrderId INT OUTPUT
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

			INSERT INTO Orders (CustomerId, OrderDate, TotalAmount)
			VALUES (@CustomerId, @OrderDate, @TotalAmount);

			SET @NewOrderId = SCOPE_IDENTITY();

		COMMIT TRANSACTION;
		/*end transaction section*/
		/****************************************/

	/*end processing section */
	/*******************************************************************************************************/

	/*******************************************************************************************************/		
	/*begin result section */
		SELECT 0 AS 'success', @NewOrderId AS 'OrderId'
		RETURN
	/*end result section */
	/*******************************************************************************************************/

	/*******************************************************************************************************/		
	/*begin error handler section */		  
	END TRY  
	BEGIN CATCH  
		SELECT @Error = ERROR_NUMBER(), @ErrorMessage = ERROR_MESSAGE()
		SET @IsCustomError = 0
		SET @NewOrderId = -1

		IF (@@TRANCOUNT > 0)
			ROLLBACK TRANSACTION

		EXEC [GetError]
			@ErrorNum = @Error,        -- INT
			@IsCustomError = @IsCustomError,  -- BIT
			@ErrorMessage = @ErrorMessage    -- NVARCHAR

		RETURN
	END CATCH  
	/*end error handler section */
	/*******************************************************************************************************/			
END
