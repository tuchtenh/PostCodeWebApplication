# PostCodeWebApplication

Database name: ClientsSQL

Database script: 

USE [ClientsSQL]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 10-Oct-21 3:40:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[address] [varchar](100) NOT NULL,
	[postcode] [varchar](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



JSON:

[
	{
		"Name":"UAB \"Gintarinė vaistinė\" fil. nr. 2",
		"Address":"Liepų al. 3-1B, Panevėžys",
		"PostCode":""
	},
	{
		"Name":"UAB \"Gintarinė vaistinė\" fil. nr. 20",
		"Address":"Nemuno g. 70, Panevėžys",
		"PostCode":""
	}
]


db and json also available as files: ClientsSQL.sql and Clients.json
