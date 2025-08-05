/*=============================================
Author:        -- Rushikesh Shelar
Create date:   -- 2025-08-05
Description:	Fetches a single order by ID with its customer and item details
=============================================*/
CREATE PROCEDURE [dbo].[Order_GetById]
(
	@Id INT
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

	/*end processing section */
	/*******************************************************************************************************/

	/*******************************************************************************************************/		
	/*begin result section */
	
		-- Get order with customer info
		SELECT 
			o.Id, 
			o.CustomerId, 
			c.Name AS CustomerName, 
			o.OrderDate, 
			o.TotalAmount
		FROM Orders o
		JOIN Customers c ON o.CustomerId = c.Id
		WHERE o.Id = @Id;

		-- Get all items in the order
		SELECT 
			oi.Id, 
			oi.PizzaId, 
			p.Name AS PizzaName, 
			oi.Quantity, 
			oi.UnitPrice
		FROM OrderItems oi
		JOIN Pizzas p ON oi.PizzaId = p.Id
		WHERE oi.OrderId = @Id;

		RETURN
	/*end result section */
	/*******************************************************************************************************/

	/*******************************************************************************************************/		
	/*begin error handler section */		  
	END TRY  
	BEGIN CATCH  
		SELECT @Error = ERROR_NUMBER(), @ErrorMessage = ERROR_MESSAGE()
		SET @IsCustomError = 0

		EXEC [GetError]
			@ErrorNum = @Error,           -- INT
			@IsCustomError = @IsCustomError, -- BIT
			@ErrorMessage = @ErrorMessage   -- NVARCHAR

		RETURN
	END CATCH  
	/*end error handler section */
	/*******************************************************************************************************/			
END
