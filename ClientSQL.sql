USE [ClientsSQL]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 19-Oct-21 7:55:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[address] [nvarchar](100) NOT NULL,
	[postcode] [nvarchar](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionLog]    Script Date: 19-Oct-21 7:55:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionLog](
	[command] [nchar](6) NOT NULL,
	[oldname] [nvarchar](100) NULL,
	[oldaddress] [nvarchar](100) NULL,
	[oldpostcode] [nvarchar](7) NULL,
	[newname] [nvarchar](100) NULL,
	[newaddress] [nvarchar](100) NULL,
	[newpostcode] [nvarchar](7) NULL,
	[modified] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Trigger [dbo].[Transaction_trigger]    Script Date: 19-Oct-21 7:55:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[Transaction_trigger]
ON [dbo].[Client]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
	DECLARE @operation CHAR(6)
		SET @operation = CASE
				WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted)
					THEN 'Update'
				WHEN EXISTS(SELECT * FROM inserted)
					THEN 'Insert'
				WHEN EXISTS(SELECT * FROM deleted)
					THEN 'Delete'
				ELSE NULL
			END

		IF @operation = 'Delete'
			INSERT INTO TransactionLog (command, oldName, oldaddress, oldpostcode, modified)
			SELECT @operation, d.name, d.address, d.postcode, GETDATE()
			from deleted d

		IF @operation = 'Insert'
			INSERT INTO TransactionLog ( command, newname, newaddress, newpostcode, modified)
			SELECT @operation, i.name, i.address, i.postcode ,GETDATE()
			from inserted i

		IF @operation = 'Update'
			INSERT INTO TransactionLog ( command, oldname, oldaddress, oldpostcode, newname, newaddress, newpostcode, modified)
			SELECT @operation, d.name, d.address, d.postcode, i.name, i.address, i.postcode, GETDATE()
			from deleted d, inserted i

END
GO
ALTER TABLE [dbo].[Client] ENABLE TRIGGER [Transaction_trigger]
GO
