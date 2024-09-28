USE [master]
GO
/****** Object:  Database [RHCQS]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[Account]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[AssignTask]    Script Date: 9/28/2024 12:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssignTask](
	[Id] [uniqueidentifier] NOT NULL,
	[ProjectId] [uniqueidentifier] NOT NULL,
	[AccountId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Status] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_AssignTask] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BactchPayment]    Script Date: 9/28/2024 12:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BactchPayment](
	[Id] [uniqueidentifier] NOT NULL,
	[ContractId] [uniqueidentifier] NOT NULL,
	[Price] [float] NULL,
	[PaymentDate] [datetime] NULL,
	[PaymentPhase] [datetime] NULL,
	[IntitialQuotationId] [uniqueidentifier] NOT NULL,
	[Percents] [nvarchar](5) NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_InstallmentPayment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Blog]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[ConstructionItems]    Script Date: 9/28/2024 12:49:43 AM ******/
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
 CONSTRAINT [PK_ConstructionItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contract]    Script Date: 9/28/2024 12:49:43 AM ******/
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
 CONSTRAINT [PK_Contract] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DesignTemplate]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[DetailedQuotation]    Script Date: 9/28/2024 12:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetailedQuotation](
	[Id] [uniqueidentifier] NOT NULL,
	[ProjectId] [uniqueidentifier] NOT NULL,
	[PromotionId] [uniqueidentifier] NULL,
	[TotalPrice] [float] NULL,
	[Note] [nvarchar](max) NULL,
	[Version] [nvarchar](10) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[Status] [nvarchar](50) NULL,
	[Deflag] [bit] NULL,
	[QuotationUlititiesId] [uniqueidentifier] NOT NULL,
	[AccountId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_OfficialQuotation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetailedQuotationItem]    Script Date: 9/28/2024 12:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetailedQuotationItem](
	[Id] [uniqueidentifier] NOT NULL,
	[DetailQuotationId] [uniqueidentifier] NOT NULL,
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
 CONSTRAINT [PK_DetailedQuotationItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HouseDesignDrawing]    Script Date: 9/28/2024 12:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HouseDesignDrawing](
	[Id] [uniqueidentifier] NOT NULL,
	[ProjectId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Step] [nvarchar](100) NULL,
	[Version] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[IsCompany] [bit] NULL,
	[InsDate] [datetime] NULL,
	[AssignTaskId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_HouseDesignDrawing] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InitialQuotation]    Script Date: 9/28/2024 12:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InitialQuotation](
	[Id] [uniqueidentifier] NOT NULL,
	[AccountId] [uniqueidentifier] NOT NULL,
	[ProjectId] [uniqueidentifier] NOT NULL,
	[PromotionId] [uniqueidentifier] NOT NULL,
	[PackageId] [uniqueidentifier] NOT NULL,
	[Area] [float] NULL,
	[TimeProcessing] [datetime] NULL,
	[TimeRough] [datetime] NULL,
	[TimeOthers] [datetime] NULL,
	[OthersAgreement] [nvarchar](max) NULL,
	[InsDate] [datetime] NULL,
	[Status] [nvarchar](50) NULL,
	[Version] [nvarchar](50) NULL,
	[IsTemplate] [bit] NULL,
	[Deflag] [bit] NULL,
	[Note] [nvarchar](max) NULL,
 CONSTRAINT [PK_PreliminaryQuotation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InitialQuotationItem]    Script Date: 9/28/2024 12:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InitialQuotationItem](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[ConstructionItemId] [uniqueidentifier] NULL,
	[SubConstruction] [uniqueidentifier] NULL,
	[Area] [float] NULL,
	[Price] [float] NULL,
	[UnitPrice] [float] NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[InitialQuotationId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_InitialQuotationItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Labor]    Script Date: 9/28/2024 12:49:43 AM ******/
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
 CONSTRAINT [PK_Labor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Material]    Script Date: 9/28/2024 12:49:43 AM ******/
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
 CONSTRAINT [PK_Material] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaterialSection]    Script Date: 9/28/2024 12:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MaterialSection](
	[Id] [uniqueidentifier] NOT NULL,
	[MaterialId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_MaterialSection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaterialType]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[Media]    Script Date: 9/28/2024 12:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Media](
	[Id] [uniqueidentifier] NOT NULL,
	[HouseDesignDrawingId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Url] [nvarchar](max) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
 CONSTRAINT [PK_Media] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Message]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[Package]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[PackageDetail]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[PackageHouse]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[PackageLabor]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[PackageMaterial]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[PackageQuotation]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[PackageType]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[Payment]    Script Date: 9/28/2024 12:49:43 AM ******/
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
	[Type] [nvarchar](100) NULL,
	[TotalPrice] [float] NULL,
 CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentType]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[Project]    Script Date: 9/28/2024 12:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[Id] [uniqueidentifier] NOT NULL,
	[AccountId] [uniqueidentifier] NOT NULL,
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
/****** Object:  Table [dbo].[Promotion]    Script Date: 9/28/2024 12:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Promotion](
	[Id] [uniqueidentifier] NOT NULL,
	[Code] [nchar](10) NULL,
	[AccountId] [uniqueidentifier] NULL,
	[Value] [int] NULL,
	[AvailableTime] [datetime] NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
 CONSTRAINT [PK_Promotion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuoationUltities]    Script Date: 9/28/2024 12:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuoationUltities](
	[Id] [uniqueidentifier] NOT NULL,
	[UltilitiesItemId] [uniqueidentifier] NOT NULL,
	[DetailedQuotationId] [uniqueidentifier] NOT NULL,
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
/****** Object:  Table [dbo].[QuotationItem]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[QuotationLabor]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[QuotationMaterial]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[QuotationSection]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[Role]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[SubConstructionItems]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[SubTemplate]    Script Date: 9/28/2024 12:49:43 AM ******/
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
	[Size] [nchar](20) NULL,
 CONSTRAINT [PK_SubTemplate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Supplier]    Script Date: 9/28/2024 12:49:43 AM ******/
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
/****** Object:  Table [dbo].[TemplateItem]    Script Date: 9/28/2024 12:49:43 AM ******/
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
 CONSTRAINT [PK_TemplateItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ultilities]    Script Date: 9/28/2024 12:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ultilities](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
 CONSTRAINT [PK_Ultilities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UltilitiesItem]    Script Date: 9/28/2024 12:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UltilitiesItem](
	[Id] [uniqueidentifier] NOT NULL,
	[SectionId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Coefficient] [float] NULL,
	[Description] [nvarchar](max) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
 CONSTRAINT [PK_UltilitiesItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UltilitiesSection]    Script Date: 9/28/2024 12:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UltilitiesSection](
	[Id] [uniqueidentifier] NOT NULL,
	[UltilitiesId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Status] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
 CONSTRAINT [PK_UltilitiesSection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'990773a2-1817-47f5-9116-301e97435c44', N'789dd57d-0f75-40d1-8366-ef6ab582efc8', N'dohehehe@gmail.com', N'Đồ', NULL, N'$2a$12$UUBeF1uXFz9fDVrTFnGFy.Ea7TV2MiRR9BR3DzIxrIXUOlUo3lqpO', N'0902579392', CAST(N'2002-01-01' AS Date), CAST(N'2024-09-26T14:30:00.000' AS DateTime), CAST(N'2024-09-26T14:30:00.000' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'7e9055c4-fa2e-45b4-b3e8-580870184283', N'789dd57d-0f75-40d1-8366-ef6ab582efc8', N'ngandolh@gmail.com', N'Ngân ', NULL, N'$2a$12$UUBeF1uXFz9fDVrTFnGFy.Ea7TV2MiRR9BR3DzIxrIXUOlUo3lqpO', N'0906697051', CAST(N'2002-01-01' AS Date), CAST(N'2024-09-26T14:30:00.000' AS DateTime), CAST(N'2024-09-26T14:30:00.000' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'de455c55-1d0a-43e3-9581-842a127b0d65', N'789dd57d-0f75-40d1-8366-ef6ab582efc8', N'tiennv@gmail.com', N'Customer', NULL, N'$2a$12$K8mjN6joedfVAcjhNnDRwOL3LOvA10Benee4skY02wbmVlyjt5wa2', NULL, NULL, CAST(N'2024-09-27T15:36:51.430' AS DateTime), CAST(N'2024-09-27T15:36:51.430' AS DateTime), 1)
GO
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'15b85094-21a2-46e7-95aa-0fba6890ae36', N'Thông Tầng lầu 4', 0, N'm2', CAST(N'2024-09-27T15:35:37.630' AS DateTime), CAST(N'2024-09-27T15:35:37.630' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'40aa0395-a4ef-423c-91f2-11d2889dc306', N'Thông Tầng lầu 1', 0, N'm2', CAST(N'2024-09-27T15:34:23.203' AS DateTime), CAST(N'2024-09-27T15:34:23.203' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'75922602-9153-4cc3-a7dc-225c9bc30a5e', N'Móng', 0, N'm2', CAST(N'2024-09-27T21:37:09.000' AS DateTime), CAST(N'2024-09-27T21:37:09.000' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'eab92d07-c140-4a5f-a22d-23cee8a1540e', N'Thông Tầng lầu 5', 0, N'm2', CAST(N'2024-09-27T15:35:43.087' AS DateTime), CAST(N'2024-09-27T15:35:43.087' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'be6c6db7-cea1-4275-9b18-2fbcfe9b2353', N'Trệt', 1, N'm2', CAST(N'2024-09-27T21:37:09.000' AS DateTime), CAST(N'2024-09-27T21:37:09.000' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'5a000367-77c4-4c16-b533-30cd8d0d6017', N'Thông Tầng lầu 2', 0, N'm2', CAST(N'2024-09-27T15:35:25.247' AS DateTime), CAST(N'2024-09-27T15:35:25.247' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'd8f3d72b-042f-4108-b9ea-3f93666921d2', N'Mái phụ', 0, N'm2', CAST(N'2024-09-27T22:11:44.510' AS DateTime), CAST(N'2024-09-27T22:11:44.510' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'13cd15d8-d1fb-48fb-8fbd-7523bc7fd04f', N'Phòng kỹ thuật thang máy', 0.5, N'm2', CAST(N'2024-09-27T15:43:05.070' AS DateTime), CAST(N'2024-09-27T15:43:05.070' AS DateTime), NULL)
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'4320ec5e-a71c-4161-95ae-857bd80763e5', N'Lầu 6', 1, N'm2', CAST(N'2024-09-27T15:37:13.277' AS DateTime), CAST(N'2024-09-27T15:37:13.277' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'bd101af5-ac48-43ba-a474-957a20a933bd', N'Mái che', 0, N'm2', CAST(N'2024-09-27T21:37:09.000' AS DateTime), CAST(N'2024-09-27T21:37:09.000' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'22fc6201-3f75-4f4f-8a51-9bf25b2b13c3', N'Lầu 2', 1, N'm2', CAST(N'2024-09-27T15:36:51.893' AS DateTime), CAST(N'2024-09-27T15:36:51.893' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'eba29420-a8db-455c-86b0-b325a1da4e1e', N'Lầu 1', 1, N'm2', CAST(N'2024-09-27T15:36:43.490' AS DateTime), CAST(N'2024-09-27T15:36:43.490' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'22b90619-892f-4aaf-9f78-b6467faee47b', N'Lầu 4', 1, N'm2', CAST(N'2024-09-27T15:37:02.927' AS DateTime), CAST(N'2024-09-27T15:37:02.927' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'903683df-19a9-4737-a8ac-c3dc0aa80949', N'Lầu 3', 1, N'm2', CAST(N'2024-09-27T15:36:56.853' AS DateTime), CAST(N'2024-09-27T15:36:56.853' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'f68ce6c6-6543-463f-b9d4-c5c967232988', N'Lầu 5', 1, N'm2', CAST(N'2024-09-27T15:37:08.653' AS DateTime), CAST(N'2024-09-27T15:37:08.653' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'75c3e21a-0abd-4699-abbe-c8371ed4d49a', N'Sân', 0.7, N'm2', CAST(N'2024-09-27T21:37:09.000' AS DateTime), CAST(N'2024-09-27T21:37:09.000' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'9696145f-bebf-4120-8354-c8aef681b351', N'Thông tầng lửng', 0, N'm2', CAST(N'2024-09-27T15:38:00.537' AS DateTime), CAST(N'2024-09-27T15:38:00.537' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'a6ce35ee-d19c-40ac-8044-cbecdb54f8d9', N'Hầm', 0, N'm2', CAST(N'2024-09-27T15:41:49.113' AS DateTime), CAST(N'2024-09-27T15:41:49.113' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'98ba07e3-d76c-42b7-8c87-cfd7b11a7f4f', N'Sân thượng có mái che', 1, N'm2', CAST(N'2024-09-27T21:37:09.000' AS DateTime), CAST(N'2024-09-27T21:37:09.000' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'6aa8e2fc-370e-4e6c-8269-d22d2c060e65', N'Thông Tầng lầu 6', 0, N'm2', CAST(N'2024-09-27T15:35:48.477' AS DateTime), CAST(N'2024-09-27T15:35:48.477' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'15030799-9d27-4270-b015-d9058d494e03', N'Sân thượng không có mái che', 0.5, N'm2', CAST(N'2024-09-27T21:37:09.000' AS DateTime), CAST(N'2024-09-27T21:37:09.000' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'f996f171-f8c6-4022-9152-fcccc486341e', N'Thông Tầng lầu 3', 0, N'm2', CAST(N'2024-09-27T15:35:31.800' AS DateTime), CAST(N'2024-09-27T15:35:31.800' AS DateTime), N'ROUGH')
GO
INSERT [dbo].[DesignTemplate] ([Id], [Name], [Description], [NumberOfFloor], [NumberOfBed], [NumberOfFront], [ImgUrl], [InsDate]) VALUES (N'7af799a9-02fa-4cd0-b954-86b102840e60', N'Nhà 1', NULL, 2, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Project] ([Id], [AccountId], [Name], [Type], [Status], [InsDate], [UpsDate], [ProjectCode], [Address], [Area]) VALUES (N'fa91e187-b0c8-46c3-b6d5-747d32bd3540', N'990773a2-1817-47f5-9116-301e97435c44', N'Dự án báo giá 09-26-2024', N'ALL', N'Processing', CAST(N'2024-09-26T14:30:00.000' AS DateTime), CAST(N'2024-09-26T14:30:00.000' AS DateTime), N'R84GH', N'45/3 Đường Dương Đình Hội, Phường Phước Long B, Quận 9, TP. Thủ Đức', 252)
INSERT [dbo].[Project] ([Id], [AccountId], [Name], [Type], [Status], [InsDate], [UpsDate], [ProjectCode], [Address], [Area]) VALUES (N'b841c593-1d05-4492-9ba2-d485f6d67260', N'7e9055c4-fa2e-45b4-b3e8-580870184283', N'Dự án báo giá 09-25-2024', N'FINISHED', N'Processing', CAST(N'2024-09-11T09:15:00.000' AS DateTime), CAST(N'2024-09-11T09:15:00.000' AS DateTime), N'G8453', N'78/9 Đường Lê Văn Việt, Phường Hiệp Phú, TP. Thủ Đức', 341)
INSERT [dbo].[Project] ([Id], [AccountId], [Name], [Type], [Status], [InsDate], [UpsDate], [ProjectCode], [Address], [Area]) VALUES (N'310806e2-876b-48d6-a87a-e534e4ffa647', N'7e9055c4-fa2e-45b4-b3e8-580870184283', N'Dự án báo giá 09-20-2024', N'ROUGH', N'Processing', CAST(N'2024-09-11T09:15:00.000' AS DateTime), CAST(N'2024-09-11T09:15:00.000' AS DateTime), N'F3421', N'120/5 Đường Nguyễn Xiển, Phường Long Thạnh Mỹ, TP.Thủ Đức', 125)
INSERT [dbo].[Project] ([Id], [AccountId], [Name], [Type], [Status], [InsDate], [UpsDate], [ProjectCode], [Address], [Area]) VALUES (N'c64e1637-635b-4d03-a68e-ed5f81d3fd57', N'7e9055c4-fa2e-45b4-b3e8-580870184283', N'Dự án báo giá 09-26-2024', N'ALL', N'Processing', CAST(N'2024-09-11T09:15:00.000' AS DateTime), CAST(N'2024-09-11T09:15:00.000' AS DateTime), N'E2245', N'56/4 Đường Nguyễn Duy Trinh, Phường Trường Thạnh, TP.Thủ Đức', 101)
GO
INSERT [dbo].[Role] ([Id], [RoleName]) VALUES (N'9959ce96-de26-40a7-b8a7-28a704062e89', N'Sales Staff')
INSERT [dbo].[Role] ([Id], [RoleName]) VALUES (N'7af0d75e-1157-48b4-899d-3196deed5fad', N'Design Staff')
INSERT [dbo].[Role] ([Id], [RoleName]) VALUES (N'789dd57d-0f75-40d1-8366-ef6ab582efc8', N'Customer')
GO
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'3b097855-0c93-4955-a25c-062abd15cf74', N'd8f3d72b-042f-4108-b9ea-3f93666921d2', N'Mái Ngói Kèo Thép (nghiêng 30 độ)', 0.91, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'f3d73d84-c6c5-47e3-9826-075db344d1ec', N'6aa8e2fc-370e-4e6c-8269-d22d2c060e65', N'Thông tầng < 8m2', 1, N'm2        ', CAST(N'2024-09-27T15:35:48.480' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'becab190-4935-4975-81d6-08630e7df54b', N'15b85094-21a2-46e7-95aa-0fba6890ae36', N'Thông tầng < 8m2', 1, N'm2        ', CAST(N'2024-09-27T15:35:37.633' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'40a79928-c9bb-4338-a8fb-0b0e9389e40e', N'a6ce35ee-d19c-40ac-8044-cbecdb54f8d9', N'DTSD < 70m2: độ sâu 1,7m -> 2m', 2.2, N'm2        ', CAST(N'2024-09-27T15:41:49.117' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'06ff2d3d-2f14-4acc-ba2e-0c4ee659ca81', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', N'Móng đơn', 0.2, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'287f222c-2d3b-4445-900c-2a32101bec00', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', N'Móng bè, móng 2 phương', 0.6, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'a53e84ae-c98c-46d0-9b4d-355ce942b5dd', N'd8f3d72b-042f-4108-b9ea-3f93666921d2', N'Mái Ngói Kèo Thép (nghiêng 45 độ)', 0.98, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'dbd38b0a-9263-4e52-a23a-35648ca4581d', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', N'Móng bằng', 0.4, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'990dda26-6171-42e7-b142-3a817dfeddb4', N'bd101af5-ac48-43ba-a474-957a20a933bd', N'Mái BTCT lợp ngói (nghiêng 30 độ)', 1.3, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'11f81c2a-bb8f-40eb-9619-3c119901f57f', N'5a000367-77c4-4c16-b533-30cd8d0d6017', N'Thông tầng < 8m2', 1, N'm2        ', CAST(N'2024-09-27T15:35:25.293' AS DateTime))
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
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'009e7ab2-3963-4edf-b520-8afb87cb7bef', N'bd101af5-ac48-43ba-a474-957a20a933bd', N'Mái BTCT lợp ngói (nghiêng 45 độ)', 1.4, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'8672e9ec-b175-4114-8b62-95ef3c4d903f', N'6aa8e2fc-370e-4e6c-8269-d22d2c060e65', N'Thông tầng > 8m2', 0.5, N'm2        ', CAST(N'2024-09-27T15:35:48.477' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'24193e3f-bc09-48f8-b7d9-9990f51ef4be', N'd8f3d72b-042f-4108-b9ea-3f93666921d2', N'Mái BTCT nghiêng', 0.7, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'b88899fd-aa23-4a66-ad1e-9a5a4dd03bca', N'40aa0395-a4ef-423c-91f2-11d2889dc306', N'Thông tầng > 8m2', 0.5, N'm2        ', CAST(N'2024-09-27T15:34:24.093' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'bf5ca42f-87d8-4520-b2d4-9cd8e36c6f42', N'bd101af5-ac48-43ba-a474-957a20a933bd', N'Mái BTCT nghiêng', 0.7, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'7e442652-eefc-43b7-918b-a264a10e679d', N'bd101af5-ac48-43ba-a474-957a20a933bd', N'Mái BTCT', 0.5, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'cb61a178-b341-4c53-8bce-b1dc7289ca91', N'a6ce35ee-d19c-40ac-8044-cbecdb54f8d9', N'DTSD < 70m2: độ sâu 1,0m -> 1,3m', 1.7, N'm2        ', CAST(N'2024-09-27T15:41:49.113' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'259152e0-fa9f-424c-8410-b4013d77137b', N'f996f171-f8c6-4022-9152-fcccc486341e', N'Thông tầng > 8m2', 0.5, N'm2        ', CAST(N'2024-09-27T15:35:31.803' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'708bad20-af57-4ab6-96e9-c22a22273724', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', N'Móng cọc', 0.3, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'27593f3d-f834-4729-8170-ddaa5cb0f747', N'9696145f-bebf-4120-8354-c8aef681b351', N'Thông tầng > 8m2', 0.5, N'm2        ', CAST(N'2024-09-27T15:38:00.537' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'51cd0448-7fba-4630-a96e-dff7405caad3', N'a6ce35ee-d19c-40ac-8044-cbecdb54f8d9', N'DTSD >= 70m2: độ sâu 1,3m -> 1,7m', 1.7, N'm2        ', CAST(N'2024-09-27T15:41:49.113' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'dff5fdc9-2275-4aaf-8d96-e63874ec153f', N'f996f171-f8c6-4022-9152-fcccc486341e', N'Thông tầng < 8m2', 1, N'm2        ', CAST(N'2024-09-27T15:35:31.803' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'78b9ace5-547b-4e42-964d-eb5dc220bd0d', N'40aa0395-a4ef-423c-91f2-11d2889dc306', N'Thông tầng < 8m2', 1, N'm2        ', CAST(N'2024-09-27T15:34:24.203' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'783c52e5-7d4c-4948-8357-f4ea7557c4e2', N'd8f3d72b-042f-4108-b9ea-3f93666921d2', N'Mái BTCT', 0.5, N'm2        ', CAST(N'2024-09-27T22:11:53.113' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'3a09a8da-e768-4b87-befc-f5365291db0c', N'd8f3d72b-042f-4108-b9ea-3f93666921d2', N'Mái BTCT lợp ngói (nghiêng 30 độ)', 1.3, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
INSERT [dbo].[SubConstructionItems] ([Id], [ConstructionItemsId], [Name], [Coefficient], [Unit], [InsDate]) VALUES (N'cd3d06cc-1f01-4010-b908-fc5d3ef244e9', N'bd101af5-ac48-43ba-a474-957a20a933bd', N'Mái ngói kèo thép (nghiêng 45 độ)', 0.98, N'm2        ', CAST(N'2024-09-27T21:37:09.000' AS DateTime))
GO
INSERT [dbo].[SubTemplate] ([Id], [DesignTemplateId], [BuildingArea], [FloorArea], [InsDate], [Size]) VALUES (N'd4afdc22-a07a-486f-b521-a9068d391821', N'7af799a9-02fa-4cd0-b954-86b102840e60', 366.1974, 117, NULL, N'R9 x D13            ')
INSERT [dbo].[SubTemplate] ([Id], [DesignTemplateId], [BuildingArea], [FloorArea], [InsDate], [Size]) VALUES (N'75723c8d-6c83-445c-83ff-d428895d5173', N'7af799a9-02fa-4cd0-b954-86b102840e60', 343.2942, 108, NULL, N'R9 x D12            ')
GO
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate]) VALUES (N'56154fda-125e-4610-ad17-027c1c3ae2d2', N'd4afdc22-a07a-486f-b521-a9068d391821', N'75c3e21a-0abd-4699-abbe-c8371ed4d49a', 28224, N'm2', NULL)
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate]) VALUES (N'24b60d27-88a7-4470-bf34-6ae17a587793', N'd4afdc22-a07a-486f-b521-a9068d391821', N'bd101af5-ac48-43ba-a474-957a20a933bd', 70.875, N'm2', NULL)
INSERT [dbo].[TemplateItem] ([Id], [SubTemplateId], [ConstructionItemId], [Area], [Unit], [InsDate]) VALUES (N'21e2cc6f-d1d2-4044-a758-c33eb2303715', N'd4afdc22-a07a-486f-b521-a9068d391821', N'be6c6db7-cea1-4275-9b18-2fbcfe9b2353', 117, N'm2', NULL)
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
ALTER TABLE [dbo].[BactchPayment]  WITH CHECK ADD  CONSTRAINT [FK_BactchPayment_Contract] FOREIGN KEY([ContractId])
REFERENCES [dbo].[Contract] ([Id])
GO
ALTER TABLE [dbo].[BactchPayment] CHECK CONSTRAINT [FK_BactchPayment_Contract]
GO
ALTER TABLE [dbo].[BactchPayment]  WITH CHECK ADD  CONSTRAINT [FK_BactchPayment_InitialQuotation] FOREIGN KEY([IntitialQuotationId])
REFERENCES [dbo].[InitialQuotation] ([Id])
GO
ALTER TABLE [dbo].[BactchPayment] CHECK CONSTRAINT [FK_BactchPayment_InitialQuotation]
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
ALTER TABLE [dbo].[DetailedQuotation]  WITH CHECK ADD  CONSTRAINT [FK_DetailedQuotation_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[DetailedQuotation] CHECK CONSTRAINT [FK_DetailedQuotation_Account]
GO
ALTER TABLE [dbo].[DetailedQuotation]  WITH CHECK ADD  CONSTRAINT [FK_DetailedQuotation_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO
ALTER TABLE [dbo].[DetailedQuotation] CHECK CONSTRAINT [FK_DetailedQuotation_Project]
GO
ALTER TABLE [dbo].[DetailedQuotation]  WITH CHECK ADD  CONSTRAINT [FK_DetailedQuotation_Promotion] FOREIGN KEY([PromotionId])
REFERENCES [dbo].[Promotion] ([Id])
GO
ALTER TABLE [dbo].[DetailedQuotation] CHECK CONSTRAINT [FK_DetailedQuotation_Promotion]
GO
ALTER TABLE [dbo].[DetailedQuotation]  WITH CHECK ADD  CONSTRAINT [FK_DetailedQuotation_QuoationUltities] FOREIGN KEY([QuotationUlititiesId])
REFERENCES [dbo].[QuoationUltities] ([Id])
GO
ALTER TABLE [dbo].[DetailedQuotation] CHECK CONSTRAINT [FK_DetailedQuotation_QuoationUltities]
GO
ALTER TABLE [dbo].[DetailedQuotationItem]  WITH CHECK ADD  CONSTRAINT [FK_DetailedQuotationItem_DetailedQuotation] FOREIGN KEY([DetailQuotationId])
REFERENCES [dbo].[DetailedQuotation] ([Id])
GO
ALTER TABLE [dbo].[DetailedQuotationItem] CHECK CONSTRAINT [FK_DetailedQuotationItem_DetailedQuotation]
GO
ALTER TABLE [dbo].[HouseDesignDrawing]  WITH CHECK ADD  CONSTRAINT [FK_HouseDesignDrawing_AssignTask] FOREIGN KEY([AssignTaskId])
REFERENCES [dbo].[AssignTask] ([Id])
GO
ALTER TABLE [dbo].[HouseDesignDrawing] CHECK CONSTRAINT [FK_HouseDesignDrawing_AssignTask]
GO
ALTER TABLE [dbo].[InitialQuotation]  WITH CHECK ADD  CONSTRAINT [FK_InitialQuotation_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[InitialQuotation] CHECK CONSTRAINT [FK_InitialQuotation_Account]
GO
ALTER TABLE [dbo].[InitialQuotation]  WITH CHECK ADD  CONSTRAINT [FK_InitialQuotation_Package] FOREIGN KEY([PackageId])
REFERENCES [dbo].[Package] ([Id])
GO
ALTER TABLE [dbo].[InitialQuotation] CHECK CONSTRAINT [FK_InitialQuotation_Package]
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
ALTER TABLE [dbo].[MaterialSection]  WITH CHECK ADD  CONSTRAINT [FK_MaterialSection_Material] FOREIGN KEY([MaterialId])
REFERENCES [dbo].[Material] ([Id])
GO
ALTER TABLE [dbo].[MaterialSection] CHECK CONSTRAINT [FK_MaterialSection_Material]
GO
ALTER TABLE [dbo].[Media]  WITH CHECK ADD  CONSTRAINT [FK_Media_HouseDesignDrawing] FOREIGN KEY([HouseDesignDrawingId])
REFERENCES [dbo].[HouseDesignDrawing] ([Id])
GO
ALTER TABLE [dbo].[Media] CHECK CONSTRAINT [FK_Media_HouseDesignDrawing]
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
REFERENCES [dbo].[BactchPayment] ([Id])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_BactchPayment]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment_PaymentType] FOREIGN KEY([PaymentTypeId])
REFERENCES [dbo].[PaymentType] ([Id])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment_PaymentType]
GO
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_Project_Account]
GO
ALTER TABLE [dbo].[QuoationUltities]  WITH CHECK ADD  CONSTRAINT [FK_QuoationUltities_UltilitiesItem] FOREIGN KEY([UltilitiesItemId])
REFERENCES [dbo].[UltilitiesItem] ([Id])
GO
ALTER TABLE [dbo].[QuoationUltities] CHECK CONSTRAINT [FK_QuoationUltities_UltilitiesItem]
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
ALTER TABLE [dbo].[UltilitiesItem]  WITH CHECK ADD  CONSTRAINT [FK_UltilitiesItem_UltilitiesSection] FOREIGN KEY([SectionId])
REFERENCES [dbo].[UltilitiesSection] ([Id])
GO
ALTER TABLE [dbo].[UltilitiesItem] CHECK CONSTRAINT [FK_UltilitiesItem_UltilitiesSection]
GO
ALTER TABLE [dbo].[UltilitiesSection]  WITH CHECK ADD  CONSTRAINT [FK_UltilitiesSection_Ultilities] FOREIGN KEY([UltilitiesId])
REFERENCES [dbo].[Ultilities] ([Id])
GO
ALTER TABLE [dbo].[UltilitiesSection] CHECK CONSTRAINT [FK_UltilitiesSection_Ultilities]
GO
USE [master]
GO
ALTER DATABASE [RHCQS] SET  READ_WRITE 
GO
