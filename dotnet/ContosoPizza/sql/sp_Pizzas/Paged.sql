/*=============================================
Author:		-- Rushikesh Shelar
Create date: -- 2025-08-07
Description:	Fetches paged and sorted list of pizzas. Also returns total count of pizzas.
=============================================*/
CREATE PROCEDURE [dbo].[Pizza_GetPagedSorted]
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
			@Offset INT,
			@Sql NVARCHAR(MAX)

	/*end declaration section */
	/*******************************************************************************************************/

	/*******************************************************************************************************/
	/*begin processing section */
	BEGIN TRY
		SELECT @TodaysDate = GETUTCDATE();
		SET @Offset = (@PageNumber - 1) * @PageSize;

		-- Validate Sort Column (whitelist only valid fields)
		IF @SortBy NOT IN ('Name', 'Price', 'Id')
			SET @SortBy = 'Name';

		-- Validate Sort Direction
		IF @SortDirection NOT IN ('ASC', 'DESC')
			SET @SortDirection = 'ASC';

		-- Dynamic SQL for paging and sorting
		SET @Sql = '
			SELECT * FROM Pizzas
			ORDER BY ' + QUOTENAME(@SortBy) + ' ' + @SortDirection + '
			OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

			SELECT COUNT(*) AS TotalCount FROM Pizzas;
		';
	/*end processing section */
	/*******************************************************************************************************/

	/*******************************************************************************************************/
	/*begin result section */
		EXEC sp_executesql
			@Sql,
			N'@Offset INT, @PageSize INT',
			@Offset = @Offset,
			@PageSize = @PageSize;

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
