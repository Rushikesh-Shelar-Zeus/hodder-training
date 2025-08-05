/*=============================================
Author:        -- Rushikesh Shelar
Create date:   -- 2025-08-05
Description:   Handles and returns error information to the caller.
=============================================*/
CREATE PROCEDURE [dbo].[GetError]
    @ErrorNum INT,
    @IsCustomError BIT,
    @ErrorMessage NVARCHAR(4000)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        1 AS HasError,
        @IsCustomError AS IsCustomError,
        @ErrorNum AS ErrorCode,
        @ErrorMessage AS ErrorMessage;
END
GO
