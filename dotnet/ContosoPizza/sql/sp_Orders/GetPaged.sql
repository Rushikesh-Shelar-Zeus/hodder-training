/*=============================================
Author:		-- Rushikesh Shelar
Create date: -- 2025-08-05
Description:	Fetches paginated orders sorted by a given column and direction. Supports dynamic ORDER BY.
=============================================*/
CREATE PROCEDURE [dbo].[Order_GetPaged]
(
	@PageNumber INT,
	@PageSize INT,
	@SortBy NVARCHAR(50),
	@SortDirection NVARCHAR(4)
)
AS
BEGIN
	SET NOCOUNT ON;

	/*******************************************************************************************************/
	/*begin declaration section */
	DECLARE @Error INT,
			@ErrorMessage NVARCHAR(4000),
			@IsCustomError BIT = 0,
			@TodaysDate DATETIME,
			@Sql NVARCHAR(MAX)

	/*end declaration section */
	/*******************************************************************************************************/

	/*******************************************************************************************************/
	/*begin processing section */
	BEGIN TRY
		SELECT @TodaysDate = GETUTCDATE();

		-- Validate Sort Column (Whitelist to prevent SQL injection)
		IF @SortBy NOT IN ('OrderDate', 'TotalAmount')
			SET @SortBy = 'OrderDate';

		-- Validate Sort Direction
		IF @SortDirection NOT IN ('ASC', 'DESC')
			SET @SortDirection = 'DESC';

		-- Dynamic SQL with parameterized paging
		SET @Sql = '
			SELECT 
				o.Id, 
				o.CustomerId, 
				c.Name AS CustomerName, 
				o.OrderDate, 
				o.TotalAmount
			FROM Orders o
			JOIN Customers c ON o.CustomerId = c.Id
			ORDER BY ' + QUOTENAME(@SortBy) + ' ' + @SortDirection + '
			OFFSET (@PageNumber - 1) * @PageSize ROWS
			FETCH NEXT @PageSize ROWS ONLY;
		';

	/*end processing section */
	/*******************************************************************************************************/

	/*******************************************************************************************************/
	/*begin result section */
		EXEC sp_executesql
			@Sql,
			N'@PageNumber INT, @PageSize INT',
			@PageNumber, @PageSize;

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
			@ErrorNum = @Error,            -- INT
			@IsCustomError = @IsCustomError, -- BIT
			@ErrorMessage = @ErrorMessage    -- NVARCHAR

		RETURN;
	END CATCH
	/*end error handler section */
	/*******************************************************************************************************/
END
