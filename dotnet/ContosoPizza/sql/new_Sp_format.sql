/*=============================================
Author:		
Create date: 
Description:	
=============================================*/
CREATE PROCEDURE [dbo].[StoredProcedureName]
(
	@Param1 UNIQUEIDENTIFIER = NULL
	, @Param2 INT
	, @Param3 INT
)
AS							
BEGIN
	SET NOCOUNT ON;
	/*******************************************************************************************************/		
	/*begin declaration section */	
	DECLARE	@Error INT
			, @ErrorMessage NVARCHAR(4000)
			, @IsCustomError BIT
			, @TodaysDate DATETIME
			, @Id UNIQUEIDENTIFIER
			, @PrimaryKey int
	
	
	DECLARE @TemporaryTable TABLE
	(
		Column1 NVARCHAR(400)
		, Column2 BIT DEFAULT(0)		
	)	
	/*end declaration section*/
	/*******************************************************************************************************/
	
	/*******************************************************************************************************/		
	/*begin processing section */			
	BEGIN TRY
	
		SELECT @TodaysDate = GETUTCDATE()
		
		INSERT INTO @TemporaryTable
			(Column1)
		SELECT SomeColumn
		FROM SomeTable
		WHERE SomeOtherColumn = 'SomeValue'
			
				
		IF EXISTS(SELECT Column1 FROM @Table1 WHERE Column1 = 1) 
		BEGIN			
			SELECT @IsCustomError = 1, @Error = 132, @ErrorMessage = 'Some message'		
			GOTO ErrorHandler 
		END	
	
		/*begin transaction section*/
		/****************************************/
		BEGIN TRANSACTION
		
			UPDATE B SET
				Column1 = 1
				, Column2 = A.Column1
			FROM Table1 A
			INNER JOIN Table2 B
				ON  A.Column3 = B.Column3
				AND A.Column4 <> ISNULL(B.Column4, -1)
			INNER JOIN Table3 C
				ON B.Column5 = C.Column1
				AND C.Column2 = 0	
				AND C.Column3 NOT IN ('X','Y','Z')
			
			IF @Param1 IS NULL
			BEGIN
			
				SELECT @Id = NEWID()
				
				INSERT INTO Table1
					(Column1, Column2
						, Column3, Column4)     
				VALUES
					(@Param1, @Param2
						, @Param3, @Id)    

				SELECT @PrimaryKey = SCOPE_IDENTITY() 			
							
			END
			ELSE
			BEGIN
			
				UPDATE A SET
					Column2 = Param2
					, Column3 = Param3
				FROM Table1
				WHERE Column1 = Param1			
			end
									
		COMMIT TRAN
		/*end transaction section*/
	/****************************************/
			
	/*end processing section */
	/*******************************************************************************************************/		
	
	/*******************************************************************************************************/		
	/*begin result section */
		/*1)*/
		SELECT 0 AS 'success', @Id AS 'Id'
			
		/*2)*/
		SELECT Column1, Column2, Column1 + Column2 AS 'SomeAlias'

	
 		RETURN
	/*end result section */
	/*******************************************************************************************************/	
	/*******************************************************************************************************/		
	/*begin error handler section */		  
	END TRY  
	BEGIN CATCH  
	  SELECT @error = @@ERROR, @errorMessage = ERROR_MESSAGE()  
	  GOTO ErrorHandler  
	END CATCH   
	ErrorHandler:
		EXEC [GetError]
			@ErrorNum = @Error, /*INT*/
			@IsCustomError = @IsCustomError, /*BIT*/
			@ErrorMessage = @ErrorMessage			
		
		IF (@@TRANCOUNT > 0)
			ROLLBACK TRANSACTION
			
		RETURN
	/*end error handler section */
	/*******************************************************************************************************/			
END


/*
EXEC StoredProcedureName
	@Param1 = null
	,@Param1 = 'SoemValue'
	,@Param2 = 'SomeValue
*/