/*=============================================
Author:        -- Rushikesh Shelar
Create date:   -- 2025-08-05
Description:	Creates a new order item record and returns the new OrderItem ID
=============================================*/
CREATE PROCEDURE [dbo].[OrderItem_Create]
(
	@OrderId INT,
	@PizzaId INT,
	@Quantity INT,
	@UnitPrice DECIMAL(10, 2),
	@NewItemId INT OUTPUT
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

			INSERT INTO OrderItems (OrderId, PizzaId, Quantity, UnitPrice)
			VALUES (@OrderId, @PizzaId, @Quantity, @UnitPrice);

			SET @NewItemId = SCOPE_IDENTITY();

		COMMIT TRANSACTION;
		/*end transaction section*/
		/****************************************/

	/*end processing section */
	/*******************************************************************************************************/

	/*******************************************************************************************************/		
	/*begin result section */
		SELECT 0 AS 'success', @NewItemId AS 'OrderItemId'
		RETURN
	/*end result section */
	/*******************************************************************************************************/

	/*******************************************************************************************************/		
	/*begin error handler section */		  
	END TRY  
	BEGIN CATCH  
		SELECT @Error = ERROR_NUMBER(), @ErrorMessage = ERROR_MESSAGE()
		SET @IsCustomError = 0
		SET @NewItemId = -1

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
