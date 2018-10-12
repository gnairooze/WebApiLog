
CREATE TABLE [dbo].[WebLog](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[RequestClientIP] [varchar](50) NOT NULL,
	[RequestHost] [varchar](50) NOT NULL,
	[RequestTime] [datetime] NOT NULL,
	[Scheme] [varchar](10) NULL,
	[Site] [nvarchar](100) NULL,
	[Path] [nvarchar](50) NULL,
	[Query] [nvarchar](500) NULL,
	[Uri] [nvarchar](4000) NOT NULL,
	[RequestMethod] [varchar](10) NOT NULL,
	[RequestHeaders] [nvarchar](max) NOT NULL CONSTRAINT [DF_WebLog_RequestHeaders]  DEFAULT (''),
	[RequestContent] [nvarchar](max) NOT NULL CONSTRAINT [DF_WebLog_RequestContent]  DEFAULT (''),
	[RequestVersion] [varchar](50) NOT NULL,
	[ResponseTime] [datetime] NOT NULL,
	[StatusCode] [varchar](20) NOT NULL,
	[ResponseHeaders] [nvarchar](max) NOT NULL CONSTRAINT [DF_WebLog_ResponseHeaders]  DEFAULT (''),
	[ResponseContent] [nvarchar](max) NOT NULL CONSTRAINT [DF_WebLog_ResponseContent]  DEFAULT (''),
	[ResponseVersion] [varchar](50) NOT NULL,
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_WebLog_CreatedOn]  DEFAULT (getdate()),
 CONSTRAINT [PK_WebLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [dbo].[WebLog_Query](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[WebLog_ID] [bigint] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Value] [varchar](50) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_WebLog_Query] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX [IX_WebLog_Method] ON [dbo].[WebLog]
(
	[RequestMethod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_WebLog_Path] ON [dbo].[WebLog]
(
	[Path] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_WebLog_ReqTime] ON [dbo].[WebLog]
(
	[RequestTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_WebLog_Query_Name] ON [dbo].[WebLog_Query]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_WebLog_Query_Value] ON [dbo].[WebLog_Query]
(
	[Value] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[WebLog_Query] ADD  CONSTRAINT [DF_WebLog_Query_Value]  DEFAULT ('') FOR [Value]
GO

ALTER TABLE [dbo].[WebLog_Query] ADD  CONSTRAINT [DF_WebLog_Query_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO

ALTER TABLE [dbo].[WebLog_Query]  WITH CHECK ADD  CONSTRAINT [FK_WebLog_Query_WebLog] FOREIGN KEY([WebLog_ID])
REFERENCES [dbo].[WebLog] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[WebLog_Query] CHECK CONSTRAINT [FK_WebLog_Query_WebLog]
GO

CREATE TYPE dbo.TextText AS TABLE 
(
	Name nvarchar(50) NULL,
	Value nvarchar(50) NULL
)
GO

Create Procedure dbo.WebLog_Add
@RequestContent nvarchar(max) = null,
@RequestHeaders nvarchar(max) = null,
@RequestMethod varchar(10),
@RequestTime DateTime,
@Scheme varchar(10),
@Site nvarchar(100),
@Path nvarchar(50),
@Query nvarchar(500),
@RequestClientIP varchar(50),
@RequestHost varchar(50),
@Uri nvarchar(4000),
@RequestVersion varchar(50),
@ResponseContent nvarchar(max) = null,
@ResponseHeaders nvarchar(max) = null,
@StatusCode varchar(20),
@ResponseTime DateTime,
@ResponseVersion varchar(50),
@Queries TextText readonly,
@ID bigint output
as
begin
	insert WebLog
	(
		RequestClientIP,
		RequestHost,
		RequestTime,
		Scheme,
		Site,
		Path,
		Query,
		Uri,
		RequestHeaders,
		RequestMethod,
		RequestContent,
		RequestVersion,
		ResponseTime,
		StatusCode,
		ResponseHeaders,
		ResponseContent,
		ResponseVersion
	)
	values
	(
		@RequestClientIP,
		@RequestHost,
		@RequestTime,
		@Scheme,
		@Site,
		@Path,
		@Query,
		@Uri,
		@RequestHeaders,
		@RequestMethod,
		@RequestContent,
		@RequestVersion,
		@ResponseTime,
		@StatusCode,
		@ResponseHeaders,
		@ResponseContent,
		@ResponseVersion
	)

	set @ID = SCOPE_IDENTITY()

	insert WebLog_Query
	(
		WebLog_ID,
		Name,
		Value
	)
	select
		@ID,
		Queries.Name,
		Queries.Value
	from @Queries Queries
end

GO
