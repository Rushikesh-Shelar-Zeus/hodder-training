/*=============================================
Author:		-- Your Name Here
Create date: -- YYYY-MM-DD
Description:	Fetches a single order by its ID
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

	/*end declaration section */
	/*******************************************************************************************************/

	/*******************************************************************************************************/
	/*begin processing section */
	BEGIN TRY
		SELECT @TodaysDate = GETUTCDATE();

	/*end processing section */
	/*******************************************************************************************************/

	/*******************************************************************************************************/
	/*begin result section */
		SELECT 
			Id,
			CustomerId,
			OrderDate,
			TotalAmount
		FROM Orders
		WHERE Id = @Id;

		RETURN;
	/*end result section */
	/*******************************************************************************************************/

	/*******************************************************************************************************/
	/*begin error handler section */
	END TRY
	BEGIN CATCH
		SELECT @Error = ERROR_NUMBER(), @ErrorMessage = ERROR_MESSAGE();
		SET @IsCustomError = 0;

		EXEC [GetError]
			@ErrorNum = @Error,              -- INT
			@IsCustomError = @IsCustomError, -- BIT
			@ErrorMessage = @ErrorMessage    -- NVARCHAR

		RETURN;
	END CATCH
	/*end error handler section */
	/*******************************************************************************************************/
END
