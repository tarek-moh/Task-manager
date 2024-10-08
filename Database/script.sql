USE [Task Manager]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 10/3/2024 2:06:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[commentID] [int] IDENTITY(1,1) NOT NULL,
	[taskID] [int] NOT NULL,
	[commentText] [nvarchar](max) NOT NULL,
	[createdDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[commentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Department]    Script Date: 10/3/2024 2:06:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[departmentID] [int] IDENTITY(1,1) NOT NULL,
	[departmentName] [varchar](30) NOT NULL,
	[headID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[departmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 10/3/2024 2:06:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[employeeID] [int] IDENTITY(1,1) NOT NULL,
	[fName] [varchar](30) NOT NULL,
	[lName] [varchar](30) NOT NULL,
	[age] [int] NULL,
	[phone] [char](11) NULL,
	[supervisorID] [int] NULL,
	[isAdmin] [bit] NOT NULL,
	[email] [varchar](30) NOT NULL,
	[password] [varchar](100) NOT NULL,
	[departmentID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[employeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Task]    Script Date: 10/3/2024 2:06:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Task](
	[taskID] [int] IDENTITY(1,1) NOT NULL,
	[taskDescription] [varchar](8000) NULL,
	[DueDate] [date] NOT NULL,
	[taskStatus] [varchar](30) NOT NULL,
	[taskPriority] [int] NOT NULL,
	[employeeID] [int] NOT NULL,
	[taskName] [varchar](30) NOT NULL,
	[supervisorID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[taskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Employee] ADD  DEFAULT ((0)) FOR [isAdmin]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD FOREIGN KEY([taskID])
REFERENCES [dbo].[Task] ([taskID])
GO
ALTER TABLE [dbo].[Department]  WITH CHECK ADD  CONSTRAINT [FK_headID] FOREIGN KEY([headID])
REFERENCES [dbo].[Employee] ([employeeID])
GO
ALTER TABLE [dbo].[Department] CHECK CONSTRAINT [FK_headID]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_departmentID] FOREIGN KEY([departmentID])
REFERENCES [dbo].[Department] ([departmentID])
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_departmentID]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_employeeTask] FOREIGN KEY([employeeID])
REFERENCES [dbo].[Employee] ([employeeID])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_employeeTask]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD CHECK  (([age]>=(18)))
GO
ALTER TABLE [dbo].[Employee] 
ALTER COLUMN [departmentID] INT NULL;
GO
INSERT INTO Employee (fName, lName, email, password, isAdmin, departmentID)
VALUES ('Admin', 'User', 'admin@Task-manager.com', '$2a$11$ezXmGfOvhzkXK7yFH6YeTu82scb/0clAQnRamDC4xAdI5bEV479Sa', 1, NULL);
GO
