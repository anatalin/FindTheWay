USE [FindTheWayDb]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 15.06.2015 20:03:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Edges]    Script Date: 15.06.2015 20:03:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Edges](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[NodeFromId] [bigint] NOT NULL,
	[NodeToId] [bigint] NULL,
	[Length] [float] NOT NULL,
 CONSTRAINT [PK_dbo.Edges] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Nodes]    Script Date: 15.06.2015 20:03:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Nodes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
 CONSTRAINT [PK_dbo.Nodes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [IX_NodeFromId]    Script Date: 15.06.2015 20:03:05 ******/
CREATE NONCLUSTERED INDEX [IX_NodeFromId] ON [dbo].[Edges]
(
	[NodeFromId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_NodeToId]    Script Date: 15.06.2015 20:03:05 ******/
CREATE NONCLUSTERED INDEX [IX_NodeToId] ON [dbo].[Edges]
(
	[NodeToId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Edges]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Edges_dbo.Nodes_NodeFromId] FOREIGN KEY([NodeFromId])
REFERENCES [dbo].[Nodes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Edges] CHECK CONSTRAINT [FK_dbo.Edges_dbo.Nodes_NodeFromId]
GO
ALTER TABLE [dbo].[Edges]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Edges_dbo.Nodes_NodeToId] FOREIGN KEY([NodeToId])
REFERENCES [dbo].[Nodes] ([Id])
GO
ALTER TABLE [dbo].[Edges] CHECK CONSTRAINT [FK_dbo.Edges_dbo.Nodes_NodeToId]
GO
USE [master]
GO
ALTER DATABASE [FindTheWayDb] SET  READ_WRITE 
GO
