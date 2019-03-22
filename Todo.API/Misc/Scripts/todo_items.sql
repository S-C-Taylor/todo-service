CREATE TABLE [dbo].[TodoItems](
    [ItemID] [int] IDENTITY(1,1) NOT NULL,
    [Title] [nvarchar](max) NULL,
    [Description] [nvarchar](max) NULL,
    [Completed] [bit] NOT NULL,
    [CreatedBy] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED 
(
    [ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
 
GO