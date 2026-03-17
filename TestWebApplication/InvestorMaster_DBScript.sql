-- Create the Table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvestorMaster]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[InvestorMaster](
        [InvestorId] [int] IDENTITY(1,1) NOT NULL,
        [FullName] [nvarchar](150) NULL,
        [Address] [nvarchar](500) NULL,
        [District] [nvarchar](100) NULL,
        [MobileNumber] [nvarchar](15) NULL,
        [Email] [nvarchar](100) NULL,
        [LandArea] [decimal](18, 2) NULL,
        [LandUnit] [nvarchar](50) NULL,
        [IsActive] [bit] NOT NULL CONSTRAINT [DF_InvestorMaster_IsActive] DEFAULT ((1)),
        [CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_InvestorMaster_CreatedOn] DEFAULT (getdate()),
    CONSTRAINT [PK_InvestorMaster] PRIMARY KEY CLUSTERED 
    (
        [InvestorId] ASC
    )
    )
END
GO

-- Create the Stored Procedure
CREATE OR ALTER PROCEDURE [dbo].[sp_WebApp_InvestorMaster]
    @Action VARCHAR(50),
    @FullName NVARCHAR(150) = NULL,
    @Address NVARCHAR(500) = NULL,
    @District NVARCHAR(100) = NULL,
    @MobileNumber NVARCHAR(15) = NULL,
    @Email NVARCHAR(100) = NULL,
    @LandArea DECIMAL(18,2) = NULL,
    @LandUnit NVARCHAR(50) = NULL,
    @IsActive BIT = 1,
    @Msg INT = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF (@Action = 'INSERT')
        BEGIN
            INSERT INTO [dbo].[InvestorMaster] 
            (
                [FullName], 
                [Address], 
                [District],
                [MobileNumber], 
                [Email], 
                [LandArea], 
                [LandUnit], 
                [IsActive]
            )
            VALUES 
            (
                @FullName, 
                @Address, 
                @District,
                @MobileNumber, 
                @Email, 
                @LandArea, 
                @LandUnit, 
                @IsActive
            );

            SET @Msg = 1; -- Success
        END

        ELSE IF (@Action = 'GET_ALL')
        BEGIN
            SELECT 
                [InvestorId],
                [FullName],
                [Address],
                [District],
                [MobileNumber],
                [Email],
                [LandArea],
                [LandUnit],
                [IsActive]
            FROM 
                [dbo].[InvestorMaster]
            ORDER BY 
                [InvestorId] DESC;
        END

    END TRY
    BEGIN CATCH
        SET @Msg = 0; -- Failure
        
        -- You can also log the actual error here if needed using ERROR_MESSAGE()
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        PRINT @ErrorMessage;

    END CATCH
END
GO
