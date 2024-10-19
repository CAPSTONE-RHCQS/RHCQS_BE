USE [master]
GO
/****** Object:  Database [RHCQS]    Script Date: 10/19/2024 11:46:29 AM ******/
CREATE DATABASE [RHCQS]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RHCQS', FILENAME = N'/var/opt/mssql/data/RHCQS.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RHCQS_log', FILENAME = N'/var/opt/mssql/data/RHCQS_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [RHCQS] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RHCQS].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RHCQS] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RHCQS] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RHCQS] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RHCQS] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RHCQS] SET ARITHABORT OFF 
GO
ALTER DATABASE [RHCQS] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [RHCQS] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RHCQS] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RHCQS] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RHCQS] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RHCQS] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RHCQS] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RHCQS] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RHCQS] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RHCQS] SET  ENABLE_BROKER 
GO
ALTER DATABASE [RHCQS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RHCQS] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RHCQS] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RHCQS] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RHCQS] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RHCQS] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RHCQS] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RHCQS] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [RHCQS] SET  MULTI_USER 
GO
ALTER DATABASE [RHCQS] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RHCQS] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RHCQS] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RHCQS] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RHCQS] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RHCQS] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'RHCQS', N'ON'
GO
ALTER DATABASE [RHCQS] SET QUERY_STORE = OFF
GO
USE [RHCQS]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 10/19/2024 11:46:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[Id] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](50) NULL,
	[Username] [nvarchar](50) NULL,
	[ImageUrl] [nvarchar](max) NULL,
	[PasswordHash] [nvarchar](60) NULL,
	[PhoneNumber] [nchar](10) NULL,
	[DateOfBirth] [date] NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[Deflag] [bit] NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AssignTask]    Script Date: 10/19/2024 11:46:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssignTask](
	[Id] [uniqueidentifier] NOT NULL,
	[AccountId] [uniqueidentifier] NOT NULL,
	[ProjectId] [uniqueidentifier] NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_AssignTask] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BatchPayment]    Script Date: 10/19/2024 11:46:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BatchPayment](
	[Id] [uniqueidentifier] NOT NULL,
	[ContractId] [uniqueidentifier] NULL,
	[Price] [float] NULL,
	[PaymentDate] [datetime] NULL,
	[PaymentPhase] [datetime] NULL,
	[IntitialQuotationId] [uniqueidentifier] NOT NULL,
	[Percents] [nvarchar](5) NULL,
	[InsDate] [datetime] NULL,
	[FinalQuotationId] [uniqueidentifier] NULL,
	[Description] [nvarchar](200) NULL,
	[Unit] [nvarchar](10) NULL,
 CONSTRAINT [PK_InstallmentPayment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Blog]    Script Date: 10/19/2024 11:46:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Blog](
	[Id] [uniqueidentifier] NOT NULL,
	[AccountId] [uniqueidentifier] NOT NULL,
	[Heading] [nvarchar](max) NULL,
	[SubHeading] [nvarchar](max) NULL,
	[Context] [nvarchar](max) NULL,
	[ImgUrl] [nvarchar](max) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
 CONSTRAINT [PK_Blog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConstructionItems]    Script Date: 10/19/2024 11:46:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConstructionItems](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Coefficient] [float] NULL,
	[Unit] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[Type] [nvarchar](50) NULL,
	[IsFinalQuotation] [bit] NULL,
 CONSTRAINT [PK_ConstructionItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contract]    Script Date: 10/19/2024 11:46:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contract](
	[Id] [uniqueidentifier] NOT NULL,
	[ProjectId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[CustomerName] [nvarchar](50) NULL,
	[ContractCode] [nchar](10) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[ValidityPeriod] [int] NULL,
	[TaxCode] [nvarchar](20) NULL,
	[Area] [float] NULL,
	[UnitPrice] [nvarchar](50) NULL,
	[ContractValue] [float] NULL,
	[UrlFile] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[Deflag] [bit] NULL,
	[RoughPackagePrice] [float] NULL,
	[FinishedPackagePrice] [float] NULL,
	[Status] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Contract] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 10/19/2024 11:46:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](50) NULL,
	[Username] [nvarchar](50) NULL,
	[ImgUrl] [nvarchar](max) NULL,
	[PhoneNumber] [nchar](11) NULL,
	[DateOfBirth] [datetime] NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[Deflag] [bit] NULL,
	[AccountId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DesignTemplate]    Script Date: 10/19/2024 11:46:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DesignTemplate](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[NumberOfFloor] [int] NULL,
	[NumberOfBed] [int] NULL,
	[NumberOfFront] [int] NULL,
	[ImgUrl] [nvarchar](max) NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_DesignTemplate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EquipmentItem]    Script Date: 10/19/2024 11:46:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EquipmentItem](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Unit] [nvarchar](50) NULL,
	[Quantity] [int] NULL,
	[UnitOfMaterial] [float] NULL,
	[TotalOfMaterial] [float] NULL,
	[Note] [nchar](10) NULL,
	[FinalQuotationId] [uniqueidentifier] NOT NULL,
	[Type] [nvarchar](100) NULL,
 CONSTRAINT [PK_EquimentItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinalQuotation]    Script Date: 10/19/2024 11:46:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinalQuotation](
	[Id] [uniqueidentifier] NOT NULL,
	[ProjectId] [uniqueidentifier] NOT NULL,
	[PromotionId] [uniqueidentifier] NULL,
	[TotalPrice] [float] NULL,
	[Note] [nvarchar](max) NULL,
	[Version] [float] NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[Status] [nvarchar](50) NULL,
	[Deflag] [bit] NULL,
	[QuotationUtilitiesId] [uniqueidentifier] NULL,
	[ReasonReject] [nvarchar](max) NULL,
 CONSTRAINT [PK_OfficialQuotation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinalQuotationItem]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinalQuotationItem](
	[Id] [uniqueidentifier] NOT NULL,
	[FinalQuotationId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Unit] [nvarchar](50) NULL,
	[Weight] [nvarchar](50) NULL,
	[UnitPriceLabor] [float] NULL,
	[UnitPriceRough] [float] NULL,
	[UnitPriceFinished] [float] NULL,
	[TotalPriceLabor] [float] NULL,
	[TotalPriceRough] [float] NULL,
	[TotalPriceFinished] [float] NULL,
	[InsDate] [datetime] NULL,
	[ConstructionItemId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_DetailedQuotationItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HouseDesignDrawing]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HouseDesignDrawing](
	[Id] [uniqueidentifier] NOT NULL,
	[ProjectId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Step] [int] NULL,
	[Status] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[IsCompany] [bit] NULL,
	[InsDate] [datetime] NULL,
	[AccountId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_HouseDesignDrawing] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HouseDesignVersion]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HouseDesignVersion](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Version] [float] NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[InsDate] [datetime] NULL,
	[HouseDesignDrawingId] [uniqueidentifier] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[UpsDate] [datetime] NULL,
	[RelatedDrawingId] [uniqueidentifier] NULL,
	[PreviousDrawingId] [uniqueidentifier] NULL,
	[Reason] [nvarchar](max) NULL,
	[Deflag] [bit] NOT NULL,
 CONSTRAINT [PK_HouseDesignVersion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InitialQuotation]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InitialQuotation](
	[Id] [uniqueidentifier] NOT NULL,
	[ProjectId] [uniqueidentifier] NOT NULL,
	[PromotionId] [uniqueidentifier] NULL,
	[Area] [float] NULL,
	[TimeProcessing] [int] NULL,
	[TimeRough] [int] NULL,
	[TimeOthers] [int] NULL,
	[OthersAgreement] [nvarchar](max) NULL,
	[InsDate] [datetime] NULL,
	[Status] [nvarchar](50) NULL,
	[Version] [float] NOT NULL,
	[IsTemplate] [bit] NULL,
	[Deflag] [bit] NULL,
	[Note] [nvarchar](max) NULL,
	[TotalRough] [float] NULL,
	[TotalUtilities] [float] NULL,
	[Unit] [nvarchar](5) NULL,
	[ReasonReject] [nvarchar](max) NULL,
 CONSTRAINT [PK_PreliminaryQuotation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InitialQuotationItem]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InitialQuotationItem](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[ConstructionItemId] [uniqueidentifier] NOT NULL,
	[SubConstructionId] [uniqueidentifier] NULL,
	[Area] [float] NULL,
	[Price] [float] NULL,
	[UnitPrice] [nvarchar](10) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[InitialQuotationId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_InitialQuotationItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Labor]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Labor](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Price] [float] NULL,
	[Quantity] [int] NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[Deflag] [bit] NULL,
	[Type] [varchar](50) NULL,
 CONSTRAINT [PK_Labor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Material]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Material](
	[Id] [uniqueidentifier] NOT NULL,
	[SupplierId] [uniqueidentifier] NOT NULL,
	[MaterialTypeId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[InventoryQuantity] [int] NULL,
	[Price] [float] NULL,
	[Unit] [nvarchar](50) NULL,
	[Size] [nvarchar](50) NULL,
	[Shape] [nvarchar](50) NULL,
	[ImgUrl] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[UnitPrice] [nvarchar](50) NULL,
	[IsAvailable] [bit] NULL,
	[MaterialSectionId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Material] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaterialSection]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MaterialSection](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_MaterialSection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaterialType]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MaterialType](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[Deflag] [bit] NULL,
 CONSTRAINT [PK_MaterialType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Media]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Media](
	[Id] [uniqueidentifier] NOT NULL,
	[HouseDesignVersionId] [uniqueidentifier] NULL,
	[Name] [nvarchar](100) NULL,
	[Url] [nvarchar](max) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[SubTemplateId] [uniqueidentifier] NULL,
	[PaymentId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Media] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Message]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Message](
	[Id] [uniqueidentifier] NOT NULL,
	[Sender] [nvarchar](50) NULL,
	[Reciver] [nvarchar](50) NULL,
	[Context] [nvarchar](max) NULL,
	[Time] [datetime] NULL,
 CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Package]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Package](
	[Id] [uniqueidentifier] NOT NULL,
	[PackageTypeId] [uniqueidentifier] NOT NULL,
	[PackageName] [nvarchar](100) NULL,
	[Unit] [nvarchar](20) NULL,
	[Price] [float] NULL,
	[Status] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
 CONSTRAINT [PK_Package] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PackageDetail]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageDetail](
	[Id] [uniqueidentifier] NOT NULL,
	[PackageId] [uniqueidentifier] NOT NULL,
	[Action] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_PackageDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PackageHouse]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageHouse](
	[Id] [uniqueidentifier] NOT NULL,
	[PackageId] [uniqueidentifier] NOT NULL,
	[DesignTemplateId] [uniqueidentifier] NOT NULL,
	[ImgUrl] [nvarchar](max) NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_PackageHouse] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PackageLabor]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageLabor](
	[Id] [uniqueidentifier] NOT NULL,
	[PackageDetailId] [uniqueidentifier] NOT NULL,
	[LaborId] [uniqueidentifier] NOT NULL,
	[Price] [float] NULL,
	[Quantity] [int] NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_PackageLabor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PackageMaterial]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageMaterial](
	[Id] [uniqueidentifier] NOT NULL,
	[MaterialSectionId] [uniqueidentifier] NOT NULL,
	[PackageDetailId] [uniqueidentifier] NOT NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_PackageMaterial] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PackageQuotation]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageQuotation](
	[Id] [uniqueidentifier] NOT NULL,
	[PackageId] [uniqueidentifier] NOT NULL,
	[InitialQuotationId] [uniqueidentifier] NOT NULL,
	[Type] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_PackageQuotation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PackageType]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageType](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_PackageType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[Id] [uniqueidentifier] NOT NULL,
	[PaymentTypeId] [uniqueidentifier] NOT NULL,
	[BatchPaymentId] [uniqueidentifier] NOT NULL,
	[Status] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[TotalPrice] [float] NULL,
 CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentType]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentType](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_PaymentType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Project]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[Id] [uniqueidentifier] NOT NULL,
	[CustomerId] [uniqueidentifier] NULL,
	[Name] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[ProjectCode] [nchar](5) NULL,
	[Address] [nvarchar](max) NULL,
	[Area] [float] NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Promotion]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Promotion](
	[Id] [uniqueidentifier] NOT NULL,
	[Code] [nchar](10) NULL,
	[Value] [int] NULL,
	[InsDate] [datetime] NULL,
	[StartTime] [datetime] NULL,
	[Name] [nvarchar](200) NULL,
	[ExpTime] [datetime] NULL,
	[IsRunning] [bit] NULL,
 CONSTRAINT [PK_Promotion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuotationItem]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuotationItem](
	[Id] [uniqueidentifier] NOT NULL,
	[QuotationSectionId] [uniqueidentifier] NOT NULL,
	[ProjectComponent] [nvarchar](100) NULL,
	[Area] [float] NULL,
	[Unit] [nvarchar](50) NULL,
	[Coefficient] [float] NULL,
	[Weight] [float] NULL,
	[LaborCost] [float] NULL,
	[MaterialPrice] [float] NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
 CONSTRAINT [PK_QuotationItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuotationLabor]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuotationLabor](
	[Id] [uniqueidentifier] NOT NULL,
	[QuotationItemId] [uniqueidentifier] NOT NULL,
	[LaborId] [uniqueidentifier] NOT NULL,
	[LaborPrice] [float] NULL,
	[Quantity] [int] NULL,
 CONSTRAINT [PK_QuotationLabor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuotationMaterial]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuotationMaterial](
	[Id] [uniqueidentifier] NOT NULL,
	[QuotationItemId] [uniqueidentifier] NOT NULL,
	[MaterialId] [uniqueidentifier] NOT NULL,
	[Unit] [nvarchar](50) NULL,
	[MaterialPrice] [float] NULL,
	[Quantity] [int] NULL,
 CONSTRAINT [PK_QuotationMaterial] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuotationSection]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuotationSection](
	[Id] [uniqueidentifier] NOT NULL,
	[PackageId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[Status] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[Note] [nvarchar](max) NULL,
	[ContractionTime] [int] NULL,
 CONSTRAINT [PK_QuotationSection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuotationUtilities]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuotationUtilities](
	[Id] [uniqueidentifier] NOT NULL,
	[UtilitiesItemId] [uniqueidentifier] NOT NULL,
	[FinalQuotationId] [uniqueidentifier] NULL,
	[InitialQuotationId] [uniqueidentifier] NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Coefiicient] [float] NULL,
	[Price] [float] NULL,
	[Description] [nvarchar](max) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
 CONSTRAINT [PK_UltilitiesDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](50) NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubConstructionItems]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubConstructionItems](
	[Id] [uniqueidentifier] NOT NULL,
	[ConstructionItemsId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Coefficient] [float] NULL,
	[Unit] [nchar](10) NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_RoughCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubTemplate]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubTemplate](
	[Id] [uniqueidentifier] NOT NULL,
	[DesignTemplateId] [uniqueidentifier] NOT NULL,
	[BuildingArea] [float] NULL,
	[FloorArea] [float] NULL,
	[InsDate] [datetime] NULL,
	[Size] [nchar](9) NULL,
 CONSTRAINT [PK_SubTemplate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Supplier]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Supplier](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[ConstractPhone] [nvarchar](11) NULL,
	[ImgUrl] [nvarchar](max) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[Deflag] [bit] NULL,
	[ShortDescription] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TemplateItem]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TemplateItem](
	[Id] [uniqueidentifier] NOT NULL,
	[SubTemplateId] [uniqueidentifier] NOT NULL,
	[ConstructionItemId] [uniqueidentifier] NOT NULL,
	[Area] [float] NULL,
	[Unit] [nvarchar](20) NULL,
	[InsDate] [datetime] NULL,
	[SubConstructionId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_TemplateItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UtilitiesItem]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UtilitiesItem](
	[Id] [uniqueidentifier] NOT NULL,
	[SectionId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Coefficient] [float] NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[Deflag] [bit] NULL,
 CONSTRAINT [PK_UltilitiesItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UtilitiesSection]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UtilitiesSection](
	[Id] [uniqueidentifier] NOT NULL,
	[UtilitiesId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Deflag] [bit] NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[UnitPrice] [float] NULL,
	[Unit] [nvarchar](100) NULL,
 CONSTRAINT [PK_UltilitiesSection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UtilityOption]    Script Date: 10/19/2024 11:46:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UtilityOption](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[Deflag] [bit] NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
 CONSTRAINT [PK_Ultilities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'782e4916-ddd5-4f6d-a03f-1d599930fff4', N'9959ce96-de26-40a7-b8a7-28a704062e89', N'sales2rhcqs@gmail.com', N'Sales ', NULL, N'$2a$12$M2caIrBBbc9ezeCTg03MrOtVoqjfiG/RESa5Iznl0awUIqM5WGnGW', N'0294529302', NULL, NULL, NULL, 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'ff17689a-46f8-42c9-8ad8-26ec34941047', N'9959ce96-de26-40a7-b8a7-28a704062e89', N'sales3rhcqs@gmail.com', N'Sales', N'https://hoanghamobile.com/tin-tuc/wp-content/uploads/2024/04/anh-meo-ngau-41.jpg', N'$2a$12$IeUUp.auEEy2gX4elWxtqOytSmY3kXWapW8WJuWjkgpFEN0lO6dPm', N'0943577123', CAST(N'2002-01-02' AS Date), NULL, CAST(N'2024-10-11T14:21:38.353' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'990773a2-1817-47f5-9116-301e97435c44', N'7af0d75e-1157-48b4-899d-3196deed5fad', N'design2rhcqs@gmail.com', N'Huy', N'https://cellphones.com.vn/sforum/wp-content/uploads/2024/02/avatar-anh-meo-cute-5.jpg', N'$2a$12$M2caIrBBbc9ezeCTg03MrOtVoqjfiG/RESa5Iznl0awUIqM5WGnGW', N'0902579391', CAST(N'2002-01-01' AS Date), CAST(N'2024-09-26T14:30:00.000' AS DateTime), CAST(N'2024-10-08T18:46:43.580' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'bf339e88-5303-45c4-a6f4-33a79681766c', N'7af0d75e-1157-48b4-899d-3196deed5fad', N'design1rhcqs@gmail.com', N'design1rhcqs@gmail.com', N'https://inkythuatso.com/uploads/thumbnails/800/2022/05/anh-meo-che-anh-meo-bua-15-31-09-19-00.jpg', N'$2a$12$xhBTbiXSxiXB51CTBFnOPOQM1Co2VOIu9n2mIZabxslQfDCDIw496', N'0586617799', CAST(N'2002-01-02' AS Date), CAST(N'2024-09-29T06:02:39.183' AS DateTime), CAST(N'2024-10-08T18:38:34.683' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'0be1eb2e-f31d-476c-800f-3a6e67ee2b08', N'9959ce96-de26-40a7-b8a7-28a704062e89', N'test', N'test', N'https://sieupet.com/sites/default/files/hinh_anh_meo_dep.jpg', N'$2a$12$ykiFtNtHI4JM7CZtpQRG3u5pGpXzDnjBRcY36sRvUzKd6FL0aTvLS', N'0828253777', NULL, CAST(N'2024-10-02T16:31:39.813' AS DateTime), CAST(N'2024-10-02T16:31:39.813' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'38f16ae9-cf92-4056-929f-51c4ef55804e', N'a3bb42ca-de7c-4c9f-8f58-d8175f96688c', N'rhcqsmanager@gmail.com', N'rhcqsmanager@gmail.com', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSxzT5167I50_3KwBagkh8DPQmmEEj0ec0ENA&s', N'$2a$12$WlgX28ZQwtocSDXwKumDqONEpzg7MSeNB1rw5KIWlSjiTT3N7jU66', N'0921659791', NULL, CAST(N'2024-09-29T05:54:32.677' AS DateTime), CAST(N'2024-10-08T18:43:29.880' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'7e9055c4-fa2e-45b4-b3e8-580870184283', N'789dd57d-0f75-40d1-8366-ef6ab582efc8', N'ngandolh@gmail.com', N'Ngân ', N'https://chungkhoantaichinh.vn/wp-content/uploads/2022/12/avatar-meo-cute-de-thuong-05.jpg', N'$2a$12$UUBeF1uXFz9fDVrTFnGFy.Ea7TV2MiRR9BR3DzIxrIXUOlUo3lqpO', N'0906697051', CAST(N'2002-01-01' AS Date), CAST(N'2024-09-26T14:30:00.000' AS DateTime), CAST(N'2024-09-26T14:30:00.000' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'b8c040b3-09a3-4975-a962-7440175b15aa', N'789dd57d-0f75-40d1-8366-ef6ab582efc8', N'mrtuan1456@', N'mrtuan1456@', N'https://hoanghamobile.com/tin-tuc/wp-content/uploads/2024/04/anh-meo-ngau-1.jpg', N'$2a$12$MXNuxZyaFg.ZjonAfHK3devdOkNJSi6lk1CEg1OgZDyc8jV40d3aW', N'0589759666', NULL, CAST(N'2024-10-02T16:32:18.907' AS DateTime), CAST(N'2024-10-02T16:32:18.907' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'08b10ff5-e37d-40bf-947c-80cbf78fa411', N'789dd57d-0f75-40d1-8366-ef6ab582efc8', N'test1@gmail.com', N'test1@gmail.com', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR_rtTqcFlN3O0TRGN_RkXL7pPqcIUBlPlUyQ&s', N'$2a$12$/ZJbb1v3DNQlJAWfxVnzfeooOPTgaciK1vCKlPS2h7RMvIFdV1dJO', N'0869337777', NULL, CAST(N'2024-10-03T07:29:08.160' AS DateTime), CAST(N'2024-10-03T07:29:08.160' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'de455c55-1d0a-43e3-9581-842a127b0d65', N'789dd57d-0f75-40d1-8366-ef6ab582efc8', N'tiennv@gmail.com', N'Tiến', N'https://mekoong.com/wp-content/uploads/2022/10/close-up-of-cat-wearing-sunglasses-while-sitting-royalty-free-image-1571755145-1024x683.jpg', N'$2a$12$UUBeF1uXFz9fDVrTFnGFy.Ea7TV2MiRR9BR3DzIxrIXUOlUo3lqpO', N'0385940273', NULL, CAST(N'2024-09-27T15:36:51.430' AS DateTime), CAST(N'2024-09-28T07:56:47.180' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'd127d3a6-6f2b-402f-bd60-90108623651f', N'9959ce96-de26-40a7-b8a7-28a704062e89', N'thien', N'Thiện', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTdph6y3dB-1RsMjk1fs5Eec4bHt2IFmRUHxw&s', N'$2a$12$3lnj3fZay46MeKa7nrdd3Ot4r0bJ/CHa7RnF7PIfCeop/f9sGEAni', N'0846002277', NULL, CAST(N'2024-09-29T05:16:24.890' AS DateTime), CAST(N'2024-10-02T11:36:06.830' AS DateTime), 0)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'84412c77-ecd0-4d08-ada8-90d4623cd5c0', N'789dd57d-0f75-40d1-8366-ef6ab582efc8', N'mrtuan1456', N'mrtuan1456', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQyfbE1OCi1qUfvlx_HM0mRCPyIUKguNiZE_w&s', N'$2a$12$7z81XBUP9QumAoXzRx8y6uyrLMvtH5xNNs8NWBdsejN6XoYfjryaS', N'0843020000', NULL, CAST(N'2024-10-02T15:39:32.203' AS DateTime), CAST(N'2024-10-02T15:39:32.203' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'45bced7f-0432-40dd-9686-91f8cc1c90dc', N'7af0d75e-1157-48b4-899d-3196deed5fad', N'design4rhcqs@gmail.com', N'design4rhcqs@gmail.com', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQb-ill7bz46Keq7bA7xpJsQrpAttqtgUQQhQ&s', N'$2a$12$nhXfB/DdiYO9uDUVA71KueHWDCA9ummGU6OcvU/ipkV9RYbvmZRHS', N'0704894555', NULL, CAST(N'2024-09-30T15:57:40.950' AS DateTime), CAST(N'2024-09-30T15:57:40.950' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'8caeb11a-1599-40c9-bdfc-a184acd7700d', N'789dd57d-0f75-40d1-8366-ef6ab582efc8', N'testDangKi', N'testDangKi', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTTacA-IBHeRlTou6YG_FmfLGAKcsol22P_7Q&s', N'$2a$12$SLZCNIwfGv1QgwCJtJt97.LyGN9ylVAz15l2waFu.cbP1DrVLVov6', N'0906993705', NULL, CAST(N'2024-10-02T16:28:07.200' AS DateTime), CAST(N'2024-10-02T16:28:07.200' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'd63a2a80-cdea-46df-8419-e5c70a7632ea', N'9959ce96-de26-40a7-b8a7-28a704062e89', N'sales@gmail.com', N'sales1rhcqs@gmail.com', NULL, N'$2a$12$cDiICnJKZHnuHi3W56CU7eKuMeKVNKVC3n1Vv1QF6c6NHN7Q8ylMy', N'0902981122', NULL, CAST(N'2024-09-30T15:57:22.750' AS DateTime), CAST(N'2024-09-30T15:57:22.750' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'd63a2a80-cdea-46df-8419-e5c70a7632ee', N'9959ce96-de26-40a7-b8a7-28a704062e89', N'sales1rhcqs@gmail.com', N'sales1rhcqs@gmail.com', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQDYCsPDKzMOClp7WmGj8fmonRjhE5I8u56Ng&s', N'$2a$12$tE5j8T5as5235dBQXynZpOBppleiOCdiqp4kZLAQdvS/P4IUANxN6', N'0931074111', NULL, CAST(N'2024-09-29T05:55:27.280' AS DateTime), CAST(N'2024-09-29T05:55:27.280' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'28247cd1-67ca-439d-bef5-fca9a9a777c5', N'7af0d75e-1157-48b4-899d-3196deed5fad', N'design3rhcqs@gmail.com', N'design3rhcqs@gmail.com', N'https://dogstar.vn/wp-content/uploads/2022/05/anh-meo-de-thuong-iu-qua-di.jpg', N'$2a$12$cDiICnJKZHnuHi3W56CU7eKuMeKVNKVC3n1Vv1QF6c6NHN7Q8ylMy', N'0764693535', NULL, CAST(N'2024-09-30T15:57:22.750' AS DateTime), CAST(N'2024-09-30T15:57:22.750' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'28247cd1-67ca-439d-bef5-fca9a9a777c6', N'7af0d75e-1157-48b4-899d-3196deed5fad', N'design4rhcqs@gmail.com', N'design4rhcqs@gmail.com', N'https://dogstar.vn/wp-content/uploads/2022/05/anh-meo-de-thuong-iu-qua-di.jpg', N'$2a$12$cDiICnJKZHnuHi3W56CU7eKuMeKVNKVC3n1Vv1QF6c6NHN7Q8ylMy', N'0902981306', NULL, CAST(N'2024-09-30T15:57:22.750' AS DateTime), CAST(N'2024-09-30T15:57:22.750' AS DateTime), 1)
GO
INSERT [dbo].[AssignTask] ([Id], [AccountId], [ProjectId], [InsDate]) VALUES (N'9f333306-1340-48ef-a58d-17931176991b', N'd63a2a80-cdea-46df-8419-e5c70a7632ee', N'fa91e187-b0c8-46c3-b6d5-747d32bd3540', CAST(N'2024-10-16T04:28:00.223' AS DateTime))
INSERT [dbo].[AssignTask] ([Id], [AccountId], [ProjectId], [InsDate]) VALUES (N'043a5a85-558b-4006-94a9-43da1e311b66', N'782e4916-ddd5-4f6d-a03f-1d599930fff4', N'799b9201-d234-47b9-a14f-7574cc84ef74', CAST(N'2024-09-26T14:30:00.000' AS DateTime))
INSERT [dbo].[AssignTask] ([Id], [AccountId], [ProjectId], [InsDate]) VALUES (N'9df4ad5b-2f93-4f58-8406-85e1c1f3ebb3', N'ff17689a-46f8-42c9-8ad8-26ec34941047', N'b841c593-1d05-4492-9ba2-d485f6d67260', CAST(N'2024-09-11T09:15:00.000' AS DateTime))
INSERT [dbo].[AssignTask] ([Id], [AccountId], [ProjectId], [InsDate]) VALUES (N'c37f5163-f7c2-4565-97be-d92649871795', N'd63a2a80-cdea-46df-8419-e5c70a7632ee', N'b81935a8-4482-43f5-ad68-558abde58d58', CAST(N'2024-10-04T12:40:31.807' AS DateTime))
GO
INSERT [dbo].[BatchPayment] ([Id], [ContractId], [Price], [PaymentDate], [PaymentPhase], [IntitialQuotationId], [Percents], [InsDate], [FinalQuotationId], [Description], [Unit]) VALUES (N'3863f126-0ae5-47ce-84b3-238dfaae9ce9', NULL, 168525000, CAST(N'2025-02-25T00:00:00.000' AS DateTime), CAST(N'2025-03-02T00:00:00.000' AS DateTime), N'4822b76f-5565-4a1a-99c0-370e8d067359', N'15', CAST(N'2024-10-10T05:19:27.217' AS DateTime), N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'Thanh toán lần 5: Sau khi hoàn thành phần hoàn thiện', N'VNĐ')
INSERT [dbo].[BatchPayment] ([Id], [ContractId], [Price], [PaymentDate], [PaymentPhase], [IntitialQuotationId], [Percents], [InsDate], [FinalQuotationId], [Description], [Unit]) VALUES (N'45a01908-2c07-49d2-9822-26657886cdb5', NULL, 168525000, CAST(N'2024-11-15T00:00:00.000' AS DateTime), CAST(N'2024-11-22T00:00:00.000' AS DateTime), N'4822b76f-5565-4a1a-99c0-370e8d067359', N'15', CAST(N'2024-10-10T05:19:27.217' AS DateTime), N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'Thanh toán lần 2: Sau khi hoàn thành phần móng', N'VNĐ')
INSERT [dbo].[BatchPayment] ([Id], [ContractId], [Price], [PaymentDate], [PaymentPhase], [IntitialQuotationId], [Percents], [InsDate], [FinalQuotationId], [Description], [Unit]) VALUES (N'966c20b1-f662-4afc-a6ae-3e003cc8dceb', NULL, 555000000, NULL, NULL, N'17d0fc6b-51a8-4d4d-aeaf-37b9a14b9db4', N'37', CAST(N'2024-10-08T13:20:47.537' AS DateTime), NULL, N'Hoàn thiện bàn giao', N'VNĐ')
INSERT [dbo].[BatchPayment] ([Id], [ContractId], [Price], [PaymentDate], [PaymentPhase], [IntitialQuotationId], [Percents], [InsDate], [FinalQuotationId], [Description], [Unit]) VALUES (N'b8e89208-6c8a-4033-ba22-49fdcbc00982', NULL, 225000000, NULL, NULL, N'17d0fc6b-51a8-4d4d-aeaf-37b9a14b9db4', N'15', CAST(N'2024-10-08T13:20:47.537' AS DateTime), NULL, N'Đổ móng + Đà kiểng', N'VNĐ')
INSERT [dbo].[BatchPayment] ([Id], [ContractId], [Price], [PaymentDate], [PaymentPhase], [IntitialQuotationId], [Percents], [InsDate], [FinalQuotationId], [Description], [Unit]) VALUES (N'902e3868-07c0-4cf3-b152-4be8fa04f257', NULL, 165000000, NULL, NULL, N'17d0fc6b-51a8-4d4d-aeaf-37b9a14b9db4', N'11', CAST(N'2024-10-08T13:20:47.537' AS DateTime), NULL, N'Đổ xong sàn mái', N'VNĐ')
INSERT [dbo].[BatchPayment] ([Id], [ContractId], [Price], [PaymentDate], [PaymentPhase], [IntitialQuotationId], [Percents], [InsDate], [FinalQuotationId], [Description], [Unit]) VALUES (N'd1bc534b-8bcd-46bd-8d96-54d185161926', NULL, 224700000, CAST(N'2024-10-15T00:00:00.000' AS DateTime), CAST(N'2024-11-01T00:00:00.000' AS DateTime), N'4822b76f-5565-4a1a-99c0-370e8d067359', N'20', CAST(N'2024-10-10T05:19:27.217' AS DateTime), N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'Thanh toán lần 1: Sau khi ký hợp đồng', N'VNĐ')
INSERT [dbo].[BatchPayment] ([Id], [ContractId], [Price], [PaymentDate], [PaymentPhase], [IntitialQuotationId], [Percents], [InsDate], [FinalQuotationId], [Description], [Unit]) VALUES (N'8def16b7-e757-44d3-afaf-764c878aeb5d', NULL, 255000000, NULL, NULL, N'17d0fc6b-51a8-4d4d-aeaf-37b9a14b9db4', N'17', CAST(N'2024-10-08T13:20:47.537' AS DateTime), NULL, N'Ký hợp đồng', N'VNĐ')
INSERT [dbo].[BatchPayment] ([Id], [ContractId], [Price], [PaymentDate], [PaymentPhase], [IntitialQuotationId], [Percents], [InsDate], [FinalQuotationId], [Description], [Unit]) VALUES (N'2b48358a-3d4f-40dc-8ff7-7d7c674a3fc3', N'93755d1c-9f47-4aca-8c90-ce1455b2bbfe', 1123500000, CAST(N'2024-10-11T00:00:00.000' AS DateTime), CAST(N'2025-01-11T00:00:00.000' AS DateTime), N'4822b76f-5565-4a1a-99c0-370e8d067359', N'100', CAST(N'2024-10-16T11:17:45.240' AS DateTime), NULL, N'Thanh toán hợp đồng tư vấn và thiết kế bản vẽ nhà ở dân dụng', N'VNĐ')
INSERT [dbo].[BatchPayment] ([Id], [ContractId], [Price], [PaymentDate], [PaymentPhase], [IntitialQuotationId], [Percents], [InsDate], [FinalQuotationId], [Description], [Unit]) VALUES (N'3725d5bd-1b66-474b-8b53-914a378f3849', NULL, 168525000, CAST(N'2025-01-20T00:00:00.000' AS DateTime), CAST(N'2025-01-27T00:00:00.000' AS DateTime), N'4822b76f-5565-4a1a-99c0-370e8d067359', N'15', CAST(N'2024-10-10T05:19:27.217' AS DateTime), N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'Thanh toán lần 4: Sau khi hoàn thành phần mái', N'VNĐ')
INSERT [dbo].[BatchPayment] ([Id], [ContractId], [Price], [PaymentDate], [PaymentPhase], [IntitialQuotationId], [Percents], [InsDate], [FinalQuotationId], [Description], [Unit]) VALUES (N'd165e833-2e68-45ad-a657-a222d01e205c', N'9d864292-9768-46d4-82f5-6b26cb1b9a3f', 685552500, CAST(N'2024-11-04T12:40:31.853' AS DateTime), CAST(N'2024-11-11T12:40:31.853' AS DateTime), N'bd31c4e5-e549-42ea-aec7-0f08446f089d', N'50', CAST(N'2024-11-04T12:40:31.853' AS DateTime), N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'Đợt 1 thanh toán 50%', N'VNĐ')
INSERT [dbo].[BatchPayment] ([Id], [ContractId], [Price], [PaymentDate], [PaymentPhase], [IntitialQuotationId], [Percents], [InsDate], [FinalQuotationId], [Description], [Unit]) VALUES (N'9f29dc1f-c94d-4078-94ad-b3ebf48a6f8a', N'9d864292-9768-46d4-82f5-6b26cb1b9a3f', 685552500, CAST(N'2024-12-01T12:40:31.853' AS DateTime), CAST(N'2024-12-08T12:40:31.853' AS DateTime), N'bd31c4e5-e549-42ea-aec7-0f08446f089d', N'50', CAST(N'2024-11-04T12:40:31.853' AS DateTime), N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'Đợt 2 thanh toán 50% nghiệm thu bản vẽ thiết kế', N'VNĐ')
INSERT [dbo].[BatchPayment] ([Id], [ContractId], [Price], [PaymentDate], [PaymentPhase], [IntitialQuotationId], [Percents], [InsDate], [FinalQuotationId], [Description], [Unit]) VALUES (N'888b45f7-aa7e-4bfc-aecd-d234f6337aab', NULL, 150000000, NULL, NULL, N'17d0fc6b-51a8-4d4d-aeaf-37b9a14b9db4', N'10', CAST(N'2024-10-08T13:20:47.537' AS DateTime), NULL, N'Hoàn thành 80% khối lượng xây tô', N'VNĐ')
INSERT [dbo].[BatchPayment] ([Id], [ContractId], [Price], [PaymentDate], [PaymentPhase], [IntitialQuotationId], [Percents], [InsDate], [FinalQuotationId], [Description], [Unit]) VALUES (N'c5a5dbf4-63cb-432d-b346-da2132f296bd', NULL, 224700000, CAST(N'2025-03-20T00:00:00.000' AS DateTime), CAST(N'2025-03-27T00:00:00.000' AS DateTime), N'4822b76f-5565-4a1a-99c0-370e8d067359', N'20', CAST(N'2024-10-10T05:19:27.217' AS DateTime), N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'Thanh toán lần 6: Sau khi nghiệm thu và bàn giao', N'VNĐ')
INSERT [dbo].[BatchPayment] ([Id], [ContractId], [Price], [PaymentDate], [PaymentPhase], [IntitialQuotationId], [Percents], [InsDate], [FinalQuotationId], [Description], [Unit]) VALUES (N'a308a60f-4a2a-4a4d-b6a3-dee540f978d3', NULL, 168525000, CAST(N'2024-12-10T00:00:00.000' AS DateTime), CAST(N'2024-12-17T00:00:00.000' AS DateTime), N'4822b76f-5565-4a1a-99c0-370e8d067359', N'15', CAST(N'2024-10-10T05:19:27.217' AS DateTime), N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'Thanh toán lần 3: Sau khi hoàn thành phần thô', N'VNĐ')
GO
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'15b85094-21a2-46e7-95aa-0fba6890ae36', N'Thông Tầng lầu 4', 0, N'm2', CAST(N'2024-09-27T15:35:37.630' AS DateTime), CAST(N'2024-09-27T15:35:37.630' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'40aa0395-a4ef-423c-91f2-11d2889dc306', N'Thông Tầng lầu 1', 0, N'm2', CAST(N'2024-09-27T15:34:23.203' AS DateTime), CAST(N'2024-09-27T15:34:23.203' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'75922602-9153-4cc3-a7dc-225c9bc30a5e', N'Móng', 0, N'm2', CAST(N'2024-09-27T21:37:09.000' AS DateTime), CAST(N'2024-09-27T21:37:09.000' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'eab92d07-c140-4a5f-a22d-23cee8a1540e', N'Thông Tầng lầu 5', 0, N'm2', CAST(N'2024-09-27T15:35:43.087' AS DateTime), CAST(N'2024-09-27T15:35:43.087' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'b908ba40-7aa8-40e0-a379-24e539469aa9', N'Phần lát gạch nền', 0, N'm2', CAST(N'2024-10-09T15:05:03.390' AS DateTime), CAST(N'2024-10-09T15:05:03.390' AS DateTime), N'FINISHED', 1)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'7f1818ea-316d-4768-a74e-2f34320e0d7d', N'Hệ thống ống cấp, thoát nước', 0, N'm2', CAST(N'2024-10-09T15:00:22.970' AS DateTime), CAST(N'2024-10-09T15:00:22.970' AS DateTime), N'ROUGH', 1)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'be6c6db7-cea1-4275-9b18-2fbcfe9b2353', N'Trệt', 1, N'm2', CAST(N'2024-09-27T21:37:09.000' AS DateTime), CAST(N'2024-09-27T21:37:09.000' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'5a000367-77c4-4c16-b533-30cd8d0d6017', N'Thông Tầng lầu 2', 0, N'm2', CAST(N'2024-09-27T15:35:25.247' AS DateTime), CAST(N'2024-09-27T15:35:25.247' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'455f781a-2981-4998-bd3f-33a6768454c9', N'Phần thạch cao', 0, N'm2', CAST(N'2024-10-09T15:04:13.110' AS DateTime), CAST(N'2024-10-09T15:04:13.110' AS DateTime), N'FINISHED', 1)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'd8f3d72b-042f-4108-b9ea-3f93666921d2', N'Mái phụ', 0, N'm2', CAST(N'2024-09-27T22:11:44.510' AS DateTime), CAST(N'2024-09-27T22:11:44.510' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'ee1295bc-a38e-40d0-9f4f-58c88ed604e3', N'Chim sẻ', 1, N'm2', CAST(N'2024-10-14T14:09:18.177' AS DateTime), CAST(N'2024-10-14T14:09:18.177' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'a1f5dc09-41f5-40e4-a6d5-58fc73d17da5', N'Phần lan can', 0, N'm2', CAST(N'2024-10-09T15:04:23.703' AS DateTime), CAST(N'2024-10-09T15:04:23.703' AS DateTime), N'FINISHED', 1)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'e736b745-28b3-4a0e-89f5-5ffc35571828', N'Phần ốp lát WC', 0, N'm2', CAST(N'2024-10-09T15:04:47.690' AS DateTime), CAST(N'2024-10-09T15:04:47.690' AS DateTime), N'FINISHED', 1)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'38675854-19d3-4fdb-90dd-657ad3683ae1', N'Tầng lửng', 1, N'm2', CAST(N'2024-10-02T14:44:52.737' AS DateTime), CAST(N'2024-10-02T14:44:52.737' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'b516add2-e383-4335-985c-68be3d733eb8', N'Thiết bị điện', 0, N'm2', CAST(N'2024-10-09T15:05:42.930' AS DateTime), CAST(N'2024-10-09T15:05:42.930' AS DateTime), N'FINISHED', 1)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'13cd15d8-d1fb-48fb-8fbd-7523bc7fd04f', N'Phòng kỹ thuật thang máy', 0.5, N'm2', CAST(N'2024-09-27T15:43:05.070' AS DateTime), CAST(N'2024-09-27T15:43:05.070' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'9e66372d-ff80-44b1-87a9-7b6073efb8e4', N'Hệ thống dây điện chiếu sáng, tín hiệu', 0, N'm2', CAST(N'2024-10-09T15:03:01.220' AS DateTime), CAST(N'2024-10-09T15:03:01.220' AS DateTime), N'ROUGH', 1)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'1ead7d47-2a8d-41c2-99d7-817ca85146b1', N'Mới', 0, N'm2', CAST(N'2024-10-14T07:18:14.437' AS DateTime), CAST(N'2024-10-14T07:18:14.437' AS DateTime), N'ROUGH', NULL)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'4320ec5e-a71c-4161-95ae-857bd80763e5', N'Lầu 6', 1, N'm2', CAST(N'2024-09-27T15:37:13.277' AS DateTime), CAST(N'2024-09-27T15:37:13.277' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'ff0d4979-b837-44ff-a80e-913f2bdf02aa', N'SAA', 1, N'm2', CAST(N'2024-10-14T10:56:43.737' AS DateTime), CAST(N'2024-10-14T10:56:43.737' AS DateTime), N'ROUGH', NULL)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'bd101af5-ac48-43ba-a474-957a20a933bd', N'Mái che', 0, N'm2', CAST(N'2024-09-27T21:37:09.000' AS DateTime), CAST(N'2024-09-27T21:37:09.000' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'c7725e64-4ed1-4188-b665-9888364ca183', N'Phần sơn nước', 0, N'm2', CAST(N'2024-10-09T15:03:51.350' AS DateTime), CAST(N'2024-10-09T15:03:51.350' AS DateTime), N'FINISHED', 1)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'57e43b25-e16b-46a6-af8b-99e87c8593b4', N'Mái phụ', 0, N'm2', CAST(N'2024-10-07T01:31:31.030' AS DateTime), CAST(N'2024-10-07T01:31:31.030' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'22fc6201-3f75-4f4f-8a51-9bf25b2b13c3', N'Lầu 2', 1, N'm2', CAST(N'2024-09-27T15:36:51.893' AS DateTime), CAST(N'2024-09-27T15:36:51.893' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'2e134036-c2d8-46e6-a4ab-a8486e6813e0', N'Công tác đất, bê tông cốt thép', 0, N'm2', CAST(N'2024-10-09T14:57:36.367' AS DateTime), CAST(N'2024-10-09T14:57:36.367' AS DateTime), N'ROUGH', 1)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'eba29420-a8db-455c-86b0-b325a1da4e1e', N'Lầu 1', 1, N'm2', CAST(N'2024-09-27T15:36:43.490' AS DateTime), CAST(N'2024-09-27T15:36:43.490' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'22b90619-892f-4aaf-9f78-b6467faee47b', N'Lầu 4', 1, N'm2', CAST(N'2024-09-27T15:37:02.927' AS DateTime), CAST(N'2024-09-27T15:37:02.927' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'7c8b7d60-a4fb-4cf0-b509-bf9f34f2b158', N'Phần cửa', 0, N'm2', CAST(N'2024-10-09T15:04:01.240' AS DateTime), CAST(N'2024-10-09T15:04:01.240' AS DateTime), N'FINISHED', 1)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'903683df-19a9-4737-a8ac-c3dc0aa80949', N'Lầu 3', 1, N'm2', CAST(N'2024-09-27T15:36:56.853' AS DateTime), CAST(N'2024-09-27T15:36:56.853' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'34002a29-1d97-4e8d-8acd-c4a6c8e2df2b', N'Hố PIT', 3, N'm2', CAST(N'2024-10-02T14:45:08.397' AS DateTime), CAST(N'2024-10-02T14:45:08.397' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'f68ce6c6-6543-463f-b9d4-c5c967232988', N'Lầu 5', 1, N'm2', CAST(N'2024-09-27T15:37:08.653' AS DateTime), CAST(N'2024-09-27T15:37:08.653' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'75c3e21a-0abd-4699-abbe-c8371ed4d49a', N'Sân', 0.6, N'm2', CAST(N'2024-09-27T21:37:09.000' AS DateTime), CAST(N'2024-09-27T21:37:09.000' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'9696145f-bebf-4120-8354-c8aef681b351', N'Thông tầng lửng', 0, N'm2', CAST(N'2024-09-27T15:38:00.537' AS DateTime), CAST(N'2024-09-27T15:38:00.537' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'1f6bfbde-cd5c-43a5-b66f-c9c72950cdb6', N'Công tác xây gạch, trát, láng', 0, N'm2', CAST(N'2024-10-09T14:58:20.110' AS DateTime), CAST(N'2024-10-09T14:58:20.110' AS DateTime), N'ROUGH', 1)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'a6ce35ee-d19c-40ac-8044-cbecdb54f8d9', N'Hầm', 0, N'm2', CAST(N'2024-09-27T15:41:49.113' AS DateTime), CAST(N'2024-09-27T15:41:49.113' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'3187e9e0-7f0c-4c3d-83d8-cd5307dfb270', N'Công tác khác', 0, N'm2', CAST(N'2024-10-09T15:05:18.447' AS DateTime), CAST(N'2024-10-09T15:05:18.447' AS DateTime), N'FINISHED', 1)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'98ba07e3-d76c-42b7-8c87-cfd7b11a7f4f', N'Sân thượng có mái che', 1, N'm2', CAST(N'2024-09-27T21:37:09.000' AS DateTime), CAST(N'2024-09-27T21:37:09.000' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'6aa8e2fc-370e-4e6c-8269-d22d2c060e65', N'Thông Tầng lầu 6', 0, N'm2', CAST(N'2024-09-27T15:35:48.477' AS DateTime), CAST(N'2024-09-27T15:35:48.477' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'759169a4-9c29-4c6d-96ed-d8b570b42533', N'Phần chống thấm', 0, N'm2', CAST(N'2024-10-09T15:03:40.567' AS DateTime), CAST(N'2024-10-09T15:03:40.567' AS DateTime), N'FINISHED', 1)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'15030799-9d27-4270-b015-d9058d494e03', N'Sân thượng không có mái che', 0.5, N'm2', CAST(N'2024-09-27T21:37:09.000' AS DateTime), CAST(N'2024-09-27T21:37:09.000' AS DateTime), N'ROUGH', 0)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'41e2d4b4-0560-485d-80fa-e5d4d1b6917f', N'Phần mái', 0, N'm2', CAST(N'2024-10-09T15:03:24.640' AS DateTime), CAST(N'2024-10-09T15:03:24.640' AS DateTime), N'FINISHED', 1)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'cc26fcc6-9915-4583-8b52-eda96e2f33da', N'Phần ốp, lát đá', 0, N'm2', CAST(N'2024-10-09T15:04:38.230' AS DateTime), CAST(N'2024-10-09T15:04:38.230' AS DateTime), N'FINISHED', 1)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'30070190-7506-42a9-83c4-f576b7d006c3', N'Thiết bị vệ sinh', 0, N'm2', CAST(N'2024-10-09T15:05:33.417' AS DateTime), CAST(N'2024-10-09T15:05:33.417' AS DateTime), N'FINISHED', 1)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'aede030e-7f58-4c5a-9de6-fcac687ab128', N'Hệ thống ngầm', 0, N'm2', CAST(N'2024-10-09T14:57:54.290' AS DateTime), CAST(N'2024-10-09T14:57:54.290' AS DateTime), N'ROUGH', 1)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type], [IsFinalQuotation]) VALUES (N'f996f171-f8c6-4022-9152-fcccc486341e', N'Thông Tầng lầu 3', 0, N'm2', CAST(N'2024-09-27T15:35:31.800' AS DateTime), CAST(N'2024-09-27T15:35:31.800' AS DateTime), N'ROUGH', 0)
GO
INSERT [dbo].[Contract] ([Id], [ProjectId], [Name], [CustomerName], [ContractCode], [StartDate], [EndDate], [ValidityPeriod], [TaxCode], [Area], [UnitPrice], [ContractValue], [UrlFile], [Note], [Deflag], [RoughPackagePrice], [FinishedPackagePrice], [Status], [Type]) VALUES (N'9d864292-9768-46d4-82f5-6b26cb1b9a3f', N'b81935a8-4482-43f5-ad68-558abde58d58', N'Hợp đồng tư vấn và thiết kế kiến trúc', N'Đồ', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Draft', N'Design')
INSERT [dbo].[Contract] ([Id], [ProjectId], [Name], [CustomerName], [ContractCode], [StartDate], [EndDate], [ValidityPeriod], [TaxCode], [Area], [UnitPrice], [ContractValue], [UrlFile], [Note], [Deflag], [RoughPackagePrice], [FinishedPackagePrice], [Status], [Type]) VALUES (N'93755d1c-9f47-4aca-8c90-ce1455b2bbfe', N'fa91e187-b0c8-46c3-b6d5-747d32bd3540', N'Hợp đồng tư vấn và thiết kế bản vẽ nhà ở dân dụng', N'Đồ', N'BU9RXAP3JT', CAST(N'2024-11-11T02:33:45.693' AS DateTime), CAST(N'2025-01-11T02:33:45.693' AS DateTime), 5, NULL, 252, N'VNĐ', 47000000, N'https://vnexpress.net/la-home-don-dau-xu-huong-do-thi-sinh-thai-4801982.html', N'', 1, 3450000, NULL, N'Processing', N'Design')
GO
INSERT [dbo].[Customer] ([Id], [Email], [Username], [ImgUrl], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag], [AccountId]) VALUES (N'2fcb2c45-4dcc-4f3a-8fe4-1d2ee48fa931', N'ngandolh@gmail.com', N'Đồ', NULL, N'0901342922 ', CAST(N'2002-01-01T00:00:00.000' AS DateTime), NULL, NULL, 1, NULL)
INSERT [dbo].[Customer] ([Id], [Email], [Username], [ImgUrl], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag], [AccountId]) VALUES (N'1e6fa320-6945-4b76-8426-ad6dfdb1ad74', N'nganttk002@gmail.com', N'Trần Ngân', NULL, N'0906697051 ', CAST(N'2002-01-01T00:00:00.000' AS DateTime), NULL, NULL, 1, NULL)
GO
INSERT [dbo].[DesignTemplate] ([Id], [Name], [Description], [NumberOfFloor], [NumberOfBed], [NumberOfFront], [ImgUrl], [InsDate]) VALUES (N'7af799a9-02fa-4cd0-b954-86b102840e60', N'Mẫu 1', N'Mẫu nhà ở nông thôn', 1, 2, NULL, N'https://res.cloudinary.com/de7pulfdj/image/upload/v1729060083/kihpiscatjatvzjkaowi.png', NULL)
INSERT [dbo].[DesignTemplate] ([Id], [Name], [Description], [NumberOfFloor], [NumberOfBed], [NumberOfFront], [ImgUrl], [InsDate]) VALUES (N'335252c2-f710-430f-ab14-a1412ee29237', N'Mẫu 3', N'Mẫu 3', 1, 2, NULL, N'https://res.cloudinary.com/de7pulfdj/image/upload/v1729060083/kihpiscatjatvzjkaowi.png', NULL)
INSERT [dbo].[DesignTemplate] ([Id], [Name], [Description], [NumberOfFloor], [NumberOfBed], [NumberOfFront], [ImgUrl], [InsDate]) VALUES (N'ec6d9970-ea66-455a-8815-dd69d72ff97f', N'Mẫu 2', N'Mẫu 2', 1, 2, NULL, N'https://res.cloudinary.com/de7pulfdj/image/upload/v1729060083/kihpiscatjatvzjkaowi.png', NULL)
GO
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'5c242154-d4c2-43c5-9d0e-0a3d4105187f', N'Lavabo màu trắng âm bàn, MS: L5125

', N'Bộ', 6, 1111000, 6666000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'SANITATION')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'04d5da1a-dece-40a3-9e5d-1158337363f1', N'Đèn ốp trần nổi', N'Bộ', 3, 407000, 1221000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'e4c9fe57-ece7-4fcd-8d99-1c8cabd8e0e9', N'Ổ cắm điện 3 lỗ Panasonic', N'Cái', 10, 120000, 120000, NULL, N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'e5523935-6924-469b-b79e-1d8cede27435', N'Thiết bị chống sét lan truyền cho hệ thống điện', N'Cái', 1, 2820000, 2820000, NULL, N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'34f2a76f-e811-461d-b7d1-1e81d2c4f661', N'Xí bệt màu trắng, MS: CD1331+TAF050', N'Bộ', 5, 4247000, 21235000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'SANITATION')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'fd1b849f-2b39-4771-a28a-3019104d43f9', N'Đèn tường trang trí ngoài trời', N'Bộ', 18, 550000, 9900000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'ebf52eef-cd77-4c1d-b9af-39bbd116238d', N'Móc giấy vệ sinh, MS: Q7304V', N'Bộ', 5, 253000, 1265000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'SANITATION')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'514b0d9a-183f-4e6d-9058-422cfbe39c0a', N'Vòi xịt, MS: BS304CW', N'Vòi', 5, 396000, 1980000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'SANITATION')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'24192c6c-ec5d-43de-8f12-45dae8be54d8', N'Đèn tường trang trí phòng ngủ', N'Cái', 4, 850000, 3400000, NULL, N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'ab6b88dd-f97b-42bb-8634-468c4a277490', N'Bộ Switch 8P D-Link', N'Bộ', 1, 820000, 820000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'd140e9fb-6c51-4cf7-b76a-48764b34a96b', N'Công tắc đơn + đế + mặt nạ', N'Bộ', 9, 45000, 405000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'3c356ba2-0d09-4e1c-874e-4c01cd56b8ab', N'Ổ cắm tivi', N'Bộ', 5, 107000, 535000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'd948e784-e161-4c57-a6f0-55f39f3d12af', N'Quạt trần 5 cánh', N'Cái', 2, 3200000, 3200000, NULL, N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'e8f9f2f7-0829-43cc-b534-57268d217b34', N' Gương soi mặt', N'Cái', 11, 600000, 6000000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'SANITATION')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'b073b3a4-7de0-4580-bed6-604c2515a4e2', N'Đèn LED âm trần 9W', N'Cái ', 10, 250000, 250000, NULL, N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'ee7190f3-8c12-42b0-b377-64fbe4211a96', N'Quạt hút gió âm trần', N'Bộ', 5, 990000, 4950000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'b4621578-1a27-40f7-93e3-67d4aa909e53', N'Móc giấy vệ sinh, MS: Q7304V', N'Cái', 5, 253000, 1265000, NULL, N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'SANITATION')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'101e46d2-f871-4d13-b019-6a37a8b69fde', N'Rơle máy bơm nước', N'Cái ', 2, 740000, 1480000, NULL, N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'7deae01c-e18d-4520-9e95-6ca88cab35ad', N'Vòi sen nóng lạnh, MS: S563C', N'Cái', 5, 1496000, 7480000, NULL, N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'SANITATION')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'd59a7f44-0d1d-496f-826e-73ed413a67be', N'Ổ cắm điện đôi mặt ba lổ chống thấm', N'Bộ', 18, 330000, 5940000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'd73d48a9-57d7-457b-9814-74de12a560f5', N'Vòi lavabo nóng lạnh, MS: B571CU', N'Vòi', 6, 1892000, 11352000, NULL, N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'SANITATION')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'dc846fd5-aca5-4f07-a3ac-7594da20d5a7', N'Máy bơm nước lên bồn 2 HP', N'Cái', 1, 5450000, 5450000, NULL, N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'18e969c5-5702-4113-9d18-7d6c88f4955e', N'Bảng điện điều khiển trung tâm', N'Cái', 1, 5800000, 5800000, NULL, N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'391cd62d-c603-4461-b02a-7f9a75637f51', N'Vòi lavabo nóng lạnh, MS: B571CU', N'Bộ', 6, 1892000, 11352000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'SANITATION')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'a6e945e0-1203-4f26-824b-860ae7cc6e9a', N'Ổ cắm điện đôi mặt ba lổ âm tường', N'Bộ', 47, 119000, 5593000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'e0d29848-f386-4391-bfaa-912f3608a079', N'Bồn tắm nằm màu trắng, MS: MT0170', N'Cái', 1, 16327000, 16327000, NULL, N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'SANITATION')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'20275275-069c-4226-8375-96871b8ad2a6', N'Móc áo, MS: Q7307V', N'Bộ', 5, 143000, 715000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'SANITATION')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'5fddf891-9e6f-4e9f-bb4c-a0fd75967e06', N'Đèn tường trang trí trong nh', N'Bộ', 2, 350000, 700000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'654aa47f-ced2-4620-b74a-a81141150439', N'Van chữ T, MS: BF427', N'Cái', 5, 176000, 880000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'SANITATION')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'3a679fd5-a58f-4bce-a585-b0dc57a02f9c', N'Đèn chùm', N'Bộ', 3, 407000, 1221000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'd4738f76-6903-40bf-8f4d-b302fb46fed7', N' Giá treo khăn, MS: Q7301V', N'Bộ', 5, 275000, 1375000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'SANITATION')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'5a232da6-dfc4-4356-a4ed-b43db22668ea', N'Giá treo khăn, MS: Q7301V', N'Cái', 5, 275000, 1375000, NULL, N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'SANITATION')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'bc254218-6e03-47ce-b613-c24db9e6c248', N'Lavabo màu trắng âm bàn, MS: L5125', N'Cái', 6, 1111000, 6666000, NULL, N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'SANITATION')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'4ad0f308-4066-456d-8615-c6a2ece08f37', N'Công tắc đôi + đế + mặt nạ', N'Bộ', 14, 64000, 896000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'93d62fa0-08f6-4a50-bbf8-c81a5c5ce350', N'Máy bơm nước tăng áp 200W', N'Cái', 1, 2450000, 2450000, NULL, N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'eb51a496-3660-430c-97e5-cad84a32351e', N'Tủ điện 16 đường loại âm tường', N'Tủ', 1, 3300000, 3300000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'b5fa738b-7b4d-41b8-8c1a-cc9ab4b12f19', N'Máy nước nóng NLMT 210L, MS: F70 CLASSIC', N'Máy', 1, 11140000, 11140000, NULL, N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'SANITATION')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'Đèn thả trần 3 bóng', N'Bộ', 1, 5000000, 5000000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'ff4bafd0-7e6e-4a87-8665-d1250a4f7bc9', N'Đèn chùm trang trí phòng khách', N'Cái', 1, 12000000, 12000000, NULL, N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'ELECTRIC')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'1492e03b-23a0-421d-b094-d69112245ceb', N'Xí bệt màu trắng, MS: CD1331+TAF050', N'Cái', 5, 4247000, 21235000, NULL, N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'SANITATION')
INSERT [dbo].[EquipmentItem] ([Id], [Name], [Unit], [Quantity], [UnitOfMaterial], [TotalOfMaterial], [Note], [FinalQuotationId], [Type]) VALUES (N'065d73e3-2ead-4af7-b5bb-f297c8a85a5d', N' Bộ xả lavabo, MS: BF603', N'Bộ', 6, 429000, 2574000, NULL, N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'SANITATION')
GO
INSERT [dbo].[FinalQuotation] ([Id], [ProjectId], [PromotionId], [TotalPrice], [Note], [Version], [InsDate], [UpsDate], [Status], [Deflag], [QuotationUtilitiesId], [ReasonReject]) VALUES (N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'fa91e187-b0c8-46c3-b6d5-747d32bd3540', NULL, 1123500000, NULL, 1, CAST(N'2024-09-29T10:00:00.000' AS DateTime), CAST(N'2024-09-29T10:00:00.000' AS DateTime), N'Processing', 1, NULL, NULL)
INSERT [dbo].[FinalQuotation] ([Id], [ProjectId], [PromotionId], [TotalPrice], [Note], [Version], [InsDate], [UpsDate], [Status], [Deflag], [QuotationUtilitiesId], [ReasonReject]) VALUES (N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'b81935a8-4482-43f5-ad68-558abde58d58', NULL, 1500000000, NULL, 1, CAST(N'2024-09-29T10:00:00.000' AS DateTime), CAST(N'2024-09-29T10:00:00.000' AS DateTime), N'Rejected', 1, NULL, NULL)
INSERT [dbo].[FinalQuotation] ([Id], [ProjectId], [PromotionId], [TotalPrice], [Note], [Version], [InsDate], [UpsDate], [Status], [Deflag], [QuotationUtilitiesId], [ReasonReject]) VALUES (N'8d91351a-bf21-4919-856d-dbef075ef411', N'b81935a8-4482-43f5-ad68-558abde58d58', NULL, 1500000000, NULL, 2, CAST(N'2024-10-03T10:00:00.000' AS DateTime), CAST(N'2024-10-03T10:00:00.000' AS DateTime), N'Processing', 0, NULL, NULL)
GO
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'b3039413-c1ba-4bf3-89b3-0122c1e085f5', N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'Hệ thống dây điện chiếu sáng, tín hiệu', N'm', N'200', 150000, 300000, NULL, 30000000, 60000000, NULL, NULL, N'9e66372d-ff80-44b1-87a9-7b6073efb8e4')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'd160e67a-eff5-45e8-aa0c-01a6854454ba', N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'Phần chống thấm', N'm2', N'300', 120000, 280000, NULL, 36000000, 84000000, NULL, NULL, N'759169a4-9c29-4c6d-96ed-d8b570b42533')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'b3cddf99-026f-4f99-a411-10c34343c827', N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'Phần cửa', N'bộ', N'10', 0, NULL, 5000000, NULL, NULL, 50000000, NULL, N'7c8b7d60-a4fb-4cf0-b509-bf9f34f2b158')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'92e40125-4aad-400a-b904-11a3be41a990', N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'Phần cửa', N'bộ', N'50', 200000, 500000, NULL, 10000000, 25000000, NULL, NULL, N'7c8b7d60-a4fb-4cf0-b509-bf9f34f2b158')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'd2770e93-d8f9-490c-acb1-16889b4698c7', N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'Phần lan can', N'm', N'200', 180000, 300000, NULL, 36000000, 60000000, NULL, NULL, N'a1f5dc09-41f5-40e4-a6d5-58fc73d17da5')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'df7751d8-66eb-4abb-90fa-2a37ec269594', N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'Công tác đất, bê tông cốt thép', N'm3', N'150', 300000, 600000, NULL, 45000000, 90000000, NULL, NULL, N'2e134036-c2d8-46e6-a4ab-a8486e6813e0')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'680fdf80-1aca-47f8-8a2a-3717b1379218', N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'Phần sơn nước', N'm2', N'500', 90000, 160000, NULL, 45000000, 80000000, NULL, NULL, N'c7725e64-4ed1-4188-b665-9888364ca183')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'985a8704-c11a-4208-ad94-448aa0bfd6fe', N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'Phần thạch cao', N'm2', N'400', 100000, 200000, NULL, 40000000, 80000000, NULL, NULL, N'455f781a-2981-4998-bd3f-33a6768454c9')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'277cd978-ce7f-44ba-b2ce-472485662238', N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'Công tác khác', N'bộ', N'50', 200000, 300000, NULL, 10000000, 15000000, NULL, NULL, N'3187e9e0-7f0c-4c3d-83d8-cd5307dfb270')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'f8a66063-a30d-4937-ace1-52a78dbe6cf9', N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'Công tác khác', N'bộ', N'20', 220000, 300000, NULL, 4400000, 6000000, NULL, NULL, N'3187e9e0-7f0c-4c3d-83d8-cd5307dfb270')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'e87f4c3b-dd0f-44ae-a7ba-63b1192ef56d', N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'Phần mái', N'm2', N'100', 350000, 500000, NULL, 35000000, 50000000, NULL, NULL, N'41e2d4b4-0560-485d-80fa-e5d4d1b6917f')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'31eb3242-c25d-450e-99c2-63dae677c2bb', N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'Phần lát gạch nền', N'm2', N'300', 160000, 240000, NULL, 48000000, 72000000, NULL, NULL, N'b908ba40-7aa8-40e0-a379-24e539469aa9')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'66c900bc-7cbb-4757-8728-655a47f953a0', N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'Công tác xây gạch, trát, láng', N'm', N'200', 250000, 350000, NULL, 50000000, 70000000, NULL, NULL, N'1f6bfbde-cd5c-43a5-b66f-c9c72950cdb6')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'b069d43d-06b7-48a7-afd0-6c3907cb35a6', N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'Phần ốp, lát đá', N'm2', N'200', 150000, 350000, NULL, 30000000, 70000000, NULL, NULL, N'cc26fcc6-9915-4583-8b52-eda96e2f33da')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'266b060f-c7ff-48fe-bd2c-911da0ad4499', N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'Phần sơn nước', N'm2', N'300', 50000, 150000, NULL, 15000000, 45000000, NULL, NULL, N'c7725e64-4ed1-4188-b665-9888364ca183')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'23164f32-4f8a-4597-9935-98b762f7d936', N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'Phần lan can', N'm', N'40', 500000, 1000000, NULL, 20000000, 40000000, NULL, NULL, N'a1f5dc09-41f5-40e4-a6d5-58fc73d17da5')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'6c200ad0-fccd-40ba-9e41-9ad52c456fa4', N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'Công tác xây gạch, trát, láng', N'm2', N'400', 150000, 200000, NULL, 60000000, 80000000, NULL, NULL, N'1f6bfbde-cd5c-43a5-b66f-c9c72950cdb6')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'e6bf1b2e-8b7a-49f7-8820-9db5a97e146a', N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'Phần thạch cao', N'm2', N'100', 150000, 300000, NULL, 15000000, 30000000, NULL, NULL, N'455f781a-2981-4998-bd3f-33a6768454c9')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'cceca595-cd97-42b5-8cfc-b04beda3337b', N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'Hệ thống dây điện chiếu sáng, tín hiệu', N'm', N'150', 300000, 400000, NULL, 45000000, 60000000, NULL, NULL, N'9e66372d-ff80-44b1-87a9-7b6073efb8e4')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'47d8dfbb-7616-4e1d-ba8e-ba9c289e653f', N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'Phần ốp lát WC', N'm2', N'30', 120000, 220000, NULL, 3600000, 6600000, NULL, NULL, N'e736b745-28b3-4a0e-89f5-5ffc35571828')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'd3bc13cf-c83c-4e56-a053-baba9eed5cc4', N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'Hệ thống ngầm', N'm', N'100', 400000, 600000, NULL, 40000000, 60000000, NULL, NULL, N'aede030e-7f58-4c5a-9de6-fcac687ab128')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'036cad68-f99e-40fb-9442-c9f19d9bdc2d', N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'Hệ thống ngầm', N'm', N'300', 150000, 250000, NULL, 45000000, 75000000, NULL, NULL, N'aede030e-7f58-4c5a-9de6-fcac687ab128')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'89a30ed9-159c-46e6-94f7-cbbb06769a8a', N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'Phần lát gạch nền', N'm2', N'120', 80000, 150000, NULL, 96000000, 18000000, NULL, NULL, N'b908ba40-7aa8-40e0-a379-24e539469aa9')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'4f2a1eaa-add6-479f-8316-d7e0f3d4dbed', N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'Phần chống thấm', N'm2', N'200', 100000, 300000, NULL, 20000000, 60000000, NULL, NULL, N'759169a4-9c29-4c6d-96ed-d8b570b42533')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'8fb699a9-fe7e-4b0c-b5eb-d9312a42f107', N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'Hệ thống ống cấp, thoát nước', N'm', N'150', 250000, 400000, NULL, 45000000, 72000000, NULL, NULL, N'7f1818ea-316d-4768-a74e-2f34320e0d7d')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'4b4f24d5-c26a-42d1-b704-f579393a0e08', N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'Phần ốp, lát đá', N'm2', N'80', 150000, 400000, NULL, 12000000, 32000000, NULL, NULL, N'cc26fcc6-9915-4583-8b52-eda96e2f33da')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'a2abde6c-c615-4359-8b5e-f6cf255f3b00', N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'Công tác đất, bê tông cốt thép', N'm3', N'500', 200000, 400000, NULL, 100000000, 20000000, NULL, NULL, N'2e134036-c2d8-46e6-a4ab-a8486e6813e0')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'04478fd7-9111-4ef9-a41c-f8e400bfd39a', N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'Hệ thống ống cấp, thoát nước', N'm', N'200', 250000, 350000, NULL, 50000000, 70000000, NULL, NULL, N'7f1818ea-316d-4768-a74e-2f34320e0d7d')
INSERT [dbo].[FinalQuotationItem] ([Id], [FinalQuotationId], [Name], [Unit], [Weight], [UnitPriceLabor], [UnitPriceRough], [UnitPriceFinished], [TotalPriceLabor], [TotalPriceRough], [TotalPriceFinished], [InsDate], [ConstructionItemId]) VALUES (N'047d577b-1fc5-4223-99c8-fa2cf0220912', N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'Phần mái', N'm2', N'120', 200000, 500000, NULL, 24000000, 60000000, NULL, NULL, N'41e2d4b4-0560-485d-80fa-e5d4d1b6917f')
GO
INSERT [dbo].[HouseDesignDrawing] ([Id], [ProjectId], [Name], [Step], [Status], [Type], [IsCompany], [InsDate], [AccountId]) VALUES (N'4135dfce-55e0-4370-b453-2d438e96dbbb', N'fa91e187-b0c8-46c3-b6d5-747d32bd3540', N'Phối cảnh', 1, N'Finalized', N'PHOICANH', 0, CAST(N'2024-09-30T12:34:56.000' AS DateTime), N'bf339e88-5303-45c4-a6f4-33a79681766c')
INSERT [dbo].[HouseDesignDrawing] ([Id], [ProjectId], [Name], [Step], [Status], [Type], [IsCompany], [InsDate], [AccountId]) VALUES (N'b1f28347-bf77-47ad-9f13-3a7658d11e0a', N'b81935a8-4482-43f5-ad68-558abde58d58', N'Kiến trúc', 2, N'Processing', N'KIENTRUC', 0, CAST(N'2024-10-07T20:02:00.660' AS DateTime), N'990773a2-1817-47f5-9116-301e97435c44')
INSERT [dbo].[HouseDesignDrawing] ([Id], [ProjectId], [Name], [Step], [Status], [Type], [IsCompany], [InsDate], [AccountId]) VALUES (N'cc2c232f-f928-45c4-a76a-665d870c7eb3', N'b81935a8-4482-43f5-ad68-558abde58d58', N'Phối cảnh', 1, N'Processing', N'PHOICANH', 0, CAST(N'2024-10-07T20:02:00.563' AS DateTime), N'bf339e88-5303-45c4-a6f4-33a79681766c')
INSERT [dbo].[HouseDesignDrawing] ([Id], [ProjectId], [Name], [Step], [Status], [Type], [IsCompany], [InsDate], [AccountId]) VALUES (N'61913bc3-9d33-4d8b-8ea3-685ad07c108e', N'b81935a8-4482-43f5-ad68-558abde58d58', N'Kết cấu', 3, N'Approved', N'KETCAU', 0, CAST(N'2024-10-07T20:02:00.670' AS DateTime), N'28247cd1-67ca-439d-bef5-fca9a9a777c5')
INSERT [dbo].[HouseDesignDrawing] ([Id], [ProjectId], [Name], [Step], [Status], [Type], [IsCompany], [InsDate], [AccountId]) VALUES (N'7c90012d-5231-4f0b-bf11-863ffefabba6', N'b81935a8-4482-43f5-ad68-558abde58d58', N'Điện & nước', 4, N'Updating', N'DIENNUOC', 0, CAST(N'2024-10-07T20:02:00.677' AS DateTime), N'28247cd1-67ca-439d-bef5-fca9a9a777c6')
INSERT [dbo].[HouseDesignDrawing] ([Id], [ProjectId], [Name], [Step], [Status], [Type], [IsCompany], [InsDate], [AccountId]) VALUES (N'd3998f6d-ae9b-4467-855b-937c7dcac45b', N'fa91e187-b0c8-46c3-b6d5-747d32bd3540', N'Kết cấu', 3, N'Approved', N'KETCAU', 0, CAST(N'2024-09-30T12:34:56.000' AS DateTime), N'28247cd1-67ca-439d-bef5-fca9a9a777c5')
INSERT [dbo].[HouseDesignDrawing] ([Id], [ProjectId], [Name], [Step], [Status], [Type], [IsCompany], [InsDate], [AccountId]) VALUES (N'9d6b7184-725d-4879-b94c-942f3320d6ac', N'fa91e187-b0c8-46c3-b6d5-747d32bd3540', N'Điện & nước', 4, N'Canceled', N'DIENNUOC', 0, CAST(N'2024-09-30T12:34:56.000' AS DateTime), N'45bced7f-0432-40dd-9686-91f8cc1c90dc')
INSERT [dbo].[HouseDesignDrawing] ([Id], [ProjectId], [Name], [Step], [Status], [Type], [IsCompany], [InsDate], [AccountId]) VALUES (N'eab8d4ff-c6d4-452a-ab89-dedc8d3f5153', N'fa91e187-b0c8-46c3-b6d5-747d32bd3540', N'Kiến trúc', 2, N'Finalized', N'KIENTRUC', 0, CAST(N'2024-09-30T12:34:56.000' AS DateTime), N'990773a2-1817-47f5-9116-301e97435c44')
GO
INSERT [dbo].[HouseDesignVersion] ([Id], [Name], [Version], [Status], [InsDate], [HouseDesignDrawingId], [Note], [UpsDate], [RelatedDrawingId], [PreviousDrawingId], [Reason], [Deflag]) VALUES (N'3905068b-8e24-4fc1-8663-a6697903de08', N'Phối cảnh', 1, N'Finished', CAST(N'2024-09-30T12:34:56.000' AS DateTime), N'4135dfce-55e0-4370-b453-2d438e96dbbb', N'Sửa cửa chính và cửa phụ 2', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[HouseDesignVersion] ([Id], [Name], [Version], [Status], [InsDate], [HouseDesignDrawingId], [Note], [UpsDate], [RelatedDrawingId], [PreviousDrawingId], [Reason], [Deflag]) VALUES (N'97ee6354-4a61-4569-bbde-e5818c49f3e9', N'Phối cảnh', 1, N'Proccesing', CAST(N'2024-10-14T10:45:23.357' AS DateTime), N'cc2c232f-f928-45c4-a76a-665d870c7eb3', N'', CAST(N'2024-10-14T10:45:23.357' AS DateTime), NULL, NULL, NULL, 0)
INSERT [dbo].[HouseDesignVersion] ([Id], [Name], [Version], [Status], [InsDate], [HouseDesignDrawingId], [Note], [UpsDate], [RelatedDrawingId], [PreviousDrawingId], [Reason], [Deflag]) VALUES (N'2e7a32d9-5ffb-4cca-b866-f03f67da256d', N'Phối cảnh', 2, N'Finished', CAST(N'2024-09-30T12:34:56.000' AS DateTime), N'4135dfce-55e0-4370-b453-2d438e96dbbb', NULL, CAST(N'2024-10-14T00:18:20.747' AS DateTime), NULL, NULL, N'Sửa cửa sổ + cửa chính', 1)
GO
INSERT [dbo].[InitialQuotation] ([Id], [ProjectId], [PromotionId], [Area], [TimeProcessing], [TimeRough], [TimeOthers], [OthersAgreement], [InsDate], [Status], [Version], [IsTemplate], [Deflag], [Note], [TotalRough], [TotalUtilities], [Unit], [ReasonReject]) VALUES (N'bd31c4e5-e549-42ea-aec7-0f08446f089d', N'b81935a8-4482-43f5-ad68-558abde58d58', N'41b828b2-7b06-4389-9003-daac937158dd', 125, NULL, NULL, NULL, NULL, CAST(N'2024-10-04T12:40:31.853' AS DateTime), N'Rejected', 1, 0, 1, NULL, 1500000000, 20000000, N'VNĐ', N'Sửa phần tiện ích')
INSERT [dbo].[InitialQuotation] ([Id], [ProjectId], [PromotionId], [Area], [TimeProcessing], [TimeRough], [TimeOthers], [OthersAgreement], [InsDate], [Status], [Version], [IsTemplate], [Deflag], [Note], [TotalRough], [TotalUtilities], [Unit], [ReasonReject]) VALUES (N'525e7aae-585c-4173-b0e7-1ebcdecdd027', N'fa91e187-b0c8-46c3-b6d5-747d32bd3540', NULL, 252, 200, 150, 50, N'- Nghiệm thu: Mỗi hạng mục sẽ được nghiệm thu sau khi hoàn thành thi công. Các chỉ số về chất lượng thi công và hệ số sẽ được kiểm tra dựa trên tiêu chuẩn kỹ thuật đã thống nhất.
- Thanh toán: Hệ số sẽ là cơ sở để tính toán chi phí từng hạng mục, và các khoản thanh toán sẽ được thực hiện sau khi hoàn tất nghiệm thu từng hạng mục.', CAST(N'2024-10-04T12:40:31.853' AS DateTime), N'Rejected', 1, 0, 1, NULL, 1000000000, 123500000, N'VNĐ', NULL)
INSERT [dbo].[InitialQuotation] ([Id], [ProjectId], [PromotionId], [Area], [TimeProcessing], [TimeRough], [TimeOthers], [OthersAgreement], [InsDate], [Status], [Version], [IsTemplate], [Deflag], [Note], [TotalRough], [TotalUtilities], [Unit], [ReasonReject]) VALUES (N'4822b76f-5565-4a1a-99c0-370e8d067359', N'fa91e187-b0c8-46c3-b6d5-747d32bd3540', NULL, 252, 200, 150, 50, N'- Nghiệm thu: Mỗi hạng mục sẽ được nghiệm thu sau khi hoàn thành thi công. Các chỉ số về chất lượng thi công và hệ số sẽ được kiểm tra dựa trên tiêu chuẩn kỹ thuật đã thống nhất.- Thanh toán: Hệ số sẽ là cơ sở để tính toán chi phí từng hạng mục, và các khoản thanh toán sẽ được thực hiện sau khi hoàn tất nghiệm thu từng hạng mục.', CAST(N'2024-10-10T05:19:27.207' AS DateTime), N'Finalized', 2, 0, 1, NULL, 1000000000, 123500000, N'VNĐ', NULL)
INSERT [dbo].[InitialQuotation] ([Id], [ProjectId], [PromotionId], [Area], [TimeProcessing], [TimeRough], [TimeOthers], [OthersAgreement], [InsDate], [Status], [Version], [IsTemplate], [Deflag], [Note], [TotalRough], [TotalUtilities], [Unit], [ReasonReject]) VALUES (N'17d0fc6b-51a8-4d4d-aeaf-37b9a14b9db4', N'b81935a8-4482-43f5-ad68-558abde58d58', NULL, 125, 165, 115, 50, N'Khách hàng phải thanh toán trong vòng 7 ngày kể từ khi hợp động được kí', CAST(N'2024-10-08T13:20:41.513' AS DateTime), N'Finalized', 3, 0, 0, NULL, 0, 0, N'VNĐ', NULL)
INSERT [dbo].[InitialQuotation] ([Id], [ProjectId], [PromotionId], [Area], [TimeProcessing], [TimeRough], [TimeOthers], [OthersAgreement], [InsDate], [Status], [Version], [IsTemplate], [Deflag], [Note], [TotalRough], [TotalUtilities], [Unit], [ReasonReject]) VALUES (N'2c6e1743-b0a0-4638-926f-8e24a120d714', N'310806e2-876b-48d6-a87a-e534e4ffa647', NULL, 100, 120, 90, 22, N'', CAST(N'2024-10-08T13:20:41.513' AS DateTime), N'Processing', 1, 0, 0, NULL, 1000000000, 2000000, N'VNĐ', N'string')
INSERT [dbo].[InitialQuotation] ([Id], [ProjectId], [PromotionId], [Area], [TimeProcessing], [TimeRough], [TimeOthers], [OthersAgreement], [InsDate], [Status], [Version], [IsTemplate], [Deflag], [Note], [TotalRough], [TotalUtilities], [Unit], [ReasonReject]) VALUES (N'27303a9e-c1ff-4060-a145-bad032564a0a', N'eac91a0b-baae-4e00-a21f-f4d05153f447', NULL, 125, NULL, NULL, NULL, NULL, CAST(N'2024-10-04T00:17:13.637' AS DateTime), N'Rejected', 1, 0, 1, NULL, 1500000000, 10000000, N'VNĐ', NULL)
INSERT [dbo].[InitialQuotation] ([Id], [ProjectId], [PromotionId], [Area], [TimeProcessing], [TimeRough], [TimeOthers], [OthersAgreement], [InsDate], [Status], [Version], [IsTemplate], [Deflag], [Note], [TotalRough], [TotalUtilities], [Unit], [ReasonReject]) VALUES (N'f6f03971-5a01-47ce-869a-c3f63d70fbb9', N'b841c593-1d05-4492-9ba2-d485f6d67260', N'41b828b2-7b06-4389-9003-daac937158dd', 125, 200, 160, 40, N'- Giá trị Hợp đồng chưa bao gồm thuế VAT
- Chủ đầu tư cam kết không thanh lý hợp đồng trước thời hạn. Trong mọi trường hợp thanh lý hợp 
đồng trước thời hạn, chủ đầu tư sẽ không được giảm trừ giá trị Hợp đồng ', CAST(N'2024-09-28T14:45:30.123' AS DateTime), N'Processing', 1, 0, 0, NULL, 1500000000, 5000000, N'VNĐ', NULL)
INSERT [dbo].[InitialQuotation] ([Id], [ProjectId], [PromotionId], [Area], [TimeProcessing], [TimeRough], [TimeOthers], [OthersAgreement], [InsDate], [Status], [Version], [IsTemplate], [Deflag], [Note], [TotalRough], [TotalUtilities], [Unit], [ReasonReject]) VALUES (N'4fe5b3e7-ac15-498d-aabe-c6f9f8987d5f', N'b81935a8-4482-43f5-ad68-558abde58d58', NULL, 125, 165, 115, 50, N'Khách hàng phải thanh toán trong vòng 7 ngày kể từ khi hợp động được kí', CAST(N'2024-10-06T01:41:02.557' AS DateTime), N'Reviewing', 2, 0, 1, NULL, 0, 0, N'VNĐ', NULL)
INSERT [dbo].[InitialQuotation] ([Id], [ProjectId], [PromotionId], [Area], [TimeProcessing], [TimeRough], [TimeOthers], [OthersAgreement], [InsDate], [Status], [Version], [IsTemplate], [Deflag], [Note], [TotalRough], [TotalUtilities], [Unit], [ReasonReject]) VALUES (N'69aded64-dc90-4b35-8059-f42dff6a10c6', N'799b9201-d234-47b9-a14f-7574cc84ef74', NULL, 125, NULL, NULL, NULL, NULL, CAST(N'2024-10-04T01:24:44.717' AS DateTime), N'Pending', 1, 0, 1, NULL, 1500000000, NULL, N'VNĐ', NULL)
GO
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'8946f6e3-32d3-4621-8e43-05db32ffa305', N'Sân', N'75c3e21a-0abd-4699-abbe-c8371ed4d49a', NULL, 101.4, NULL, N'đ', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), N'f6f03971-5a01-47ce-869a-c3f63d70fbb9')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'fdd1bda1-efd8-46b4-b0ad-09bf2c09214e', NULL, N'eba29420-a8db-455c-86b0-b325a1da4e1e', NULL, 252, 642600000, N'đ', CAST(N'2024-10-10T05:19:27.217' AS DateTime), CAST(N'2024-10-10T05:19:27.217' AS DateTime), N'4822b76f-5565-4a1a-99c0-370e8d067359')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'9bca0a0e-a4b3-435a-ae0a-15f5f191bf16', NULL, N'be6c6db7-cea1-4275-9b18-2fbcfe9b2353', NULL, 99, 331650000, N'đ', CAST(N'2024-10-08T13:20:41.523' AS DateTime), CAST(N'2024-10-08T13:20:41.523' AS DateTime), N'17d0fc6b-51a8-4d4d-aeaf-37b9a14b9db4')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'795f3eea-3610-4e13-b720-1c6bf1dc2dc9', NULL, N'75c3e21a-0abd-4699-abbe-c8371ed4d49a', NULL, 151.2, 385560000, N'đ', CAST(N'2024-10-10T05:19:27.217' AS DateTime), CAST(N'2024-10-10T05:19:27.217' AS DateTime), N'4822b76f-5565-4a1a-99c0-370e8d067359')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'5ca9cb47-d260-4486-9b82-4df29af1bfbc', N'Lầu 2', N'22fc6201-3f75-4f4f-8a51-9bf25b2b13c3', NULL, 132.27, NULL, N'đ', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), N'f6f03971-5a01-47ce-869a-c3f63d70fbb9')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'83ad2005-cf26-41a5-9756-586f0f2e256b', N'', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', N'06ff2d3d-2f14-4acc-ba2e-0c4ee659ca81', 49.5, 66330000, N'đ', CAST(N'2024-10-04T00:17:13.733' AS DateTime), CAST(N'2024-10-04T00:17:13.733' AS DateTime), N'27303a9e-c1ff-4060-a145-bad032564a0a')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'5453c25f-b2a4-4f93-9071-5891f3fe870e', N'', N'bd101af5-ac48-43ba-a474-957a20a933bd', N'7e442652-eefc-43b7-918b-a264a10e679d', 49.5, 66330000, N'đ', CAST(N'2024-10-04T00:17:13.770' AS DateTime), CAST(N'2024-10-04T00:17:13.770' AS DateTime), N'27303a9e-c1ff-4060-a145-bad032564a0a')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'feccb840-5df4-4b31-9a51-83fbfc698081', N'Lỗ trống lầu 1', N'40aa0395-a4ef-423c-91f2-11d2889dc306', N'b88899fd-aa23-4a66-ad1e-9a5a4dd03bca', 11.325, NULL, N'đ', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), N'f6f03971-5a01-47ce-869a-c3f63d70fbb9')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'09fb8bac-3b2d-4a14-8a10-90711d84f13e', N'Móng', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', NULL, 35.1, NULL, N'đ', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), N'f6f03971-5a01-47ce-869a-c3f63d70fbb9')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'5b792ed1-a726-44ce-9820-9629d459ed8b', N'', N'bd101af5-ac48-43ba-a474-957a20a933bd', N'7e442652-eefc-43b7-918b-a264a10e679d', 49.5, 66330000, N'đ', CAST(N'2024-10-04T12:40:31.910' AS DateTime), CAST(N'2024-10-04T12:40:31.910' AS DateTime), N'bd31c4e5-e549-42ea-aec7-0f08446f089d')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'0093f6a8-8623-499c-8ced-9f4d911968ac', NULL, N'd8f3d72b-042f-4108-b9ea-3f93666921d2', N'144008ad-b489-4e97-8d62-3d7521ed7c71', 75.6, 192780000, N'đ', CAST(N'2024-10-10T05:19:27.217' AS DateTime), CAST(N'2024-10-10T05:19:27.217' AS DateTime), N'4822b76f-5565-4a1a-99c0-370e8d067359')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'ab9beec4-14ec-4491-b115-a5ed9cc0b3c9', NULL, N'bd101af5-ac48-43ba-a474-957a20a933bd', N'7e442652-eefc-43b7-918b-a264a10e679d', 49.5, 66330000, N'đ', CAST(N'2024-10-08T13:20:41.523' AS DateTime), CAST(N'2024-10-08T13:20:41.523' AS DateTime), N'17d0fc6b-51a8-4d4d-aeaf-37b9a14b9db4')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'1bf45d93-ddb9-40ff-b1a4-b01180bc5955', N'', N'be6c6db7-cea1-4275-9b18-2fbcfe9b2353', N'06ff2d3d-2f14-4acc-ba2e-0c4ee659ca81', 99, 331650000, N'đ', CAST(N'2024-10-04T12:40:31.910' AS DateTime), CAST(N'2024-10-04T12:40:31.910' AS DateTime), N'bd31c4e5-e549-42ea-aec7-0f08446f089d')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'748663a3-d458-4f51-b12e-c48bd7cb70a3', NULL, N'75922602-9153-4cc3-a7dc-225c9bc30a5e', N'287f222c-2d3b-4445-900c-2a32101bec00', 151.2, 385560000, N'đ', CAST(N'2024-10-10T05:19:27.217' AS DateTime), CAST(N'2024-10-10T05:19:27.217' AS DateTime), N'4822b76f-5565-4a1a-99c0-370e8d067359')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'3c414d7a-58d0-488c-9e48-ccef30093ea1', N'', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', N'06ff2d3d-2f14-4acc-ba2e-0c4ee659ca81', 49.5, 66330000, N'đ', CAST(N'2024-10-04T12:40:31.893' AS DateTime), CAST(N'2024-10-04T12:40:31.893' AS DateTime), N'bd31c4e5-e549-42ea-aec7-0f08446f089d')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'9f25f16d-8333-4391-b272-cd897fb0a92a', N'Mái BTCT', N'bd101af5-ac48-43ba-a474-957a20a933bd', N'7e442652-eefc-43b7-918b-a264a10e679d', 83, NULL, N'đ', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), N'f6f03971-5a01-47ce-869a-c3f63d70fbb9')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'eb249d42-2ba7-4953-8c26-dc21de5400de', N'', N'be6c6db7-cea1-4275-9b18-2fbcfe9b2353', N'06ff2d3d-2f14-4acc-ba2e-0c4ee659ca81', 99, 331650000, N'đ', CAST(N'2024-10-04T00:17:13.770' AS DateTime), CAST(N'2024-10-04T00:17:13.770' AS DateTime), N'27303a9e-c1ff-4060-a145-bad032564a0a')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'437ac8b6-e40c-4280-9d17-dd62d190335d', NULL, N'5a000367-77c4-4c16-b533-30cd8d0d6017', N'11f81c2a-bb8f-40eb-9619-3c119901f57f', 252, 642600000, N'đ', CAST(N'2024-10-10T05:19:27.217' AS DateTime), CAST(N'2024-10-10T05:19:27.217' AS DateTime), N'4822b76f-5565-4a1a-99c0-370e8d067359')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'4dbfb65d-6b9c-4009-b8e8-e6c24c13cb61', NULL, N'75922602-9153-4cc3-a7dc-225c9bc30a5e', N'06ff2d3d-2f14-4acc-ba2e-0c4ee659ca81', 49.5, 66330000, N'đ', CAST(N'2024-10-08T13:20:41.523' AS DateTime), CAST(N'2024-10-08T13:20:41.523' AS DateTime), N'17d0fc6b-51a8-4d4d-aeaf-37b9a14b9db4')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'9f93be33-32a5-4925-afbd-f0b864f9e63d', N'Lầu 1', N'eba29420-a8db-455c-86b0-b325a1da4e1e', NULL, 110.05, NULL, N'đ', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), N'f6f03971-5a01-47ce-869a-c3f63d70fbb9')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'5bb3eeab-8c35-43c2-93ae-f5bb9e72260a', N' Lầu trệt', N'be6c6db7-cea1-4275-9b18-2fbcfe9b2353', NULL, 117, NULL, N'đ', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), N'f6f03971-5a01-47ce-869a-c3f63d70fbb9')
GO
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'0d207781-eebb-4a03-96ce-005434963f44', N'Tổ chức công trường', 350000, 1, NULL, NULL, 1, N'Finished')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'a9850299-ea54-4f53-af09-1d4eb528fcd4', N'Vệ sinh cơ bản trước khi bàn giao', 150000, 1, NULL, NULL, 1, N'Finished')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'fec0ca84-5523-4531-b20b-1d70375f3699', N'Xây tường bao quanh công trình', 350000, 1, NULL, NULL, 1, N'Rough')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'f8df9b3d-47aa-4579-8f90-1f0de2112f85', N'Dọn dẹp vệ sinh công trình hàng ngày', 350000, 1, NULL, NULL, 1, N'Finished')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'58543f35-e6f1-4851-b012-2137432a71c2', N'NC ốp len gạch trang trí theo bản vẽ(nếu có)', 350000, 1, NULL, NULL, 1, N'Finished')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'4b82dcfc-6ded-4212-a363-3dd11b74ff6c', N'Bảo vệ công trình', 160000, 1, NULL, NULL, 1, N'Finished')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'63a7953f-a220-4938-8c27-40bddc408617', N'Vệ sinh mặt bằng', 350000, 1, NULL, NULL, 1, N'Rough')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'36c8ea55-6bef-4822-99dc-474fa2242d84', N'Chống thấm sàn vệ sinh, sân thượng', 350000, 1, NULL, NULL, 1, N'Rough')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'6dd8f613-43bc-4ba2-859f-4a6b69440ec1', N'NC lắp đạt bồn nước, máy bơm, và WC', 350000, 1, NULL, NULL, 1, N'Finished')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'37804252-49d1-423f-98fd-4ca4a19c58f0', N'NC sơn nước toàn bộ ngôi nhà', 350000, 1, NULL, NULL, 1, N'Finished')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'de396246-83f6-48f5-b049-76b16134d31b', N'Cốt thép,cofa và đổ bê tông gạch thẻ(cầu thang)', 350000, 1, NULL, NULL, 1, N'Rough')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'4cd9a933-87d6-49e8-bf10-774c01afe235', N'Thi công cọc tiếp địa', 350000, 1, NULL, NULL, 1, N'Rough')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'5a21c01b-954e-40da-93d2-7e1d69013c9e', N'Lợp ngói mái Tole nếu có', 350000, 1, NULL, NULL, 1, N'Rough')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'1413ec55-981b-406e-8e78-83c742c39c9e', N'NC lắp đạt hệ thống điện', 400000, 1, NULL, NULL, 1, N'Finished')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'e267a87e-383f-457d-874e-9acbd42fc1ad', N'Tô các vách', 350000, 1, NULL, NULL, 1, N'Rough')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'66719137-e8b1-4198-9a9e-a40101c77aea', N'Lắp đặt điện âm, ông nước âm', 350000, 1, NULL, NULL, 1, N'Rough')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'98ca966c-d741-4aa3-a982-a420fb08d670', N'Cán nền các tầng lầu', 350000, 1, NULL, NULL, 1, N'Rough')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'17b73a9b-33ee-416b-8307-b46aa4417f90', N'Thi công dây TE theo bản vẽ', 350000, 1, NULL, NULL, 1, N'Rough')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'89587e2c-5a40-4d45-95c7-ba4146e5e4d3', N'Xây tô hoàn thiện mặt tiền', 350000, 1, NULL, NULL, 1, N'Rough')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'd12798bf-c423-415c-b367-bb826793f57f', N'Cốt thép,cofa và đổ bê tông(đáy, nắp hầm, hố ga)', 350000, 1, NULL, NULL, 1, N'Rough')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'c41ad3f6-0ce3-437f-a313-d6268ce4c15f', N'Đào đất móng', 350000, 1, NULL, NULL, 1, N'Rough')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'fb22bbac-f43d-4aee-8fe4-e522aca80398', N'Cốt thép,cofa và đổ bê tông(vách hầm nếu có)', 350000, 1, NULL, NULL, 1, N'Rough')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'5ecffef5-6441-437c-903b-ed469e4a819a', N'Đập, cắt đầu cọc BTCT', 350000, 1, NULL, NULL, 1, N'Rough')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'47b540b2-8d41-45a9-9685-f62e67ddbe0d', N'NC lát gạch, ốp len chân tường', 350000, 1, NULL, NULL, 1, N'Finished')
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag], [Type]) VALUES (N'bc3b0032-1c54-4496-aa18-f7fcb9f9e147', N'Cốt thép,cofa và đổ bê tông(sàn các lầu, thượng)', 350000, 1, NULL, NULL, 1, N'Rough')
GO
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'4232c02f-912c-41c9-aaef-0101a39de9fd', N'e0e57083-de54-4be8-aa42-5d15adb1ed92', N'ca5f7a58-7184-468c-9ce5-dcce3525e5e9', N'Dây cáp truyền hình MPE', NULL, 13000, N'cuộn', N'305m/cuộn', N'cuộn', NULL, NULL, NULL, NULL, N'vnđ', 1, N'499f0974-2a9b-440c-877f-e1e2f5cebfec')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'6641641b-8808-410b-a4c9-0545b0540b07', N'0cbd8027-cf5e-4849-b5c1-a7a537f0c55a', N'fb2f4d82-f30e-4963-967a-156f763deb94', N'Ngói tráng men Hàn Quốc	', NULL, 45000, N'viên', N'28cm x 28cm', N'Hình vuông', NULL, N'Ngói tráng men công nghệ Hàn Quốc, có khả năng chống rêu mốc, bền màu theo thời gian.	', NULL, NULL, N'vnđ ', 1, N'eb96ae7b-7d96-4aff-850c-16d618e37ee4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'8df2018b-1f82-4dd5-902e-066550bb7774', N'd8b48345-6fd0-45e6-bdcc-705dc45d1f18', N'4b352083-96e9-498f-a9a9-627b26ac86a7', N'Cửa mặt tiền chính', NULL, 1850000, N'm2', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'616cd71f-20ca-404e-9e34-14189b6ba0ee')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'941ef5fd-2dcc-4703-aab1-0dd4b71c0606', N'1ddabadf-7a17-49c0-9e4c-7fb3ffcffaa3', N'f053cee3-1e43-4fb9-bd32-9070d6500c86', N'Máy LASER', NULL, 1950000, N'máy', N'5 tia', N'máy', NULL, NULL, NULL, NULL, N'vnđ', 1, N'f64aa8ed-f0f4-4b9f-856f-05f1c40297d8')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'0849a212-9fb2-43ac-959b-0e926c938289', N'02bc8fd0-79cc-4248-bc4e-65d7deb22831', N'2070cde6-ec5c-4d56-8119-4f75df100e1e', N'Đèn vệ sinh', NULL, 130000, N'cái', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'8d0cfd81-03ee-4a21-8616-8ce398d781d4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'663f76a6-5d13-4fb0-8669-1415824e89e3', N'd8b48345-6fd0-45e6-bdcc-705dc45d1f18', N'4b352083-96e9-498f-a9a9-627b26ac86a7', N'Khóa cửa phòng, cửa chính, ban công , ST', NULL, 350000, N'bộ', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'616cd71f-20ca-404e-9e34-14189b6ba0ee')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'f9c9a292-3023-498a-a128-1b241176cb18', N'e962f51c-b52a-4ac5-b94c-af92a7f0128c', N'62870bcd-3c5c-41db-bfb2-d0abe3092009', N'Dây điện CADIVI', NULL, 444440, N'm', N'100m', N'cuộn', NULL, NULL, NULL, NULL, N'vnđ', 1, N'082dfe19-5250-43dd-ae01-02a34d37fe13')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'114c60bd-0d6b-4292-9d0b-1bb82e00a692', N'c02a7db9-3dd6-434f-9fb9-1890c4209dee', N'ae56ad0c-acee-48b2-bfb0-05196a466d5e', N'Sơn nước trong nhà', NULL, NULL, N'm2', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'f825b65b-d75b-4973-9e1c-b3d317e501b5')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'e21f67ee-3ff4-4bf2-83e9-1e28f2ce1aad', N'1d768413-54f0-48c7-a488-718077d3cfe1', N'fb2f4d82-f30e-4963-967a-156f763deb94', N'Ngói bê tông nhẹ', NULL, 22000, N'viên', N'30cm x 30cm', N'Hình vuông ', NULL, N'Ngói bê tông nhẹ, chịu nhiệt tốt, trọng lượng nhẹ giúp giảm tải trọng mái nhà.	', NULL, NULL, N'vnđ', 1, N'eb96ae7b-7d96-4aff-850c-16d618e37ee4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'4ad54e0c-b6e0-4d9e-83f3-22a8b26103c3', N'd8b48345-6fd0-45e6-bdcc-705dc45d1f18', N'4b352083-96e9-498f-a9a9-627b26ac86a7', N'Cửa đi các phòng', NULL, 3500000, N'bộ', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'616cd71f-20ca-404e-9e34-14189b6ba0ee')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'9b8af4d8-517f-4e59-85de-22e60f953aa9', N'05987962-d932-4108-adee-706240a33272', N'95798bfc-46ad-4208-9de8-b331102ebf22', N'Lan can ban công', NULL, 600000, N'md', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'e82f2b8c-ff36-4858-9fb5-264f6d65379d')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'8961731d-9389-4b9a-a86e-23ad1f8211c5', N'ddf513eb-c9e8-4430-ad57-f9cf7f9178b2', N'fe9b1b34-3fd8-4b71-b748-8abaa17d65fe', N'Xi măng INSEE', NULL, 90000, N'bao', N'Đa dụng', N'hạt', NULL, NULL, NULL, NULL, N'vnđ', 1, N'baf50ad2-8f54-48b7-a240-2a7db6f7a33d')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'049607a3-60a0-4774-a5bb-265cb3953038', N'977ef799-78ff-4ecd-adcc-5cded5fc7b37', N'ae6be775-baad-485d-8c99-3f2143ca5163', N'Len cầu thang ngạch cửa 100', NULL, 160000, N'md', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'909c18df-c79c-45ad-9367-d48791cf43b6')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'790f552e-11c3-4faa-b84b-29b3e69c42ad', N'1d05ed6d-1c55-496a-82e2-bb4176b6361a', N'fb2f4d82-f30e-4963-967a-156f763deb94', N'Ngói bitum phủ sứ	', NULL, 38000, N'm2', N'100cm x 40cm', N'Hình chữ nhật ', NULL, N'Ngói bitum phủ sứ cao cấp, chống chịu được thời tiết khắc nghiệt và bền màu.	', NULL, NULL, N'vnđ', 1, N'eb96ae7b-7d96-4aff-850c-16d618e37ee4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'1998d66a-621f-40e8-a027-2c0fb52cf36f', N'fcaebcbe-acff-42ee-9ce7-20adc7c852ea', N'ae56ad0c-acee-48b2-bfb0-05196a466d5e', N'Sơn epoxy KCC', NULL, 1200000, N'bộ', N'20l', NULL, NULL, N'Sơn epoxy 2 thành phần, chịu lực và mài mòn tốt, chuyên dùng cho sàn công nghiệp và nhà xưởng.	', NULL, NULL, N'vnđ', 1, N'f825b65b-d75b-4973-9e1c-b3d317e501b5')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'ad055095-5e70-470f-a056-34cee45ec359', N'977ef799-78ff-4ecd-adcc-5cded5fc7b37', N'ae6be775-baad-485d-8c99-3f2143ca5163', N'Bậc tam cấp (nếu có)', NULL, 700000, N'm2', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'909c18df-c79c-45ad-9367-d48791cf43b6')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'67550016-828d-4148-9dc7-359349f6cbeb', N'c02a7db9-3dd6-434f-9fb9-1890c4209dee', N'ae56ad0c-acee-48b2-bfb0-05196a466d5e', N'Sơn nước ngoài nhà', NULL, NULL, N'm2', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'f825b65b-d75b-4973-9e1c-b3d317e501b5')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'423cff9d-bdac-4b62-ade6-35f4250ba836', N'0c9f2fb1-845b-4ce0-b73c-6a2228955248', N'776c5f97-f429-4939-b306-acda1b734aa5', N'Ống thoát nước Bình Minh', NULL, 41700, N'm', N'90 x 1,7mm', N'ống', NULL, NULL, NULL, NULL, N'vnđ', 1, N'e146371e-5659-489e-8ac7-7aeb57c23efa')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'e4a4141e-1d78-495f-a5b6-363848e3c011', N'b4ed7d93-7761-4588-beaa-43dc46ac34d3', N'f1f81a08-f128-46b6-b94e-a8d307975bbf', N'Tay vịn cầu thang', NULL, 500000, N'md', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'4646b9a0-99f4-42c7-91c9-a89cf6d2a4f5')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'173e63bb-b9ac-4f16-a9f3-377037cf046e', N'c4a27fd3-b2c3-4a8b-9079-73e5f4e84557', N'fb2f4d82-f30e-4963-967a-156f763deb94', N'Ngói bitum phủ đá', NULL, 35000, N'm2', N'100cm x 34cm', N'Hình vuông', NULL, N'Ngói bitum phủ đá nhập khẩu, màu sắc đa dạng, thích hợp cho biệt thự và các công trình hiện đại.	', NULL, NULL, N'vnđ', 1, N'eb96ae7b-7d96-4aff-850c-16d618e37ee4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'153d12d4-be1e-4aa7-b094-3a612dcb1f38', N'76e59599-f025-4530-8f4d-756ca999484d', N'dd788959-1f87-48d7-9e85-b40a367071f6', N'Vòi xả sen WC', NULL, 1000000, N'cái', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'62eebc1a-bbd1-4a0f-b623-ce40480e8417')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'0e31286e-5423-462a-9144-3c633f211569', N'02bc8fd0-79cc-4248-bc4e-65d7deb22831', N'2070cde6-ec5c-4d56-8119-4f75df100e1e', N'Đèn thép sáng trong phòng, ngoài sân', NULL, 130000, N'cái', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'8d0cfd81-03ee-4a21-8616-8ce398d781d4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'5168e1e4-4bf1-41ba-b1ed-3fe0f4fe2362', N'a962af3b-7bf9-4efd-81bc-ef59c57bb02b', N'ae56ad0c-acee-48b2-bfb0-05196a466d5e', N'Sơn chống thấm Sika', NULL, 620000, N'thùng', N'10l', NULL, NULL, N'Sơn chống thấm gốc xi măng, bảo vệ bề mặt bê tông khỏi nước và ẩm mốc.	', NULL, NULL, N'vnđ', 1, N'f825b65b-d75b-4973-9e1c-b3d317e501b5')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'c8edfd8c-c847-4c26-a2bf-40bd8987b405', N'977ef799-78ff-4ecd-adcc-5cded5fc7b37', N'ae6be775-baad-485d-8c99-3f2143ca5163', N'Mặt tiền tầng trệt', NULL, 900000, N'm2', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'909c18df-c79c-45ad-9367-d48791cf43b6')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'ade263bc-1305-4090-9865-499967d47a14', N'0ed852e5-e829-409c-ac42-584986299d3c', N'ae56ad0c-acee-48b2-bfb0-05196a466d5e', N'Sơn bóng ngoại thất Spec', NULL, 500000, N'thùng ', N'18l', NULL, NULL, N'Sơn bóng ngoại thất chống bụi bẩn, dễ lau chùi và có độ bóng cao, giữ màu lâu bền.	', NULL, NULL, N'vnđ', 1, N'f825b65b-d75b-4973-9e1c-b3d317e501b5')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'388f9ccd-c493-43e0-8cf0-4d86436b6f6f', N'70a56e5f-ae64-4b17-94c8-76da4be65e0b', N'fb2f4d82-f30e-4963-967a-156f763deb94', N'Ngói nhựa chống nóng', NULL, 28000, N'm2', N'100cm x 50cm', N'Hình sóng', NULL, N'Ngói nhựa cách nhiệt cao cấp, giúp giảm nhiệt độ bên trong nhà, bền đẹp theo thời gian.	', NULL, NULL, N'vnđ', 1, N'eb96ae7b-7d96-4aff-850c-16d618e37ee4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'c74df7ce-bcbf-4186-8808-4e5fd12db642', N'6bd1240d-93c0-47cd-b8f7-9445edb57451', N'ae56ad0c-acee-48b2-bfb0-05196a466d5e', N'Sơn lót chống kiềm Nippon	', NULL, 200000, N'thùng', N'17l', NULL, NULL, N'Sơn lót chống kiềm, bảo vệ bề mặt tường khỏi bị ẩm mốc, thấm nước.	', NULL, NULL, N'vnđ', 1, N'f825b65b-d75b-4973-9e1c-b3d317e501b5')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'7bb66fe4-1af4-412e-b516-500ccd90fd7d', N'f850fd4e-cf8a-4775-9cac-d05dccd3dce8', N'297efa40-de4c-47f2-9ba9-1364dcba6788', N'Ống dây điện vega', NULL, 35200, N'ống', N'2.92m', N'ống', NULL, NULL, NULL, NULL, N'vnđ', 1, N'c94c2294-7658-4917-9452-5b3a349ec1b1')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'fb5f4231-3138-47ae-a758-5077d29e11f4', N'977ef799-78ff-4ecd-adcc-5cded5fc7b37', N'ae6be775-baad-485d-8c99-3f2143ca5163', N'Mạch cầu thang ngạch cửa 200', NULL, 700000, N'm2', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'909c18df-c79c-45ad-9367-d48791cf43b6')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'82370574-34a1-4490-be7d-5a1f6a534beb', N'cfa53294-ff12-4d18-9c1a-63194d40de5d', N'fb2f4d82-f30e-4963-967a-156f763deb94', N'Ngói xi măng màu xanh', NULL, 15500, N'viên', N'32cm  x 32cm ', N'Hình vuông	', NULL, N'Ngói xi măng phủ sơn màu xanh, chịu được thời tiết khắc nghiệt, dễ dàng lắp đặt và bảo dưỡng.	', NULL, NULL, N'vnđ', 1, N'eb96ae7b-7d96-4aff-850c-16d618e37ee4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'61e9afdb-c029-4e3c-8065-600117c41e1a', N'76e59599-f025-4530-8f4d-756ca999484d', N'dd788959-1f87-48d7-9e85-b40a367071f6', N'Vòi xả lavabo', NULL, 700000, N'cái', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'62eebc1a-bbd1-4a0f-b623-ce40480e8417')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'98ffb411-09ee-4b68-a2ae-60489e8260b9', N'ff324b13-7525-4fa2-a5fb-ab89a261e800', N'38b980c3-c1b3-4d5e-abc5-f82a8c2ba31f', N'Gạch nền các tầng', NULL, 250000, N'm2', N'600 x 600', NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'fb6da2e0-07be-4009-81cd-d70e5b01110f')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'714f0c84-7241-4fee-828c-61be7ac58651', N'1b3c5381-bedf-4350-9f29-c4e12fb70ca1', N'fb2f4d82-f30e-4963-967a-156f763deb94', N'Ngói Thái Lan màu xám', NULL, 20000, N'viên ', N'30cm x 30cm', N'Hình vuông', NULL, N'Ngói nhập khẩu từ Thái Lan, màu xám hiện đại, chống chịu được gió bão và thời tiết khắc nghiệt.	', NULL, NULL, N'vnđ', 1, N'eb96ae7b-7d96-4aff-850c-16d618e37ee4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'1b6eeabe-ae2c-42f8-8aac-6248562ff486', N'b3fd9918-86f3-46ec-886f-1325dc5db342', N'ae56ad0c-acee-48b2-bfb0-05196a466d5e', N'Sơn PU gỗ OEXA', NULL, 600000, N'bộ', N'4l', NULL, NULL, N'Sơn PU gỗ bóng đẹp, tạo lớp bảo vệ cho các đồ nội thất gỗ, dễ dàng thi công.	', NULL, NULL, N'vnđ', 1, N'f825b65b-d75b-4973-9e1c-b3d317e501b5')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'433a136b-98a9-4ed7-bffd-62d0a030fc9e', N'ac46a8e6-1975-407f-a034-551b32078cfb', N'ae56ad0c-acee-48b2-bfb0-05196a466d5e', N'Sơn giả gỗ Deco Art	', NULL, 750000, N'thùng', N'5l', NULL, NULL, N'Sơn giả gỗ dùng cho các bề mặt gỗ công nghiệp và kim loại, tạo hiệu ứng gỗ tự nhiên, bền màu.	', NULL, NULL, N'vnđ', 1, N'f825b65b-d75b-4973-9e1c-b3d317e501b5')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'fab8040b-5239-4578-9aec-63123f0e585e', N'35d3b37e-5028-48f1-9895-9d853f02c8cf', N'ae56ad0c-acee-48b2-bfb0-05196a466d5e', N'Sơn epoxy chống trơn trượt', NULL, 1500000, N'bộ ', N'20l', NULL, NULL, N'Sơn epoxy chống trơn trượt, thích hợp cho các bề mặt sàn nhà kho, nhà xưởng.	', NULL, NULL, N'vnđ', 1, N'f825b65b-d75b-4973-9e1c-b3d317e501b5')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'7406a0cc-0182-4e9b-aa6c-647d6dcdda59', N'76e59599-f025-4530-8f4d-756ca999484d', N'dd788959-1f87-48d7-9e85-b40a367071f6', N'Chân nâng bồn nước', NULL, NULL, N'cái', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'62eebc1a-bbd1-4a0f-b623-ce40480e8417')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'e55be57b-7d49-4a80-943d-67af18fdaaab', N'deabd2db-f62f-4c45-8980-643fc15f7d01', N'fb2f4d82-f30e-4963-967a-156f763deb94', N'Ngói sứ chống thấm	', NULL, 40000, N'viên ', N'30cm x 30cm', N'Hình vuông', NULL, N'Ngói sứ phủ men chống thấm, không bám rêu, chống mốc và dễ vệ sinh.	', NULL, NULL, N'vnđ', 1, N'eb96ae7b-7d96-4aff-850c-16d618e37ee4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'50d362e6-069d-4615-a5cb-6d1667158658', N'76e59599-f025-4530-8f4d-756ca999484d', N'dd788959-1f87-48d7-9e85-b40a367071f6', N'Bồn nước inox', NULL, NULL, N'cái', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'62eebc1a-bbd1-4a0f-b623-ce40480e8417')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'648953de-132b-49d6-991e-6e72a4cb7533', N'256e70c9-2aa8-4e48-86f5-4eb8ec9d6dbc', N'ae56ad0c-acee-48b2-bfb0-05196a466d5e', N'Sơn nước cao cấp Maxilite	', NULL, 400000, N'thùng', N'18l', NULL, NULL, N'Sơn nước nội thất, có khả năng chống thấm và chống mốc hiệu quả, thích hợp cho khí hậu nhiệt đới.	', NULL, NULL, N'vnđ ', 1, N'f825b65b-d75b-4973-9e1c-b3d317e501b5')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'8c9fb592-22c6-4ae9-8bee-7bc369b60e15', N'd8b48345-6fd0-45e6-bdcc-705dc45d1f18', N'4b352083-96e9-498f-a9a9-627b26ac86a7', N'Khóa cửa WC', NULL, 250000, N'bộ', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'616cd71f-20ca-404e-9e34-14189b6ba0ee')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'0089bed6-832a-464e-874b-7e86d8af62bf', N'872c5421-3b36-4c06-88fd-faa09f34c705', N'fb2f4d82-f30e-4963-967a-156f763deb94', N'Ngói lợp truyền thống', NULL, 12000, N'viên', N'30cm x 30cm', N'	Hình vuông', NULL, N'Ngói truyền thống bằng đất nung, màu đỏ tươi, thích hợp cho các công trình nhà ở, chùa chiền.', NULL, NULL, N'vnđ', 1, N'eb96ae7b-7d96-4aff-850c-16d618e37ee4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'b8b581ea-3353-4786-b007-838bfd41760a', N'7d7a584c-1ad5-4a91-898b-0166748b0abb', N'aa61efd9-482d-4ae8-86c5-8f096c2cd064', N'Thép Việt-Nhật', NULL, 15000, N'kg', N'SD295', N'cuộn', NULL, NULL, NULL, NULL, N'vnđ', 1, N'8d4ee99a-85a1-468c-943b-8a63bf34a4d5')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'b56d86a3-dbe9-4d5c-a661-83fed0df4e44', N'49cdf959-7ba7-43fa-bc63-53b312a3dcd0', N'297efa40-de4c-47f2-9ba9-1364dcba6788', N'Ống dây điện SINO-MPE', NULL, 18000, N'cây', N'D16, dày 1.15mm', N'cây', NULL, NULL, NULL, NULL, N'vnđ', 1, N'c94c2294-7658-4917-9452-5b3a349ec1b1')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'3a6e8cc7-34cf-4d5c-ae4c-84f0a37f48f1', N'05987962-d932-4108-adee-706240a33272', N'95798bfc-46ad-4208-9de8-b331102ebf22', N'Thạch cao trang trí', NULL, 170000, N'm2', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'e82f2b8c-ff36-4858-9fb5-264f6d65379d')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'b662820b-2339-44fa-803f-864b9edcaa7d', N'02bc8fd0-79cc-4248-bc4e-65d7deb22831', N'2070cde6-ec5c-4d56-8119-4f75df100e1e', N'Ổ cắm điện thoại, internet, và cáp', NULL, NULL, N'cái', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'8d0cfd81-03ee-4a21-8616-8ce398d781d4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'2846551d-45d2-4b7f-90be-8bd9acc4e5ce', N'b2cc8909-cc6e-45c0-b6d3-9debf98cc58b', N'ae6be775-baad-485d-8c99-3f2143ca5163', N'Đá Bình Điền 1/2', NULL, 260000, N'm3', N'10 mm x 20mm', N'viên', NULL, NULL, NULL, NULL, N'vnđ', 1, N'4a106337-e216-4768-9d3d-ecb1f3ef5399')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'6d6eca47-b1f6-49a6-87e3-8cd9606a7755', N'76e59599-f025-4530-8f4d-756ca999484d', N'dd788959-1f87-48d7-9e85-b40a367071f6', N'Lavabo + bộ xả', NULL, 1100000, N'cái', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'62eebc1a-bbd1-4a0f-b623-ce40480e8417')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'4730739c-2644-49c8-b352-8e0b50ea9753', N'76e59599-f025-4530-8f4d-756ca999484d', N'dd788959-1f87-48d7-9e85-b40a367071f6', N'Phễu thu sàn, cầu chắn rác', NULL, 150000, N'cái', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'62eebc1a-bbd1-4a0f-b623-ce40480e8417')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'654a406a-e131-4116-864d-8e6c1106c8e8', N'ff324b13-7525-4fa2-a5fb-ab89a261e800', N'38b980c3-c1b3-4d5e-abc5-f82a8c2ba31f', N'Keo chà ron', NULL, NULL, N'kg', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'fb6da2e0-07be-4009-81cd-d70e5b01110f')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'e316f1ca-25f7-4b61-82fd-8f0ec2e27e9f', N'f0554cb3-252f-4a0d-a3fe-f281b227d8c7', N'ae56ad0c-acee-48b2-bfb0-05196a466d5e', N'Sơn cách nhiệt Kova', NULL, 950000, N'thùng', N'20l', NULL, NULL, N'Sơn cách nhiệt hiệu quả, giảm nhiệt độ bên trong công trình, tiết kiệm năng lượng.	', NULL, NULL, N'vnđ ', 1, N'f825b65b-d75b-4973-9e1c-b3d317e501b5')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'82e7baa6-aec9-4a77-a1ee-9000bc7cbd90', N'787502b1-30b2-49a1-b92f-75da8bbeaa3d', N'f053cee3-1e43-4fb9-bd32-9070d6500c86', N'Dàn giáo', NULL, 620000, N'bộ', N'2 khung ,2 giằng chéo', N'bộ', NULL, NULL, NULL, NULL, N'vnđ', 1, N'f64aa8ed-f0f4-4b9f-856f-05f1c40297d8')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'15bcae29-8fb5-475e-bb3c-94aa9128802e', N'bc1daf58-e2d6-4ca0-90f3-fe64f1fe7eb3', N'ec0afa2f-6058-4228-9d7f-79860c2f97c5', N'Cục kê bê tông dầm đà', NULL, 95000, N'viên', N'3.5cm-4cm', N'viên', NULL, NULL, NULL, NULL, N'vnđ', 1, N'750ea00e-d8fb-46b6-9d53-d1cab5bb8909')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'3b615b90-844e-46be-b95e-94d705600033', N'9fbe91ab-c875-4252-b9e9-c8698e891c28', N'fe0fd671-85a0-4ae0-8c90-b595bcdd6851', N'Cọc tiếp địa  mạ đồng', NULL, 16500, N'cọc', N'D16 L=2,4m', N'cọc', NULL, NULL, NULL, NULL, N'vnđ', 1, N'2bb50b0c-905a-4ccd-8798-b130331651ba')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'f4aebe74-1d4b-41ce-bd33-94f8a8c642b7', N'd8b48345-6fd0-45e6-bdcc-705dc45d1f18', N'4b352083-96e9-498f-a9a9-627b26ac86a7', N'Cửa sổ mặt tiền chính', NULL, 1850000, N'm2', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'616cd71f-20ca-404e-9e34-14189b6ba0ee')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'77683b48-c706-4b9e-aadd-95685a5b351b', N'2158c85c-effd-42e1-a3a2-2abfc406e046', N'ae56ad0c-acee-48b2-bfb0-05196a466d5e', N'Sơn chống thấm Mykolor	', NULL, 550000, N'thùng ', N'18l ', NULL, NULL, N'Sơn chống thấm cao cấp cho ngoại thất, chống ẩm mốc và thấm nước cho tường nhà.	', NULL, NULL, N'vnđ', 1, N'f825b65b-d75b-4973-9e1c-b3d317e501b5')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'8b6f4b10-8eeb-49e9-84c6-99f94ddbabe9', N'cebc9c64-6315-45f3-8c74-745efd34a987', N'fb2f4d82-f30e-4963-967a-156f763deb94', N'Ngói kính chịu lực	', NULL, 50000, N'viên ', N'25cm x 25cm', N'Hình vuông', NULL, N'Ngói kính chịu lực cao, cho phép ánh sáng tự nhiên xuyên qua, phù hợp cho các công trình hiện đại.	', NULL, NULL, N'vnđ', 1, N'eb96ae7b-7d96-4aff-850c-16d618e37ee4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'79a5e6ef-63f3-4742-aca6-9c5a77b1204a', N'ff324b13-7525-4fa2-a5fb-ab89a261e800', N'38b980c3-c1b3-4d5e-abc5-f82a8c2ba31f', N'Gạch nền sân thượng, trước và sau', NULL, 180000, N'm2', N'400 x 400', NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'fb6da2e0-07be-4009-81cd-d70e5b01110f')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'c6232b4c-dfbf-46e6-b2c6-9da1e3a2df47', N'c9e8a527-4b7d-42f1-90a4-76599c5b7ecf', N'8178e41f-6ef5-4938-98b1-77560ca62bb0', N'Ván coffa phủ phim', NULL, 315000, N'tấm', N'1220 x 2440mm D12mm', N'tấm', NULL, NULL, NULL, NULL, N'vnđ', 1, N'73fdfe91-f661-45bb-9be2-22a65ab2651c')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'82bab136-1a39-4abf-998d-9e2b562f3a49', N'02bc8fd0-79cc-4248-bc4e-65d7deb22831', N'2070cde6-ec5c-4d56-8119-4f75df100e1e', N'Đèn cầu thang', NULL, 300000, N'cái', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'8d0cfd81-03ee-4a21-8616-8ce398d781d4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'8f24a2a3-b465-473e-846f-a2f264e2f1c2', N'02bc8fd0-79cc-4248-bc4e-65d7deb22831', N'2070cde6-ec5c-4d56-8119-4f75df100e1e', N'MCB, công tắc, ổ cắm', NULL, NULL, N'cái', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'8d0cfd81-03ee-4a21-8616-8ce398d781d4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'93f7f2bb-3ebf-44db-8075-a414fbdcf209', N'76e59599-f025-4530-8f4d-756ca999484d', N'dd788959-1f87-48d7-9e85-b40a367071f6', N'Vòi sân thượng ban công', NULL, 150000, N'cái', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'62eebc1a-bbd1-4a0f-b623-ce40480e8417')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'8af905d4-8f65-4480-a1fc-a4dc820a5dc0', N'1f261e5c-3850-4c3e-a9ac-874dbf8116cb', N'ae56ad0c-acee-48b2-bfb0-05196a466d5e', N'Sơn nước Dulux Inspire', NULL, 450000, N'thùng', N'18l', NULL, NULL, N'Sơn nước ngoại thất chất lượng cao, bảo vệ tường nhà chống lại tác động của thời tiết khắc nghiệt.	', NULL, NULL, N'vnđ', 1, N'f825b65b-d75b-4973-9e1c-b3d317e501b5')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'1f5ed33e-86d4-4f91-8bbb-a4e9cd5d212b', N'28d7781e-2c4e-4c0c-bf03-fec6d7ec2bd3', N'fb2f4d82-f30e-4963-967a-156f763deb94', N'Ngói nhựa PVC cách nhiệt	', NULL, 25000, N'viên', N'35cm x 35cm', N'Hình vuông', NULL, N'Ngói nhựa PVC nhẹ, có khả năng cách nhiệt và chống cháy tốt, phù hợp cho các nhà kho, nhà xưởng.	', NULL, NULL, N'vnđ ', 1, N'eb96ae7b-7d96-4aff-850c-16d618e37ee4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'd210379d-535a-4929-904f-a6e01932ede3', N'b4ed7d93-7761-4588-beaa-43dc46ac34d3', N'f1f81a08-f128-46b6-b94e-a8d307975bbf', N'Lan can cầu thang', NULL, 550000, N'md', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'4646b9a0-99f4-42c7-91c9-a89cf6d2a4f5')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'fb42f86c-6d24-4210-8855-a8c87eb5690f', N'05987962-d932-4108-adee-706240a33272', N'95798bfc-46ad-4208-9de8-b331102ebf22', N'Tay vịn cho lan can ban công', NULL, 300000, N'md', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'e82f2b8c-ff36-4858-9fb5-264f6d65379d')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'88ca5400-56d3-40db-8aac-a91be85c325c', N'9b862787-2bd4-44c9-9363-a90108cd3cec', N'ae56ad0c-acee-48b2-bfb0-05196a466d5e', N'Sơn ngoại thất Joton	', NULL, 480000, N'thùng', N'18l', NULL, NULL, N'Sơn ngoại thất chống thấm cao cấp, bền màu với thời gian và chịu được thời tiết khắc nghiệt.	', NULL, NULL, N'vnđ', 1, N'f825b65b-d75b-4973-9e1c-b3d317e501b5')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'dc17f6bf-5f30-4cc5-a2bd-a94141d08493', N'ff324b13-7525-4fa2-a5fb-ab89a261e800', N'38b980c3-c1b3-4d5e-abc5-f82a8c2ba31f', N'Gạch ốp tường WC', NULL, 170000, N'm2', N'300 x 600', NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'fb6da2e0-07be-4009-81cd-d70e5b01110f')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'5854b9e2-a97f-4d41-9dd5-aab6fc8ad224', N'02bc8fd0-79cc-4248-bc4e-65d7deb22831', N'2070cde6-ec5c-4d56-8119-4f75df100e1e', N'Vỏ tủ điện tổng và tủ điện tầng', NULL, NULL, N'cái', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'8d0cfd81-03ee-4a21-8616-8ce398d781d4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'3efe3bd5-5f5d-4d55-a358-b0aeca377a52', N'cd865cdb-1458-4b87-ba7d-1c77b7048729', N'fb2f4d82-f30e-4963-967a-156f763deb94', N'Ngói sóng lớn đỏ', NULL, 14000, N'viên ', N'30cm x 40cm ', N'Hình sóng ', NULL, N'Ngói sóng lớn bằng đất nung, màu đỏ đậm, tạo hiệu ứng thẩm mỹ đẹp cho các công trình cổ điển.	', NULL, NULL, N'vnđ', 1, N'eb96ae7b-7d96-4aff-850c-16d618e37ee4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'609d031c-a06a-4e6e-981e-b428ff7300e8', N'ba5dcb20-4036-4f4d-b840-6431bc674d68', N'fb2f4d82-f30e-4963-967a-156f763deb94', N'Ngói đất nung màu nâu	', NULL, 17000, N'viên ', N'33cm x 33cm', N'Hình vuông', NULL, N'Ngói đất nung màu nâu truyền thống, phù hợp cho các công trình nhà cổ và biệt thự.	', NULL, NULL, N'vnđ', 1, N'eb96ae7b-7d96-4aff-850c-16d618e37ee4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'cd330088-f401-4ce8-9b9c-b60b9ebb9ad5', N'4674adf8-75fa-47e1-b27a-6ca23b102d88', N'fb2f4d82-f30e-4963-967a-156f763deb94', N'Ngói', NULL, 174000, N'viên', N'40 x40', N'viên', NULL, NULL, NULL, NULL, N'vnđ', 1, N'eb96ae7b-7d96-4aff-850c-16d618e37ee4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'ddd037ae-3e55-4972-a6a6-ba0784baa0f9', N'49cdf959-7ba7-43fa-bc63-53b312a3dcd0', N'297efa40-de4c-47f2-9ba9-1364dcba6788', N'Ống ruột gà luồn dây điện âm tầng', NULL, 190000, N'cuộn', N'SP16-50m ', N'cuộn', NULL, NULL, NULL, NULL, N'vnđ', 1, N'48c19935-3d8c-4ec1-930f-decdfd96d4df')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'6b4a25fe-d50b-4df9-aece-c1b566385bfb', N'ff324b13-7525-4fa2-a5fb-ab89a261e800', N'38b980c3-c1b3-4d5e-abc5-f82a8c2ba31f', N'Gach nền WC', NULL, 180000, N'm2', N'300 x 600', NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'fb6da2e0-07be-4009-81cd-d70e5b01110f')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'9a217dde-ea01-45a5-bd1a-c40d6f806c35', N'd8b48345-6fd0-45e6-bdcc-705dc45d1f18', N'4b352083-96e9-498f-a9a9-627b26ac86a7', N'Cửa đi WC', NULL, 1850000, N'm2', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'616cd71f-20ca-404e-9e34-14189b6ba0ee')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'fda7bb25-fafb-4042-a86a-c59a96434ed0', N'd8b48345-6fd0-45e6-bdcc-705dc45d1f18', N'4b352083-96e9-498f-a9a9-627b26ac86a7', N'Khung sắt bảo vệ ô cửa sổ', NULL, 450000, N'm2', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'616cd71f-20ca-404e-9e34-14189b6ba0ee')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'80bc455d-28e3-48c3-9ac0-c90ab21e51c4', N'76e59599-f025-4530-8f4d-756ca999484d', N'dd788959-1f87-48d7-9e85-b40a367071f6', N'Máy nước nóng NlMT', NULL, 8300000, N'cái', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'62eebc1a-bbd1-4a0f-b623-ce40480e8417')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'65cf7529-6984-482d-8e70-ca115140c818', N'd7188275-5ed3-4af6-bef5-8d3c38bb6f81', N'ae56ad0c-acee-48b2-bfb0-05196a466d5e', N'Sơn giả đá cẩm thạch	', NULL, 900000, N'thùng', N'15l', NULL, NULL, N'Sơn tạo hiệu ứng giả đá cẩm thạch, dùng cho tường và các bề mặt trang trí nội thất.	', NULL, NULL, N'vnđ', 1, N'f825b65b-d75b-4973-9e1c-b3d317e501b5')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'bc4f2ff3-80c7-4e91-add0-ca9b21085aa4', N'5542a8de-2e1c-4c44-b990-795ab05e3685', N'fb2f4d82-f30e-4963-967a-156f763deb94', N'Ngói xi măng màu đỏ	', NULL, 16000, N'viên', N'32cm x 32cm', N'Hình vuông', NULL, N'Ngói xi măng phủ sơn màu đỏ tươi, độ bền cao, chống thấm tốt, thích hợp cho khí hậu nhiệt đới.	', NULL, NULL, N'vnđ', 1, N'eb96ae7b-7d96-4aff-850c-16d618e37ee4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'23ca40f9-0e9b-4c72-aa7b-ce13dbdc4ae8', N'8a7275b0-6b83-4372-a15a-d120637ccc18', N'ae56ad0c-acee-48b2-bfb0-05196a466d5e', N'Sơn tường Nippon Super Matex	', NULL, 340000, N'thùng', N'17l', NULL, NULL, N'Sơn nước nội thất siêu mịn, bền màu, dễ thi công và vệ sinh.	', NULL, NULL, N'vnđ', 1, N'f825b65b-d75b-4973-9e1c-b3d317e501b5')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'34150479-aaeb-426b-803a-d37c99317e2c', N'606bf3a4-ab3d-4793-9d36-e40ae6b64b95', N'fb2f4d82-f30e-4963-967a-156f763deb94', N'Ngói đất nung cao cấp	', NULL, 18000, N'viên', N'33cm x 33cm', N'Hình vuông', NULL, N'Ngói làm từ đất nung chất lượng cao, có độ bền lớn và khả năng cách nhiệt tốt.	', NULL, NULL, N'vnđ ', 1, N'eb96ae7b-7d96-4aff-850c-16d618e37ee4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'8bcbfc33-ee19-48ca-9b26-d4387c1c2f11', N'76e59599-f025-4530-8f4d-756ca999484d', N'dd788959-1f87-48d7-9e85-b40a367071f6', N'Máy bơm nước', NULL, NULL, N'WC', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'62eebc1a-bbd1-4a0f-b623-ce40480e8417')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'cfce6501-9715-4041-b421-d5641ff64e83', N'20f32cbb-9415-4a0e-9404-2f21c770bda4', N'f88ab54a-2304-4d48-b324-3a99b5f1cc77', N'Chống thấm BestLatex ', NULL, 59000, N'can', N'R114', N'lỏng', NULL, NULL, NULL, NULL, N'vnđ', 1, N'eebabf3a-1cd5-4736-9ee9-3c3e5c3b43ef')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'30513f6d-d33a-4ac9-a191-d6819bf38f89', N'02bc8fd0-79cc-4248-bc4e-65d7deb22831', N'2070cde6-ec5c-4d56-8119-4f75df100e1e', N'Đèn ban công', NULL, 200000, N'cái', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'8d0cfd81-03ee-4a21-8616-8ce398d781d4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'e5db1045-1564-4236-9195-d89a5b3cdc11', N'76e59599-f025-4530-8f4d-756ca999484d', N'dd788959-1f87-48d7-9e85-b40a367071f6', N'Các phụ kiện trong WC', NULL, 500000, N'bộ', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'62eebc1a-bbd1-4a0f-b623-ce40480e8417')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'70ff4ee4-c010-415e-a8a9-d97abbdbf5ad', N'76e59599-f025-4530-8f4d-756ca999484d', N'dd788959-1f87-48d7-9e85-b40a367071f6', N'Vòi xịt WC', NULL, 200000, N'cái', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'62eebc1a-bbd1-4a0f-b623-ce40480e8417')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'7e9ce669-d0ff-4a78-bbba-de6194a93094', N'76e59599-f025-4530-8f4d-756ca999484d', N'dd788959-1f87-48d7-9e85-b40a367071f6', N'Bàn cầu', NULL, 2800000, N'cái', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'62eebc1a-bbd1-4a0f-b623-ce40480e8417')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'8b6c4e26-090b-44f6-bda7-e1701d9f93a1', N'58135989-3c74-4485-a1ab-5ddab11bb6f6', N'74f27d12-b066-4846-8867-8a8a764670fa', N'Cát vàng', NULL, 20000, N'bao', N'Xây dựng', N'bịch', NULL, NULL, NULL, NULL, N'vnđ', 1, N'c8e9092b-31c7-402a-b5f0-561e87a59c0a')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'7ed72eb6-76bf-44b1-a6b6-e5ac7e234b86', N'1b2d1a24-a9d0-44d0-969c-5a6732a519fa', N'7facd531-7971-49b6-bd00-fdaacc147cc3', N'Cáp mạng LS-DVH ', NULL, 9220, N'm', N'CAT5E UTP', N'cuộn', NULL, NULL, NULL, NULL, N'vnđ', 1, N'30b17727-c177-4865-9a75-11ac9b493023')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'e81f8fe4-0c01-43f2-9248-e9d204ecce6f', N'76e59599-f025-4530-8f4d-756ca999484d', N'dd788959-1f87-48d7-9e85-b40a367071f6', N'Ống đồng máy lạnh', NULL, NULL, N'md', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'62eebc1a-bbd1-4a0f-b623-ce40480e8417')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'9bd59976-073b-4908-9277-ec714db074ea', N'05987962-d932-4108-adee-706240a33272', N'95798bfc-46ad-4208-9de8-b331102ebf22', N'Cửa cổng', NULL, 1200000, N'm2', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'e82f2b8c-ff36-4858-9fb5-264f6d65379d')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'2cf7a5eb-5620-4eb0-9e3b-f3eab938f8c4', N'37dcdc28-8521-45d2-86e3-c91ad906cf84', N'fb2f4d82-f30e-4963-967a-156f763deb94', N'Ngói polycarbonate trong', NULL, 55000, N'viên', N'25cm x 25cm', N'Hình vuông', NULL, N'Ngói polycarbonate trong suốt, chịu nhiệt và chống UV, dùng cho các khu vực cần lấy sáng tự nhiên.	', NULL, NULL, N'vnđ ', 1, N'eb96ae7b-7d96-4aff-850c-16d618e37ee4')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'8419526f-43f9-4fc7-a8a1-f91c516655a7', N'06eb8dc0-1ff9-410f-9d05-7f18d4a8f504', N'38b980c3-c1b3-4d5e-abc5-f82a8c2ba31f', N'Gạch Tuynel 4 lỗ', NULL, 2000, N'viên', N'7.5 x 7.5 x 17', N'cục', NULL, NULL, NULL, NULL, N'vnđ', 1, N'c7dbe224-7d2f-4430-b0e9-0e20f50bbcf0')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'4e7f3998-de39-47a0-9f1a-fdc4e5422c0b', N'991193eb-0e32-4479-bd7a-a7cfb30524a7', N'ae56ad0c-acee-48b2-bfb0-05196a466d5e', N'Sơn dầu Jotun Gardex', NULL, 300000, N'lon', N'5l', NULL, NULL, N'Sơn dầu bảo vệ bề mặt gỗ và kim loại, chống rỉ sét và bền màu, dễ lau chùi.	', NULL, NULL, N'vnđ', 1, N'f825b65b-d75b-4973-9e1c-b3d317e501b5')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'eee90754-a582-4539-a7f5-fec3c18f7578', N'58135989-3c74-4485-a1ab-5ddab11bb6f6', N'c3fadd14-d052-4ce0-a599-e187f53356b9', N'Bê tông thương phẩm đá đen', NULL, 1370000, N'm3', N'350 R28', N'lỏng', NULL, NULL, NULL, NULL, N'vnđ', 1, N'5f4009fd-541f-4c3c-b37e-3c151e801cc2')
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable], [MaterialSectionId]) VALUES (N'01267e30-0733-4204-9d24-ff899ea78503', N'05987962-d932-4108-adee-706240a33272', N'95798bfc-46ad-4208-9de8-b331102ebf22', N'Khung sắt mái lấy sáng cầu thang', NULL, 650000, N'm2', NULL, NULL, NULL, NULL, NULL, NULL, N'vnđ', 1, N'e82f2b8c-ff36-4858-9fb5-264f6d65379d')
GO
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'082dfe19-5250-43dd-ae01-02a34d37fe13', N'Dây điện', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'f64aa8ed-f0f4-4b9f-856f-05f1c40297d8', N'Thiết bị', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'c7dbe224-7d2f-4430-b0e9-0e20f50bbcf0', N'Gạch', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'30b17727-c177-4865-9a75-11ac9b493023', N'Cáp mạng', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'616cd71f-20ca-404e-9e34-14189b6ba0ee', N'Cửa đi, cửa sổ', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'eb96ae7b-7d96-4aff-850c-16d618e37ee4', N'Ngói', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'73fdfe91-f661-45bb-9be2-22a65ab2651c', N'Ván coffa phủ phim', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'e82f2b8c-ff36-4858-9fb5-264f6d65379d', N'Hạng mục khác', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'baf50ad2-8f54-48b7-a240-2a7db6f7a33d', N'Xi măng', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'5f4009fd-541f-4c3c-b37e-3c151e801cc2', N'Bê tông', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'eebabf3a-1cd5-4736-9ee9-3c3e5c3b43ef', N'Chống thấm', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'c8e9092b-31c7-402a-b5f0-561e87a59c0a', N'Cát vàng', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'c94c2294-7658-4917-9452-5b3a349ec1b1', N'Ống dây điện', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'2ea7bebc-1d78-4b55-ad35-61486caaec9d', N'Máy LASER', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'e146371e-5659-489e-8ac7-7aeb57c23efa', N'Ống thoát nước', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'8d4ee99a-85a1-468c-943b-8a63bf34a4d5', N'Thép', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'8d0cfd81-03ee-4a21-8616-8ce398d781d4', N'Thiết bị điện', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'4646b9a0-99f4-42c7-91c9-a89cf6d2a4f5', N'Cầu thang', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'2bb50b0c-905a-4ccd-8798-b130331651ba', N'Cọc tiếp địa  mạ đồng', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'f825b65b-d75b-4973-9e1c-b3d317e501b5', N'Sơn Nước', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'62eebc1a-bbd1-4a0f-b623-ce40480e8417', N'Thiết bị vệ sinh/nước', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'750ea00e-d8fb-46b6-9d53-d1cab5bb8909', N'Cục kê bê tông', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'909c18df-c79c-45ad-9367-d48791cf43b6', N'Đá Granite', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'fb6da2e0-07be-4009-81cd-d70e5b01110f', N'Gạch Ốp-Lát', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'48c19935-3d8c-4ec1-930f-decdfd96d4df', N'Ống ruột gà', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'499f0974-2a9b-440c-877f-e1e2f5cebfec', N'Dây cáp điện', NULL)
INSERT [dbo].[MaterialSection] ([Id], [Name], [InsDate]) VALUES (N'4a106337-e216-4768-9d3d-ecb1f3ef5399', N'Đá', NULL)
GO
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'ae56ad0c-acee-48b2-bfb0-05196a466d5e', N'Sơn', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'297efa40-de4c-47f2-9ba9-1364dcba6788', N'Ống dây điện', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'fb2f4d82-f30e-4963-967a-156f763deb94', N'Ngói', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'f88ab54a-2304-4d48-b324-3a99b5f1cc77', N'Hóa chất', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'ae6be775-baad-485d-8c99-3f2143ca5163', N'Đá', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'2070cde6-ec5c-4d56-8119-4f75df100e1e', N'Thiết bị điện', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'4b352083-96e9-498f-a9a9-627b26ac86a7', N'Cửa, cửa sổ', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'8178e41f-6ef5-4938-98b1-77560ca62bb0', N'Ván Coffa', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'ec0afa2f-6058-4228-9d7f-79860c2f97c5', N'Cục kê bê tông', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'74f27d12-b066-4846-8867-8a8a764670fa', N'Cát', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'fe9b1b34-3fd8-4b71-b748-8abaa17d65fe', N'Xi Măng', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'aa61efd9-482d-4ae8-86c5-8f096c2cd064', N'Thép', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'f053cee3-1e43-4fb9-bd32-9070d6500c86', N'Thiết bị', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'f1f81a08-f128-46b6-b94e-a8d307975bbf', N'Cầu thang', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'776c5f97-f429-4939-b306-acda1b734aa5', N'Ống thoát nước', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'95798bfc-46ad-4208-9de8-b331102ebf22', N'Hạng mục khác', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'dd788959-1f87-48d7-9e85-b40a367071f6', N'Thiết bị vệ sinh/nước', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'fe0fd671-85a0-4ae0-8c90-b595bcdd6851', N'Cọc tiếp địa', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'62870bcd-3c5c-41db-bfb2-d0abe3092009', N'Dây điện', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'ca5f7a58-7184-468c-9ce5-dcce3525e5e9', N'Dây cáp', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'c3fadd14-d052-4ce0-a599-e187f53356b9', N'Bê Tông', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'38b980c3-c1b3-4d5e-abc5-f82a8c2ba31f', N'Gạch', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'7facd531-7971-49b6-bd00-fdaacc147cc3', N'Dây Internet', NULL, NULL, 1)
GO
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'14cf420e-4cb0-47e9-9a17-1037a8054485', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632869/ujx7apujvsq6puztdtnq.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'7f3c7712-139a-4444-8a3c-79fc3831259f', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'8a758e9f-daeb-44fa-901d-18dd5737c92e', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632592/zl0t5gi9azkunpslp5rn.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'75723c8d-6c83-445c-83ff-d428895d5173', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'21ce715e-c923-4667-948d-1a7e1301fa81', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632810/igazlpla9u1znh0mjz6p.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'7f3c7712-139a-4444-8a3c-79fc3831259f', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'79d61fd0-467e-490c-a956-1beaaada3067', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632592/zl0t5gi9azkunpslp5rn.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'3321a8e5-bc40-4039-a4bf-d138f379c2b7', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'3ab95940-140c-4de3-8fea-2e1f8cee70f9', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632592/zl0t5gi9azkunpslp5rn.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'442a11bf-d03b-4f32-8c92-e925b39bbfa9', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'22653d23-23f7-416e-856a-31523f8c0caf', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632424/mxhtfrqgja9nwjhwhusl.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'442a11bf-d03b-4f32-8c92-e925b39bbfa9', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'69304787-ea55-481e-abde-40c72bd09e22', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632731/pzzms6xybcyqh2oagbgk.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'7f3c7712-139a-4444-8a3c-79fc3831259f', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'2c953729-b249-4d0f-9a73-44989a38f7b3', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632592/zl0t5gi9azkunpslp5rn.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'420e61d1-cd1f-4d90-8e93-1c2eee62f086', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'adf5a3e5-62c5-41d4-bafa-45fcbd37daf3', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632470/ozithihga2joj5ofjc79.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'd4afdc22-a07a-486f-b521-a9068d391821', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'56511803-cb47-4fb7-bb88-48976b5d62e6', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632592/zl0t5gi9azkunpslp5rn.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'bcf7b1e4-1cf5-42c2-958c-3f05e3c4ce68', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'b2e71b6f-cd3e-4db2-aaf6-4ce60bff3f0b', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632275/doowmllpuglxti4lnipq.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'420e61d1-cd1f-4d90-8e93-1c2eee62f086', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'7eb4626d-a86d-4e65-bf5e-5b3f870c1751', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632275/doowmllpuglxti4lnipq.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'442a11bf-d03b-4f32-8c92-e925b39bbfa9', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'72154c63-0897-40f8-8b0b-6b93d39bca32', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632470/ozithihga2joj5ofjc79.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'bcf7b1e4-1cf5-42c2-958c-3f05e3c4ce68', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'1e43907e-71a3-449a-99fe-6c60842cfd76', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632424/mxhtfrqgja9nwjhwhusl.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'd4afdc22-a07a-486f-b521-a9068d391821', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'dc95fa0e-8abe-4afb-9055-6d5fdef80b7b', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632275/doowmllpuglxti4lnipq.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'64317b00-a7a2-4ffe-b67e-b68e669950eb', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'cb803e62-4e06-453a-a72b-6ff6d645aed1', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632939/oisv9v9mrejofmon7nfs.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'719365ed-a384-4521-a0d0-b0feb7236e98', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'ca0f2900-8ef6-45b7-b7bd-73051fcd519b', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632275/doowmllpuglxti4lnipq.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'75723c8d-6c83-445c-83ff-d428895d5173', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'2afa3b38-0eb5-4c01-9521-7633e261374e', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632424/mxhtfrqgja9nwjhwhusl.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'420e61d1-cd1f-4d90-8e93-1c2eee62f086', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'20a0ae0f-8adf-46eb-87a2-866d088bff46', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632424/mxhtfrqgja9nwjhwhusl.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'3321a8e5-bc40-4039-a4bf-d138f379c2b7', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'd297c8f3-b5dd-49ab-872c-8c4b78be6626', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632470/ozithihga2joj5ofjc79.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'3321a8e5-bc40-4039-a4bf-d138f379c2b7', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'e5a75f2e-e32a-4520-8360-9e6727636d46', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632424/mxhtfrqgja9nwjhwhusl.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'64317b00-a7a2-4ffe-b67e-b68e669950eb', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'd7b71231-d776-43da-ab7a-a2caa3388e37', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632470/ozithihga2joj5ofjc79.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'75723c8d-6c83-445c-83ff-d428895d5173', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'2a46085a-7507-40b5-bae4-a77be074b98a', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632592/zl0t5gi9azkunpslp5rn.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'719365ed-a384-4521-a0d0-b0feb7236e98', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'9265fc3a-23ca-4a4e-bb92-a9cd24c38691', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632470/ozithihga2joj5ofjc79.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'64317b00-a7a2-4ffe-b67e-b68e669950eb', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'a45bd1f5-daf1-4dbe-bd1f-aa8818a5b3cf', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632470/ozithihga2joj5ofjc79.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'442a11bf-d03b-4f32-8c92-e925b39bbfa9', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'1de0b79f-d59f-44bc-ba9a-aac4f5fd0d36', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632682/gqjxlxkmfbs20j5f3hmj.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'7f3c7712-139a-4444-8a3c-79fc3831259f', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'3bf4fb5c-7e4b-4d65-9677-b4f1c3fcfa2f', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632592/zl0t5gi9azkunpslp5rn.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'd4afdc22-a07a-486f-b521-a9068d391821', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'fbdcc29a-8466-4890-ae20-c6af2d949787', NULL, N'Hóa đơn', N'http://res.cloudinary.com/de7pulfdj/image/upload/v1729177421/Contract/Hoa_don_thiet_ke_89b4a065-0ae0-4553-9b7f-fdd534b498f9.jpg', CAST(N'2024-10-17T22:03:43.343' AS DateTime), CAST(N'2024-10-17T22:03:43.343' AS DateTime), NULL, N'89b4a065-0ae0-4553-9b7f-fdd534b498f9')
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'e6e09210-39cd-453f-9e0b-ceaca82affb1', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632470/ozithihga2joj5ofjc79.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'420e61d1-cd1f-4d90-8e93-1c2eee62f086', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'6947be8f-899a-4074-9b17-d0925fc24e65', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632275/doowmllpuglxti4lnipq.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'3321a8e5-bc40-4039-a4bf-d138f379c2b7', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'9d697166-2b07-4cf4-8b66-d7f408ecd9ca', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632905/un4wwp2pnnwwjvllc14n.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'719365ed-a384-4521-a0d0-b0feb7236e98', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'852ad627-2c45-4495-b6b6-d86741476459', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632424/mxhtfrqgja9nwjhwhusl.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'bcf7b1e4-1cf5-42c2-958c-3f05e3c4ce68', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'1ee32da9-9ab8-49a3-9fe9-e039e8ad88ed', N'3905068b-8e24-4fc1-8663-a6697903de08', N'Phối cảnh', N'http://res.cloudinary.com/de7pulfdj/image/upload/v1728839899/HouseDesignDrawing/Phối cảnh_210/14/2024 12:18:18 AM.jpg', CAST(N'2024-10-14T00:18:20.660' AS DateTime), CAST(N'2024-10-14T00:18:20.660' AS DateTime), NULL, NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'd6d64790-1d7a-485c-a614-e283d4966b83', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632275/doowmllpuglxti4lnipq.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'd4afdc22-a07a-486f-b521-a9068d391821', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'7ca65c9c-b6e7-43f6-8aca-e72e923e91d7', N'2e7a32d9-5ffb-4cca-b866-f03f67da256d', N'Phối cảnh', N'http://res.cloudinary.com/de7pulfdj/image/upload/v1728019936/HouseDesignDrawing/Phối cảnh_210/4/2024 12:32:10 PM.png', CAST(N'2024-09-30T12:34:56.000' AS DateTime), CAST(N'2024-09-30T12:34:56.000' AS DateTime), NULL, NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'daf74d11-c01f-4884-9a23-eb868a0ccb50', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632275/doowmllpuglxti4lnipq.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'bcf7b1e4-1cf5-42c2-958c-3f05e3c4ce68', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'abe43d1f-bc48-47b3-9c52-ec322ab330a8', N'97ee6354-4a61-4569-bbde-e5818c49f3e9', N'Phối cảnh', N'https://i1-vnexpress.vnecdn.net/2024/10/14/5563187178137268537a-Israel-3982-1728870302.jpg?w=1020&h=0&q=100&dpr=1&fit=crop&s=3DOqcYA_ykBKuPFFtNVRKA', CAST(N'2024-10-14T10:45:23.430' AS DateTime), CAST(N'2024-10-14T10:45:23.430' AS DateTime), NULL, NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'5d7caeab-a694-48f8-845a-edadbb119d12', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632592/zl0t5gi9azkunpslp5rn.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'64317b00-a7a2-4ffe-b67e-b68e669950eb', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'c4cf9e9e-2a0e-400e-8b2d-f2976e33e3e7', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632424/mxhtfrqgja9nwjhwhusl.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'75723c8d-6c83-445c-83ff-d428895d5173', NULL)
INSERT [dbo].[Media] ([Id], [HouseDesignVersionId], [Name], [Url], [InsDate], [UpsDate], [SubTemplateId], [PaymentId]) VALUES (N'f60f2f07-d0b2-419d-8c7c-fe414ce11cf5', NULL, N'Bản vẽ', N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632275/doowmllpuglxti4lnipq.png', CAST(N'2024-10-16T15:43:23.430' AS DateTime), CAST(N'2024-10-16T15:43:23.430' AS DateTime), N'719365ed-a384-4521-a0d0-b0feb7236e98', NULL)
GO
INSERT [dbo].[Package] ([Id], [PackageTypeId], [PackageName], [Unit], [Price], [Status], [InsDate], [UpsDate]) VALUES (N'abeeddf8-487d-4dea-afb9-173b3feb0338', N'313b205d-8dbd-438c-9935-8b460f3b7237', N'Gói hoàn thiện tiết kiệm', N'm2', 3350000, N'Active', CAST(N'2024-10-08T21:06:22.913' AS DateTime), NULL)
INSERT [dbo].[Package] ([Id], [PackageTypeId], [PackageName], [Unit], [Price], [Status], [InsDate], [UpsDate]) VALUES (N'a1625f79-0af9-4fe6-8021-2f317ee68b0d', N'313b205d-8dbd-438c-9935-8b460f3b7237', N'Gói hoàn thiện tiêu chuẩn', N'm2', 3550000, N'Active', CAST(N'2024-10-08T21:08:06.547' AS DateTime), NULL)
INSERT [dbo].[Package] ([Id], [PackageTypeId], [PackageName], [Unit], [Price], [Status], [InsDate], [UpsDate]) VALUES (N'd32ad002-f9a8-4e0c-a3d6-4f6d5890eb91', N'e4f968ed-74b2-4164-a8be-3f83220be61d', N'Gói thô linh hoạt', N'm2', 2950000, N'Active', CAST(N'2024-10-08T00:14:59.440' AS DateTime), NULL)
INSERT [dbo].[Package] ([Id], [PackageTypeId], [PackageName], [Unit], [Price], [Status], [InsDate], [UpsDate]) VALUES (N'aa5057d8-9b30-4f17-a8ee-7aad655b63cc', N'e4f968ed-74b2-4164-a8be-3f83220be61d', N'Gói thô tiết kiệm', N'm2', 2550000, N'Active', CAST(N'2024-10-03T10:00:00.000' AS DateTime), CAST(N'2024-10-03T10:00:00.000' AS DateTime))
INSERT [dbo].[Package] ([Id], [PackageTypeId], [PackageName], [Unit], [Price], [Status], [InsDate], [UpsDate]) VALUES (N'5245f485-c546-4051-b04e-954fd773811e', N'313b205d-8dbd-438c-9935-8b460f3b7237', N'Gói hoàn thiện linh hoạt', N'm2', 3600000, N'Active', CAST(N'2024-10-08T21:12:01.680' AS DateTime), NULL)
INSERT [dbo].[Package] ([Id], [PackageTypeId], [PackageName], [Unit], [Price], [Status], [InsDate], [UpsDate]) VALUES (N'90c30024-60b6-49af-8b0c-9c8d8ac02e63', N'e4f968ed-74b2-4164-a8be-3f83220be61d', N'Gói thô nâng cao', N'm2', 1800000, N'Active', CAST(N'2024-10-08T00:11:22.083' AS DateTime), NULL)
INSERT [dbo].[Package] ([Id], [PackageTypeId], [PackageName], [Unit], [Price], [Status], [InsDate], [UpsDate]) VALUES (N'bec0c96c-ddce-4d9b-8117-c455c53269d9', N'313b205d-8dbd-438c-9935-8b460f3b7237', N'Gói hoàn thiện nâng cao', N'm2', 3900000, N'Active', CAST(N'2024-10-08T21:08:36.467' AS DateTime), NULL)
INSERT [dbo].[Package] ([Id], [PackageTypeId], [PackageName], [Unit], [Price], [Status], [InsDate], [UpsDate]) VALUES (N'20f42d47-5ea5-469d-a064-fabfa2b2bc15', N'e4f968ed-74b2-4164-a8be-3f83220be61d', N'Gói thô tiêu chuẩn', N'm2', 3450000, N'Active', CAST(N'2024-10-08T00:03:48.897' AS DateTime), NULL)
GO
INSERT [dbo].[PackageDetail] ([Id], [PackageId], [Action], [Type], [InsDate]) VALUES (N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'90c30024-60b6-49af-8b0c-9c8d8ac02e63', N'', N'Rough', CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageDetail] ([Id], [PackageId], [Action], [Type], [InsDate]) VALUES (N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'aa5057d8-9b30-4f17-a8ee-7aad655b63cc', NULL, N'Rough', CAST(N'2024-10-08T00:10:22.087' AS DateTime))
INSERT [dbo].[PackageDetail] ([Id], [PackageId], [Action], [Type], [InsDate]) VALUES (N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'20f42d47-5ea5-469d-a064-fabfa2b2bc15', N'', N'Rough', CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageDetail] ([Id], [PackageId], [Action], [Type], [InsDate]) VALUES (N'1107bb45-9b4b-4064-8035-cea172cc8506', N'd32ad002-f9a8-4e0c-a3d6-4f6d5890eb91', N'', N'Rough', CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageDetail] ([Id], [PackageId], [Action], [Type], [InsDate]) VALUES (N'da7c229e-3a23-4560-a813-d3f4462ed232', N'a1625f79-0af9-4fe6-8021-2f317ee68b0d', N'', N'Finished', CAST(N'2024-10-08T21:08:06.547' AS DateTime))
INSERT [dbo].[PackageDetail] ([Id], [PackageId], [Action], [Type], [InsDate]) VALUES (N'6fe531e6-dc8a-45c0-936f-eed1185a35ef', N'abeeddf8-487d-4dea-afb9-173b3feb0338', N'', N'Finished', CAST(N'2024-10-08T21:06:22.913' AS DateTime))
INSERT [dbo].[PackageDetail] ([Id], [PackageId], [Action], [Type], [InsDate]) VALUES (N'92f02e2e-d98a-4928-a1b4-f02768525f71', N'bec0c96c-ddce-4d9b-8117-c455c53269d9', N'', N'Finished', CAST(N'2024-10-08T21:08:36.467' AS DateTime))
INSERT [dbo].[PackageDetail] ([Id], [PackageId], [Action], [Type], [InsDate]) VALUES (N'312621d3-bad0-4663-979d-fb64af52af86', N'5245f485-c546-4051-b04e-954fd773811e', N'', N'Finished', CAST(N'2024-10-08T21:12:01.680' AS DateTime))
GO
INSERT [dbo].[PackageHouse] ([Id], [PackageId], [DesignTemplateId], [ImgUrl], [InsDate]) VALUES (N'4e54f9ed-8288-474c-9968-3833d4ded5a5', N'bec0c96c-ddce-4d9b-8117-c455c53269d9', N'ec6d9970-ea66-455a-8815-dd69d72ff97f', N'', CAST(N'2024-10-08T21:08:36.467' AS DateTime))
INSERT [dbo].[PackageHouse] ([Id], [PackageId], [DesignTemplateId], [ImgUrl], [InsDate]) VALUES (N'60a50f1c-5d44-4e92-acb5-3a63846ace51', N'a1625f79-0af9-4fe6-8021-2f317ee68b0d', N'ec6d9970-ea66-455a-8815-dd69d72ff97f', N'', CAST(N'2024-10-08T21:08:06.547' AS DateTime))
INSERT [dbo].[PackageHouse] ([Id], [PackageId], [DesignTemplateId], [ImgUrl], [InsDate]) VALUES (N'1bdf5255-c72b-4692-81ff-50f5e0ec402e', N'90c30024-60b6-49af-8b0c-9c8d8ac02e63', N'ec6d9970-ea66-455a-8815-dd69d72ff97f', N'', CAST(N'2024-10-08T00:11:22.090' AS DateTime))
INSERT [dbo].[PackageHouse] ([Id], [PackageId], [DesignTemplateId], [ImgUrl], [InsDate]) VALUES (N'2956464c-c173-4980-896f-8b47047f54d8', N'abeeddf8-487d-4dea-afb9-173b3feb0338', N'ec6d9970-ea66-455a-8815-dd69d72ff97f', N'', CAST(N'2024-10-08T21:06:22.917' AS DateTime))
INSERT [dbo].[PackageHouse] ([Id], [PackageId], [DesignTemplateId], [ImgUrl], [InsDate]) VALUES (N'8ca1375c-939b-49f0-b8fc-b3a2a16e3448', N'aa5057d8-9b30-4f17-a8ee-7aad655b63cc', N'335252c2-f710-430f-ab14-a1412ee29237', NULL, NULL)
INSERT [dbo].[PackageHouse] ([Id], [PackageId], [DesignTemplateId], [ImgUrl], [InsDate]) VALUES (N'19e3ce86-e1d6-42e1-abe8-bd2723d5d129', N'd32ad002-f9a8-4e0c-a3d6-4f6d5890eb91', N'ec6d9970-ea66-455a-8815-dd69d72ff97f', N'', CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageHouse] ([Id], [PackageId], [DesignTemplateId], [ImgUrl], [InsDate]) VALUES (N'32582f2a-6ab2-4933-a8d8-c23461155f82', N'20f42d47-5ea5-469d-a064-fabfa2b2bc15', N'ec6d9970-ea66-455a-8815-dd69d72ff97f', N'', CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageHouse] ([Id], [PackageId], [DesignTemplateId], [ImgUrl], [InsDate]) VALUES (N'975aa8d1-88fc-4db8-b035-e7ef746ee272', N'aa5057d8-9b30-4f17-a8ee-7aad655b63cc', N'7af799a9-02fa-4cd0-b954-86b102840e60', NULL, NULL)
INSERT [dbo].[PackageHouse] ([Id], [PackageId], [DesignTemplateId], [ImgUrl], [InsDate]) VALUES (N'567e4002-65fe-4f31-bee2-f067e7b4488b', N'5245f485-c546-4051-b04e-954fd773811e', N'ec6d9970-ea66-455a-8815-dd69d72ff97f', N'', CAST(N'2024-10-08T21:12:01.680' AS DateTime))
GO
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'7942fc30-b95f-4607-bda0-01522d7a8b76', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'1413ec55-981b-406e-8e78-83c742c39c9e', 400000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'70926bd1-025f-4e91-8e93-05584ed73085', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'63a7953f-a220-4938-8c27-40bddc408617', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'b7200e9b-cbf1-4167-95da-0b0f8f1d99fe', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'fec0ca84-5523-4531-b20b-1d70375f3699', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'dd2a1684-bac8-4baa-bb21-155a9f2abdb6', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'5ecffef5-6441-437c-903b-ed469e4a819a', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'b2bb48f4-85e8-4e1d-8bd5-18b09f1729f3', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'58543f35-e6f1-4851-b012-2137432a71c2', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'69988f3f-3f9a-4844-bf44-18f80ef5c1dc', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'98ca966c-d741-4aa3-a982-a420fb08d670', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'f98165e0-25ad-4a0e-93a7-1b8931e7b6c3', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'd12798bf-c423-415c-b367-bb826793f57f', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'283b7067-6099-4661-a36a-1bdf7707f70b', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'4cd9a933-87d6-49e8-bf10-774c01afe235', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'8ec31def-d6dd-41e8-82a5-1da31ce74323', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'66719137-e8b1-4198-9a9e-a40101c77aea', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'02ce72eb-239c-4f33-b026-1e0493657852', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'4b82dcfc-6ded-4212-a363-3dd11b74ff6c', 160000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'3ba9ab07-ef79-4f64-919f-1e65efe1cb4e', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'd12798bf-c423-415c-b367-bb826793f57f', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'105fa09c-12da-4989-958d-2566d3e1cbb3', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'1413ec55-981b-406e-8e78-83c742c39c9e', 400000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'1db551cc-3560-4d2e-9048-26e3fda91b31', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'f8df9b3d-47aa-4579-8f90-1f0de2112f85', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'9926bcfa-6ee7-4bc2-aa16-289a517ddeaa', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'c41ad3f6-0ce3-437f-a313-d6268ce4c15f', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'7b845995-1bbc-4a31-ae2e-2b192b05f9ca', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'4cd9a933-87d6-49e8-bf10-774c01afe235', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'a5519fa4-d065-4d48-9ef0-2cb64c2c5b46', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'89587e2c-5a40-4d45-95c7-ba4146e5e4d3', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'037e3c20-a4e9-41b6-95df-2e3b4c9001c2', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'a9850299-ea54-4f53-af09-1d4eb528fcd4', 150000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'311191ba-b8e4-4bab-bb89-2ebcfd12f12c', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'37804252-49d1-423f-98fd-4ca4a19c58f0', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'79b40b7e-14f3-4ff0-a546-2f8c807044e7', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'58543f35-e6f1-4851-b012-2137432a71c2', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'64bd6b98-d3b9-430c-bf1d-32d7a12b920e', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'66719137-e8b1-4198-9a9e-a40101c77aea', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'6406955c-c28f-4e71-8fe7-3982b4015b7b', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'f8df9b3d-47aa-4579-8f90-1f0de2112f85', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'3c2803c8-f6f9-458a-a093-3d5bdf1ef7ba', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'37804252-49d1-423f-98fd-4ca4a19c58f0', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'7a9e7bd0-46d9-4569-a4f6-3e38719aa0f3', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'5ecffef5-6441-437c-903b-ed469e4a819a', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'62a04f9e-fe80-4d6b-a455-3e6f7da1d255', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'fec0ca84-5523-4531-b20b-1d70375f3699', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'88a3e5fe-7e6b-45bd-aca6-3f4a03d11d30', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'de396246-83f6-48f5-b049-76b16134d31b', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'16834904-afeb-43af-82ea-4435eed462db', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'63a7953f-a220-4938-8c27-40bddc408617', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'9b104bd6-7a20-4bdb-9bcd-470c9b5af4f7', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'47b540b2-8d41-45a9-9685-f62e67ddbe0d', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'e9356ade-2c8b-498b-b12a-484db87651fe', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'fb22bbac-f43d-4aee-8fe4-e522aca80398', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'd2596676-38f2-47bb-8fdb-4863c38a87bc', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'37804252-49d1-423f-98fd-4ca4a19c58f0', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'7747bfc8-f038-4257-9831-49b0de05a653', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'6dd8f613-43bc-4ba2-859f-4a6b69440ec1', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'121832de-fad4-4554-b0a7-4b8fc6e26bce', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'17b73a9b-33ee-416b-8307-b46aa4417f90', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'c1a9bb17-de90-4cf6-91e9-4d3b7caefbd9', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'4b82dcfc-6ded-4212-a363-3dd11b74ff6c', 160000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'7bc90ad6-91ec-4657-b2f7-4d5e6d14bfd9', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'bc3b0032-1c54-4496-aa18-f7fcb9f9e147', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'1b2944b8-b479-4dc9-b975-50a6eda1c9e3', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'66719137-e8b1-4198-9a9e-a40101c77aea', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'79188b14-c7bd-4710-9166-5290a7932af8', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'5a21c01b-954e-40da-93d2-7e1d69013c9e', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'a2c6b8a1-4fda-47a8-b96f-59da242f85ea', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'36c8ea55-6bef-4822-99dc-474fa2242d84', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'd9521cb2-6d68-4caa-9cb5-5b7619843557', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'bc3b0032-1c54-4496-aa18-f7fcb9f9e147', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'5954fb52-8e72-4266-9350-5b8cc7d33178', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'5a21c01b-954e-40da-93d2-7e1d69013c9e', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'dd52061c-1cfc-414c-b06f-5d2365c5d4ea', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'17b73a9b-33ee-416b-8307-b46aa4417f90', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'c0a1209e-dcf7-41c7-bd99-637fba1a2069', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'de396246-83f6-48f5-b049-76b16134d31b', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'0f982e6b-4a73-42d3-b032-6382a5c6feb1', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'bc3b0032-1c54-4496-aa18-f7fcb9f9e147', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'e7c193e0-6d56-4ebe-9dc8-686965cd55dc', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'fec0ca84-5523-4531-b20b-1d70375f3699', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'fc4e4c36-fbf7-4115-a684-6a1d8cbcf123', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'c41ad3f6-0ce3-437f-a313-d6268ce4c15f', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'954ce44b-f04c-466b-8c4f-6cb1e2d9e229', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'47b540b2-8d41-45a9-9685-f62e67ddbe0d', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'fece79ba-a71f-4259-842c-6d12ee386cfc', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'47b540b2-8d41-45a9-9685-f62e67ddbe0d', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'f952d8da-64ed-49b0-8e00-6fbc02f502ca', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'17b73a9b-33ee-416b-8307-b46aa4417f90', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'12442833-9e7b-4bfd-a364-71ab6a3da0b1', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'66719137-e8b1-4198-9a9e-a40101c77aea', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'7aa21579-48ae-41c2-a624-73203eb4ae5a', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'fb22bbac-f43d-4aee-8fe4-e522aca80398', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'2f6b446a-b826-4b72-8fc7-74342e19f5ba', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'58543f35-e6f1-4851-b012-2137432a71c2', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'4bdd68bb-1127-44e7-86e9-7749acde7c04', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'd12798bf-c423-415c-b367-bb826793f57f', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'7984c5a5-7842-4dd6-8b70-7ddc7d17d34e', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'36c8ea55-6bef-4822-99dc-474fa2242d84', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'414496c2-07af-4c68-98c7-80ea113d2450', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'e267a87e-383f-457d-874e-9acbd42fc1ad', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'8126dc54-9e2c-4cef-b5e7-8255badf7c8a', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'36c8ea55-6bef-4822-99dc-474fa2242d84', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'1f6ffb97-4775-4b66-9db6-8271f1c5ae5e', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'5a21c01b-954e-40da-93d2-7e1d69013c9e', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'e8125fec-8b86-4921-a2f1-88a5a67b5e40', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'4cd9a933-87d6-49e8-bf10-774c01afe235', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'444d8591-32a3-4389-8a6b-8950bd07872f', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'c41ad3f6-0ce3-437f-a313-d6268ce4c15f', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'd480e5cb-048b-4b65-87cf-8cf51b432b5c', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'4b82dcfc-6ded-4212-a363-3dd11b74ff6c', 160000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'ae62c30d-c610-461c-960e-8e25ca0b39fd', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'bc3b0032-1c54-4496-aa18-f7fcb9f9e147', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'2287603f-14ec-4721-9b67-8e7f01c094db', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'17b73a9b-33ee-416b-8307-b46aa4417f90', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'7f383823-ef5a-452f-a88d-8fa55c82a720', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'f8df9b3d-47aa-4579-8f90-1f0de2112f85', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'ca27ceed-b49e-4de6-93bd-91bd07bcbba7', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'e267a87e-383f-457d-874e-9acbd42fc1ad', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'09c0a3d7-9e1d-4586-b157-94d40fb32312', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'e267a87e-383f-457d-874e-9acbd42fc1ad', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'29bcfa8b-c514-4046-bbe2-95787eeace6b', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'58543f35-e6f1-4851-b012-2137432a71c2', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'8b0b0f3e-06ea-405c-800b-98e8c1f0a540', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'4b82dcfc-6ded-4212-a363-3dd11b74ff6c', 160000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'64ad41ac-1157-41f7-8bd7-99bc91d095e5', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'd12798bf-c423-415c-b367-bb826793f57f', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'364f7669-8e49-438b-b9b1-9b54107e56e3', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'de396246-83f6-48f5-b049-76b16134d31b', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'9a732926-ff59-41f2-a966-9facb9e6a8c6', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'98ca966c-d741-4aa3-a982-a420fb08d670', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'e421a029-b951-470a-8971-a107725e8277', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'0d207781-eebb-4a03-96ce-005434963f44', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'0d18e8c4-527e-4025-8854-a33522a07533', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'6dd8f613-43bc-4ba2-859f-4a6b69440ec1', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'8017b8e4-a65a-4df7-b228-a65b8d1c5f36', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'63a7953f-a220-4938-8c27-40bddc408617', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'4876f707-de5d-4749-9b1e-aa73c83d6080', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'0d207781-eebb-4a03-96ce-005434963f44', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'9512f258-f0c1-471f-b1d3-abf3912dab5a', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'47b540b2-8d41-45a9-9685-f62e67ddbe0d', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'f12b4985-1103-4e0c-8410-ad931311f16c', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'98ca966c-d741-4aa3-a982-a420fb08d670', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'3c07bc15-c216-4e89-8c7c-adf2c022667b', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'fb22bbac-f43d-4aee-8fe4-e522aca80398', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'a765f5c4-61ff-46fc-a9d2-afc62932d43f', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'4cd9a933-87d6-49e8-bf10-774c01afe235', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'bcc526f6-6737-4edd-abe2-afca582aff5a', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'37804252-49d1-423f-98fd-4ca4a19c58f0', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'73d77ca8-b693-406d-9b76-b2a90aeb9166', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'5ecffef5-6441-437c-903b-ed469e4a819a', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'b8f535e8-d41e-4758-84e5-b3a40dbb266a', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'1413ec55-981b-406e-8e78-83c742c39c9e', 400000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'859e594b-dd42-4729-988a-b699281556ef', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'63a7953f-a220-4938-8c27-40bddc408617', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'9b5dad1f-38d3-4ad1-867a-bd1d2dcafdbf', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'5a21c01b-954e-40da-93d2-7e1d69013c9e', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'ae77674b-5713-4d1d-8e2d-be21b0e0a860', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'0d207781-eebb-4a03-96ce-005434963f44', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'54f838dd-928c-472d-a2a3-be72a0f4c61c', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'fb22bbac-f43d-4aee-8fe4-e522aca80398', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'fab144a3-064c-4c8d-9425-c6d27ac5cbbe', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'89587e2c-5a40-4d45-95c7-ba4146e5e4d3', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'4f3e5c43-25af-4188-ae02-cae061c6b469', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'a9850299-ea54-4f53-af09-1d4eb528fcd4', 150000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'582b1200-3d49-438c-aa01-ccc34f000215', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'89587e2c-5a40-4d45-95c7-ba4146e5e4d3', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'80e94e4b-b75d-4711-9a4c-cef91222e13f', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'6dd8f613-43bc-4ba2-859f-4a6b69440ec1', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'2b593003-0157-4f46-ab31-d62a4dcc07c9', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'fec0ca84-5523-4531-b20b-1d70375f3699', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'7995a761-465c-4d09-856d-d7e491b790ab', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'0d207781-eebb-4a03-96ce-005434963f44', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'47feccf4-1948-4e0d-9ab0-d8ac625cb883', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'f8df9b3d-47aa-4579-8f90-1f0de2112f85', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'7d871996-7580-460a-8dde-dadd128476cc', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'98ca966c-d741-4aa3-a982-a420fb08d670', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'40d42c24-7002-4a58-b877-dc52e7e854ef', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'e267a87e-383f-457d-874e-9acbd42fc1ad', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'f7d5cdb4-782c-415e-8395-de30ec218ffb', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'c41ad3f6-0ce3-437f-a313-d6268ce4c15f', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'8dd70f35-6d4e-4d9a-b7d9-e46b85890612', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'89587e2c-5a40-4d45-95c7-ba4146e5e4d3', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'44aaf3d5-c82a-4652-8bb8-e4faf98b1919', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'a9850299-ea54-4f53-af09-1d4eb528fcd4', 150000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'f5d36cb9-9524-45e8-8342-efb848e64958', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', N'5ecffef5-6441-437c-903b-ed469e4a819a', 350000, 1, CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'5ab7db52-ad86-4c9a-b2d4-f119b5d9ba29', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'6dd8f613-43bc-4ba2-859f-4a6b69440ec1', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'898e1a1a-f413-4fcf-b70d-f5779be3da79', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'1413ec55-981b-406e-8e78-83c742c39c9e', 400000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'3f67bece-39c7-4910-b9a6-f9db52bfc300', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'a9850299-ea54-4f53-af09-1d4eb528fcd4', 150000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'3ef43c33-74ff-4a00-aa9a-fa7fbe153676', N'1107bb45-9b4b-4064-8035-cea172cc8506', N'36c8ea55-6bef-4822-99dc-474fa2242d84', 350000, 1, CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'7be616c7-4de9-4a8b-b203-ff8abb981c9e', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', N'de396246-83f6-48f5-b049-76b16134d31b', 350000, 1, CAST(N'2024-10-08T00:11:22.087' AS DateTime))
GO
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'0cbaca09-7b6e-4103-a50e-00335932e128', N'eebabf3a-1cd5-4736-9ee9-3c3e5c3b43ef', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'73030553-73b1-47e4-a29d-066507bbad3a', N'c8e9092b-31c7-402a-b5f0-561e87a59c0a', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'a48c147c-cfa9-43b4-9419-0aa93287fae3', N'082dfe19-5250-43dd-ae01-02a34d37fe13', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'b3b77092-e198-455c-9542-0ec6c310988c', N'2ea7bebc-1d78-4b55-ad35-61486caaec9d', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'162f232b-055d-413d-ac8f-0fad8ac27f5a', N'4646b9a0-99f4-42c7-91c9-a89cf6d2a4f5', N'da7c229e-3a23-4560-a813-d3f4462ed232', CAST(N'2024-10-08T21:08:06.547' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'112aa8e3-f533-492c-a2ff-11a02cb09095', N'8d0cfd81-03ee-4a21-8616-8ce398d781d4', N'da7c229e-3a23-4560-a813-d3f4462ed232', CAST(N'2024-10-08T21:08:06.547' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'fd7f3f41-9cf6-4a52-a7e6-12579389264d', N'e82f2b8c-ff36-4858-9fb5-264f6d65379d', N'312621d3-bad0-4663-979d-fb64af52af86', CAST(N'2024-10-08T21:12:01.680' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'8f5bf419-791b-441a-a718-13e7d8c8757f', N'082dfe19-5250-43dd-ae01-02a34d37fe13', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'7da64b18-9c6a-4c39-97d3-15cb98e981a3', N'baf50ad2-8f54-48b7-a240-2a7db6f7a33d', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'861c93e5-abb9-4821-bfd1-17c0d3bf5215', N'baf50ad2-8f54-48b7-a240-2a7db6f7a33d', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'12bd84de-4f8b-477e-8aca-1e9983f819be', N'4646b9a0-99f4-42c7-91c9-a89cf6d2a4f5', N'92f02e2e-d98a-4928-a1b4-f02768525f71', CAST(N'2024-10-08T21:08:36.467' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'5c5b325a-048c-4a87-8079-218ab60a9e82', N'8d4ee99a-85a1-468c-943b-8a63bf34a4d5', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'd97a789f-c198-4b3a-8194-22b0aeff379f', N'e82f2b8c-ff36-4858-9fb5-264f6d65379d', N'da7c229e-3a23-4560-a813-d3f4462ed232', CAST(N'2024-10-08T21:08:06.547' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'91f9b9d9-60cd-4a88-be7b-2446b38f8ae1', N'30b17727-c177-4865-9a75-11ac9b493023', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'2a8d81e2-5d34-4eb0-8c79-2569a0f4d642', N'4a106337-e216-4768-9d3d-ecb1f3ef5399', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'2cddb905-1973-4655-94bd-2904ce2a6a9f', N'616cd71f-20ca-404e-9e34-14189b6ba0ee', N'6fe531e6-dc8a-45c0-936f-eed1185a35ef', CAST(N'2024-10-08T21:06:22.917' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'ce278c22-3fe7-4deb-9aac-2da29ea444b1', N'909c18df-c79c-45ad-9367-d48791cf43b6', N'da7c229e-3a23-4560-a813-d3f4462ed232', CAST(N'2024-10-08T21:08:06.547' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'2bb32400-e3f0-4937-8461-3070d55d715b', N'909c18df-c79c-45ad-9367-d48791cf43b6', N'312621d3-bad0-4663-979d-fb64af52af86', CAST(N'2024-10-08T21:12:01.680' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'825085e8-1044-4224-964f-31bf42d728d4', N'8d0cfd81-03ee-4a21-8616-8ce398d781d4', N'92f02e2e-d98a-4928-a1b4-f02768525f71', CAST(N'2024-10-08T21:08:36.467' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'2f389eee-7c66-4eae-86ed-3c2153b0cfb1', N'30b17727-c177-4865-9a75-11ac9b493023', N'1107bb45-9b4b-4064-8035-cea172cc8506', CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'fb96e2da-6589-49c4-8b1b-3dfb74c0005b', N'2ea7bebc-1d78-4b55-ad35-61486caaec9d', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'914719f2-c96f-41d2-9ee5-3e0e84daa4d7', N'499f0974-2a9b-440c-877f-e1e2f5cebfec', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'2f760da5-32f1-4a9c-880f-3e193fae526f', N'5f4009fd-541f-4c3c-b37e-3c151e801cc2', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'3c320df4-d0b1-46a8-9b6d-3ec430c39fe9', N'c7dbe224-7d2f-4430-b0e9-0e20f50bbcf0', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'e7cc7fad-c8c3-4c6a-bd46-4138eff5a9d2', N'baf50ad2-8f54-48b7-a240-2a7db6f7a33d', N'1107bb45-9b4b-4064-8035-cea172cc8506', CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'4c26235e-a2a1-4041-bdb9-47aa1009e051', N'62eebc1a-bbd1-4a0f-b623-ce40480e8417', N'312621d3-bad0-4663-979d-fb64af52af86', CAST(N'2024-10-08T21:12:01.680' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'864c8b60-84e5-4a95-9655-47af50a5cc32', N'8d4ee99a-85a1-468c-943b-8a63bf34a4d5', N'1107bb45-9b4b-4064-8035-cea172cc8506', CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'8efe1f48-054e-452e-9216-4a5b869f38df', N'eebabf3a-1cd5-4736-9ee9-3c3e5c3b43ef', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'10fb396d-2837-461f-a577-4e5b1dd0b880', N'f825b65b-d75b-4973-9e1c-b3d317e501b5', N'da7c229e-3a23-4560-a813-d3f4462ed232', CAST(N'2024-10-08T21:08:06.547' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'ac834dcf-e019-45e2-80cc-4ee23aae5ba6', N'48c19935-3d8c-4ec1-930f-decdfd96d4df', N'92f02e2e-d98a-4928-a1b4-f02768525f71', CAST(N'2024-10-08T21:08:36.467' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'a887914c-2884-45b1-acbd-4ee8ab23f087', N'2ea7bebc-1d78-4b55-ad35-61486caaec9d', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'e55c2f50-a276-46aa-b18e-4f7c49697674', N'e82f2b8c-ff36-4858-9fb5-264f6d65379d', N'6fe531e6-dc8a-45c0-936f-eed1185a35ef', CAST(N'2024-10-08T21:06:22.917' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'be738748-6c89-4043-a7fd-51646c557439', N'909c18df-c79c-45ad-9367-d48791cf43b6', N'6fe531e6-dc8a-45c0-936f-eed1185a35ef', CAST(N'2024-10-08T21:06:22.917' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'23d8cbd2-4c53-452a-a810-52119d514493', N'c94c2294-7658-4917-9452-5b3a349ec1b1', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'b338b506-74bf-4616-ad3b-521938425360', N'082dfe19-5250-43dd-ae01-02a34d37fe13', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'93884621-696b-41ec-b119-5c967c5efc5c', N'750ea00e-d8fb-46b6-9d53-d1cab5bb8909', N'1107bb45-9b4b-4064-8035-cea172cc8506', CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'c0e7249e-f6b9-48a4-999d-60e0aa0c851f', N'8d0cfd81-03ee-4a21-8616-8ce398d781d4', N'6fe531e6-dc8a-45c0-936f-eed1185a35ef', CAST(N'2024-10-08T21:06:22.917' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'e7cf2e2d-bc96-4ba0-9b68-61032b9dc2c9', N'eebabf3a-1cd5-4736-9ee9-3c3e5c3b43ef', N'1107bb45-9b4b-4064-8035-cea172cc8506', CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'f1522bd1-718e-4b84-82de-65072ba1a78d', N'4a106337-e216-4768-9d3d-ecb1f3ef5399', N'1107bb45-9b4b-4064-8035-cea172cc8506', CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'9dbd7054-7373-4ec0-bcdf-67cba7ace717', N'48c19935-3d8c-4ec1-930f-decdfd96d4df', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'59620fa7-20c6-4c9c-acca-696e1e941a32', N'e146371e-5659-489e-8ac7-7aeb57c23efa', N'1107bb45-9b4b-4064-8035-cea172cc8506', CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'2b322b58-b1f0-4ec6-8f2b-6f0b7f190862', N'750ea00e-d8fb-46b6-9d53-d1cab5bb8909', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'fab39154-218a-42b2-b018-71b902442fa1', N'c7dbe224-7d2f-4430-b0e9-0e20f50bbcf0', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'd2752ca9-f0e0-41fd-be94-732fc1787a31', N'48c19935-3d8c-4ec1-930f-decdfd96d4df', N'312621d3-bad0-4663-979d-fb64af52af86', CAST(N'2024-10-08T21:12:01.680' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'1ce5cff1-7702-42e4-b989-73326a5f6ec3', N'4a106337-e216-4768-9d3d-ecb1f3ef5399', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'd8335c79-f441-4a22-ada3-74f394492060', N'f825b65b-d75b-4973-9e1c-b3d317e501b5', N'312621d3-bad0-4663-979d-fb64af52af86', CAST(N'2024-10-08T21:12:01.680' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'4fff9db3-1054-4803-85f8-78d489d6a92c', N'750ea00e-d8fb-46b6-9d53-d1cab5bb8909', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'b7bf48ce-09d3-486b-9e0d-7c5b61a26833', N'2bb50b0c-905a-4ccd-8798-b130331651ba', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'ccdb62cf-63e2-4911-9523-81f113b812ea', N'baf50ad2-8f54-48b7-a240-2a7db6f7a33d', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'c70ad7a6-fa3c-4271-a2c5-8286fd77e3a0', N'c94c2294-7658-4917-9452-5b3a349ec1b1', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'5bea0000-cbdd-467e-84fe-828f8ad757dd', N'c8e9092b-31c7-402a-b5f0-561e87a59c0a', N'1107bb45-9b4b-4064-8035-cea172cc8506', CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'f79ec746-a96f-444f-bdc1-82dc6f75032b', N'499f0974-2a9b-440c-877f-e1e2f5cebfec', N'1107bb45-9b4b-4064-8035-cea172cc8506', CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'e9b5f2d9-ede3-40e6-8b27-85808d51af03', N'eebabf3a-1cd5-4736-9ee9-3c3e5c3b43ef', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'cdd14da0-7b90-4376-9f26-88673ebb0c35', N'48c19935-3d8c-4ec1-930f-decdfd96d4df', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'c31d1c91-d01e-4275-86a9-8c36e82a3e49', N'73fdfe91-f661-45bb-9be2-22a65ab2651c', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'68a9f0d9-c1ac-4d9b-af54-958d532e0554', N'c8e9092b-31c7-402a-b5f0-561e87a59c0a', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'd207b908-f90a-45cc-8413-9769f67bb890', N'e82f2b8c-ff36-4858-9fb5-264f6d65379d', N'92f02e2e-d98a-4928-a1b4-f02768525f71', CAST(N'2024-10-08T21:08:36.467' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'36966bd6-1f54-4b36-8aa1-9bad7e9408b0', N'c8e9092b-31c7-402a-b5f0-561e87a59c0a', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'a6812dd3-0c64-4ddb-bb2b-9db53e646ee1', N'eb96ae7b-7d96-4aff-850c-16d618e37ee4', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'189ed6b5-1e63-49b5-889d-9f2598f05067', N'5f4009fd-541f-4c3c-b37e-3c151e801cc2', N'1107bb45-9b4b-4064-8035-cea172cc8506', CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'49897bd4-b1c8-426f-aec8-a2ab97ed4f23', N'48c19935-3d8c-4ec1-930f-decdfd96d4df', N'6fe531e6-dc8a-45c0-936f-eed1185a35ef', CAST(N'2024-10-08T21:06:22.917' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'7fddbd72-c5f9-4261-ad7e-a32114999133', N'5f4009fd-541f-4c3c-b37e-3c151e801cc2', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'2399eb3a-28f0-4146-be23-a61ce72716f2', N'f64aa8ed-f0f4-4b9f-856f-05f1c40297d8', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'863208ec-2380-476d-a724-a6808efcdeca', N'4646b9a0-99f4-42c7-91c9-a89cf6d2a4f5', N'312621d3-bad0-4663-979d-fb64af52af86', CAST(N'2024-10-08T21:12:01.680' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'eebeb85e-92d8-4b10-a522-a93fae0fcde2', N'eb96ae7b-7d96-4aff-850c-16d618e37ee4', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'5a7c27de-db70-45ce-94ce-ab2e38cbdba8', N'909c18df-c79c-45ad-9367-d48791cf43b6', N'92f02e2e-d98a-4928-a1b4-f02768525f71', CAST(N'2024-10-08T21:08:36.467' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'49854350-52e8-4d7d-b7d2-af61a59f0a15', N'5f4009fd-541f-4c3c-b37e-3c151e801cc2', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'6c12d5ae-96ee-4e3a-9e93-b43dede62ecb', N'c94c2294-7658-4917-9452-5b3a349ec1b1', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'ec0c658e-b649-4697-b186-b671bb1e0b6a', N'c7dbe224-7d2f-4430-b0e9-0e20f50bbcf0', N'1107bb45-9b4b-4064-8035-cea172cc8506', CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'13f48fac-2c50-4abb-a233-b6f28de22e23', N'082dfe19-5250-43dd-ae01-02a34d37fe13', N'1107bb45-9b4b-4064-8035-cea172cc8506', CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'4fd16695-50d1-4053-85f9-ba99852a0664', N'e146371e-5659-489e-8ac7-7aeb57c23efa', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'478711c2-b0be-4c3c-ad09-bb3114914195', N'616cd71f-20ca-404e-9e34-14189b6ba0ee', N'92f02e2e-d98a-4928-a1b4-f02768525f71', CAST(N'2024-10-08T21:08:36.467' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'a109fe30-4003-4bcb-b39d-bf63107d2108', N'4646b9a0-99f4-42c7-91c9-a89cf6d2a4f5', N'6fe531e6-dc8a-45c0-936f-eed1185a35ef', CAST(N'2024-10-08T21:06:22.917' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'fa66f99c-506f-460b-9277-c00563e8b4f3', N'eb96ae7b-7d96-4aff-850c-16d618e37ee4', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'c3dfdf25-1487-444e-be08-c01eca04528c', N'62eebc1a-bbd1-4a0f-b623-ce40480e8417', N'da7c229e-3a23-4560-a813-d3f4462ed232', CAST(N'2024-10-08T21:08:06.547' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'ce00ae98-6ecf-47cd-9e39-c1a133bcb343', N'f64aa8ed-f0f4-4b9f-856f-05f1c40297d8', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'5a8d5f80-cb36-44a4-89df-c26caccc6b1a', N'e146371e-5659-489e-8ac7-7aeb57c23efa', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'751e9de4-8f43-488e-82b8-c4a0bd4c6c3b', N'8d0cfd81-03ee-4a21-8616-8ce398d781d4', N'312621d3-bad0-4663-979d-fb64af52af86', CAST(N'2024-10-08T21:12:01.680' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'672183fa-2ac5-42b0-adfc-c71f9daef757', N'616cd71f-20ca-404e-9e34-14189b6ba0ee', N'312621d3-bad0-4663-979d-fb64af52af86', CAST(N'2024-10-08T21:12:01.680' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'c0489a52-6472-485f-9cee-d73f52007ea4', N'8d4ee99a-85a1-468c-943b-8a63bf34a4d5', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'19cef76c-8679-4151-8167-d89f3c309aef', N'616cd71f-20ca-404e-9e34-14189b6ba0ee', N'da7c229e-3a23-4560-a813-d3f4462ed232', CAST(N'2024-10-08T21:08:06.547' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'15349448-8a66-431a-a46a-d9c6a3e623fd', N'e146371e-5659-489e-8ac7-7aeb57c23efa', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'6936e736-d565-454a-b6c7-da44aca0cdec', N'f825b65b-d75b-4973-9e1c-b3d317e501b5', N'6fe531e6-dc8a-45c0-936f-eed1185a35ef', CAST(N'2024-10-08T21:06:22.917' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'2621fb49-46e5-4dbb-a714-dbd7095ab42f', N'4a106337-e216-4768-9d3d-ecb1f3ef5399', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'7965df7c-cb06-460a-9cc3-e3ccac6ae7a8', N'30b17727-c177-4865-9a75-11ac9b493023', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'2fb63371-c16d-47e8-bcbe-e9b64df16279', N'f64aa8ed-f0f4-4b9f-856f-05f1c40297d8', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'f340541f-a2ef-4320-b176-eafc250a9323', N'eb96ae7b-7d96-4aff-850c-16d618e37ee4', N'1107bb45-9b4b-4064-8035-cea172cc8506', CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'6d66ae7d-2a3a-4978-ae5a-eb2b38d83cf1', N'62eebc1a-bbd1-4a0f-b623-ce40480e8417', N'92f02e2e-d98a-4928-a1b4-f02768525f71', CAST(N'2024-10-08T21:08:36.467' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'77051d29-dc9b-4212-abd8-ec73fa96d22d', N'48c19935-3d8c-4ec1-930f-decdfd96d4df', N'1107bb45-9b4b-4064-8035-cea172cc8506', CAST(N'2024-10-08T00:14:59.440' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'c8d56cf8-c9f9-4063-81bb-ecb9f9f91706', N'62eebc1a-bbd1-4a0f-b623-ce40480e8417', N'6fe531e6-dc8a-45c0-936f-eed1185a35ef', CAST(N'2024-10-08T21:06:22.917' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'ef937005-8786-4755-8ab4-f0e0ede3eb61', N'f825b65b-d75b-4973-9e1c-b3d317e501b5', N'92f02e2e-d98a-4928-a1b4-f02768525f71', CAST(N'2024-10-08T21:08:36.467' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'4cc8fd0a-22f5-4bb9-93cb-f3186ee772e0', N'499f0974-2a9b-440c-877f-e1e2f5cebfec', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'4f8f5f50-241a-4fad-b08d-f7181ad50a7d', N'8d4ee99a-85a1-468c-943b-8a63bf34a4d5', N'4748e844-a9b6-4246-bd7c-17570d9b9a9d', CAST(N'2024-10-08T00:11:22.087' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'1254a072-d800-417c-b721-f84bf35997b7', N'30b17727-c177-4865-9a75-11ac9b493023', N'fb242fd4-d4a7-4cdd-bdda-a47b092fe012', CAST(N'2024-10-08T00:03:48.900' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'91fa2920-2b5d-41ac-a16b-fbf288dae853', N'c7dbe224-7d2f-4430-b0e9-0e20f50bbcf0', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'5b83edd8-970a-4838-a5bd-fe1f22a76d38', N'48c19935-3d8c-4ec1-930f-decdfd96d4df', N'da7c229e-3a23-4560-a813-d3f4462ed232', CAST(N'2024-10-08T21:08:06.547' AS DateTime))
GO
INSERT [dbo].[PackageQuotation] ([Id], [PackageId], [InitialQuotationId], [Type], [InsDate]) VALUES (N'7c5f1dc7-6b72-4ca0-8b02-0c0ffa9533cb', N'20f42d47-5ea5-469d-a064-fabfa2b2bc15', N'17d0fc6b-51a8-4d4d-aeaf-37b9a14b9db4', N'ROUGH', CAST(N'2024-10-08T13:20:41.523' AS DateTime))
INSERT [dbo].[PackageQuotation] ([Id], [PackageId], [InitialQuotationId], [Type], [InsDate]) VALUES (N'c4ea8d68-5cb9-4bc2-af8b-13d56efcdb61', N'20f42d47-5ea5-469d-a064-fabfa2b2bc15', N'bd31c4e5-e549-42ea-aec7-0f08446f089d', N'FINISHED', CAST(N'2024-10-04T12:40:31.893' AS DateTime))
INSERT [dbo].[PackageQuotation] ([Id], [PackageId], [InitialQuotationId], [Type], [InsDate]) VALUES (N'9a7103d9-f2f1-4a21-90f7-2955a792e8b0', N'a1625f79-0af9-4fe6-8021-2f317ee68b0d', N'bd31c4e5-e549-42ea-aec7-0f08446f089d', N'ROUGH', CAST(N'2024-10-04T12:40:31.887' AS DateTime))
INSERT [dbo].[PackageQuotation] ([Id], [PackageId], [InitialQuotationId], [Type], [InsDate]) VALUES (N'57588302-7e06-47ba-9b22-44a938e54471', N'abeeddf8-487d-4dea-afb9-173b3feb0338', N'f6f03971-5a01-47ce-869a-c3f63d70fbb9', N'ROUGH', CAST(N'2024-09-29T10:00:00.000' AS DateTime))
INSERT [dbo].[PackageQuotation] ([Id], [PackageId], [InitialQuotationId], [Type], [InsDate]) VALUES (N'fb684049-5be1-4e3c-90f0-4b9650c8d21b', N'a1625f79-0af9-4fe6-8021-2f317ee68b0d', N'17d0fc6b-51a8-4d4d-aeaf-37b9a14b9db4', N'FINISHED', CAST(N'2024-10-08T13:20:41.523' AS DateTime))
INSERT [dbo].[PackageQuotation] ([Id], [PackageId], [InitialQuotationId], [Type], [InsDate]) VALUES (N'28034288-7cfc-4f20-a017-784af05aefef', N'abeeddf8-487d-4dea-afb9-173b3feb0338', N'27303a9e-c1ff-4060-a145-bad032564a0a', N'ROUGH', CAST(N'2024-10-04T00:17:13.713' AS DateTime))
INSERT [dbo].[PackageQuotation] ([Id], [PackageId], [InitialQuotationId], [Type], [InsDate]) VALUES (N'c9a06b51-2917-4ce9-bf50-caabb3afa27b', N'20f42d47-5ea5-469d-a064-fabfa2b2bc15', N'4822b76f-5565-4a1a-99c0-370e8d067359', N'FINISHED', CAST(N'2024-10-10T05:19:27.217' AS DateTime))
INSERT [dbo].[PackageQuotation] ([Id], [PackageId], [InitialQuotationId], [Type], [InsDate]) VALUES (N'c8afcc05-fe15-4fbb-9228-edd35beba688', N'aa5057d8-9b30-4f17-a8ee-7aad655b63cc', N'4822b76f-5565-4a1a-99c0-370e8d067359', N'ROUGH', CAST(N'2024-10-10T05:19:27.217' AS DateTime))
GO
INSERT [dbo].[PackageType] ([Id], [Name], [InsDate]) VALUES (N'e4f968ed-74b2-4164-a8be-3f83220be61d', N'ROUGH', NULL)
INSERT [dbo].[PackageType] ([Id], [Name], [InsDate]) VALUES (N'313b205d-8dbd-438c-9935-8b460f3b7237', N'FINISHED', NULL)
GO
INSERT [dbo].[Payment] ([Id], [PaymentTypeId], [BatchPaymentId], [Status], [InsDate], [UpsDate], [TotalPrice]) VALUES (N'e71213c4-2731-4e5b-aa6e-2d7feb733f32', N'2d4a2343-d102-4dc9-8a4f-6647ea397e6c', N'9f29dc1f-c94d-4078-94ad-b3ebf48a6f8a', N'Progress', CAST(N'2024-12-01T12:40:31.853' AS DateTime), CAST(N'2024-12-01T12:40:31.853' AS DateTime), 685552500)
INSERT [dbo].[Payment] ([Id], [PaymentTypeId], [BatchPaymentId], [Status], [InsDate], [UpsDate], [TotalPrice]) VALUES (N'41513564-6f1f-4610-9d01-f938de94a2ef', N'2d4a2343-d102-4dc9-8a4f-6647ea397e6c', N'd165e833-2e68-45ad-a657-a222d01e205c', N'Progress', CAST(N'2024-11-04T12:40:31.853' AS DateTime), CAST(N'2024-11-04T12:40:31.853' AS DateTime), 685552500)
INSERT [dbo].[Payment] ([Id], [PaymentTypeId], [BatchPaymentId], [Status], [InsDate], [UpsDate], [TotalPrice]) VALUES (N'89b4a065-0ae0-4553-9b7f-fdd534b498f9', N'2d4a2343-d102-4dc9-8a4f-6647ea397e6c', N'2b48358a-3d4f-40dc-8ff7-7d7c674a3fc3', N'Paid', CAST(N'2024-10-16T11:17:45.247' AS DateTime), CAST(N'2024-10-16T11:17:45.247' AS DateTime), 47000000)
GO
INSERT [dbo].[PaymentType] ([Id], [Name]) VALUES (N'2db967c9-5928-4a1e-a9ec-5130eb179d6e', N'Hợp đồng thi công nhà ở dân dụng')
INSERT [dbo].[PaymentType] ([Id], [Name]) VALUES (N'2d4a2343-d102-4dc9-8a4f-6647ea397e6c', N'Hợp đồng tư vấn và thiết kế bản vẽ nhà ở dân dụng')
GO
INSERT [dbo].[Project] ([Id], [CustomerId], [Name], [Type], [Status], [InsDate], [UpsDate], [ProjectCode], [Address], [Area]) VALUES (N'b81935a8-4482-43f5-ad68-558abde58d58', N'1e6fa320-6945-4b76-8426-ad6dfdb1ad74', N'Báo giá sơ bộ 10/4/2024 12:40:31 PM', N'ALL', N'Processing', CAST(N'2024-10-04T12:40:31.807' AS DateTime), CAST(N'2024-10-04T12:40:31.807' AS DateTime), N'7IUXJ', N'382 D2, Phường Tân Phú, TP.Thủ Đức', 125)
INSERT [dbo].[Project] ([Id], [CustomerId], [Name], [Type], [Status], [InsDate], [UpsDate], [ProjectCode], [Address], [Area]) VALUES (N'fa91e187-b0c8-46c3-b6d5-747d32bd3540', N'2fcb2c45-4dcc-4f3a-8fe4-1d2ee48fa931', N'Dự án báo giá 09-26-2024', N'ALL', N'Processing', CAST(N'2024-09-26T14:30:00.000' AS DateTime), CAST(N'2024-09-26T14:30:00.000' AS DateTime), N'R84GH', N'45/3 Đường Dương Đình Hội, Phường Phước Long B, Quận 9, TP. Thủ Đức', 252)
INSERT [dbo].[Project] ([Id], [CustomerId], [Name], [Type], [Status], [InsDate], [UpsDate], [ProjectCode], [Address], [Area]) VALUES (N'799b9201-d234-47b9-a14f-7574cc84ef74', N'1e6fa320-6945-4b76-8426-ad6dfdb1ad74', N'Báo giá sơ bộ 10/4/2024 1:24:43 AM', N'ALL', N'Processing', CAST(N'2024-10-04T01:24:43.810' AS DateTime), CAST(N'2024-10-04T01:24:43.810' AS DateTime), N'4PWUI', N'382 D2, Phường Tân Phú, TP.Thủ Đức', 125)
INSERT [dbo].[Project] ([Id], [CustomerId], [Name], [Type], [Status], [InsDate], [UpsDate], [ProjectCode], [Address], [Area]) VALUES (N'b841c593-1d05-4492-9ba2-d485f6d67260', N'2fcb2c45-4dcc-4f3a-8fe4-1d2ee48fa931', N'Dự án báo giá 09-25-2024', N'FINISHED', N'Processing', CAST(N'2024-09-11T09:15:00.000' AS DateTime), CAST(N'2024-09-11T09:15:00.000' AS DateTime), N'G8453', N'78/9 Đường Lê Văn Việt, Phường Hiệp Phú, TP. Thủ Đức', 341)
INSERT [dbo].[Project] ([Id], [CustomerId], [Name], [Type], [Status], [InsDate], [UpsDate], [ProjectCode], [Address], [Area]) VALUES (N'310806e2-876b-48d6-a87a-e534e4ffa647', N'2fcb2c45-4dcc-4f3a-8fe4-1d2ee48fa931', N'Dự án báo giá 09-20-2024', N'ROUGH', N'Ended', CAST(N'2024-09-11T09:15:00.000' AS DateTime), CAST(N'2024-09-11T09:15:00.000' AS DateTime), N'F3421', N'120/5 Đường Nguyễn Xiển, Phường Long Thạnh Mỹ, TP.Thủ Đức', 125)
INSERT [dbo].[Project] ([Id], [CustomerId], [Name], [Type], [Status], [InsDate], [UpsDate], [ProjectCode], [Address], [Area]) VALUES (N'c64e1637-635b-4d03-a68e-ed5f81d3fd57', N'1e6fa320-6945-4b76-8426-ad6dfdb1ad74', N'Dự án báo giá 09-26-2024', N'ALL', N'Processing', CAST(N'2024-09-11T09:15:00.000' AS DateTime), CAST(N'2024-09-11T09:15:00.000' AS DateTime), N'E2245', N'56/4 Đường Nguyễn Duy Trinh, Phường Trường Thạnh, TP.Thủ Đức', 101)
INSERT [dbo].[Project] ([Id], [CustomerId], [Name], [Type], [Status], [InsDate], [UpsDate], [ProjectCode], [Address], [Area]) VALUES (N'eac91a0b-baae-4e00-a21f-f4d05153f447', N'1e6fa320-6945-4b76-8426-ad6dfdb1ad74', N'Báo giá sơ bộ - TP.Thủ Đức', N'ALL', N'Processing', CAST(N'2024-10-04T00:17:12.263' AS DateTime), CAST(N'2024-10-04T00:17:12.263' AS DateTime), N'SJRYA', N'382 D2, Phường Tân Phú, TP.Thủ Đức', 125)
GO
INSERT [dbo].[Promotion] ([Id], [Code], [Value], [InsDate], [StartTime], [Name], [ExpTime], [IsRunning]) VALUES (N'0d941c9f-7dc3-4932-ad01-a1a6591f55e1', N'383QG0UVKO', 5, CAST(N'2024-10-18T16:37:49.823' AS DateTime), CAST(N'2024-10-20T13:06:23.600' AS DateTime), N'Khuyến mãi 20/10 giảm 5% gạch ốp WC khi xây dựng trên 125m2', CAST(N'2024-10-23T13:06:23.600' AS DateTime), 0)
INSERT [dbo].[Promotion] ([Id], [Code], [Value], [InsDate], [StartTime], [Name], [ExpTime], [IsRunning]) VALUES (N'41b828b2-7b06-4389-9003-daac937158dd', NULL, 10, NULL, NULL, N'Giảm 10% cho khách hàng may mắn', NULL, 1)
GO
INSERT [dbo].[QuotationUtilities] ([Id], [UtilitiesItemId], [FinalQuotationId], [InitialQuotationId], [Name], [Coefiicient], [Price], [Description], [InsDate], [UpsDate]) VALUES (N'9329718d-7707-49d5-8cc5-1105ad5c14ac', N'2ec103aa-aa83-4d58-9e85-22a6247f4cd6', NULL, N'17d0fc6b-51a8-4d4d-aeaf-37b9a14b9db4', N'Sàn từ 30m2 ~ 40m2', 0.8, 50000, N'Sàn từ 30m2 ~ 40m2', CAST(N'2024-10-08T13:20:41.523' AS DateTime), CAST(N'2024-10-08T13:20:41.523' AS DateTime))
INSERT [dbo].[QuotationUtilities] ([Id], [UtilitiesItemId], [FinalQuotationId], [InitialQuotationId], [Name], [Coefiicient], [Price], [Description], [InsDate], [UpsDate]) VALUES (N'7cb191cf-3578-40c2-9490-2364d9516622', N'2ec103aa-aa83-4d58-9e85-22a6247f4cd6', NULL, N'27303a9e-c1ff-4060-a145-bad032564a0a', N'', 0.05, 110617000, N'Sàn từ 30m2 ~ 40m2', CAST(N'2024-10-04T00:17:13.770' AS DateTime), CAST(N'2024-10-04T00:17:13.770' AS DateTime))
INSERT [dbo].[QuotationUtilities] ([Id], [UtilitiesItemId], [FinalQuotationId], [InitialQuotationId], [Name], [Coefiicient], [Price], [Description], [InsDate], [UpsDate]) VALUES (N'b1d96a8b-41c4-47de-adf4-280c9bb17b22', N'3b3bd90b-c1e7-47d8-bfdb-bd699eeadd1d', NULL, N'4822b76f-5565-4a1a-99c0-370e8d067359', N'Chi phí ép cừ vây gia cố vách hầm - có nhà liền kề', 0, 3300000, N'Chi phí ép cừ vây gia cố vách hầm - có nhà liền kề', CAST(N'2024-10-10T05:19:27.217' AS DateTime), CAST(N'2024-10-10T05:19:27.217' AS DateTime))
INSERT [dbo].[QuotationUtilities] ([Id], [UtilitiesItemId], [FinalQuotationId], [InitialQuotationId], [Name], [Coefiicient], [Price], [Description], [InsDate], [UpsDate]) VALUES (N'80d56366-12b5-4a1c-8b00-3e9d9a08d8e6', N'2ec103aa-aa83-4d58-9e85-22a6247f4cd6', NULL, N'bd31c4e5-e549-42ea-aec7-0f08446f089d', N'', 0.05, 110617000, N'Sàn từ 30m2 ~ 40m2', CAST(N'2024-10-04T12:40:31.910' AS DateTime), CAST(N'2024-10-04T12:40:31.910' AS DateTime))
GO
INSERT [dbo].[Role] ([Id], [RoleName]) VALUES (N'9959ce96-de26-40a7-b8a7-28a704062e89', N'SalesStaff')
INSERT [dbo].[Role] ([Id], [RoleName]) VALUES (N'7af0d75e-1157-48b4-899d-3196deed5fad', N'DesignStaff')
INSERT [dbo].[Role] ([Id], [RoleName]) VALUES (N'a3bb42ca-de7c-4c9f-8f58-d8175f96688c', N'Manager')
INSERT [dbo].[Role] ([Id], [RoleName]) VALUES (N'789dd57d-0f75-40d1-8366-ef6ab582efc8', N'Customer')
GO
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'3b097855-0c93-4955-a25c-062abd15cf74', N'd8f3d72b-042f-4108-b9ea-3f93666921d2', N'Mái Ngói Kèo Thép (nghiêng 30 độ)', 0.91, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'f3d73d84-c6c5-47e3-9826-075db344d1ec', N'6aa8e2fc-370e-4e6c-8269-d22d2c060e65', N'Thông tầng < 8m2', 1, N'm2        ', CAST(N'2024-09-27T15:35:48.480' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'becab190-4935-4975-81d6-08630e7df54b', N'15b85094-21a2-46e7-95aa-0fba6890ae36', N'Thông tầng < 8m2', 1, N'm2        ', CAST(N'2024-09-27T15:35:37.633' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'40a79928-c9bb-4338-a8fb-0b0e9389e40e', N'a6ce35ee-d19c-40ac-8044-cbecdb54f8d9', N'DTSD < 70m2: độ sâu 1,7m -> 2m', 2.2, N'm2        ', CAST(N'2024-09-27T15:41:49.117' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'06ff2d3d-2f14-4acc-ba2e-0c4ee659ca81', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', N'Móng đơn', 0.2, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'3aae8c18-de92-4100-979b-22df3ff69ddc', N'ee1295bc-a38e-40d0-9f4f-58c88ed604e3', N'Chim sẻ con', 3, N'm2        ', CAST(N'2024-10-14T14:09:18.177' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'287f222c-2d3b-4445-900c-2a32101bec00', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', N'Móng bè, móng 2 phương', 0.6, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'a53e84ae-c98c-46d0-9b4d-355ce942b5dd', N'd8f3d72b-042f-4108-b9ea-3f93666921d2', N'Mái Ngói Kèo Thép (nghiêng 45 độ)', 0.98, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'dbd38b0a-9263-4e52-a23a-35648ca4581d', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', N'Móng bằng', 0.4, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'990dda26-6171-42e7-b142-3a817dfeddb4', N'bd101af5-ac48-43ba-a474-957a20a933bd', N'Mái BTCT lợp ngói (nghiêng 30 độ)', 1.3, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'11f81c2a-bb8f-40eb-9619-3c119901f57f', N'5a000367-77c4-4c16-b533-30cd8d0d6017', N'Thông tầng < 8m2', 1, N'm2        ', CAST(N'2024-09-27T15:35:25.293' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'603a19e3-8f01-492d-bb1b-3d3ea2fefedf', N'57e43b25-e16b-46a6-af8b-99e87c8593b4', N'Mái BTCT', 0.5, N'm2        ', CAST(N'2024-10-07T01:31:31.437' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'144008ad-b489-4e97-8d62-3d7521ed7c71', N'd8f3d72b-042f-4108-b9ea-3f93666921d2', N'Mái Tole', 0.3, N'm2        ', CAST(N'2024-09-27T22:11:53.150' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'b020bcc0-c8f3-438b-a357-3dfd75aadeda', N'9696145f-bebf-4120-8354-c8aef681b351', N'Thông tầng < 8m2', 1, N'm2        ', CAST(N'2024-09-27T15:38:00.537' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'9d80d430-c93b-48b1-9ab5-3f8a4949e1aa', N'd8f3d72b-042f-4108-b9ea-3f93666921d2', N'Mái BTCT lợp ngói (nghiêng 45 độ)', 1.4, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'fa90158e-2d54-43dc-9968-476426ba4f79', N'a6ce35ee-d19c-40ac-8044-cbecdb54f8d9', N'DTSD >= 70m2: độ sâu 1,7m -> 2m', 2, N'm2        ', CAST(N'2024-09-27T15:41:49.113' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'751237b8-2523-4c94-80c8-4a7275f4de9e', N'bd101af5-ac48-43ba-a474-957a20a933bd', N'Mái ngói kèo thép (nghiêng 30 độ)', 0.91, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'872289ad-50af-420a-aa6b-53177d2858f2', N'eab92d07-c140-4a5f-a22d-23cee8a1540e', N'Thông tầng > 8m2', 0.5, N'm2        ', CAST(N'2024-09-27T15:35:43.087' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'61503297-8977-4228-89a3-55670d0f516c', N'eab92d07-c140-4a5f-a22d-23cee8a1540e', N'Thông tầng < 8m2', 1, N'm2        ', CAST(N'2024-09-27T15:35:43.087' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'92fe78e1-86f7-48f1-94c0-57cc859bc449', N'a6ce35ee-d19c-40ac-8044-cbecdb54f8d9', N'DTSD < 70m2: độ sâu 1,3m -> 1,7m', 1.9, N'm2        ', CAST(N'2024-09-27T15:41:49.117' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'1900afa1-5cf6-433e-a66d-5b808c640e62', N'5a000367-77c4-4c16-b533-30cd8d0d6017', N'Thông tầng > 8m2', 0.5, N'm2        ', CAST(N'2024-09-27T15:35:25.257' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'4566fd7c-0914-446c-b45f-5f261f923244', N'15b85094-21a2-46e7-95aa-0fba6890ae36', N'Thông tầng > 8m2', 0.5, N'm2        ', CAST(N'2024-09-27T15:35:37.633' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'fb5fad8e-4e02-4994-9316-670cd861f3ff', N'a6ce35ee-d19c-40ac-8044-cbecdb54f8d9', N'DTSD < 70m2: độ sâu > 2m', 2.4, N'm2        ', CAST(N'2024-09-27T15:41:49.117' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'ad26eb66-871d-4ec4-a9f4-82eecf6c5fad', N'a6ce35ee-d19c-40ac-8044-cbecdb54f8d9', N'DTSD >= 70m2: độ sâu > 2m', 2.2, N'm2        ', CAST(N'2024-09-27T15:41:49.113' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'1579a58c-0bf3-48fe-b73a-860c11c456c6', N'a6ce35ee-d19c-40ac-8044-cbecdb54f8d9', N'DTSD >= 70m2: độ sâu 1,0m -> 1,3m', 1.5, N'm2        ', CAST(N'2024-09-27T15:41:49.113' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'16d73c91-8a66-4b18-b2d6-86b8564888b3', N'1ead7d47-2a8d-41c2-99d7-817ca85146b1', N'Mới con sửa', 22, N'm2        ', CAST(N'2024-10-14T07:18:14.437' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'009e7ab2-3963-4edf-b520-8afb87cb7bef', N'bd101af5-ac48-43ba-a474-957a20a933bd', N'Mái BTCT lợp ngói (nghiêng 45 độ)', 1.4, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'8672e9ec-b175-4114-8b62-95ef3c4d903f', N'6aa8e2fc-370e-4e6c-8269-d22d2c060e65', N'Thông tầng > 8m2', 0.5, N'm2        ', CAST(N'2024-09-27T15:35:48.477' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'24193e3f-bc09-48f8-b7d9-9990f51ef4be', N'd8f3d72b-042f-4108-b9ea-3f93666921d2', N'Mái BTCT nghiêng', 0.7, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'b88899fd-aa23-4a66-ad1e-9a5a4dd03bca', N'40aa0395-a4ef-423c-91f2-11d2889dc306', N'Thông tầng > 8m2', 0.5, N'm2        ', CAST(N'2024-09-27T15:34:24.093' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'bf5ca42f-87d8-4520-b2d4-9cd8e36c6f42', N'bd101af5-ac48-43ba-a474-957a20a933bd', N'Mái BTCT nghiêng', 0.7, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'ce7f06c0-6d51-4058-98a9-9f81d16d0c00', N'57e43b25-e16b-46a6-af8b-99e87c8593b4', N'Mái Tole', 0.3, N'm2        ', CAST(N'2024-10-07T01:31:31.627' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'7e442652-eefc-43b7-918b-a264a10e679d', N'bd101af5-ac48-43ba-a474-957a20a933bd', N'Mái BTCT', 0.5, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'337366dd-f56b-4df5-878b-b17abb616e67', N'ee1295bc-a38e-40d0-9f4f-58c88ed604e3', N'Chim sẻ chị', 6, N'm2        ', CAST(N'2024-10-14T14:09:18.180' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'cb61a178-b341-4c53-8bce-b1dc7289ca91', N'a6ce35ee-d19c-40ac-8044-cbecdb54f8d9', N'DTSD < 70m2: độ sâu 1,0m -> 1,3m', 1.7, N'm2        ', CAST(N'2024-09-27T15:41:49.113' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'259152e0-fa9f-424c-8410-b4013d77137b', N'f996f171-f8c6-4022-9152-fcccc486341e', N'Thông tầng > 8m2', 0.5, N'm2        ', CAST(N'2024-09-27T15:35:31.803' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'708bad20-af57-4ab6-96e9-c22a22273724', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', N'Móng cọc', 0.3, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'27593f3d-f834-4729-8170-ddaa5cb0f747', N'9696145f-bebf-4120-8354-c8aef681b351', N'Thông tầng > 8m2', 0.5, N'm2        ', CAST(N'2024-09-27T15:38:00.537' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'51cd0448-7fba-4630-a96e-dff7405caad3', N'a6ce35ee-d19c-40ac-8044-cbecdb54f8d9', N'DTSD >= 70m2: độ sâu 1,3m -> 1,7m', 1.7, N'm2        ', CAST(N'2024-09-27T15:41:49.113' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'dff5fdc9-2275-4aaf-8d96-e63874ec153f', N'f996f171-f8c6-4022-9152-fcccc486341e', N'Thông tầng < 8m2', 1, N'm2        ', CAST(N'2024-09-27T15:35:31.803' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'78b9ace5-547b-4e42-964d-eb5dc220bd0d', N'40aa0395-a4ef-423c-91f2-11d2889dc306', N'Thông tầng < 8m2', 1, N'm2        ', CAST(N'2024-09-27T15:34:24.203' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'5834624a-37a5-41b4-b014-efa2c34f70c1', N'ee1295bc-a38e-40d0-9f4f-58c88ed604e3', N'Chim sẻ chú', 5, N'm2        ', CAST(N'2024-10-14T14:09:18.180' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'783c52e5-7d4c-4948-8357-f4ea7557c4e2', N'd8f3d72b-042f-4108-b9ea-3f93666921d2', N'Mái BTCT', 0.5, N'm2        ', CAST(N'2024-09-27T22:11:53.113' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'3a09a8da-e768-4b87-befc-f5365291db0c', N'd8f3d72b-042f-4108-b9ea-3f93666921d2', N'Mái BTCT lợp ngói (nghiêng 30 độ)', 1.3, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'cd3d06cc-1f01-4010-b908-fc5d3ef244e9', N'bd101af5-ac48-43ba-a474-957a20a933bd', N'Mái ngói kèo thép (nghiêng 45 độ)', 0.98, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
GO
INSERT [dbo].[SubTemplate] ([Id], [DesignTemplateId], [BuildingArea], [FloorArea], [InsDate], [Size]) VALUES (N'420e61d1-cd1f-4d90-8e93-1c2eee62f086', N'ec6d9970-ea66-455a-8815-dd69d72ff97f', 390.207, 120, NULL, N'R8 x D15 ')
INSERT [dbo].[SubTemplate] ([Id], [DesignTemplateId], [BuildingArea], [FloorArea], [InsDate], [Size]) VALUES (N'bcf7b1e4-1cf5-42c2-958c-3f05e3c4ce68', N'335252c2-f710-430f-ab14-a1412ee29237', 417.8, 130, NULL, N'R10 x D13')
INSERT [dbo].[SubTemplate] ([Id], [DesignTemplateId], [BuildingArea], [FloorArea], [InsDate], [Size]) VALUES (N'7f3c7712-139a-4444-8a3c-79fc3831259f', N'ec6d9970-ea66-455a-8815-dd69d72ff97f', 436.663, 135, NULL, N'R9 x D15 ')
INSERT [dbo].[SubTemplate] ([Id], [DesignTemplateId], [BuildingArea], [FloorArea], [InsDate], [Size]) VALUES (N'd4afdc22-a07a-486f-b521-a9068d391821', N'7af799a9-02fa-4cd0-b954-86b102840e60', 365.7942, 117, NULL, N'R9 x D13 ')
INSERT [dbo].[SubTemplate] ([Id], [DesignTemplateId], [BuildingArea], [FloorArea], [InsDate], [Size]) VALUES (N'719365ed-a384-4521-a0d0-b0feb7236e98', N'335252c2-f710-430f-ab14-a1412ee29237', 385.39952, 120, NULL, N'R10 x D12')
INSERT [dbo].[SubTemplate] ([Id], [DesignTemplateId], [BuildingArea], [FloorArea], [InsDate], [Size]) VALUES (N'64317b00-a7a2-4ffe-b67e-b68e669950eb', N'ec6d9970-ea66-455a-8815-dd69d72ff97f', 365.212, 112, NULL, N'R8 x D14 ')
INSERT [dbo].[SubTemplate] ([Id], [DesignTemplateId], [BuildingArea], [FloorArea], [InsDate], [Size]) VALUES (N'3321a8e5-bc40-4039-a4bf-d138f379c2b7', N'335252c2-f710-430f-ab14-a1412ee29237', 457.375, 143, NULL, N'R11 x D13')
INSERT [dbo].[SubTemplate] ([Id], [DesignTemplateId], [BuildingArea], [FloorArea], [InsDate], [Size]) VALUES (N'75723c8d-6c83-445c-83ff-d428895d5173', N'7af799a9-02fa-4cd0-b954-86b102840e60', 343.2942, 108, NULL, N'R9 x D12 ')
INSERT [dbo].[SubTemplate] ([Id], [DesignTemplateId], [BuildingArea], [FloorArea], [InsDate], [Size]) VALUES (N'442a11bf-d03b-4f32-8c92-e925b39bbfa9', N'7af799a9-02fa-4cd0-b954-86b102840e60', 376.9738, 120, NULL, N'R10 x D12')
GO
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'7d7a584c-1ad5-4a91-898b-0166748b0abb', N'Cty Việt-Nhật', NULL, NULL, NULL, NULL, NULL, 1, N'Thép Việt-Nhật', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'b3fd9918-86f3-46ec-886f-1325dc5db342', N'OEXA Paint Việt Nam', NULL, NULL, NULL, NULL, NULL, 1, N'CT Sơn', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'c02a7db9-3dd6-434f-9fb9-1890c4209dee', N'Kova', NULL, NULL, NULL, NULL, NULL, 1, N'Sơn', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'cd865cdb-1458-4b87-ba7d-1c77b7048729', N'Công ty VLXD Địa Trung Hải', NULL, NULL, NULL, NULL, NULL, 1, N'CTy Ngói', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'fcaebcbe-acff-42ee-9ce7-20adc7c852ea', N'KCC Paint Việt Nam', NULL, NULL, NULL, NULL, NULL, 1, N'CT Sơn', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'2158c85c-effd-42e1-a3a2-2abfc406e046', N'Công ty TNHH Mykolor Paint', NULL, NULL, NULL, NULL, NULL, 1, N'CT Sơn', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'20f32cbb-9415-4a0e-9404-2f21c770bda4', N'Bestmix', NULL, NULL, NULL, NULL, NULL, 1, N'Chống thấm BestLatex ', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'b4ed7d93-7761-4588-beaa-43dc46ac34d3', N'CTTNHH  Bình Gia Phát', NULL, NULL, NULL, NULL, NULL, 1, N'Cầu thang', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'256e70c9-2aa8-4e48-86f5-4eb8ec9d6dbc', N'Maxilite Paint', NULL, NULL, NULL, NULL, NULL, 1, N'CT Sơn', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'49cdf959-7ba7-43fa-bc63-53b312a3dcd0', N'Chinhanelectric', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'ac46a8e6-1975-407f-a034-551b32078cfb', N'Deco Art Paint Việt Nam', NULL, NULL, NULL, NULL, NULL, 1, N'CT Sơn', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'0ed852e5-e829-409c-ac42-584986299d3c', N'Spec Paint', NULL, NULL, NULL, NULL, NULL, 1, N'CT Sơn', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'1b2d1a24-a9d0-44d0-969c-5a6732a519fa', N'Viet Han', NULL, NULL, NULL, NULL, NULL, 1, N'Dây Internet', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'977ef799-78ff-4ecd-adcc-5cded5fc7b37', N'Granite', NULL, NULL, NULL, NULL, NULL, 1, N'Đá Granite', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'e0e57083-de54-4be8-aa42-5d15adb1ed92', N'Minhanhelectric', NULL, NULL, NULL, NULL, NULL, 1, N'Dây cáp', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'58135989-3c74-4485-a1ab-5ddab11bb6f6', N'VLXD Vạn thành công', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'27528179-b5b6-4b5e-837b-62a202a37c10', N'Nhà máy sát thép', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'cfa53294-ff12-4d18-9c1a-63194d40de5d', N'Công ty Ngói Xanh Thành Đạt', NULL, NULL, NULL, NULL, NULL, 1, N'CTy Ngói', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'ba5dcb20-4036-4f4d-b840-6431bc674d68', N'Ngói Đất Nung Châu Á', NULL, NULL, NULL, NULL, NULL, 1, N'CTy Ngói', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'deabd2db-f62f-4c45-8980-643fc15f7d01', N'Công ty Ngói Sứ Hòa Phát', NULL, NULL, NULL, NULL, NULL, 1, N'CTy Ngói', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'02bc8fd0-79cc-4248-bc4e-65d7deb22831', N'Panasonic', NULL, NULL, NULL, NULL, NULL, 1, N'Thiết bị điện', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'0c9f2fb1-845b-4ce0-b73c-6a2228955248', N'Cty Bình Minh', NULL, NULL, NULL, NULL, NULL, 1, N'', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'4674adf8-75fa-47e1-b27a-6ca23b102d88', N'Gạch Xinh', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'd8b48345-6fd0-45e6-bdcc-705dc45d1f18', N'Eurowindow', NULL, NULL, NULL, NULL, NULL, 1, N'Cửa', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'05987962-d932-4108-adee-706240a33272', N'Vạn Phát Hưng', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'1d768413-54f0-48c7-a488-718077d3cfe1', N'Công ty TNHH Bê Tông Nhẹ Vina', NULL, NULL, NULL, NULL, NULL, 1, N'CTy Ngói', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'c4a27fd3-b2c3-4a8b-9079-73e5f4e84557', N'Bitum Đá Phủ Nam Việt', NULL, NULL, NULL, NULL, NULL, 1, N'CTy Ngói', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'cebc9c64-6315-45f3-8c74-745efd34a987', N'Vật Liệu Xây Dựng Kính Hưng Thịnh', NULL, NULL, NULL, NULL, NULL, 1, N'CTy Ngói', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'76e59599-f025-4530-8f4d-756ca999484d', N'Thietbivesinh', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'787502b1-30b2-49a1-b92f-75da8bbeaa3d', N'Quangminhhung', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'c9e8a527-4b7d-42f1-90a4-76599c5b7ecf', N'Somma', NULL, NULL, NULL, NULL, NULL, 1, N'Ván coffa', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'70a56e5f-ae64-4b17-94c8-76da4be65e0b', N'Công ty Phát Triển Xây Dựng ABC', NULL, NULL, NULL, NULL, NULL, 1, N'CTy Ngói', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'5542a8de-2e1c-4c44-b990-795ab05e3685', N'Vật Liệu Xây Dựng An Thịnh', NULL, NULL, NULL, NULL, NULL, 1, N'CTy Ngói', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'06eb8dc0-1ff9-410f-9d05-7f18d4a8f504', N'Nhà máy Tân Uyên - Bình dương', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'1ddabadf-7a17-49c0-9e4c-7fb3ffcffaa3', N'yamasu', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'1f261e5c-3850-4c3e-a9ac-874dbf8116cb', N'Công ty TNHH Dulux Việt Nam', NULL, NULL, NULL, NULL, NULL, 1, N'CT Sơn', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'd7188275-5ed3-4af6-bef5-8d3c38bb6f81', N'Công ty TNHH Sơn Trang Trí Đá VN', NULL, NULL, NULL, NULL, NULL, 1, N'CT Sơn', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'6bd1240d-93c0-47cd-b8f7-9445edb57451', N'Nippon Paint VN', NULL, NULL, NULL, NULL, NULL, 1, N'CT Sơn', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'35d3b37e-5028-48f1-9895-9d853f02c8cf', N'KCC Paint Việt Nam', NULL, NULL, NULL, NULL, NULL, 1, N'CT Sơn', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'b2cc8909-cc6e-45c0-b6d3-9debf98cc58b', N'VLXD Hiệp Hà', NULL, NULL, NULL, NULL, NULL, 1, N'Bê Tông thương phẩn đá đen', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'0cbd8027-cf5e-4849-b5c1-a7a537f0c55a', N'Công ty Ngói Hàn Quốc Vina', NULL, NULL, NULL, NULL, NULL, 1, N'CTy Ngói', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'991193eb-0e32-4479-bd7a-a7cfb30524a7', N'Jotun Paint Việt Nam', NULL, NULL, NULL, NULL, NULL, 1, N'CT Sơn', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'9b862787-2bd4-44c9-9363-a90108cd3cec', N'Joton Paint', NULL, NULL, NULL, NULL, NULL, 1, N'CT Sơn', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'ff324b13-7525-4fa2-a5fb-ab89a261e800', N'Vilacera', NULL, NULL, NULL, NULL, NULL, 1, N'Gạch ốp lát', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'e962f51c-b52a-4ac5-b94c-af92a7f0128c', N'Cadivi', NULL, NULL, NULL, NULL, NULL, 1, N'Dây điện', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'1d05ed6d-1c55-496a-82e2-bb4176b6361a', N'Bitum Sứ Đông Á', NULL, NULL, NULL, NULL, NULL, 1, N'CTy Ngói', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'1b3c5381-bedf-4350-9f29-c4e12fb70ca1', N'Thái Lan Ngói Export', NULL, NULL, NULL, NULL, NULL, 1, N'CTy Ngói', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'9fbe91ab-c875-4252-b9e9-c8698e891c28', N'Chongsetbinhan', NULL, NULL, NULL, NULL, NULL, 1, N'Cọc tiếp địa', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'37dcdc28-8521-45d2-86e3-c91ad906cf84', N'Ngói Polycarbonate Nhật Bản', NULL, NULL, NULL, NULL, NULL, 1, N'CTy Ngói', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'f850fd4e-cf8a-4775-9cac-d05dccd3dce8', N'Diennuocquocdung', NULL, NULL, NULL, NULL, NULL, 1, N'Ống dây điện', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'8a7275b0-6b83-4372-a15a-d120637ccc18', N'Nippon Paint VN', NULL, NULL, NULL, NULL, NULL, 1, N'CT Sơn', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'606bf3a4-ab3d-4793-9d36-e40ae6b64b95', N'Ngói Việt Thành', NULL, NULL, NULL, NULL, NULL, 1, N'CTy Ngói', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'a962af3b-7bf9-4efd-81bc-ef59c57bb02b', N'Sika Việt Nam
', NULL, NULL, NULL, NULL, NULL, 1, N'CT Sơn', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'f0554cb3-252f-4a0d-a3fe-f281b227d8c7', N'Công ty TNHH Sơn Kova', NULL, NULL, NULL, NULL, NULL, 1, N'CT Sơn', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'ddf513eb-c9e8-4430-ad57-f9cf7f9178b2', N'Siam City Cement', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'872c5421-3b36-4c06-88fd-faa09f34c705', N'Công ty VLXD ABC', NULL, NULL, NULL, NULL, NULL, 1, N'CTy Ngói', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'bc1daf58-e2d6-4ca0-90f3-fe64f1fe7eb3', N'VLXD GiaPhuc', NULL, NULL, NULL, NULL, NULL, 1, N'Cục kê bê tông', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'28d7781e-2c4e-4c0c-bf03-fec6d7ec2bd3', N'Ngói Nhựa PVC Bình Minh', NULL, NULL, NULL, NULL, NULL, 1, N'CTy Ngói', NULL)
GO
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'56154fda-125e-4610-ad17-027c1c3ae2d2', N'd4afdc22-a07a-486f-b521-a9068d391821', N'75c3e21a-0abd-4699-abbe-c8371ed4d49a', 2.4192, N'm2', NULL, NULL)
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'9378f67c-1054-4702-9791-06aaa254ab47', N'bcf7b1e4-1cf5-42c2-958c-3f05e3c4ce68', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 78, N'm2', NULL, N'287f222c-2d3b-4445-900c-2a32101bec00')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'09f46d40-ade0-48a4-84aa-0728f794f2dd', N'719365ed-a384-4521-a0d0-b0feb7236e98', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 48, N'm2', NULL, N'dbd38b0a-9263-4e52-a23a-35648ca4581d')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'6cc0a169-489b-4a16-933e-0d1e813dcad1', N'7f3c7712-139a-4444-8a3c-79fc3831259f', N'75c3e21a-0abd-4699-abbe-c8371ed4d49a', 9.045, N'm2', NULL, NULL)
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'16a21eed-efe3-4327-957b-11d110bf2479', N'7f3c7712-139a-4444-8a3c-79fc3831259f', N'bd101af5-ac48-43ba-a474-957a20a933bd', 90.118, N'm2', NULL, N'7e442652-eefc-43b7-918b-a264a10e679d')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'770282a0-c45a-4c3b-8053-16c31fa3e7f3', N'442a11bf-d03b-4f32-8c92-e925b39bbfa9', N'bd101af5-ac48-43ba-a474-957a20a933bd', 73.75, N'm2', NULL, N'7e442652-eefc-43b7-918b-a264a10e679d')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'509f7c8a-c606-4bb9-8724-1df2701fbfd8', N'3321a8e5-bc40-4039-a4bf-d138f379c2b7', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 57.2, N'm2', NULL, N'dbd38b0a-9263-4e52-a23a-35648ca4581d')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'd1490cb6-6d89-409a-a8f4-21af02e63cbd', N'719365ed-a384-4521-a0d0-b0feb7236e98', N'75c3e21a-0abd-4699-abbe-c8371ed4d49a', 10.18152, N'm2', NULL, NULL)
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'08497090-dded-43d0-b643-231bdc8c0e62', N'bcf7b1e4-1cf5-42c2-958c-3f05e3c4ce68', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 52, N'm2', NULL, N'dbd38b0a-9263-4e52-a23a-35648ca4581d')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'14ae05b3-2f99-4c96-b3cb-244f07523a83', N'420e61d1-cd1f-4d90-8e93-1c2eee62f086', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 36, N'm2', NULL, N'708bad20-af57-4ab6-96e9-c22a22273724')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'10356900-b95a-40e4-a45e-31e36beaa3bc', N'75723c8d-6c83-445c-83ff-d428895d5173', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 43.2, N'm2', NULL, N'dbd38b0a-9263-4e52-a23a-35648ca4581d')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'88614cc3-3cf7-4509-82e0-37c707a52678', N'7f3c7712-139a-4444-8a3c-79fc3831259f', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 27, N'm2', NULL, N'06ff2d3d-2f14-4acc-ba2e-0c4ee659ca81')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'fa8b478a-cbbf-4917-9c53-3a21600b6452', N'64317b00-a7a2-4ffe-b67e-b68e669950eb', N'75c3e21a-0abd-4699-abbe-c8371ed4d49a', 8.289, N'm2', NULL, NULL)
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'f5931cc3-418f-4afc-baec-3ae4565f4650', N'420e61d1-cd1f-4d90-8e93-1c2eee62f086', N'bd101af5-ac48-43ba-a474-957a20a933bd', 81.918, N'm2', NULL, N'7e442652-eefc-43b7-918b-a264a10e679d')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'788142bf-c91d-4a96-89e1-40be02d877c5', N'd4afdc22-a07a-486f-b521-a9068d391821', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 23.4, N'm2', NULL, N'06ff2d3d-2f14-4acc-ba2e-0c4ee659ca81')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'857b129f-817b-4936-bc82-4886efb01399', N'64317b00-a7a2-4ffe-b67e-b68e669950eb', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 44.8, N'm2', NULL, N'dbd38b0a-9263-4e52-a23a-35648ca4581d')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'323da013-4103-451d-8033-520b96468e78', N'719365ed-a384-4521-a0d0-b0feb7236e98', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 24, N'm2', NULL, N'06ff2d3d-2f14-4acc-ba2e-0c4ee659ca81')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'85371eeb-e579-434e-9c8a-5404cdef0f62', N'75723c8d-6c83-445c-83ff-d428895d5173', N'be6c6db7-cea1-4275-9b18-2fbcfe9b2353', 108, N'm2', NULL, NULL)
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'46d79015-46fa-4b5f-be06-574f01010a5f', N'3321a8e5-bc40-4039-a4bf-d138f379c2b7', N'bd101af5-ac48-43ba-a474-957a20a933bd', 88.013, N'm2', NULL, N'7e442652-eefc-43b7-918b-a264a10e679d')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'6eabd942-2ca0-45c8-8236-58fbe2077bea', N'64317b00-a7a2-4ffe-b67e-b68e669950eb', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 33.6, N'm2', NULL, N'708bad20-af57-4ab6-96e9-c22a22273724')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'6fc83058-e735-499a-a30b-5b89b8c6bc82', N'420e61d1-cd1f-4d90-8e93-1c2eee62f086', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 72, N'm2', NULL, N'287f222c-2d3b-4445-900c-2a32101bec00')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'baf5c9b7-ff87-46c5-97ef-601b61c30f79', N'64317b00-a7a2-4ffe-b67e-b68e669950eb', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 67.2, N'm2', NULL, N'287f222c-2d3b-4445-900c-2a32101bec00')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'2e16296a-d9dd-4b61-ba7f-602906adae67', N'64317b00-a7a2-4ffe-b67e-b68e669950eb', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 22.4, N'm2', NULL, N'06ff2d3d-2f14-4acc-ba2e-0c4ee659ca81')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'fdb5c8b8-ad5f-4c21-9bba-63858b68d15d', N'3321a8e5-bc40-4039-a4bf-d138f379c2b7', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 28.6, N'm2', NULL, N'06ff2d3d-2f14-4acc-ba2e-0c4ee659ca81')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'24b60d27-88a7-4470-bf34-6ae17a587793', N'd4afdc22-a07a-486f-b521-a9068d391821', N'bd101af5-ac48-43ba-a474-957a20a933bd', 70.875, N'm2', NULL, N'7e442652-eefc-43b7-918b-a264a10e679d')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'875205cc-9927-4ce0-991f-6e71e2fcdc6d', N'64317b00-a7a2-4ffe-b67e-b68e669950eb', N'bd101af5-ac48-43ba-a474-957a20a933bd', 76.923, N'm2', NULL, N'7e442652-eefc-43b7-918b-a264a10e679d')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'df2a499b-a87f-43b0-add6-74cdabb53b4d', N'75723c8d-6c83-445c-83ff-d428895d5173', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 64.8, N'm2', NULL, N'287f222c-2d3b-4445-900c-2a32101bec00')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'e89f4896-176a-44ae-b545-795c4c909a3a', N'442a11bf-d03b-4f32-8c92-e925b39bbfa9', N'75c3e21a-0abd-4699-abbe-c8371ed4d49a', 3.2238, N'm2', NULL, NULL)
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'8ee40492-832f-4766-a6c6-7a29101ee599', N'bcf7b1e4-1cf5-42c2-958c-3f05e3c4ce68', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 26, N'm2', NULL, N'06ff2d3d-2f14-4acc-ba2e-0c4ee659ca81')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'f188f3d4-2cc3-42ab-8ada-7db5cebe875b', N'442a11bf-d03b-4f32-8c92-e925b39bbfa9', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 24, N'm2', NULL, N'06ff2d3d-2f14-4acc-ba2e-0c4ee659ca81')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'd582d15a-4de5-482a-9b98-8015a03a9b95', N'442a11bf-d03b-4f32-8c92-e925b39bbfa9', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 36, N'm2', NULL, N'708bad20-af57-4ab6-96e9-c22a22273724')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'a33f444e-bf58-4d47-bb2d-81828ee9554d', N'd4afdc22-a07a-486f-b521-a9068d391821', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 46.8, N'm2', NULL, N'dbd38b0a-9263-4e52-a23a-35648ca4581d')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'10c8ea93-4cfc-4a9c-85c8-8db0f394c069', N'd4afdc22-a07a-486f-b521-a9068d391821', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 70.2, N'm2', NULL, N'287f222c-2d3b-4445-900c-2a32101bec00')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'c60044a5-3f04-416a-b96d-90c55b89871c', N'442a11bf-d03b-4f32-8c92-e925b39bbfa9', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 72, N'm2', NULL, N'287f222c-2d3b-4445-900c-2a32101bec00')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'395a6366-dd9c-4c33-810f-9219da38921f', N'75723c8d-6c83-445c-83ff-d428895d5173', N'bd101af5-ac48-43ba-a474-957a20a933bd', 70.875, N'm2', NULL, N'7e442652-eefc-43b7-918b-a264a10e679d')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'55384837-4347-4f86-a980-92890c441731', N'719365ed-a384-4521-a0d0-b0feb7236e98', N'bd101af5-ac48-43ba-a474-957a20a933bd', 75.218, N'm2', NULL, N'7e442652-eefc-43b7-918b-a264a10e679d')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'ae87983b-7285-493e-ac62-9bfbd3d84b30', N'75723c8d-6c83-445c-83ff-d428895d5173', N'75c3e21a-0abd-4699-abbe-c8371ed4d49a', 2.4192, N'm2', NULL, NULL)
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'18b76895-665e-4b7c-9b69-9f5e81a9b453', N'420e61d1-cd1f-4d90-8e93-1c2eee62f086', N'75c3e21a-0abd-4699-abbe-c8371ed4d49a', 8.289, N'm2', NULL, NULL)
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'92b13047-6442-49a5-9c4b-a16a7a0a12c5', N'7f3c7712-139a-4444-8a3c-79fc3831259f', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 81, N'm2', NULL, N'287f222c-2d3b-4445-900c-2a32101bec00')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'1a35f7ea-f108-4157-b597-a846e23709d4', N'bcf7b1e4-1cf5-42c2-958c-3f05e3c4ce68', N'be6c6db7-cea1-4275-9b18-2fbcfe9b2353', 130, N'm2', NULL, NULL)
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'535cc517-1954-403d-84d9-adea10ea2d17', N'75723c8d-6c83-445c-83ff-d428895d5173', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 32.4, N'm2', NULL, N'708bad20-af57-4ab6-96e9-c22a22273724')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'ad3ba94b-28cd-4e0e-8585-b1bcb7b8f2f8', N'bcf7b1e4-1cf5-42c2-958c-3f05e3c4ce68', N'75c3e21a-0abd-4699-abbe-c8371ed4d49a', 11.862, N'm2', NULL, NULL)
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'93aceff4-f779-48c0-9f25-b280d0d50e47', N'420e61d1-cd1f-4d90-8e93-1c2eee62f086', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 24, N'm2', NULL, N'06ff2d3d-2f14-4acc-ba2e-0c4ee659ca81')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'138ed403-6330-457e-9203-b9673c9c012d', N'7f3c7712-139a-4444-8a3c-79fc3831259f', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 54, N'm2', NULL, N'dbd38b0a-9263-4e52-a23a-35648ca4581d')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'28fae8a4-318e-4a6f-8d32-bb6bf779d9a2', N'442a11bf-d03b-4f32-8c92-e925b39bbfa9', N'be6c6db7-cea1-4275-9b18-2fbcfe9b2353', 120, N'm2', NULL, NULL)
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'245cdcfc-a09d-436b-ac6e-be34435b273d', N'3321a8e5-bc40-4039-a4bf-d138f379c2b7', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 85.5, N'm2', NULL, N'287f222c-2d3b-4445-900c-2a32101bec00')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'66dc8690-fcf6-4631-b93c-be71b702014a', N'7f3c7712-139a-4444-8a3c-79fc3831259f', N'be6c6db7-cea1-4275-9b18-2fbcfe9b2353', 135, N'm2', NULL, NULL)
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'6ad12911-a807-4c90-802f-c09f40551878', N'3321a8e5-bc40-4039-a4bf-d138f379c2b7', N'be6c6db7-cea1-4275-9b18-2fbcfe9b2353', 143, N'm2', NULL, NULL)
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'21e2cc6f-d1d2-4044-a758-c33eb2303715', N'd4afdc22-a07a-486f-b521-a9068d391821', N'be6c6db7-cea1-4275-9b18-2fbcfe9b2353', 117, N'm2', NULL, NULL)
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'1928cb0b-ba52-477e-aa31-c4010299220a', N'442a11bf-d03b-4f32-8c92-e925b39bbfa9', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 48, N'm2', NULL, N'dbd38b0a-9263-4e52-a23a-35648ca4581d')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'0b4358d0-8c75-42f1-8b25-c48414c7c536', N'64317b00-a7a2-4ffe-b67e-b68e669950eb', N'be6c6db7-cea1-4275-9b18-2fbcfe9b2353', 112, N'm2', NULL, NULL)
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'd6817d69-3631-45a6-9ab5-c8c105dc8218', N'7f3c7712-139a-4444-8a3c-79fc3831259f', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 40.5, N'm2', NULL, N'708bad20-af57-4ab6-96e9-c22a22273724')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'b2cf6e3b-e884-4e1c-8b2b-ca468aa5eeb0', N'719365ed-a384-4521-a0d0-b0feb7236e98', N'be6c6db7-cea1-4275-9b18-2fbcfe9b2353', 120, N'm2', NULL, NULL)
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'75723c8d-6c83-445c-83ff-d428895d5173', N'75723c8d-6c83-445c-83ff-d428895d5173', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 21.6, N'm2', NULL, N'06ff2d3d-2f14-4acc-ba2e-0c4ee659ca81')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'35532ffb-40c5-4d44-827c-d6fe9b5d608d', N'bcf7b1e4-1cf5-42c2-958c-3f05e3c4ce68', N'bd101af5-ac48-43ba-a474-957a20a933bd', 80.938, N'm2', NULL, N'7e442652-eefc-43b7-918b-a264a10e679d')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'90c03c2f-167e-4a0f-9648-dda7ec3acaef', N'420e61d1-cd1f-4d90-8e93-1c2eee62f086', N'be6c6db7-cea1-4275-9b18-2fbcfe9b2353', 120, N'm2', NULL, NULL)
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'e519d638-2b3c-4ed8-8b11-ddf696fd0100', N'719365ed-a384-4521-a0d0-b0feb7236e98', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 36, N'm2', NULL, N'708bad20-af57-4ab6-96e9-c22a22273724')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'b7250cf1-5da1-4f1d-bbcf-ef92a16d79c2', N'3321a8e5-bc40-4039-a4bf-d138f379c2b7', N'75c3e21a-0abd-4699-abbe-c8371ed4d49a', 11.862, N'm2', NULL, NULL)
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'1f17c13e-5888-4ad8-a64d-f0e588677569', N'420e61d1-cd1f-4d90-8e93-1c2eee62f086', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 48, N'm2', NULL, N'dbd38b0a-9263-4e52-a23a-35648ca4581d')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'4d1f93f7-1165-42e2-85a3-f138a9fe8dea', N'719365ed-a384-4521-a0d0-b0feb7236e98', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 72, N'm2', NULL, N'287f222c-2d3b-4445-900c-2a32101bec00')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'81386b79-d044-482a-a141-f26ee9869759', N'3321a8e5-bc40-4039-a4bf-d138f379c2b7', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 42.9, N'm2', NULL, N'708bad20-af57-4ab6-96e9-c22a22273724')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'863110eb-3907-4ae9-afe8-f5988d7066ca', N'd4afdc22-a07a-486f-b521-a9068d391821', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 35.1, N'm2', NULL, N'708bad20-af57-4ab6-96e9-c22a22273724')
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate], [SubConstructionId]) VALUES (N'e50b080e-4eeb-45c9-bc03-f83710193e6b', N'bcf7b1e4-1cf5-42c2-958c-3f05e3c4ce68', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', 39, N'm2', NULL, N'708bad20-af57-4ab6-96e9-c22a22273724')
GO
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate], [Deflag]) VALUES (N'388ebe68-a4ce-4662-8297-0a49a2a5fe4d', N'1a091f31-a1d1-41c5-a018-3fc6b47408c5', N'Chi phí trọn gói', 0.02, CAST(N'2024-10-07T23:31:19.780' AS DateTime), CAST(N'2024-10-07T23:31:19.780' AS DateTime), 1)
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate], [Deflag]) VALUES (N'baba9651-91ee-40a2-a824-0b90f861b72e', N'8d94e702-1a40-4316-815c-1668ab01d7d6', N'Hẻm < 2m', 0.05, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), NULL)
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate], [Deflag]) VALUES (N'8f17e0a9-6192-4155-a360-165073ce598d', N'8d94e702-1a40-4316-815c-1668ab01d7d6', N'Hẻm 2m - 3m', 0.04, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), NULL)
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate], [Deflag]) VALUES (N'2ec103aa-aa83-4d58-9e85-22a6247f4cd6', N'dc3e0910-dae6-4faa-abfd-6e79d2a0a9aa', N'Sàn từ 30m2 ~ 40m2', 0.05, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), NULL)
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate], [Deflag]) VALUES (N'0a76055d-3aa0-41bf-868f-637ef0c7b19b', N'dc3e0910-dae6-4faa-abfd-6e79d2a0a9aa', N'Sàn từ 40m2 ~ 50m2', 0.04, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), NULL)
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate], [Deflag]) VALUES (N'3a071c11-b5f0-4b04-a7b2-7ae2fb655815', N'8d94e702-1a40-4316-815c-1668ab01d7d6', N'Hẻm 4m - 5m', 0.02, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), NULL)
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate], [Deflag]) VALUES (N'422bb684-c541-47f5-ae3b-7f8f38e91e84', N'8d94e702-1a40-4316-815c-1668ab01d7d6', N'Hẻm 5m - 6m', 0.01, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), NULL)
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate], [Deflag]) VALUES (N'5e9decc2-35bd-4553-a230-85f61e28ff1f', N'85fb4802-4529-4197-ada1-3544cb7b1d65', N'Xung quanh đất trống (2,800,000)', 0, CAST(N'2024-10-07T23:34:38.083' AS DateTime), CAST(N'2024-10-07T23:34:38.083' AS DateTime), 1)
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate], [Deflag]) VALUES (N'6c17d40f-8eae-470b-8bcf-a944b09a84b7', N'1fe2bb9d-fc40-48b0-8cef-73f3e54b960a', N'Nâng 2 sàn', 0.08, CAST(N'2024-10-07T23:16:06.527' AS DateTime), CAST(N'2024-10-07T23:16:06.527' AS DateTime), 1)
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate], [Deflag]) VALUES (N'69ac1a58-c762-4178-8a0b-ae1aa8d99edd', N'dc3e0910-dae6-4faa-abfd-6e79d2a0a9aa', N'Sàn từ 50m2 ~ 60m2', 0.03, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), NULL)
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate], [Deflag]) VALUES (N'd4cea410-4530-4db7-a1f6-b3090965536c', N'1fe2bb9d-fc40-48b0-8cef-73f3e54b960a', N'Nâng 1 sàn', 0.04, CAST(N'2024-10-07T23:16:06.527' AS DateTime), CAST(N'2024-10-07T23:16:06.527' AS DateTime), 1)
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate], [Deflag]) VALUES (N'3b3bd90b-c1e7-47d8-bfdb-bd699eeadd1d', N'85fb4802-4529-4197-ada1-3544cb7b1d65', N'Có nhà liền kề (3,300,000)', 0, CAST(N'2024-10-07T23:34:38.080' AS DateTime), CAST(N'2024-10-07T23:34:38.080' AS DateTime), 1)
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate], [Deflag]) VALUES (N'f9cacd68-a453-4ddf-ac9d-be97911d0e90', N'dc3e0910-dae6-4faa-abfd-6e79d2a0a9aa', N'Sàn từ 60m2 ~ 70m2', 0.01, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), NULL)
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate], [Deflag]) VALUES (N'8ebff368-da33-4437-b93e-c6fa1642e1b5', N'8d94e702-1a40-4316-815c-1668ab01d7d6', N'Hẻm 3m - 4m', 0.03, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), NULL)
GO
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'8d94e702-1a40-4316-815c-1668ab01d7d6', N'002e459a-e010-493f-8585-d729d3cf357b', N'Chi phí thi công trình hẻm nhỏ', 1, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'928e114f-3f3b-4a1a-b296-25ba413e1a54', N'cb87f1f6-f9d4-4c43-8d8c-5f16810e75cb', N'Chi phí bản vẽ hoàn công', 1, CAST(N'2024-10-14T13:33:58.557' AS DateTime), CAST(N'2024-10-14T13:33:58.557' AS DateTime), N'Chi phí bản vẽ hoàn công', 5000000, N'đ')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'0c808ed8-1da2-45c0-b70b-2a54610a5f54', N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Chi phí xây tường 100mm thành tường 200mm(vách ngăn 200)', 1, CAST(N'2024-10-07T23:20:08.517' AS DateTime), CAST(N'2024-10-07T23:20:08.517' AS DateTime), N'', 250000, N'đ/m2')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'ab717159-d6ea-4c04-b536-3327330ca315', N'cb87f1f6-f9d4-4c43-8d8c-5f16810e75cb', N'Chi phí bản vẽ xin phép xây dựng', 1, CAST(N'2024-10-14T13:33:58.557' AS DateTime), CAST(N'2024-10-14T13:33:58.557' AS DateTime), N'Chi phí bản vẽ xin phép xây dựng', 5000000, N'đ')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'85fb4802-4529-4197-ada1-3544cb7b1d65', N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Chi phí ép cừ vây gia cố vách hầm', 1, CAST(N'2024-10-07T23:34:38.080' AS DateTime), CAST(N'2024-10-07T23:34:38.080' AS DateTime), N'Gia cố cừ vây loại 4 - 4.5m', 0, NULL)
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'e9297263-8faf-4094-b29a-35792495beb5', N'04f30a66-9758-45db-88a7-6f098edc4837', N'Mua nhà vệ sinh di động', 1, CAST(N'2024-10-14T13:17:53.177' AS DateTime), CAST(N'2024-10-14T13:17:53.177' AS DateTime), N'Nhà vệ sinh di động', 12000000, N'đ')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'ce5a41ee-72dd-48d1-92c5-35e78c51c143', N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Vật tư tiếp địa, dây TE', 1, CAST(N'2024-10-07T23:14:19.440' AS DateTime), CAST(N'2024-10-07T23:14:19.440' AS DateTime), N'Vật tư tiếp địa, dây TE', 12000000, N'đ')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'1a091f31-a1d1-41c5-a018-3fc6b47408c5', N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Chi phí thi công nhà lệch tầng', 1, CAST(N'2024-10-07T23:31:19.777' AS DateTime), CAST(N'2024-10-07T23:31:19.777' AS DateTime), N'Kỹ thuật xây dựng khó, tốn nhân công và vật tự so với nhà không lệch tầng', 200000, N'đ/m2')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'09ae25ed-dccc-48c1-a6bd-4429386194d2', N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Chi phí tô vách trong thang máy', 1, CAST(N'2024-10-07T23:42:44.827' AS DateTime), CAST(N'2024-10-07T23:42:44.827' AS DateTime), N'Chi phí tô vách trong thang máy', 250000, N'đ')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'a59f489a-3e57-4281-b0de-57d2103ec002', N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Ống nước nóng năng lượng mặt trời PPR', 1, CAST(N'2024-10-07T23:39:27.063' AS DateTime), CAST(N'2024-10-07T23:39:27.063' AS DateTime), N'Ống PPR là ống nước sử dụng trong nhà vệ sinh hoặc bếp. Chi phí tính theo số lượng nhà vệ sinh', 3000000, N'đ/wc')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'361fb613-2c5c-4bae-b2c4-5ed92c58a0d7', N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Chi phí bê tông sàn tầng trệt', 1, CAST(N'2024-10-07T23:12:35.997' AS DateTime), CAST(N'2024-10-07T23:12:35.997' AS DateTime), N'Áp dụng với công trình trên nền đất yếu, nên cần đổ sàn chắc thêm. Hoặc do nhu cầu sử dụng cần tải trọng cao hơn.', NULL, NULL)
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'e81e26b6-96a3-4348-98cb-60048f757d0a', N'cb87f1f6-f9d4-4c43-8d8c-5f16810e75cb', N'Combo Hạch Toán', 1, CAST(N'2024-10-14T13:33:58.557' AS DateTime), CAST(N'2024-10-14T13:33:58.557' AS DateTime), N'Combo hạch toán bao gồm: lập hồ sơ dự toán tham khảo, chi phí thuyết minh kết cấu', 14000000, N'đ')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'1bb1aa25-c477-46c6-a439-60386576618a', N'04f30a66-9758-45db-88a7-6f098edc4837', N'Gói Đo Đạc, Cắm Mốc', 1, CAST(N'2024-10-14T13:12:31.393' AS DateTime), CAST(N'2024-10-14T13:12:31.393' AS DateTime), N'Gói đo đạc, cắm mốc cho quy mô nhà phố 4-6 mốc', 10000000, N'đ')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'de123e50-d83a-4a70-a043-60dad2516a55', N'cb87f1f6-f9d4-4c43-8d8c-5f16810e75cb', N'Chi phí Coffa phim', 1, CAST(N'2024-10-14T13:33:58.557' AS DateTime), CAST(N'2024-10-14T13:33:58.557' AS DateTime), N'Chi phí Coffa phim', 40000000, N'đ')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'1dd687da-7ff9-43dc-9d50-646107bd1f06', N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Dự trù thi công thêm cầu thang bộ', 1, CAST(N'2024-10-07T23:14:45.573' AS DateTime), CAST(N'2024-10-07T23:14:45.573' AS DateTime), N'Dự trù thi công thêm cầu thang bộ', 20000000, N'đ/cầu thang')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'7e645c71-0332-41ce-8890-65836f8f46b7', N'04f30a66-9758-45db-88a7-6f098edc4837', N'Chống mối cho công trình', 1, CAST(N'2024-10-14T13:15:25.577' AS DateTime), CAST(N'2024-10-14T13:15:25.577' AS DateTime), N'Chống mối cho công trình theo m2 đất', 200000, N'm2')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'3a860af1-d246-4e46-8fb1-67896c2287b2', N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Ống ruột gà chuyển thành ống cứng Vega', 1, CAST(N'2024-10-07T23:38:09.347' AS DateTime), CAST(N'2024-10-07T23:38:09.347' AS DateTime), N'Ống cứng Vega luồn dây điện âm tường - chi phí trọn gói', 10000000, N'đ')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'dc3e0910-dae6-4faa-abfd-6e79d2a0a9aa', N'002e459a-e010-493f-8585-d729d3cf357b', N'Chi phí thi công sàn nhỏ hơn 70m2', 1, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), N'Với diện tích sàn < 70m2, vẫn phải tốn chi phí tương đương cho thao tác thi công, thời gian thi công...Vì vậy, đây là hạng mục được đưa vào chi phí bất lợi do điều kiện thi công', NULL, NULL)
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'3b942678-e9b9-4ee9-b26f-6ece15aef11f', N'04f30a66-9758-45db-88a7-6f098edc4837', N'Chi phí lắp thêm camera từng tầng', 1, CAST(N'2024-10-14T13:16:53.120' AS DateTime), CAST(N'2024-10-14T13:16:53.120' AS DateTime), N'Mỗi công trình đều có 1 camera được kết nối trực tiếp với công ty và chủ đầu tư', 3000000, N'đ')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'1fe2bb9d-fc40-48b0-8cef-73f3e54b960a', N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Chi phí dự trù kết cấu nâng tầng', 1, CAST(N'2024-10-07T23:16:06.527' AS DateTime), CAST(N'2024-10-07T23:16:06.527' AS DateTime), N'Dự trù kết cấu nâng tầng', NULL, NULL)
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'8b51e4a1-c4a4-4a4c-9e16-746a4da5d3f0', N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Chi phí chống thấm vách hầm chuyên nghiệp', 1, CAST(N'2024-10-07T23:36:06.930' AS DateTime), CAST(N'2024-10-07T23:36:06.930' AS DateTime), N'Chi phí theo khối lượng công việc', 180000, N'đ')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'bf9a651d-32a9-4b71-a4de-772b30082971', N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Chi phí thi công ván phủ phim', 1, CAST(N'2024-10-07T23:35:16.687' AS DateTime), CAST(N'2024-10-07T23:35:16.687' AS DateTime), N'Chi phí theo khối lượng công việc', 100000, NULL)
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'0f08625c-4509-4413-a784-7d0183a39f03', N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Chi phí tô trần', 1, CAST(N'2024-10-07T23:29:50.133' AS DateTime), CAST(N'2024-10-07T23:29:50.133' AS DateTime), N'Đơn giá xây dựng đã bao gồm đổ sàn bê tông cốt thép và đóng trần thạch cao(không bao gồm chi phí tô trần)', 200000, N'đ/m2')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'fbf781d8-5060-44a8-af1c-7db7fa2e15ea', N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Chi phí nâng nền', 1, CAST(N'2024-10-07T23:09:59.373' AS DateTime), CAST(N'2024-10-07T23:09:59.373' AS DateTime), N'Không bao gồm hệ đà kiềng thứ 2, khối lượng tính theo m3', NULL, NULL)
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'4a7886a6-1c60-43e1-861c-7ef0bbc47d97', N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Bê tông có hóa chất chống thấm B6', 1, CAST(N'2024-10-07T23:41:23.967' AS DateTime), CAST(N'2024-10-07T23:41:23.967' AS DateTime), N'100% Bê tông có hóa chất chống thấm tính theo khối lượng m2', 50000, N'đ')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'8c45df6d-f0be-4ef8-bd01-8ba2bbb995b5', N'cb87f1f6-f9d4-4c43-8d8c-5f16810e75cb', N'Combo Điện Nước', 1, CAST(N'2024-10-14T13:33:58.557' AS DateTime), CAST(N'2024-10-14T13:33:58.557' AS DateTime), N'Combo điện nước bao gồm: ống nước nóng PPR, ống cứng luồn dây điện âm tường', 13000000, N'đ')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'46124ab4-d9f4-46b3-8b37-8cd2720efc18', N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Chuyển đổi phần vách hầm nổi trên mặt đất từ xây gạch đinh sang vách đổ bê tông, cốt thép', 1, CAST(N'2024-10-07T23:43:45.700' AS DateTime), CAST(N'2024-10-07T23:43:45.700' AS DateTime), N'Chuyển đổi phần vách hầm nổi trên mặt đất từ xây gạch đinh sang vách đổ bê tông, cốt thép', 1000000, N'đ')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'71c78b90-44fd-41a1-b7c5-93a2165df2a5', N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Sử dụng keo chuyên dụng ốp lát gạch', 1, CAST(N'2024-10-07T23:42:13.457' AS DateTime), CAST(N'2024-10-07T23:42:13.457' AS DateTime), N'Sử dụng keo chuyên dụng ốp lát gạch, khối lượng tính theo m2 gạch', 130000, N'đ')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'c1e67f56-cce3-4b77-ae5d-9db64e2902ca', N'cb87f1f6-f9d4-4c43-8d8c-5f16810e75cb', N'Combo Giấy Phép', 1, CAST(N'2024-10-14T13:33:58.350' AS DateTime), CAST(N'2024-10-14T13:33:58.350' AS DateTime), N'Combo giấy phép bao gồm: chi phí thuyết minh kết cấu, chi phí bản vẽ xin phép xây dựng, chi phí bản vẽ hoàn công', 18000000, N'đ')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'787cb231-0ed3-443d-b1da-ab05567284bb', N'002e459a-e010-493f-8585-d729d3cf357b', N'Hỗ trợ bãi tập kết, điều kiện thi công khó khăn', 1, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'1f0298c3-96fb-4997-8efa-af5b7e0eabf3', N'002e459a-e010-493f-8585-d729d3cf357b', N'Chi phí thi công nhà 2 mặt tiền trở lên', 1, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'a3681867-6365-4a1d-9aa8-b2c68766f536', N'002e459a-e010-493f-8585-d729d3cf357b', N'Chi phí thi công công trình tỉnh', 1, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'73cc3975-6326-47a6-aae6-ca96c29694af', N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Chi phí nhân công ốp lát gạch', 1, CAST(N'2024-10-07T23:32:34.383' AS DateTime), CAST(N'2024-10-07T23:32:34.383' AS DateTime), N'Chi phí nhân công và xi măng ốp gạch theo khối lượng', 220000, N'đ/m2')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'2caad165-6bfd-40e5-a0b4-cdaa26301cb9', N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Chi phí thi công điểm dừng thang máy', 1, CAST(N'2024-10-07T23:40:26.620' AS DateTime), CAST(N'2024-10-07T23:40:26.620' AS DateTime), N'Chi phi thi công theo số lượng điểm dừng thang máy', 20000000, N'đ/điểm dừng')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'f27342d7-e636-4ccb-a297-ce6478088070', N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Chi phí nguồn điện 3 pha thang máy', 1, CAST(N'2024-10-07T23:13:31.717' AS DateTime), CAST(N'2024-10-07T23:13:31.717' AS DateTime), N'', 10000000, N'đ/nguồn')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'f3280771-69dd-4acb-8093-d87fa359576f', N'04f30a66-9758-45db-88a7-6f098edc4837', N'Dịch vụ xem ngày và cúng động thổ', 1, CAST(N'2024-10-14T13:14:23.633' AS DateTime), CAST(N'2024-10-14T13:14:23.633' AS DateTime), N'Dịch vụ xem ngày, cúng động thổ đã bao gồm đồ cúng', 7000000, N'đ')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'd7f9c599-0bc4-46d8-8a2b-e288efb98d68', N'cb87f1f6-f9d4-4c43-8d8c-5f16810e75cb', N'Combo thi công thô', 1, CAST(N'2024-10-11T20:21:48.323' AS DateTime), CAST(N'2024-10-11T20:21:48.323' AS DateTime), N'Combo thi công bao gồm: Đóng lưới mắt cá toàn bộ công trình trước khi tô, Coffa phim, Vật tư tiếp địa, đây TE', 81000000, N'đ')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Deflag], [InsDate], [UpsDate], [Description], [UnitPrice], [Unit]) VALUES (N'c08ddc36-d35d-4e1d-93d8-fd68fbed6214', N'cb87f1f6-f9d4-4c43-8d8c-5f16810e75cb', N'Đóng lưới mắt cáo toàn bộ công trình', 1, CAST(N'2024-10-14T13:33:58.557' AS DateTime), CAST(N'2024-10-14T13:33:58.557' AS DateTime), N'Đóng lười mắt cái toàn bộ công trình trước khi tô', 30000000, N'đ')
GO
INSERT [dbo].[UtilityOption] ([Id], [Name], [Type], [Deflag], [InsDate], [UpsDate]) VALUES (N'cb87f1f6-f9d4-4c43-8d8c-5f16810e75cb', N'5 - Combo', N'FINISHED', 1, CAST(N'2024-10-14T13:33:51.950' AS DateTime), CAST(N'2024-10-14T13:33:51.950' AS DateTime))
INSERT [dbo].[UtilityOption] ([Id], [Name], [Type], [Deflag], [InsDate], [UpsDate]) VALUES (N'04f30a66-9758-45db-88a7-6f098edc4837', N'3 - Dịch vụ tiện ích thêm', N'FINISHED', 1, CAST(N'2024-09-28T14:47:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime))
INSERT [dbo].[UtilityOption] ([Id], [Name], [Type], [Deflag], [InsDate], [UpsDate]) VALUES (N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'2 - Nâng cao chất lượng phần thô', N'ROUGH', 1, CAST(N'2024-09-28T14:46:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime))
INSERT [dbo].[UtilityOption] ([Id], [Name], [Type], [Deflag], [InsDate], [UpsDate]) VALUES (N'05430765-97fe-4186-900d-d5dc850e8cdb', N'4 - Tiện ích công trình', N'FINISHED', 1, CAST(N'2024-09-28T14:49:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime))
INSERT [dbo].[UtilityOption] ([Id], [Name], [Type], [Deflag], [InsDate], [UpsDate]) VALUES (N'002e459a-e010-493f-8585-d729d3cf357b', N'1 - Điều kiện thi công không thuận lợi', N'ROUGH', 1, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime))
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_Role]
GO
ALTER TABLE [dbo].[AssignTask]  WITH CHECK ADD  CONSTRAINT [FK_AssignTask_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[AssignTask] CHECK CONSTRAINT [FK_AssignTask_Account]
GO
ALTER TABLE [dbo].[AssignTask]  WITH CHECK ADD  CONSTRAINT [FK_AssignTask_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO
ALTER TABLE [dbo].[AssignTask] CHECK CONSTRAINT [FK_AssignTask_Project]
GO
ALTER TABLE [dbo].[BatchPayment]  WITH CHECK ADD  CONSTRAINT [FK_BactchPayment_Contract] FOREIGN KEY([ContractId])
REFERENCES [dbo].[Contract] ([Id])
GO
ALTER TABLE [dbo].[BatchPayment] CHECK CONSTRAINT [FK_BactchPayment_Contract]
GO
ALTER TABLE [dbo].[BatchPayment]  WITH CHECK ADD  CONSTRAINT [FK_BactchPayment_FinalQuotation] FOREIGN KEY([FinalQuotationId])
REFERENCES [dbo].[FinalQuotation] ([Id])
GO
ALTER TABLE [dbo].[BatchPayment] CHECK CONSTRAINT [FK_BactchPayment_FinalQuotation]
GO
ALTER TABLE [dbo].[BatchPayment]  WITH CHECK ADD  CONSTRAINT [FK_BactchPayment_InitialQuotation] FOREIGN KEY([IntitialQuotationId])
REFERENCES [dbo].[InitialQuotation] ([Id])
GO
ALTER TABLE [dbo].[BatchPayment] CHECK CONSTRAINT [FK_BactchPayment_InitialQuotation]
GO
ALTER TABLE [dbo].[Blog]  WITH CHECK ADD  CONSTRAINT [FK_Blog_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Blog] CHECK CONSTRAINT [FK_Blog_Account]
GO
ALTER TABLE [dbo].[Contract]  WITH CHECK ADD  CONSTRAINT [FK_Contract_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO
ALTER TABLE [dbo].[Contract] CHECK CONSTRAINT [FK_Contract_Project]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_Account]
GO
ALTER TABLE [dbo].[EquipmentItem]  WITH CHECK ADD  CONSTRAINT [FK_EquimentItem_FinalQuotation] FOREIGN KEY([FinalQuotationId])
REFERENCES [dbo].[FinalQuotation] ([Id])
GO
ALTER TABLE [dbo].[EquipmentItem] CHECK CONSTRAINT [FK_EquimentItem_FinalQuotation]
GO
ALTER TABLE [dbo].[FinalQuotation]  WITH CHECK ADD  CONSTRAINT [FK_FinalQuotation_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO
ALTER TABLE [dbo].[FinalQuotation] CHECK CONSTRAINT [FK_FinalQuotation_Project]
GO
ALTER TABLE [dbo].[FinalQuotation]  WITH CHECK ADD  CONSTRAINT [FK_FinalQuotation_Promotion] FOREIGN KEY([PromotionId])
REFERENCES [dbo].[Promotion] ([Id])
GO
ALTER TABLE [dbo].[FinalQuotation] CHECK CONSTRAINT [FK_FinalQuotation_Promotion]
GO
ALTER TABLE [dbo].[FinalQuotation]  WITH CHECK ADD  CONSTRAINT [FK_FinalQuotation_QuotationUtilities] FOREIGN KEY([QuotationUtilitiesId])
REFERENCES [dbo].[QuotationUtilities] ([Id])
GO
ALTER TABLE [dbo].[FinalQuotation] CHECK CONSTRAINT [FK_FinalQuotation_QuotationUtilities]
GO
ALTER TABLE [dbo].[FinalQuotationItem]  WITH CHECK ADD  CONSTRAINT [FK_FinalQuotationItem_ConstructionItems] FOREIGN KEY([ConstructionItemId])
REFERENCES [dbo].[ConstructionItems] ([Id])
GO
ALTER TABLE [dbo].[FinalQuotationItem] CHECK CONSTRAINT [FK_FinalQuotationItem_ConstructionItems]
GO
ALTER TABLE [dbo].[FinalQuotationItem]  WITH CHECK ADD  CONSTRAINT [FK_FinalQuotationItem_FinalQuotation] FOREIGN KEY([FinalQuotationId])
REFERENCES [dbo].[FinalQuotation] ([Id])
GO
ALTER TABLE [dbo].[FinalQuotationItem] CHECK CONSTRAINT [FK_FinalQuotationItem_FinalQuotation]
GO
ALTER TABLE [dbo].[HouseDesignDrawing]  WITH CHECK ADD  CONSTRAINT [FK_HouseDesignDrawing_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[HouseDesignDrawing] CHECK CONSTRAINT [FK_HouseDesignDrawing_Account]
GO
ALTER TABLE [dbo].[HouseDesignDrawing]  WITH CHECK ADD  CONSTRAINT [FK_HouseDesignDrawing_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO
ALTER TABLE [dbo].[HouseDesignDrawing] CHECK CONSTRAINT [FK_HouseDesignDrawing_Project]
GO
ALTER TABLE [dbo].[HouseDesignVersion]  WITH CHECK ADD  CONSTRAINT [FK_HouseDesignVersion_HouseDesignDrawing] FOREIGN KEY([HouseDesignDrawingId])
REFERENCES [dbo].[HouseDesignDrawing] ([Id])
GO
ALTER TABLE [dbo].[HouseDesignVersion] CHECK CONSTRAINT [FK_HouseDesignVersion_HouseDesignDrawing]
GO
ALTER TABLE [dbo].[InitialQuotation]  WITH CHECK ADD  CONSTRAINT [FK_InitialQuotation_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO
ALTER TABLE [dbo].[InitialQuotation] CHECK CONSTRAINT [FK_InitialQuotation_Project]
GO
ALTER TABLE [dbo].[InitialQuotation]  WITH CHECK ADD  CONSTRAINT [FK_InitialQuotation_Promotion] FOREIGN KEY([PromotionId])
REFERENCES [dbo].[Promotion] ([Id])
GO
ALTER TABLE [dbo].[InitialQuotation] CHECK CONSTRAINT [FK_InitialQuotation_Promotion]
GO
ALTER TABLE [dbo].[InitialQuotationItem]  WITH CHECK ADD  CONSTRAINT [FK_InitialQuotationItem_ConstructionItems] FOREIGN KEY([ConstructionItemId])
REFERENCES [dbo].[ConstructionItems] ([Id])
GO
ALTER TABLE [dbo].[InitialQuotationItem] CHECK CONSTRAINT [FK_InitialQuotationItem_ConstructionItems]
GO
ALTER TABLE [dbo].[InitialQuotationItem]  WITH CHECK ADD  CONSTRAINT [FK_InitialQuotationItem_InitialQuotation] FOREIGN KEY([InitialQuotationId])
REFERENCES [dbo].[InitialQuotation] ([Id])
GO
ALTER TABLE [dbo].[InitialQuotationItem] CHECK CONSTRAINT [FK_InitialQuotationItem_InitialQuotation]
GO
ALTER TABLE [dbo].[Material]  WITH CHECK ADD  CONSTRAINT [FK_Material_MaterialType] FOREIGN KEY([MaterialTypeId])
REFERENCES [dbo].[MaterialType] ([Id])
GO
ALTER TABLE [dbo].[Material] CHECK CONSTRAINT [FK_Material_MaterialType]
GO
ALTER TABLE [dbo].[Material]  WITH CHECK ADD  CONSTRAINT [FK_Material_Supplier] FOREIGN KEY([SupplierId])
REFERENCES [dbo].[Supplier] ([Id])
GO
ALTER TABLE [dbo].[Material] CHECK CONSTRAINT [FK_Material_Supplier]
GO
ALTER TABLE [dbo].[Material]  WITH CHECK ADD  CONSTRAINT [Material_MaterialSection_FK] FOREIGN KEY([MaterialSectionId])
REFERENCES [dbo].[MaterialSection] ([Id])
GO
ALTER TABLE [dbo].[Material] CHECK CONSTRAINT [Material_MaterialSection_FK]
GO
ALTER TABLE [dbo].[Media]  WITH CHECK ADD  CONSTRAINT [FK_Media_HouseDesignVersion] FOREIGN KEY([HouseDesignVersionId])
REFERENCES [dbo].[HouseDesignVersion] ([Id])
GO
ALTER TABLE [dbo].[Media] CHECK CONSTRAINT [FK_Media_HouseDesignVersion]
GO
ALTER TABLE [dbo].[Media]  WITH CHECK ADD  CONSTRAINT [FK_Media_Payment] FOREIGN KEY([PaymentId])
REFERENCES [dbo].[Payment] ([Id])
GO
ALTER TABLE [dbo].[Media] CHECK CONSTRAINT [FK_Media_Payment]
GO
ALTER TABLE [dbo].[Media]  WITH CHECK ADD  CONSTRAINT [Media_SubTemplate_FK] FOREIGN KEY([SubTemplateId])
REFERENCES [dbo].[SubTemplate] ([Id])
GO
ALTER TABLE [dbo].[Media] CHECK CONSTRAINT [Media_SubTemplate_FK]
GO
ALTER TABLE [dbo].[Package]  WITH CHECK ADD  CONSTRAINT [FK_Package_PackageType] FOREIGN KEY([PackageTypeId])
REFERENCES [dbo].[PackageType] ([Id])
GO
ALTER TABLE [dbo].[Package] CHECK CONSTRAINT [FK_Package_PackageType]
GO
ALTER TABLE [dbo].[PackageDetail]  WITH CHECK ADD  CONSTRAINT [FK_PackageDetail_Package] FOREIGN KEY([PackageId])
REFERENCES [dbo].[Package] ([Id])
GO
ALTER TABLE [dbo].[PackageDetail] CHECK CONSTRAINT [FK_PackageDetail_Package]
GO
ALTER TABLE [dbo].[PackageHouse]  WITH CHECK ADD  CONSTRAINT [FK_PackageHouse_DesignTemplate] FOREIGN KEY([DesignTemplateId])
REFERENCES [dbo].[DesignTemplate] ([Id])
GO
ALTER TABLE [dbo].[PackageHouse] CHECK CONSTRAINT [FK_PackageHouse_DesignTemplate]
GO
ALTER TABLE [dbo].[PackageHouse]  WITH CHECK ADD  CONSTRAINT [FK_PackageHouse_Package] FOREIGN KEY([PackageId])
REFERENCES [dbo].[Package] ([Id])
GO
ALTER TABLE [dbo].[PackageHouse] CHECK CONSTRAINT [FK_PackageHouse_Package]
GO
ALTER TABLE [dbo].[PackageLabor]  WITH CHECK ADD  CONSTRAINT [FK_PackageLabor_Labor] FOREIGN KEY([LaborId])
REFERENCES [dbo].[Labor] ([Id])
GO
ALTER TABLE [dbo].[PackageLabor] CHECK CONSTRAINT [FK_PackageLabor_Labor]
GO
ALTER TABLE [dbo].[PackageLabor]  WITH CHECK ADD  CONSTRAINT [FK_PackageLabor_PackageDetail] FOREIGN KEY([PackageDetailId])
REFERENCES [dbo].[PackageDetail] ([Id])
GO
ALTER TABLE [dbo].[PackageLabor] CHECK CONSTRAINT [FK_PackageLabor_PackageDetail]
GO
ALTER TABLE [dbo].[PackageMaterial]  WITH CHECK ADD  CONSTRAINT [FK_PackageMaterial_MaterialSection] FOREIGN KEY([MaterialSectionId])
REFERENCES [dbo].[MaterialSection] ([Id])
GO
ALTER TABLE [dbo].[PackageMaterial] CHECK CONSTRAINT [FK_PackageMaterial_MaterialSection]
GO
ALTER TABLE [dbo].[PackageMaterial]  WITH CHECK ADD  CONSTRAINT [FK_PackageMaterial_PackageMaterial] FOREIGN KEY([PackageDetailId])
REFERENCES [dbo].[PackageDetail] ([Id])
GO
ALTER TABLE [dbo].[PackageMaterial] CHECK CONSTRAINT [FK_PackageMaterial_PackageMaterial]
GO
ALTER TABLE [dbo].[PackageQuotation]  WITH CHECK ADD  CONSTRAINT [FK_PackageQuotation_InitialQuotation] FOREIGN KEY([InitialQuotationId])
REFERENCES [dbo].[InitialQuotation] ([Id])
GO
ALTER TABLE [dbo].[PackageQuotation] CHECK CONSTRAINT [FK_PackageQuotation_InitialQuotation]
GO
ALTER TABLE [dbo].[PackageQuotation]  WITH CHECK ADD  CONSTRAINT [FK_PackageQuotation_Package] FOREIGN KEY([PackageId])
REFERENCES [dbo].[Package] ([Id])
GO
ALTER TABLE [dbo].[PackageQuotation] CHECK CONSTRAINT [FK_PackageQuotation_Package]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_BactchPayment] FOREIGN KEY([BatchPaymentId])
REFERENCES [dbo].[BatchPayment] ([Id])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_BactchPayment]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_PaymentType] FOREIGN KEY([PaymentTypeId])
REFERENCES [dbo].[PaymentType] ([Id])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_PaymentType]
GO
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_Project_Customer]
GO
ALTER TABLE [dbo].[QuotationItem]  WITH CHECK ADD  CONSTRAINT [FK_QuotationItem_QuotationSection] FOREIGN KEY([QuotationSectionId])
REFERENCES [dbo].[QuotationSection] ([Id])
GO
ALTER TABLE [dbo].[QuotationItem] CHECK CONSTRAINT [FK_QuotationItem_QuotationSection]
GO
ALTER TABLE [dbo].[QuotationLabor]  WITH CHECK ADD  CONSTRAINT [FK_QuotationLabor_Labor] FOREIGN KEY([LaborId])
REFERENCES [dbo].[Labor] ([Id])
GO
ALTER TABLE [dbo].[QuotationLabor] CHECK CONSTRAINT [FK_QuotationLabor_Labor]
GO
ALTER TABLE [dbo].[QuotationLabor]  WITH CHECK ADD  CONSTRAINT [FK_QuotationLabor_QuotationItem] FOREIGN KEY([QuotationItemId])
REFERENCES [dbo].[QuotationItem] ([Id])
GO
ALTER TABLE [dbo].[QuotationLabor] CHECK CONSTRAINT [FK_QuotationLabor_QuotationItem]
GO
ALTER TABLE [dbo].[QuotationMaterial]  WITH CHECK ADD  CONSTRAINT [FK_QuotationMaterial_Material] FOREIGN KEY([MaterialId])
REFERENCES [dbo].[Material] ([Id])
GO
ALTER TABLE [dbo].[QuotationMaterial] CHECK CONSTRAINT [FK_QuotationMaterial_Material]
GO
ALTER TABLE [dbo].[QuotationMaterial]  WITH CHECK ADD  CONSTRAINT [FK_QuotationMaterial_QuotationItem] FOREIGN KEY([QuotationItemId])
REFERENCES [dbo].[QuotationItem] ([Id])
GO
ALTER TABLE [dbo].[QuotationMaterial] CHECK CONSTRAINT [FK_QuotationMaterial_QuotationItem]
GO
ALTER TABLE [dbo].[QuotationSection]  WITH CHECK ADD  CONSTRAINT [FK_QuotationSection_Package] FOREIGN KEY([PackageId])
REFERENCES [dbo].[Package] ([Id])
GO
ALTER TABLE [dbo].[QuotationSection] CHECK CONSTRAINT [FK_QuotationSection_Package]
GO
ALTER TABLE [dbo].[QuotationUtilities]  WITH CHECK ADD  CONSTRAINT [FK_QuoationUltities_UltilitiesItem] FOREIGN KEY([UtilitiesItemId])
REFERENCES [dbo].[UtilitiesItem] ([Id])
GO
ALTER TABLE [dbo].[QuotationUtilities] CHECK CONSTRAINT [FK_QuoationUltities_UltilitiesItem]
GO
ALTER TABLE [dbo].[QuotationUtilities]  WITH CHECK ADD  CONSTRAINT [FK_QuotationUtilities_InitialQuotation] FOREIGN KEY([InitialQuotationId])
REFERENCES [dbo].[InitialQuotation] ([Id])
GO
ALTER TABLE [dbo].[QuotationUtilities] CHECK CONSTRAINT [FK_QuotationUtilities_InitialQuotation]
GO
ALTER TABLE [dbo].[SubConstructionItems]  WITH CHECK ADD  CONSTRAINT [FK_SubConstructionItems_ConstructionItems] FOREIGN KEY([ConstructionItemsId])
REFERENCES [dbo].[ConstructionItems] ([Id])
GO
ALTER TABLE [dbo].[SubConstructionItems] CHECK CONSTRAINT [FK_SubConstructionItems_ConstructionItems]
GO
ALTER TABLE [dbo].[SubTemplate]  WITH CHECK ADD  CONSTRAINT [FK_SubTemplate_DesignTemplate] FOREIGN KEY([DesignTemplateId])
REFERENCES [dbo].[DesignTemplate] ([Id])
GO
ALTER TABLE [dbo].[SubTemplate] CHECK CONSTRAINT [FK_SubTemplate_DesignTemplate]
GO
ALTER TABLE [dbo].[TemplateItem]  WITH CHECK ADD  CONSTRAINT [FK_TemplateItem_ConstructionItems] FOREIGN KEY([ConstructionItemId])
REFERENCES [dbo].[ConstructionItems] ([Id])
GO
ALTER TABLE [dbo].[TemplateItem] CHECK CONSTRAINT [FK_TemplateItem_ConstructionItems]
GO
ALTER TABLE [dbo].[TemplateItem]  WITH CHECK ADD  CONSTRAINT [FK_TemplateItem_SubTemplate] FOREIGN KEY([SubTemplateId])
REFERENCES [dbo].[SubTemplate] ([Id])
GO
ALTER TABLE [dbo].[TemplateItem] CHECK CONSTRAINT [FK_TemplateItem_SubTemplate]
GO
ALTER TABLE [dbo].[TemplateItem]  WITH CHECK ADD  CONSTRAINT [TemplateItem_SubConstructionItems_FK] FOREIGN KEY([SubConstructionId])
REFERENCES [dbo].[SubConstructionItems] ([Id])
GO
ALTER TABLE [dbo].[TemplateItem] CHECK CONSTRAINT [TemplateItem_SubConstructionItems_FK]
GO
ALTER TABLE [dbo].[UtilitiesItem]  WITH CHECK ADD  CONSTRAINT [FK_UltilitiesItem_UltilitiesSection] FOREIGN KEY([SectionId])
REFERENCES [dbo].[UtilitiesSection] ([Id])
GO
ALTER TABLE [dbo].[UtilitiesItem] CHECK CONSTRAINT [FK_UltilitiesItem_UltilitiesSection]
GO
ALTER TABLE [dbo].[UtilitiesSection]  WITH CHECK ADD  CONSTRAINT [FK_UltilitiesSection_Ultilities] FOREIGN KEY([UtilitiesId])
REFERENCES [dbo].[UtilityOption] ([Id])
GO
ALTER TABLE [dbo].[UtilitiesSection] CHECK CONSTRAINT [FK_UltilitiesSection_Ultilities]
GO
USE [master]
GO
ALTER DATABASE [RHCQS] SET  READ_WRITE 
GO
