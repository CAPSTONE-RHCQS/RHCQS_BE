USE [master]
GO
/****** Object:  Database [RHCQS]    Script Date: 10/7/2024 2:38:03 AM ******/
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
/****** Object:  Table [dbo].[Account]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[AssignTask]    Script Date: 10/7/2024 2:38:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssignTask](
	[Id] [uniqueidentifier] NOT NULL,
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
/****** Object:  Table [dbo].[BatchPayment]    Script Date: 10/7/2024 2:38:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BatchPayment](
	[Id] [uniqueidentifier] NOT NULL,
	[ContractId] [uniqueidentifier] NOT NULL,
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
/****** Object:  Table [dbo].[Blog]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[ConstructionItems]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[Contract]    Script Date: 10/7/2024 2:38:04 AM ******/
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
 CONSTRAINT [PK_Contract] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 10/7/2024 2:38:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](50) NULL,
	[Username] [nvarchar](50) NULL,
	[ImgUrl] [nvarchar](max) NULL,
	[PasswordHash] [nvarchar](60) NULL,
	[PhoneNumber] [nchar](11) NULL,
	[DateOfBirth] [datetime] NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[Deflag] [bit] NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DesignTemplate]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[EquipmentItem]    Script Date: 10/7/2024 2:38:04 AM ******/
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
 CONSTRAINT [PK_EquimentItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinalQuotation]    Script Date: 10/7/2024 2:38:04 AM ******/
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
	[AccountId] [uniqueidentifier] NOT NULL,
	[ReasonReject] [nvarchar](max) NULL,
 CONSTRAINT [PK_OfficialQuotation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinalQuotationItem]    Script Date: 10/7/2024 2:38:04 AM ******/
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
 CONSTRAINT [PK_DetailedQuotationItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HouseDesignDrawing]    Script Date: 10/7/2024 2:38:04 AM ******/
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
	[AssignTaskId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_HouseDesignDrawing] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HouseDesignVersion]    Script Date: 10/7/2024 2:38:04 AM ******/
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
	[FileUrl] [nvarchar](max) NULL,
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
/****** Object:  Table [dbo].[InitialQuotation]    Script Date: 10/7/2024 2:38:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InitialQuotation](
	[Id] [uniqueidentifier] NOT NULL,
	[AccountId] [uniqueidentifier] NULL,
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
/****** Object:  Table [dbo].[InitialQuotationItem]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[Labor]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[Material]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[MaterialSection]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[MaterialType]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[Media]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[Message]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[Package]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[PackageDetail]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[PackageHouse]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[PackageLabor]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[PackageMaterial]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[PackageQuotation]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[PackageType]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[Payment]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[PaymentType]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[Project]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[Promotion]    Script Date: 10/7/2024 2:38:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Promotion](
	[Id] [uniqueidentifier] NOT NULL,
	[Code] [nchar](10) NULL,
	[Value] [int] NULL,
	[AvailableTime] [datetime] NULL,
	[InsDate] [datetime] NULL,
	[Name] [nvarchar](200) NULL,
 CONSTRAINT [PK_Promotion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuotationItem]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[QuotationLabor]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[QuotationMaterial]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[QuotationSection]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[QuotationUtilities]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[Role]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[SubConstructionItems]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[SubTemplate]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[Supplier]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[TemplateItem]    Script Date: 10/7/2024 2:38:04 AM ******/
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
/****** Object:  Table [dbo].[Utilities]    Script Date: 10/7/2024 2:38:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Utilities](
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
/****** Object:  Table [dbo].[UtilitiesItem]    Script Date: 10/7/2024 2:38:04 AM ******/
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
 CONSTRAINT [PK_UltilitiesItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UtilitiesSection]    Script Date: 10/7/2024 2:38:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UtilitiesSection](
	[Id] [uniqueidentifier] NOT NULL,
	[UtilitiesId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Status] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_UltilitiesSection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'990773a2-1817-47f5-9116-301e97435c44', N'7af0d75e-1157-48b4-899d-3196deed5fad', N'design2rhcqs@gmail.com', N'Huy', N'https://cellphones.com.vn/sforum/wp-content/uploads/2024/02/avatar-anh-meo-cute-5.jpg', N'$2a$12$UUBeF1uXFz9fDVrTFnGFy.Ea7TV2MiRR9BR3DzIxrIXUOlUo3lqpO', N'0902579392', CAST(N'2002-01-01' AS Date), CAST(N'2024-09-26T14:30:00.000' AS DateTime), CAST(N'2024-09-26T14:30:00.000' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'bf339e88-5303-45c4-a6f4-33a79681766c', N'7af0d75e-1157-48b4-899d-3196deed5fad', N'design1rhcqs@gmail.com', N'design1rhcqs@gmail.com', N'https://inkythuatso.com/uploads/thumbnails/800/2022/05/anh-meo-che-anh-meo-bua-15-31-09-19-00.jpg', N'$2a$12$LqFux1Fe.MrFYw9JhwUAAuy2E.uLQTSS17cEyGQGMgZ4TYK38eykK', N'0586617799', NULL, CAST(N'2024-09-29T06:02:39.183' AS DateTime), CAST(N'2024-09-29T06:02:39.183' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'0be1eb2e-f31d-476c-800f-3a6e67ee2b08', N'789dd57d-0f75-40d1-8366-ef6ab582efc8', N'test', N'test', N'https://sieupet.com/sites/default/files/hinh_anh_meo_dep.jpg', N'$2a$12$ykiFtNtHI4JM7CZtpQRG3u5pGpXzDnjBRcY36sRvUzKd6FL0aTvLS', N'0828253777', NULL, CAST(N'2024-10-02T16:31:39.813' AS DateTime), CAST(N'2024-10-02T16:31:39.813' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'38f16ae9-cf92-4056-929f-51c4ef55804e', N'a3bb42ca-de7c-4c9f-8f58-d8175f96688c', N'rhcqsmanager@gmail.com', N'rhcqsmanager@gmail.com', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSxzT5167I50_3KwBagkh8DPQmmEEj0ec0ENA&s', N'$2a$12$aNTDoiJNbs7Wmpbj5nrxiedJeSyqkz1ta7REfGpWbOy/.YPxwx17G', N'0921659797', NULL, CAST(N'2024-09-29T05:54:32.677' AS DateTime), CAST(N'2024-09-29T05:54:32.677' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'7e9055c4-fa2e-45b4-b3e8-580870184283', N'789dd57d-0f75-40d1-8366-ef6ab582efc8', N'ngandolh@gmail.com', N'Ngân ', N'https://chungkhoantaichinh.vn/wp-content/uploads/2022/12/avatar-meo-cute-de-thuong-05.jpg', N'$2a$12$UUBeF1uXFz9fDVrTFnGFy.Ea7TV2MiRR9BR3DzIxrIXUOlUo3lqpO', N'0906697051', CAST(N'2002-01-01' AS Date), CAST(N'2024-09-26T14:30:00.000' AS DateTime), CAST(N'2024-09-26T14:30:00.000' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'b8c040b3-09a3-4975-a962-7440175b15aa', N'789dd57d-0f75-40d1-8366-ef6ab582efc8', N'mrtuan1456@', N'mrtuan1456@', N'https://hoanghamobile.com/tin-tuc/wp-content/uploads/2024/04/anh-meo-ngau-1.jpg', N'$2a$12$MXNuxZyaFg.ZjonAfHK3devdOkNJSi6lk1CEg1OgZDyc8jV40d3aW', N'0589759666', NULL, CAST(N'2024-10-02T16:32:18.907' AS DateTime), CAST(N'2024-10-02T16:32:18.907' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'08b10ff5-e37d-40bf-947c-80cbf78fa411', N'789dd57d-0f75-40d1-8366-ef6ab582efc8', N'test1@gmail.com', N'test1@gmail.com', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR_rtTqcFlN3O0TRGN_RkXL7pPqcIUBlPlUyQ&s', N'$2a$12$/ZJbb1v3DNQlJAWfxVnzfeooOPTgaciK1vCKlPS2h7RMvIFdV1dJO', N'0869337777', NULL, CAST(N'2024-10-03T07:29:08.160' AS DateTime), CAST(N'2024-10-03T07:29:08.160' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'de455c55-1d0a-43e3-9581-842a127b0d65', N'789dd57d-0f75-40d1-8366-ef6ab582efc8', N'tiennv@gmail.com', N'Tiến', N'https://mekoong.com/wp-content/uploads/2022/10/close-up-of-cat-wearing-sunglasses-while-sitting-royalty-free-image-1571755145-1024x683.jpg', N'$2a$12$UUBeF1uXFz9fDVrTFnGFy.Ea7TV2MiRR9BR3DzIxrIXUOlUo3lqpO', N'0385940273', NULL, CAST(N'2024-09-27T15:36:51.430' AS DateTime), CAST(N'2024-09-28T07:56:47.180' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'd127d3a6-6f2b-402f-bd60-90108623651f', N'789dd57d-0f75-40d1-8366-ef6ab582efc8', N'thien', N'Thiện', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTdph6y3dB-1RsMjk1fs5Eec4bHt2IFmRUHxw&s', N'$2a$12$3lnj3fZay46MeKa7nrdd3Ot4r0bJ/CHa7RnF7PIfCeop/f9sGEAni', N'0846002277', NULL, CAST(N'2024-09-29T05:16:24.890' AS DateTime), CAST(N'2024-10-02T11:36:06.830' AS DateTime), 0)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'84412c77-ecd0-4d08-ada8-90d4623cd5c0', N'789dd57d-0f75-40d1-8366-ef6ab582efc8', N'mrtuan1456', N'mrtuan1456', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQyfbE1OCi1qUfvlx_HM0mRCPyIUKguNiZE_w&s', N'$2a$12$7z81XBUP9QumAoXzRx8y6uyrLMvtH5xNNs8NWBdsejN6XoYfjryaS', N'0843020000', NULL, CAST(N'2024-10-02T15:39:32.203' AS DateTime), CAST(N'2024-10-02T15:39:32.203' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'45bced7f-0432-40dd-9686-91f8cc1c90dc', N'7af0d75e-1157-48b4-899d-3196deed5fad', N'design4rhcqs@gmail.com', N'design4rhcqs@gmail.com', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQb-ill7bz46Keq7bA7xpJsQrpAttqtgUQQhQ&s', N'$2a$12$nhXfB/DdiYO9uDUVA71KueHWDCA9ummGU6OcvU/ipkV9RYbvmZRHS', N'0704894555', NULL, CAST(N'2024-09-30T15:57:40.950' AS DateTime), CAST(N'2024-09-30T15:57:40.950' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'8caeb11a-1599-40c9-bdfc-a184acd7700d', N'789dd57d-0f75-40d1-8366-ef6ab582efc8', N'testDangKi', N'testDangKi', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTTacA-IBHeRlTou6YG_FmfLGAKcsol22P_7Q&s', N'$2a$12$SLZCNIwfGv1QgwCJtJt97.LyGN9ylVAz15l2waFu.cbP1DrVLVov6', N'0906993705', NULL, CAST(N'2024-10-02T16:28:07.200' AS DateTime), CAST(N'2024-10-02T16:28:07.200' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'd63a2a80-cdea-46df-8419-e5c70a7632ee', N'9959ce96-de26-40a7-b8a7-28a704062e89', N'sales1rhcqs@gmail.com', N'sales1rhcqs@gmail.com', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQDYCsPDKzMOClp7WmGj8fmonRjhE5I8u56Ng&s', N'$2a$12$tE5j8T5as5235dBQXynZpOBppleiOCdiqp4kZLAQdvS/P4IUANxN6', N'0931074111', NULL, CAST(N'2024-09-29T05:55:27.280' AS DateTime), CAST(N'2024-09-29T05:55:27.280' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'28247cd1-67ca-439d-bef5-fca9a9a777c5', N'7af0d75e-1157-48b4-899d-3196deed5fad', N'design3rhcqs@gmail.com', N'design3rhcqs@gmail.com', N'https://dogstar.vn/wp-content/uploads/2022/05/anh-meo-de-thuong-iu-qua-di.jpg', N'$2a$12$cDiICnJKZHnuHi3W56CU7eKuMeKVNKVC3n1Vv1QF6c6NHN7Q8ylMy', N'0764693535', NULL, CAST(N'2024-09-30T15:57:22.750' AS DateTime), CAST(N'2024-09-30T15:57:22.750' AS DateTime), 1)
INSERT [dbo].[Account] ([Id], [RoleId], [Email], [Username], [ImageUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'28247cd1-67ca-439d-bef5-fca9a9a777c6', N'7af0d75e-1157-48b4-899d-3196deed5fad', N'design4rhcqs@gmail.com', N'design4rhcqs@gmail.com', N'https://dogstar.vn/wp-content/uploads/2022/05/anh-meo-de-thuong-iu-qua-di.jpg', N'$2a$12$cDiICnJKZHnuHi3W56CU7eKuMeKVNKVC3n1Vv1QF6c6NHN7Q8ylMy', N'0902981306', NULL, CAST(N'2024-09-30T15:57:22.750' AS DateTime), CAST(N'2024-09-30T15:57:22.750' AS DateTime), 1)
GO
INSERT [dbo].[AssignTask] ([Id], [AccountId], [Name], [Status], [InsDate]) VALUES (N'47f6b73e-8ecf-4505-83c8-0bdcb6668bfd', N'28247cd1-67ca-439d-bef5-fca9a9a777c5', N'', N'Pending', CAST(N'2024-10-01T13:33:24.847' AS DateTime))
INSERT [dbo].[AssignTask] ([Id], [AccountId], [Name], [Status], [InsDate]) VALUES (N'643f3138-4fa1-433b-8bde-205291c54b3a', N'990773a2-1817-47f5-9116-301e97435c44', N'Huy', N'Pending', NULL)
INSERT [dbo].[AssignTask] ([Id], [AccountId], [Name], [Status], [InsDate]) VALUES (N'9b46d8cd-7729-4779-a656-3e96f275df0b', N'45bced7f-0432-40dd-9686-91f8cc1c90dc', N'', N'Pending', CAST(N'2024-10-01T13:33:24.860' AS DateTime))
INSERT [dbo].[AssignTask] ([Id], [AccountId], [Name], [Status], [InsDate]) VALUES (N'1cad86ac-8972-42c1-812b-50b03c9450f8', N'bf339e88-5303-45c4-a6f4-33a79681766c', N'Quang', N'Processing', NULL)
INSERT [dbo].[AssignTask] ([Id], [AccountId], [Name], [Status], [InsDate]) VALUES (N'fbdff783-6875-4cee-b801-7787abca561b', N'28247cd1-67ca-439d-bef5-fca9a9a777c5', N'Văn', N'Pending', NULL)
INSERT [dbo].[AssignTask] ([Id], [AccountId], [Name], [Status], [InsDate]) VALUES (N'7c42029a-f7a0-48e4-80fd-81b3c0afbf22', N'bf339e88-5303-45c4-a6f4-33a79681766c', N'', N'Pending', CAST(N'2024-10-01T13:33:24.620' AS DateTime))
INSERT [dbo].[AssignTask] ([Id], [AccountId], [Name], [Status], [InsDate]) VALUES (N'09ea2069-b74e-41de-a9a8-83135383478e', N'990773a2-1817-47f5-9116-301e97435c44', N'', N'Pending', CAST(N'2024-10-01T13:33:24.823' AS DateTime))
INSERT [dbo].[AssignTask] ([Id], [AccountId], [Name], [Status], [InsDate]) VALUES (N'44ca8188-5131-41ff-a8e7-8d5798d6b9fe', N'bf339e88-5303-45c4-a6f4-33a79681766c', N'', N'Pending', CAST(N'2024-10-01T13:23:30.510' AS DateTime))
INSERT [dbo].[AssignTask] ([Id], [AccountId], [Name], [Status], [InsDate]) VALUES (N'f89439b2-e07d-40fa-a1de-dbddde1f91dd', N'45bced7f-0432-40dd-9686-91f8cc1c90dc', N'Lộc', N'Pending', NULL)
GO
INSERT [dbo].[BatchPayment] ([Id], [ContractId], [Price], [PaymentDate], [PaymentPhase], [IntitialQuotationId], [Percents], [InsDate], [FinalQuotationId], [Description], [Unit]) VALUES (N'd165e833-2e68-45ad-a657-a222d01e205c', N'9d864292-9768-46d4-82f5-6b26cb1b9a3f', 685552500, CAST(N'2024-11-04T12:40:31.853' AS DateTime), CAST(N'2024-11-11T12:40:31.853' AS DateTime), N'bd31c4e5-e549-42ea-aec7-0f08446f089d', N'50', CAST(N'2024-11-04T12:40:31.853' AS DateTime), NULL, N'Đợt 1 thanh toán 50%', N'VNĐ')
INSERT [dbo].[BatchPayment] ([Id], [ContractId], [Price], [PaymentDate], [PaymentPhase], [IntitialQuotationId], [Percents], [InsDate], [FinalQuotationId], [Description], [Unit]) VALUES (N'9f29dc1f-c94d-4078-94ad-b3ebf48a6f8a', N'9d864292-9768-46d4-82f5-6b26cb1b9a3f', 685552500, CAST(N'2024-12-01T12:40:31.853' AS DateTime), CAST(N'2024-12-08T12:40:31.853' AS DateTime), N'bd31c4e5-e549-42ea-aec7-0f08446f089d', N'50', CAST(N'2024-11-04T12:40:31.853' AS DateTime), NULL, N'Đợt 2 thanh toán 50% nghiệm thu bản vẽ thiết kế', N'VNĐ')
GO
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'15b85094-21a2-46e7-95aa-0fba6890ae36', N'Thông Tầng lầu 4', 0, N'm2', CAST(N'2024-09-27T15:35:37.630' AS DateTime), CAST(N'2024-09-27T15:35:37.630' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'40aa0395-a4ef-423c-91f2-11d2889dc306', N'Thông Tầng lầu 1', 0, N'm2', CAST(N'2024-09-27T15:34:23.203' AS DateTime), CAST(N'2024-09-27T15:34:23.203' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'75922602-9153-4cc3-a7dc-225c9bc30a5e', N'Móng', 0, N'm2', CAST(N'2024-09-27T21:37:09.000' AS DateTime), CAST(N'2024-09-27T21:37:09.000' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'eab92d07-c140-4a5f-a22d-23cee8a1540e', N'Thông Tầng lầu 5', 0, N'm2', CAST(N'2024-09-27T15:35:43.087' AS DateTime), CAST(N'2024-09-27T15:35:43.087' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'be6c6db7-cea1-4275-9b18-2fbcfe9b2353', N'Trệt', 1, N'm2', CAST(N'2024-09-27T21:37:09.000' AS DateTime), CAST(N'2024-09-27T21:37:09.000' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'5a000367-77c4-4c16-b533-30cd8d0d6017', N'Thông Tầng lầu 2', 0, N'm2', CAST(N'2024-09-27T15:35:25.247' AS DateTime), CAST(N'2024-09-27T15:35:25.247' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'd8f3d72b-042f-4108-b9ea-3f93666921d2', N'Mái phụ', 0, N'm2', CAST(N'2024-09-27T22:11:44.510' AS DateTime), CAST(N'2024-09-27T22:11:44.510' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'38675854-19d3-4fdb-90dd-657ad3683ae1', N'Tầng lửng', 1, N'm2', CAST(N'2024-10-02T14:44:52.737' AS DateTime), CAST(N'2024-10-02T14:44:52.737' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'13cd15d8-d1fb-48fb-8fbd-7523bc7fd04f', N'Phòng kỹ thuật thang máy', 0.5, N'm2', CAST(N'2024-09-27T15:43:05.070' AS DateTime), CAST(N'2024-09-27T15:43:05.070' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'4320ec5e-a71c-4161-95ae-857bd80763e5', N'Lầu 6', 1, N'm2', CAST(N'2024-09-27T15:37:13.277' AS DateTime), CAST(N'2024-09-27T15:37:13.277' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'bd101af5-ac48-43ba-a474-957a20a933bd', N'Mái che', 0, N'm2', CAST(N'2024-09-27T21:37:09.000' AS DateTime), CAST(N'2024-09-27T21:37:09.000' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'22fc6201-3f75-4f4f-8a51-9bf25b2b13c3', N'Lầu 2', 1, N'm2', CAST(N'2024-09-27T15:36:51.893' AS DateTime), CAST(N'2024-09-27T15:36:51.893' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'eba29420-a8db-455c-86b0-b325a1da4e1e', N'Lầu 1', 1, N'm2', CAST(N'2024-09-27T15:36:43.490' AS DateTime), CAST(N'2024-09-27T15:36:43.490' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'22b90619-892f-4aaf-9f78-b6467faee47b', N'Lầu 4', 1, N'm2', CAST(N'2024-09-27T15:37:02.927' AS DateTime), CAST(N'2024-09-27T15:37:02.927' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'903683df-19a9-4737-a8ac-c3dc0aa80949', N'Lầu 3', 1, N'm2', CAST(N'2024-09-27T15:36:56.853' AS DateTime), CAST(N'2024-09-27T15:36:56.853' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'34002a29-1d97-4e8d-8acd-c4a6c8e2df2b', N'Hố PIT', 3, N'm2', CAST(N'2024-10-02T14:45:08.397' AS DateTime), CAST(N'2024-10-02T14:45:08.397' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'f68ce6c6-6543-463f-b9d4-c5c967232988', N'Lầu 5', 1, N'm2', CAST(N'2024-09-27T15:37:08.653' AS DateTime), CAST(N'2024-09-27T15:37:08.653' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'75c3e21a-0abd-4699-abbe-c8371ed4d49a', N'Sân', 0.6, N'm2', CAST(N'2024-09-27T21:37:09.000' AS DateTime), CAST(N'2024-09-27T21:37:09.000' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'9696145f-bebf-4120-8354-c8aef681b351', N'Thông tầng lửng', 0, N'm2', CAST(N'2024-09-27T15:38:00.537' AS DateTime), CAST(N'2024-09-27T15:38:00.537' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'a6ce35ee-d19c-40ac-8044-cbecdb54f8d9', N'Hầm', 0, N'm2', CAST(N'2024-09-27T15:41:49.113' AS DateTime), CAST(N'2024-09-27T15:41:49.113' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'98ba07e3-d76c-42b7-8c87-cfd7b11a7f4f', N'Sân thượng có mái che', 1, N'm2', CAST(N'2024-09-27T21:37:09.000' AS DateTime), CAST(N'2024-09-27T21:37:09.000' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'6aa8e2fc-370e-4e6c-8269-d22d2c060e65', N'Thông Tầng lầu 6', 0, N'm2', CAST(N'2024-09-27T15:35:48.477' AS DateTime), CAST(N'2024-09-27T15:35:48.477' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'15030799-9d27-4270-b015-d9058d494e03', N'Sân thượng không có mái che', 0.5, N'm2', CAST(N'2024-09-27T21:37:09.000' AS DateTime), CAST(N'2024-09-27T21:37:09.000' AS DateTime), N'ROUGH')
INSERT [dbo].[ConstructionItems] ([Id], [Name], [Coefficient], [Unit], [InsDate], [UpsDate], [Type]) VALUES (N'f996f171-f8c6-4022-9152-fcccc486341e', N'Thông Tầng lầu 3', 0, N'm2', CAST(N'2024-09-27T15:35:31.800' AS DateTime), CAST(N'2024-09-27T15:35:31.800' AS DateTime), N'ROUGH')
GO
INSERT [dbo].[Contract] ([Id], [ProjectId], [Name], [CustomerName], [ContractCode], [StartDate], [EndDate], [ValidityPeriod], [TaxCode], [Area], [UnitPrice], [ContractValue], [UrlFile], [Note], [Deflag], [RoughPackagePrice], [FinishedPackagePrice], [Status]) VALUES (N'9d864292-9768-46d4-82f5-6b26cb1b9a3f', N'b81935a8-4482-43f5-ad68-558abde58d58', N'Hợp đồng tư vấn và thiết kế kiến trúc', N'Đồ', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'Draft')
GO
INSERT [dbo].[Customer] ([Id], [Email], [Username], [ImgUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'2fcb2c45-4dcc-4f3a-8fe4-1d2ee48fa931', N'ngandolh@gmail.com', N'Đồ', NULL, NULL, N'0901342922 ', CAST(N'2002-01-01T00:00:00.000' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[Customer] ([Id], [Email], [Username], [ImgUrl], [PasswordHash], [PhoneNumber], [DateOfBirth], [InsDate], [UpsDate], [Deflag]) VALUES (N'1e6fa320-6945-4b76-8426-ad6dfdb1ad74', N'nganttk002@gmail.com', N'Trần Ngân', NULL, NULL, N'0906697051 ', CAST(N'2002-01-01T00:00:00.000' AS DateTime), NULL, NULL, 1)
GO
INSERT [dbo].[DesignTemplate] ([Id], [Name], [Description], [NumberOfFloor], [NumberOfBed], [NumberOfFront], [ImgUrl], [InsDate]) VALUES (N'7af799a9-02fa-4cd0-b954-86b102840e60', N'Mẫu 1', N'Mẫu nhà ở nông thôn', 1, 2, NULL, N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632275/doowmllpuglxti4lnipq.png', NULL)
INSERT [dbo].[DesignTemplate] ([Id], [Name], [Description], [NumberOfFloor], [NumberOfBed], [NumberOfFront], [ImgUrl], [InsDate]) VALUES (N'335252c2-f710-430f-ab14-a1412ee29237', N'Mẫu 3', N'Mẫu 3', 1, 2, NULL, N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632424/mxhtfrqgja9nwjhwhusl.png', NULL)
INSERT [dbo].[DesignTemplate] ([Id], [Name], [Description], [NumberOfFloor], [NumberOfBed], [NumberOfFront], [ImgUrl], [InsDate]) VALUES (N'ec6d9970-ea66-455a-8815-dd69d72ff97f', N'Mẫu 2', N'Mẫu 2', 1, 2, NULL, N'https://res.cloudinary.com/de7pulfdj/image/upload/v1727632470/ozithihga2joj5ofjc79.png', NULL)
GO
INSERT [dbo].[FinalQuotation] ([Id], [ProjectId], [PromotionId], [TotalPrice], [Note], [Version], [InsDate], [UpsDate], [Status], [Deflag], [QuotationUtilitiesId], [AccountId], [ReasonReject]) VALUES (N'04cb9c6f-cf0c-4fa7-9565-4c3b4166c6f9', N'fa91e187-b0c8-46c3-b6d5-747d32bd3540', NULL, 1123500000, NULL, 1, CAST(N'2024-09-29T10:00:00.000' AS DateTime), CAST(N'2024-09-29T10:00:00.000' AS DateTime), N'Proccessing', 1, NULL, N'd63a2a80-cdea-46df-8419-e5c70a7632ee', NULL)
INSERT [dbo].[FinalQuotation] ([Id], [ProjectId], [PromotionId], [TotalPrice], [Note], [Version], [InsDate], [UpsDate], [Status], [Deflag], [QuotationUtilitiesId], [AccountId], [ReasonReject]) VALUES (N'3bc03044-e974-4f03-921a-cf07bc00eec3', N'b81935a8-4482-43f5-ad68-558abde58d58', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'd63a2a80-cdea-46df-8419-e5c70a7632ee', NULL)
GO
INSERT [dbo].[HouseDesignDrawing] ([Id], [ProjectId], [Name], [Step], [Status], [Type], [IsCompany], [InsDate], [AssignTaskId]) VALUES (N'4135dfce-55e0-4370-b453-2d438e96dbbb', N'fa91e187-b0c8-46c3-b6d5-747d32bd3540', N'Phối cảnh', 1, N'Proccessing', N'PHOICANH', 0, CAST(N'2024-09-30T12:34:56.000' AS DateTime), N'1cad86ac-8972-42c1-812b-50b03c9450f8')
INSERT [dbo].[HouseDesignDrawing] ([Id], [ProjectId], [Name], [Step], [Status], [Type], [IsCompany], [InsDate], [AssignTaskId]) VALUES (N'23dba73e-5fa5-479e-b143-5e1c4150295f', N'c64e1637-635b-4d03-a68e-ed5f81d3fd57', N'Điện & nước', 4, N'Pending', N'DIENNUOC', 0, CAST(N'2024-10-01T10:12:05.523' AS DateTime), N'9b46d8cd-7729-4779-a656-3e96f275df0b')
INSERT [dbo].[HouseDesignDrawing] ([Id], [ProjectId], [Name], [Step], [Status], [Type], [IsCompany], [InsDate], [AssignTaskId]) VALUES (N'b528f8bc-3992-499b-af7d-67510d730087', N'c64e1637-635b-4d03-a68e-ed5f81d3fd57', N'Phối cảnh', 1, N'Proccessing', N'PHOICANH', 0, CAST(N'2024-10-01T10:11:50.990' AS DateTime), N'7c42029a-f7a0-48e4-80fd-81b3c0afbf22')
INSERT [dbo].[HouseDesignDrawing] ([Id], [ProjectId], [Name], [Step], [Status], [Type], [IsCompany], [InsDate], [AssignTaskId]) VALUES (N'8d34c630-8a6c-4229-b209-81f7b40037cd', N'c64e1637-635b-4d03-a68e-ed5f81d3fd57', N'Kết cấu', 3, N'Pending', N'KETCAU', 0, CAST(N'2024-10-01T10:12:02.637' AS DateTime), N'47f6b73e-8ecf-4505-83c8-0bdcb6668bfd')
INSERT [dbo].[HouseDesignDrawing] ([Id], [ProjectId], [Name], [Step], [Status], [Type], [IsCompany], [InsDate], [AssignTaskId]) VALUES (N'd3998f6d-ae9b-4467-855b-937c7dcac45b', N'fa91e187-b0c8-46c3-b6d5-747d32bd3540', N'Kết cấu', 3, N'Pending', N'KETCAU', 0, CAST(N'2024-09-30T12:34:56.000' AS DateTime), N'fbdff783-6875-4cee-b801-7787abca561b')
INSERT [dbo].[HouseDesignDrawing] ([Id], [ProjectId], [Name], [Step], [Status], [Type], [IsCompany], [InsDate], [AssignTaskId]) VALUES (N'9d6b7184-725d-4879-b94c-942f3320d6ac', N'fa91e187-b0c8-46c3-b6d5-747d32bd3540', N'Điện & nước', 4, N'Pending', N'DIENNUOC', 0, CAST(N'2024-09-30T12:34:56.000' AS DateTime), N'f89439b2-e07d-40fa-a1de-dbddde1f91dd')
INSERT [dbo].[HouseDesignDrawing] ([Id], [ProjectId], [Name], [Step], [Status], [Type], [IsCompany], [InsDate], [AssignTaskId]) VALUES (N'a09ed901-92c9-4c5e-950b-d9c5a7a99f4c', N'c64e1637-635b-4d03-a68e-ed5f81d3fd57', N'Kiến trúc', 2, N'Pending', N'KIENTRUC', 0, CAST(N'2024-10-01T10:12:01.643' AS DateTime), N'09ea2069-b74e-41de-a9a8-83135383478e')
INSERT [dbo].[HouseDesignDrawing] ([Id], [ProjectId], [Name], [Step], [Status], [Type], [IsCompany], [InsDate], [AssignTaskId]) VALUES (N'eab8d4ff-c6d4-452a-ab89-dedc8d3f5153', N'fa91e187-b0c8-46c3-b6d5-747d32bd3540', N'Kiến trúc', 2, N'Pending', N'KIENTRUC', 0, CAST(N'2024-09-30T12:34:56.000' AS DateTime), N'643f3138-4fa1-433b-8bde-205291c54b3a')
GO
INSERT [dbo].[HouseDesignVersion] ([Id], [Name], [Version], [Status], [InsDate], [HouseDesignDrawingId], [Note], [FileUrl], [UpsDate], [RelatedDrawingId], [PreviousDrawingId], [Reason], [Deflag]) VALUES (N'3905068b-8e24-4fc1-8663-a6697903de08', N'Phối cảnh', 1, N'Finished', CAST(N'2024-09-30T12:34:56.000' AS DateTime), N'4135dfce-55e0-4370-b453-2d438e96dbbb', N'Sửa cửa chính và cửa phụ 2', N'https://i1-kinhdoanh.vnecdn.net/2024/10/01/dien-gio-ngoai-jpeg-3688-16383-4247-2437-1727785076.jpg?w=1020&h=0&q=100&dpr=1&fit=crop&s=W7nRN5_GjYwxdfYkdVYPiw', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[HouseDesignVersion] ([Id], [Name], [Version], [Status], [InsDate], [HouseDesignDrawingId], [Note], [FileUrl], [UpsDate], [RelatedDrawingId], [PreviousDrawingId], [Reason], [Deflag]) VALUES (N'2e7a32d9-5ffb-4cca-b866-f03f67da256d', N'Phối cảnh', 2, N'Approved', CAST(N'2024-09-30T12:34:56.000' AS DateTime), N'4135dfce-55e0-4370-b453-2d438e96dbbb', NULL, N'http://res.cloudinary.com/de7pulfdj/image/upload/v1728019936/HouseDesignDrawing/Phối cảnh_210/4/2024 12:32:10 PM.png', CAST(N'2024-10-04T12:32:17.987' AS DateTime), NULL, NULL, N'Sửa cửa sổ + cửa chính', 1)
GO
INSERT [dbo].[InitialQuotation] ([Id], [AccountId], [ProjectId], [PromotionId], [Area], [TimeProcessing], [TimeRough], [TimeOthers], [OthersAgreement], [InsDate], [Status], [Version], [IsTemplate], [Deflag], [Note], [TotalRough], [TotalUtilities], [Unit], [ReasonReject]) VALUES (N'bd31c4e5-e549-42ea-aec7-0f08446f089d', N'd63a2a80-cdea-46df-8419-e5c70a7632ee', N'b81935a8-4482-43f5-ad68-558abde58d58', N'41b828b2-7b06-4389-9003-daac937158dd', 125, NULL, NULL, NULL, NULL, CAST(N'2024-10-04T12:40:31.853' AS DateTime), N'Rejected', 1, 0, 1, NULL, 1500000000, 20000000, N'VNĐ', N'Sửa phần tiện ích')
INSERT [dbo].[InitialQuotation] ([Id], [AccountId], [ProjectId], [PromotionId], [Area], [TimeProcessing], [TimeRough], [TimeOthers], [OthersAgreement], [InsDate], [Status], [Version], [IsTemplate], [Deflag], [Note], [TotalRough], [TotalUtilities], [Unit], [ReasonReject]) VALUES (N'27303a9e-c1ff-4060-a145-bad032564a0a', N'7e9055c4-fa2e-45b4-b3e8-580870184283', N'eac91a0b-baae-4e00-a21f-f4d05153f447', NULL, 125, NULL, NULL, NULL, NULL, CAST(N'2024-10-04T00:17:13.637' AS DateTime), N'Pending', 1, 0, 1, NULL, 1500000000, 10000000, N'VNĐ', NULL)
INSERT [dbo].[InitialQuotation] ([Id], [AccountId], [ProjectId], [PromotionId], [Area], [TimeProcessing], [TimeRough], [TimeOthers], [OthersAgreement], [InsDate], [Status], [Version], [IsTemplate], [Deflag], [Note], [TotalRough], [TotalUtilities], [Unit], [ReasonReject]) VALUES (N'f6f03971-5a01-47ce-869a-c3f63d70fbb9', N'7e9055c4-fa2e-45b4-b3e8-580870184283', N'b841c593-1d05-4492-9ba2-d485f6d67260', N'41b828b2-7b06-4389-9003-daac937158dd', 125, 200, 160, 40, N'- Giá trị Hợp đồng chưa bao gồm thuế VAT
- Chủ đầu tư cam kết không thanh lý hợp đồng trước thời hạn. Trong mọi trường hợp thanh lý hợp 
đồng trước thời hạn, chủ đầu tư sẽ không được giảm trừ giá trị Hợp đồng ', CAST(N'2024-09-28T14:45:30.123' AS DateTime), N'Proccessing', 1, 0, 1, NULL, 1500000000, 5000000, N'VNĐ', NULL)
INSERT [dbo].[InitialQuotation] ([Id], [AccountId], [ProjectId], [PromotionId], [Area], [TimeProcessing], [TimeRough], [TimeOthers], [OthersAgreement], [InsDate], [Status], [Version], [IsTemplate], [Deflag], [Note], [TotalRough], [TotalUtilities], [Unit], [ReasonReject]) VALUES (N'4fe5b3e7-ac15-498d-aabe-c6f9f8987d5f', N'd63a2a80-cdea-46df-8419-e5c70a7632ee', N'b81935a8-4482-43f5-ad68-558abde58d58', NULL, 125, 165, 115, 50, N'Khách hàng phải thanh toán trong vòng 7 ngày kể từ khi hợp động được kí', CAST(N'2024-10-06T01:41:02.557' AS DateTime), N'Under Review', 2, 0, 1, NULL, 0, 0, N'VNĐ', NULL)
INSERT [dbo].[InitialQuotation] ([Id], [AccountId], [ProjectId], [PromotionId], [Area], [TimeProcessing], [TimeRough], [TimeOthers], [OthersAgreement], [InsDate], [Status], [Version], [IsTemplate], [Deflag], [Note], [TotalRough], [TotalUtilities], [Unit], [ReasonReject]) VALUES (N'69aded64-dc90-4b35-8059-f42dff6a10c6', N'd63a2a80-cdea-46df-8419-e5c70a7632ee', N'799b9201-d234-47b9-a14f-7574cc84ef74', NULL, 125, NULL, NULL, NULL, NULL, CAST(N'2024-10-04T01:24:44.717' AS DateTime), N'Pending', 1, 0, 1, NULL, 1500000000, NULL, N'VNĐ', NULL)
GO
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'8946f6e3-32d3-4621-8e43-05db32ffa305', N'Sân', N'75c3e21a-0abd-4699-abbe-c8371ed4d49a', NULL, 101.4, NULL, N'đ', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), N'f6f03971-5a01-47ce-869a-c3f63d70fbb9')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'5ca9cb47-d260-4486-9b82-4df29af1bfbc', N'Lầu 2', N'22fc6201-3f75-4f4f-8a51-9bf25b2b13c3', NULL, 132.27, NULL, N'đ', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), N'f6f03971-5a01-47ce-869a-c3f63d70fbb9')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'83ad2005-cf26-41a5-9756-586f0f2e256b', N'', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', N'06ff2d3d-2f14-4acc-ba2e-0c4ee659ca81', 49.5, 66330000, N'đ', CAST(N'2024-10-04T00:17:13.733' AS DateTime), CAST(N'2024-10-04T00:17:13.733' AS DateTime), N'27303a9e-c1ff-4060-a145-bad032564a0a')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'5453c25f-b2a4-4f93-9071-5891f3fe870e', N'', N'bd101af5-ac48-43ba-a474-957a20a933bd', N'7e442652-eefc-43b7-918b-a264a10e679d', 49.5, 66330000, N'đ', CAST(N'2024-10-04T00:17:13.770' AS DateTime), CAST(N'2024-10-04T00:17:13.770' AS DateTime), N'27303a9e-c1ff-4060-a145-bad032564a0a')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'feccb840-5df4-4b31-9a51-83fbfc698081', N'Lỗ trống lầu 1', N'40aa0395-a4ef-423c-91f2-11d2889dc306', N'b88899fd-aa23-4a66-ad1e-9a5a4dd03bca', 11.325, NULL, N'đ', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), N'f6f03971-5a01-47ce-869a-c3f63d70fbb9')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'09fb8bac-3b2d-4a14-8a10-90711d84f13e', N'Móng', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', NULL, 35.1, NULL, N'đ', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), N'f6f03971-5a01-47ce-869a-c3f63d70fbb9')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'5b792ed1-a726-44ce-9820-9629d459ed8b', N'', N'bd101af5-ac48-43ba-a474-957a20a933bd', N'7e442652-eefc-43b7-918b-a264a10e679d', 49.5, 66330000, N'đ', CAST(N'2024-10-04T12:40:31.910' AS DateTime), CAST(N'2024-10-04T12:40:31.910' AS DateTime), N'bd31c4e5-e549-42ea-aec7-0f08446f089d')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'1bf45d93-ddb9-40ff-b1a4-b01180bc5955', N'', N'be6c6db7-cea1-4275-9b18-2fbcfe9b2353', N'06ff2d3d-2f14-4acc-ba2e-0c4ee659ca81', 99, 331650000, N'đ', CAST(N'2024-10-04T12:40:31.910' AS DateTime), CAST(N'2024-10-04T12:40:31.910' AS DateTime), N'bd31c4e5-e549-42ea-aec7-0f08446f089d')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'3c414d7a-58d0-488c-9e48-ccef30093ea1', N'', N'75922602-9153-4cc3-a7dc-225c9bc30a5e', N'06ff2d3d-2f14-4acc-ba2e-0c4ee659ca81', 49.5, 66330000, N'đ', CAST(N'2024-10-04T12:40:31.893' AS DateTime), CAST(N'2024-10-04T12:40:31.893' AS DateTime), N'bd31c4e5-e549-42ea-aec7-0f08446f089d')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'9f25f16d-8333-4391-b272-cd897fb0a92a', N'Mái BTCT', N'bd101af5-ac48-43ba-a474-957a20a933bd', N'7e442652-eefc-43b7-918b-a264a10e679d', 83, NULL, N'đ', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), N'f6f03971-5a01-47ce-869a-c3f63d70fbb9')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'eb249d42-2ba7-4953-8c26-dc21de5400de', N'', N'be6c6db7-cea1-4275-9b18-2fbcfe9b2353', N'06ff2d3d-2f14-4acc-ba2e-0c4ee659ca81', 99, 331650000, N'đ', CAST(N'2024-10-04T00:17:13.770' AS DateTime), CAST(N'2024-10-04T00:17:13.770' AS DateTime), N'27303a9e-c1ff-4060-a145-bad032564a0a')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'9f93be33-32a5-4925-afbd-f0b864f9e63d', N'Lầu 1', N'eba29420-a8db-455c-86b0-b325a1da4e1e', NULL, 110.05, NULL, N'đ', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), N'f6f03971-5a01-47ce-869a-c3f63d70fbb9')
INSERT [dbo].[InitialQuotationItem] ([Id], [Name], [ConstructionItemId], [SubConstructionId], [Area], [Price], [UnitPrice], [InsDate], [UpsDate], [InitialQuotationId]) VALUES (N'5bb3eeab-8c35-43c2-93ae-f5bb9e72260a', N' Lầu trệt', N'be6c6db7-cea1-4275-9b18-2fbcfe9b2353', NULL, 117, NULL, N'đ', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), N'f6f03971-5a01-47ce-869a-c3f63d70fbb9')
GO
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'0d207781-eebb-4a03-96ce-005434963f44', N'Tổ chức công trường', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'a9850299-ea54-4f53-af09-1d4eb528fcd4', N'Vệ sinh cơ bản trước khi bàn giao', 150000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'fec0ca84-5523-4531-b20b-1d70375f3699', N'Xây tường bao quanh công trình', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'f8df9b3d-47aa-4579-8f90-1f0de2112f85', N'Dọn dẹp vệ sinh công trình hàng ngày', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'58543f35-e6f1-4851-b012-2137432a71c2', N'NC ốp len gạch trang trí theo bản vẽ(nếu có)', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'4b82dcfc-6ded-4212-a363-3dd11b74ff6c', N'Bảo vệ công trình', 160000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'63a7953f-a220-4938-8c27-40bddc408617', N'Vệ sinh mặt bằng', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'36c8ea55-6bef-4822-99dc-474fa2242d84', N'Chống thấm sàn vệ sinh, sân thượng', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'6dd8f613-43bc-4ba2-859f-4a6b69440ec1', N'NC lắp đạt bồn nước, máy bơm, và WC', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'37804252-49d1-423f-98fd-4ca4a19c58f0', N'NC sơn nước toàn bộ ngôi nhà', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'de396246-83f6-48f5-b049-76b16134d31b', N'Cốt thép,cofa và đổ bê tông gạch thẻ(cầu thang)', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'4cd9a933-87d6-49e8-bf10-774c01afe235', N'Thi công cọc tiếp địa', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'5a21c01b-954e-40da-93d2-7e1d69013c9e', N'Lợp ngói mái Tole nếu có', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'1413ec55-981b-406e-8e78-83c742c39c9e', N'NC lắp đạt hệ thống điện', 400000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'e267a87e-383f-457d-874e-9acbd42fc1ad', N'Tô các vách', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'66719137-e8b1-4198-9a9e-a40101c77aea', N'Lắp đặt điện âm, ông nước âm', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'98ca966c-d741-4aa3-a982-a420fb08d670', N'Cán nền các tầng lầu', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'17b73a9b-33ee-416b-8307-b46aa4417f90', N'Thi công dây TE theo bản vẽ', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'89587e2c-5a40-4d45-95c7-ba4146e5e4d3', N'Xây tô hoàn thiện mặt tiền', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'd12798bf-c423-415c-b367-bb826793f57f', N'Cốt thép,cofa và đổ bê tông(đáy, nắp hầm, hố ga)', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'c41ad3f6-0ce3-437f-a313-d6268ce4c15f', N'Đào đất móng', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'fb22bbac-f43d-4aee-8fe4-e522aca80398', N'Cốt thép,cofa và đổ bê tông(vách hầm nếu có)', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'5ecffef5-6441-437c-903b-ed469e4a819a', N'Đập, cắt đầu cọc BTCT', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'47b540b2-8d41-45a9-9685-f62e67ddbe0d', N'NC lát gạch, ốp len chân tường', 350000, 1, NULL, NULL, 1)
INSERT [dbo].[Labor] ([Id], [Name], [Price], [Quantity], [InsDate], [UpsDate], [Deflag]) VALUES (N'bc3b0032-1c54-4496-aa18-f7fcb9f9e147', N'Cốt thép,cofa và đổ bê tông(sàn các lầu, thượng)', 350000, 1, NULL, NULL, 1)
GO
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable]) VALUES (N'941ef5fd-2dcc-4703-aab1-0dd4b71c0606', N'1ddabadf-7a17-49c0-9e4c-7fb3ffcffaa3', N'f053cee3-1e43-4fb9-bd32-9070d6500c86', N'Máy LASER', 50, 1950000, N'máy', N'5 tia', N'máy', NULL, NULL, NULL, NULL, N'vnđ', 1)
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable]) VALUES (N'f9c9a292-3023-498a-a128-1b241176cb18', N'e962f51c-b52a-4ac5-b94c-af92a7f0128c', N'62870bcd-3c5c-41db-bfb2-d0abe3092009', N'Dây điện CADIVI', 500, 444440, N'm', N'100m', N'cuộn', NULL, NULL, NULL, NULL, N'vnđ', 1)
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable]) VALUES (N'8961731d-9389-4b9a-a86e-23ad1f8211c5', N'ddf513eb-c9e8-4430-ad57-f9cf7f9178b2', N'fe9b1b34-3fd8-4b71-b748-8abaa17d65fe', N'Xi măng INSEE', 500, 90000, N'bao', N'Đa dụng', N'hạt', NULL, NULL, NULL, NULL, N'vnđ', 1)
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable]) VALUES (N'423cff9d-bdac-4b62-ade6-35f4250ba836', N'0c9f2fb1-845b-4ce0-b73c-6a2228955248', N'776c5f97-f429-4939-b306-acda1b734aa5', N'Ống thoát nước Bình Minh', 500, 41700, N'm', N'90 x 1,7mm', N'ống', NULL, NULL, NULL, NULL, N'vnđ', 1)
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable]) VALUES (N'b8b581ea-3353-4786-b007-838bfd41760a', N'7d7a584c-1ad5-4a91-898b-0166748b0abb', N'aa61efd9-482d-4ae8-86c5-8f096c2cd064', N'Thép Việt-Nhật', 500, 15000, N'kg', N'SD295', N'cuộn', NULL, NULL, NULL, NULL, N'vnđ', 1)
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable]) VALUES (N'b56d86a3-dbe9-4d5c-a661-83fed0df4e44', N'49cdf959-7ba7-43fa-bc63-53b312a3dcd0', N'297efa40-de4c-47f2-9ba9-1364dcba6788', N'Ống dây điện', 500, 18000, N'cây', N'D16, dày 1.15mm', N'cây', NULL, NULL, NULL, NULL, N'vnđ', 1)
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable]) VALUES (N'2846551d-45d2-4b7f-90be-8bd9acc4e5ce', N'b2cc8909-cc6e-45c0-b6d3-9debf98cc58b', N'ae6be775-baad-485d-8c99-3f2143ca5163', N'Đá Bình Điền 1/2', 500, 260000, N'm3', N'10 mm x 20mm', N'viên', NULL, NULL, NULL, NULL, N'vnđ', 1)
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable]) VALUES (N'82e7baa6-aec9-4a77-a1ee-9000bc7cbd90', N'787502b1-30b2-49a1-b92f-75da8bbeaa3d', N'f053cee3-1e43-4fb9-bd32-9070d6500c86', N'Dàn giáo', 50, 620000, N'bộ', N'2 khung ,2 giằng chéo', N'bộ', NULL, NULL, NULL, NULL, N'vnđ', 1)
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable]) VALUES (N'cd330088-f401-4ce8-9b9c-b60b9ebb9ad5', N'4674adf8-75fa-47e1-b27a-6ca23b102d88', N'fb2f4d82-f30e-4963-967a-156f763deb94', N'Ngói', 500, 174000, N'viên', N'40 x40', N'viên', NULL, NULL, NULL, NULL, N'vnđ', 1)
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable]) VALUES (N'cfce6501-9715-4041-b421-d5641ff64e83', N'20f32cbb-9415-4a0e-9404-2f21c770bda4', N'f88ab54a-2304-4d48-b324-3a99b5f1cc77', N'Chống thấm BestLatex ', 500, 59000, N'can', N'R114', N'lỏng', NULL, NULL, NULL, NULL, N'vnđ', 1)
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable]) VALUES (N'8b6c4e26-090b-44f6-bda7-e1701d9f93a1', N'58135989-3c74-4485-a1ab-5ddab11bb6f6', N'74f27d12-b066-4846-8867-8a8a764670fa', N'Cát vàng', 500, 20000, N'bao', N'Xây dựng', N'bịch', NULL, NULL, NULL, NULL, N'vnđ', 1)
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable]) VALUES (N'7ed72eb6-76bf-44b1-a6b6-e5ac7e234b86', N'1b2d1a24-a9d0-44d0-969c-5a6732a519fa', N'7facd531-7971-49b6-bd00-fdaacc147cc3', N'Cáp mạng LS-DVH ', 500, 9220, N'm', N'CAT5E UTP', N'cuộn', NULL, NULL, NULL, NULL, N'vnđ', 1)
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable]) VALUES (N'8419526f-43f9-4fc7-a8a1-f91c516655a7', N'06eb8dc0-1ff9-410f-9d05-7f18d4a8f504', N'38b980c3-c1b3-4d5e-abc5-f82a8c2ba31f', N'Gạch Tuynel 4 lỗ', 500, 2000, N'viên', N'7.5 x 7.5 x 17', N'cục', NULL, NULL, NULL, NULL, N'vnđ', 1)
INSERT [dbo].[Material] ([Id], [SupplierId], [MaterialTypeId], [Name], [InventoryQuantity], [Price], [Unit], [Size], [Shape], [ImgUrl], [Description], [InsDate], [UpsDate], [UnitPrice], [IsAvailable]) VALUES (N'eee90754-a582-4539-a7f5-fec3c18f7578', N'58135989-3c74-4485-a1ab-5ddab11bb6f6', N'c3fadd14-d052-4ce0-a599-e187f53356b9', N'Bê tông thương phẩm đá đen', 500, 1370000, N'm3', N'350 R28', N'lỏng', NULL, NULL, NULL, NULL, N'vnđ', 1)
GO
INSERT [dbo].[MaterialSection] ([Id], [MaterialId], [Name], [InsDate]) VALUES (N'082dfe19-5250-43dd-ae01-02a34d37fe13', N'f9c9a292-3023-498a-a128-1b241176cb18', N'Dây điện', NULL)
INSERT [dbo].[MaterialSection] ([Id], [MaterialId], [Name], [InsDate]) VALUES (N'f64aa8ed-f0f4-4b9f-856f-05f1c40297d8', N'82e7baa6-aec9-4a77-a1ee-9000bc7cbd90', N'Dàn giáo', NULL)
INSERT [dbo].[MaterialSection] ([Id], [MaterialId], [Name], [InsDate]) VALUES (N'c7dbe224-7d2f-4430-b0e9-0e20f50bbcf0', N'8419526f-43f9-4fc7-a8a1-f91c516655a7', N'Gạch', NULL)
INSERT [dbo].[MaterialSection] ([Id], [MaterialId], [Name], [InsDate]) VALUES (N'30b17727-c177-4865-9a75-11ac9b493023', N'7ed72eb6-76bf-44b1-a6b6-e5ac7e234b86', N'Cáp mạng', NULL)
INSERT [dbo].[MaterialSection] ([Id], [MaterialId], [Name], [InsDate]) VALUES (N'eb96ae7b-7d96-4aff-850c-16d618e37ee4', N'cd330088-f401-4ce8-9b9c-b60b9ebb9ad5', N'Ngói', NULL)
INSERT [dbo].[MaterialSection] ([Id], [MaterialId], [Name], [InsDate]) VALUES (N'baf50ad2-8f54-48b7-a240-2a7db6f7a33d', N'8961731d-9389-4b9a-a86e-23ad1f8211c5', N'Xi măng', NULL)
INSERT [dbo].[MaterialSection] ([Id], [MaterialId], [Name], [InsDate]) VALUES (N'5f4009fd-541f-4c3c-b37e-3c151e801cc2', N'eee90754-a582-4539-a7f5-fec3c18f7578', N'Bê tông', NULL)
INSERT [dbo].[MaterialSection] ([Id], [MaterialId], [Name], [InsDate]) VALUES (N'eebabf3a-1cd5-4736-9ee9-3c3e5c3b43ef', N'cfce6501-9715-4041-b421-d5641ff64e83', N'Chống thấm', NULL)
INSERT [dbo].[MaterialSection] ([Id], [MaterialId], [Name], [InsDate]) VALUES (N'c8e9092b-31c7-402a-b5f0-561e87a59c0a', N'8b6c4e26-090b-44f6-bda7-e1701d9f93a1', N'Cát vàng', NULL)
INSERT [dbo].[MaterialSection] ([Id], [MaterialId], [Name], [InsDate]) VALUES (N'c94c2294-7658-4917-9452-5b3a349ec1b1', N'b56d86a3-dbe9-4d5c-a661-83fed0df4e44', N'Ống dây điện', NULL)
INSERT [dbo].[MaterialSection] ([Id], [MaterialId], [Name], [InsDate]) VALUES (N'2ea7bebc-1d78-4b55-ad35-61486caaec9d', N'941ef5fd-2dcc-4703-aab1-0dd4b71c0606', N'Máy LASER', NULL)
INSERT [dbo].[MaterialSection] ([Id], [MaterialId], [Name], [InsDate]) VALUES (N'e146371e-5659-489e-8ac7-7aeb57c23efa', N'423cff9d-bdac-4b62-ade6-35f4250ba836', N'Ống thoát nước', NULL)
INSERT [dbo].[MaterialSection] ([Id], [MaterialId], [Name], [InsDate]) VALUES (N'8d4ee99a-85a1-468c-943b-8a63bf34a4d5', N'b8b581ea-3353-4786-b007-838bfd41760a', N'Thép', NULL)
INSERT [dbo].[MaterialSection] ([Id], [MaterialId], [Name], [InsDate]) VALUES (N'4a106337-e216-4768-9d3d-ecb1f3ef5399', N'2846551d-45d2-4b7f-90be-8bd9acc4e5ce', N'Đá', NULL)
GO
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'297efa40-de4c-47f2-9ba9-1364dcba6788', N'Ống dây điện', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'fb2f4d82-f30e-4963-967a-156f763deb94', N'Ngói', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'f88ab54a-2304-4d48-b324-3a99b5f1cc77', N'Hóa chất', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'ae6be775-baad-485d-8c99-3f2143ca5163', N'Đá', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'74f27d12-b066-4846-8867-8a8a764670fa', N'Cát', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'fe9b1b34-3fd8-4b71-b748-8abaa17d65fe', N'Xi Măng', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'aa61efd9-482d-4ae8-86c5-8f096c2cd064', N'Thép', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'f053cee3-1e43-4fb9-bd32-9070d6500c86', N'Thiết bị', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'776c5f97-f429-4939-b306-acda1b734aa5', N'Ống thoát nước', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'62870bcd-3c5c-41db-bfb2-d0abe3092009', N'Dây điện', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'c3fadd14-d052-4ce0-a599-e187f53356b9', N'Bê Tông', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'38b980c3-c1b3-4d5e-abc5-f82a8c2ba31f', N'Gạch', NULL, NULL, 1)
INSERT [dbo].[MaterialType] ([Id], [Name], [InsDate], [UpsDate], [Deflag]) VALUES (N'7facd531-7971-49b6-bd00-fdaacc147cc3', N'Dây Internet', NULL, NULL, 1)
GO
INSERT [dbo].[Package] ([Id], [PackageTypeId], [PackageName], [Unit], [Price], [Status], [InsDate], [UpsDate]) VALUES (N'3f58eafc-772e-4d14-9adc-04e0736bdf2c', N'e4f968ed-74b2-4164-a8be-3f83220be61d', N'Gói hoàn thiện tiết kiệm', N'm2', 3350000, N'Active', CAST(N'2024-09-29T10:00:00.000' AS DateTime), CAST(N'2024-09-29T10:00:00.000' AS DateTime))
INSERT [dbo].[Package] ([Id], [PackageTypeId], [PackageName], [Unit], [Price], [Status], [InsDate], [UpsDate]) VALUES (N'0bfb83dd-04af-4f8c-a6d0-2cd8ee1ff0f5', N'313b205d-8dbd-438c-9935-8b460f3b7237', N'Gói thô tiêu chuẩn', N'm2', 3450000, N'Active', CAST(N'2024-10-03T10:00:00.000' AS DateTime), CAST(N'2024-10-03T10:00:00.000' AS DateTime))
INSERT [dbo].[Package] ([Id], [PackageTypeId], [PackageName], [Unit], [Price], [Status], [InsDate], [UpsDate]) VALUES (N'02f9265e-e0aa-4809-9230-395f312847cf', N'313b205d-8dbd-438c-9935-8b460f3b7237', N'Gói thô nâng cao', N'm2', 1800000, N'Active', CAST(N'2024-10-03T10:00:00.000' AS DateTime), CAST(N'2024-10-03T10:00:00.000' AS DateTime))
INSERT [dbo].[Package] ([Id], [PackageTypeId], [PackageName], [Unit], [Price], [Status], [InsDate], [UpsDate]) VALUES (N'54698692-2845-4310-9b9e-3b3ed180481c', N'e4f968ed-74b2-4164-a8be-3f83220be61d', N'Gói thô cao cấp vip', N'm2', 9999, N'Active', CAST(N'2024-10-06T22:47:41.517' AS DateTime), CAST(N'2024-10-06T23:19:34.683' AS DateTime))
INSERT [dbo].[Package] ([Id], [PackageTypeId], [PackageName], [Unit], [Price], [Status], [InsDate], [UpsDate]) VALUES (N'e3a10a06-8e7d-49b0-b83d-3d7c9b25d0b5', N'e4f968ed-74b2-4164-a8be-3f83220be61d', N'Gói hoàn thiện linh hoạt', N'm2', 3600000, N'Active', CAST(N'2024-09-29T10:00:00.000' AS DateTime), CAST(N'2024-09-29T10:00:00.000' AS DateTime))
INSERT [dbo].[Package] ([Id], [PackageTypeId], [PackageName], [Unit], [Price], [Status], [InsDate], [UpsDate]) VALUES (N'59f5fd78-b895-4d60-934a-4727c219b2d9', N'e4f968ed-74b2-4164-a8be-3f83220be61d', N'Gói hoàn thiện tiêu chuẩn', N'm2', 3550000, N'Active', CAST(N'2024-09-29T10:00:00.000' AS DateTime), CAST(N'2024-09-29T10:00:00.000' AS DateTime))
INSERT [dbo].[Package] ([Id], [PackageTypeId], [PackageName], [Unit], [Price], [Status], [InsDate], [UpsDate]) VALUES (N'fa5a68c7-cd33-471d-b3c0-4f0076e535e6', N'e4f968ed-74b2-4164-a8be-3f83220be61d', N'Gói hoàn thiện nâng cao', N'm2', 3900000, N'Active', CAST(N'2024-09-29T10:00:00.000' AS DateTime), CAST(N'2024-09-29T10:00:00.000' AS DateTime))
INSERT [dbo].[Package] ([Id], [PackageTypeId], [PackageName], [Unit], [Price], [Status], [InsDate], [UpsDate]) VALUES (N'aa5057d8-9b30-4f17-a8ee-7aad655b63cc', N'313b205d-8dbd-438c-9935-8b460f3b7237', N'Gói thô tiết kiệm', N'm2', 2550000, N'Active', CAST(N'2024-10-03T10:00:00.000' AS DateTime), CAST(N'2024-10-03T10:00:00.000' AS DateTime))
INSERT [dbo].[Package] ([Id], [PackageTypeId], [PackageName], [Unit], [Price], [Status], [InsDate], [UpsDate]) VALUES (N'b5780823-1731-4a85-aaa3-a05e83e1b99f', N'313b205d-8dbd-438c-9935-8b460f3b7237', N'Gói thô linh hoạt', N'm2', 2950000, N'Active', CAST(N'2024-10-03T10:00:00.000' AS DateTime), CAST(N'2024-10-03T10:00:00.000' AS DateTime))
GO
INSERT [dbo].[PackageDetail] ([Id], [PackageId], [Action], [Type], [InsDate]) VALUES (N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'aa5057d8-9b30-4f17-a8ee-7aad655b63cc', NULL, N'Rough', NULL)
INSERT [dbo].[PackageDetail] ([Id], [PackageId], [Action], [Type], [InsDate]) VALUES (N'a06e1a0c-4afa-4ae2-a470-a47892e2d294', N'54698692-2845-4310-9b9e-3b3ed180481c', N'', N'Rough', CAST(N'2024-10-06T22:47:41.520' AS DateTime))
GO
INSERT [dbo].[PackageHouse] ([Id], [PackageId], [DesignTemplateId], [ImgUrl], [InsDate]) VALUES (N'82b4c9b6-7d93-4c0d-bac0-92a582227de7', N'02f9265e-e0aa-4809-9230-395f312847cf', N'ec6d9970-ea66-455a-8815-dd69d72ff97f', NULL, NULL)
INSERT [dbo].[PackageHouse] ([Id], [PackageId], [DesignTemplateId], [ImgUrl], [InsDate]) VALUES (N'9d162aeb-a03c-4d5c-9a29-9db98bc72aa3', N'54698692-2845-4310-9b9e-3b3ed180481c', N'335252c2-f710-430f-ab14-a1412ee29237', N'string', CAST(N'2024-10-06T22:47:41.520' AS DateTime))
INSERT [dbo].[PackageHouse] ([Id], [PackageId], [DesignTemplateId], [ImgUrl], [InsDate]) VALUES (N'8ca1375c-939b-49f0-b8fc-b3a2a16e3448', N'aa5057d8-9b30-4f17-a8ee-7aad655b63cc', N'335252c2-f710-430f-ab14-a1412ee29237', NULL, NULL)
INSERT [dbo].[PackageHouse] ([Id], [PackageId], [DesignTemplateId], [ImgUrl], [InsDate]) VALUES (N'975aa8d1-88fc-4db8-b035-e7ef746ee272', N'aa5057d8-9b30-4f17-a8ee-7aad655b63cc', N'7af799a9-02fa-4cd0-b954-86b102840e60', NULL, NULL)
GO
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'70926bd1-025f-4e91-8e93-05584ed73085', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'63a7953f-a220-4938-8c27-40bddc408617', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'b2bb48f4-85e8-4e1d-8bd5-18b09f1729f3', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'58543f35-e6f1-4851-b012-2137432a71c2', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'69988f3f-3f9a-4844-bf44-18f80ef5c1dc', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'98ca966c-d741-4aa3-a982-a420fb08d670', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'f98165e0-25ad-4a0e-93a7-1b8931e7b6c3', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'd12798bf-c423-415c-b367-bb826793f57f', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'283b7067-6099-4661-a36a-1bdf7707f70b', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'4cd9a933-87d6-49e8-bf10-774c01afe235', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'105fa09c-12da-4989-958d-2566d3e1cbb3', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'1413ec55-981b-406e-8e78-83c742c39c9e', 400000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'9926bcfa-6ee7-4bc2-aa16-289a517ddeaa', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'c41ad3f6-0ce3-437f-a313-d6268ce4c15f', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'64bd6b98-d3b9-430c-bf1d-32d7a12b920e', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'66719137-e8b1-4198-9a9e-a40101c77aea', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'7a9e7bd0-46d9-4569-a4f6-3e38719aa0f3', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'5ecffef5-6441-437c-903b-ed469e4a819a', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'88a3e5fe-7e6b-45bd-aca6-3f4a03d11d30', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'de396246-83f6-48f5-b049-76b16134d31b', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'428e8d6e-d42e-4e23-9046-51d80e568118', N'a06e1a0c-4afa-4ae2-a470-a47892e2d294', N'63a7953f-a220-4938-8c27-40bddc408617', 9999, 100, CAST(N'2024-10-06T22:47:41.520' AS DateTime))
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'd9521cb2-6d68-4caa-9cb5-5b7619843557', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'bc3b0032-1c54-4496-aa18-f7fcb9f9e147', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'fece79ba-a71f-4259-842c-6d12ee386cfc', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'47b540b2-8d41-45a9-9685-f62e67ddbe0d', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'7984c5a5-7842-4dd6-8b70-7ddc7d17d34e', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'36c8ea55-6bef-4822-99dc-474fa2242d84', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'2287603f-14ec-4721-9b67-8e7f01c094db', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'17b73a9b-33ee-416b-8307-b46aa4417f90', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'8b0b0f3e-06ea-405c-800b-98e8c1f0a540', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'4b82dcfc-6ded-4212-a363-3dd11b74ff6c', 160000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'bcc526f6-6737-4edd-abe2-afca582aff5a', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'37804252-49d1-423f-98fd-4ca4a19c58f0', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'9b5dad1f-38d3-4ad1-867a-bd1d2dcafdbf', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'5a21c01b-954e-40da-93d2-7e1d69013c9e', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'54f838dd-928c-472d-a2a3-be72a0f4c61c', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'fb22bbac-f43d-4aee-8fe4-e522aca80398', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'fab144a3-064c-4c8d-9425-c6d27ac5cbbe', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'89587e2c-5a40-4d45-95c7-ba4146e5e4d3', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'2b593003-0157-4f46-ab31-d62a4dcc07c9', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'fec0ca84-5523-4531-b20b-1d70375f3699', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'7995a761-465c-4d09-856d-d7e491b790ab', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'0d207781-eebb-4a03-96ce-005434963f44', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'47feccf4-1948-4e0d-9ab0-d8ac625cb883', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'f8df9b3d-47aa-4579-8f90-1f0de2112f85', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'40d42c24-7002-4a58-b877-dc52e7e854ef', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'e267a87e-383f-457d-874e-9acbd42fc1ad', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'5ab7db52-ad86-4c9a-b2d4-f119b5d9ba29', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'6dd8f613-43bc-4ba2-859f-4a6b69440ec1', 350000, 1, NULL)
INSERT [dbo].[PackageLabor] ([Id], [PackageDetailId], [LaborId], [Price], [Quantity], [InsDate]) VALUES (N'3f67bece-39c7-4910-b9a6-f9db52bfc300', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', N'a9850299-ea54-4f53-af09-1d4eb528fcd4', 150000, 1, NULL)
GO
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'a48c147c-cfa9-43b4-9419-0aa93287fae3', N'082dfe19-5250-43dd-ae01-02a34d37fe13', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'7da64b18-9c6a-4c39-97d3-15cb98e981a3', N'baf50ad2-8f54-48b7-a240-2a7db6f7a33d', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'5c5b325a-048c-4a87-8079-218ab60a9e82', N'8d4ee99a-85a1-468c-943b-8a63bf34a4d5', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'91f9b9d9-60cd-4a88-be7b-2446b38f8ae1', N'30b17727-c177-4865-9a75-11ac9b493023', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'2a8d81e2-5d34-4eb0-8c79-2569a0f4d642', N'4a106337-e216-4768-9d3d-ecb1f3ef5399', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'8efe1f48-054e-452e-9216-4a5b869f38df', N'eebabf3a-1cd5-4736-9ee9-3c3e5c3b43ef', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'a887914c-2884-45b1-acbd-4ee8ab23f087', N'2ea7bebc-1d78-4b55-ad35-61486caaec9d', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'68a9f0d9-c1ac-4d9b-af54-958d532e0554', N'c8e9092b-31c7-402a-b5f0-561e87a59c0a', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'7fddbd72-c5f9-4261-ad7e-a32114999133', N'5f4009fd-541f-4c3c-b37e-3c151e801cc2', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'6c12d5ae-96ee-4e3a-9e93-b43dede62ecb', N'c94c2294-7658-4917-9452-5b3a349ec1b1', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'2a68d82a-6c20-4139-80f5-bb7ce60e1f03', N'eb96ae7b-7d96-4aff-850c-16d618e37ee4', N'a06e1a0c-4afa-4ae2-a470-a47892e2d294', CAST(N'2024-10-06T22:47:41.520' AS DateTime))
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'fa66f99c-506f-460b-9277-c00563e8b4f3', N'eb96ae7b-7d96-4aff-850c-16d618e37ee4', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'15349448-8a66-431a-a46a-d9c6a3e623fd', N'e146371e-5659-489e-8ac7-7aeb57c23efa', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'2fb63371-c16d-47e8-bcbe-e9b64df16279', N'f64aa8ed-f0f4-4b9f-856f-05f1c40297d8', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
INSERT [dbo].[PackageMaterial] ([Id], [MaterialSectionId], [PackageDetailId], [InsDate]) VALUES (N'91fa2920-2b5d-41ac-a16b-fbf288dae853', N'c7dbe224-7d2f-4430-b0e9-0e20f50bbcf0', N'e0e4212f-b98f-4aff-99c8-677ba7f3c13f', NULL)
GO
INSERT [dbo].[PackageQuotation] ([Id], [PackageId], [InitialQuotationId], [Type], [InsDate]) VALUES (N'c4ea8d68-5cb9-4bc2-af8b-13d56efcdb61', N'0bfb83dd-04af-4f8c-a6d0-2cd8ee1ff0f5', N'bd31c4e5-e549-42ea-aec7-0f08446f089d', N'FINISHED', CAST(N'2024-10-04T12:40:31.893' AS DateTime))
INSERT [dbo].[PackageQuotation] ([Id], [PackageId], [InitialQuotationId], [Type], [InsDate]) VALUES (N'9a7103d9-f2f1-4a21-90f7-2955a792e8b0', N'59f5fd78-b895-4d60-934a-4727c219b2d9', N'bd31c4e5-e549-42ea-aec7-0f08446f089d', N'ROUGH', CAST(N'2024-10-04T12:40:31.887' AS DateTime))
INSERT [dbo].[PackageQuotation] ([Id], [PackageId], [InitialQuotationId], [Type], [InsDate]) VALUES (N'57588302-7e06-47ba-9b22-44a938e54471', N'3f58eafc-772e-4d14-9adc-04e0736bdf2c', N'f6f03971-5a01-47ce-869a-c3f63d70fbb9', N'ROUGH', CAST(N'2024-09-29T10:00:00.000' AS DateTime))
INSERT [dbo].[PackageQuotation] ([Id], [PackageId], [InitialQuotationId], [Type], [InsDate]) VALUES (N'28034288-7cfc-4f20-a017-784af05aefef', N'3f58eafc-772e-4d14-9adc-04e0736bdf2c', N'27303a9e-c1ff-4060-a145-bad032564a0a', N'ROUGH', CAST(N'2024-10-04T00:17:13.713' AS DateTime))
GO
INSERT [dbo].[PackageType] ([Id], [Name], [InsDate]) VALUES (N'e4f968ed-74b2-4164-a8be-3f83220be61d', N'Rough', NULL)
INSERT [dbo].[PackageType] ([Id], [Name], [InsDate]) VALUES (N'313b205d-8dbd-438c-9935-8b460f3b7237', N'Finished', NULL)
GO
INSERT [dbo].[Project] ([Id], [CustomerId], [Name], [Type], [Status], [InsDate], [UpsDate], [ProjectCode], [Address], [Area]) VALUES (N'b81935a8-4482-43f5-ad68-558abde58d58', N'1e6fa320-6945-4b76-8426-ad6dfdb1ad74', N'Báo giá sơ bộ 10/4/2024 12:40:31 PM', N'ALL', N'Processing', CAST(N'2024-10-04T12:40:31.807' AS DateTime), CAST(N'2024-10-04T12:40:31.807' AS DateTime), N'7IUXJ', N'382 D2, Phường Tân Phú, TP.Thủ Đức', 125)
INSERT [dbo].[Project] ([Id], [CustomerId], [Name], [Type], [Status], [InsDate], [UpsDate], [ProjectCode], [Address], [Area]) VALUES (N'fa91e187-b0c8-46c3-b6d5-747d32bd3540', N'2fcb2c45-4dcc-4f3a-8fe4-1d2ee48fa931', N'Dự án báo giá 09-26-2024', N'ALL', N'Processing', CAST(N'2024-09-26T14:30:00.000' AS DateTime), CAST(N'2024-09-26T14:30:00.000' AS DateTime), N'R84GH', N'45/3 Đường Dương Đình Hội, Phường Phước Long B, Quận 9, TP. Thủ Đức', 252)
INSERT [dbo].[Project] ([Id], [CustomerId], [Name], [Type], [Status], [InsDate], [UpsDate], [ProjectCode], [Address], [Area]) VALUES (N'799b9201-d234-47b9-a14f-7574cc84ef74', N'1e6fa320-6945-4b76-8426-ad6dfdb1ad74', N'Báo giá sơ bộ 10/4/2024 1:24:43 AM', N'ALL', N'Processing', CAST(N'2024-10-04T01:24:43.810' AS DateTime), CAST(N'2024-10-04T01:24:43.810' AS DateTime), N'4PWUI', N'382 D2, Phường Tân Phú, TP.Thủ Đức', 125)
INSERT [dbo].[Project] ([Id], [CustomerId], [Name], [Type], [Status], [InsDate], [UpsDate], [ProjectCode], [Address], [Area]) VALUES (N'b841c593-1d05-4492-9ba2-d485f6d67260', N'2fcb2c45-4dcc-4f3a-8fe4-1d2ee48fa931', N'Dự án báo giá 09-25-2024', N'FINISHED', N'Processing', CAST(N'2024-09-11T09:15:00.000' AS DateTime), CAST(N'2024-09-11T09:15:00.000' AS DateTime), N'G8453', N'78/9 Đường Lê Văn Việt, Phường Hiệp Phú, TP. Thủ Đức', 341)
INSERT [dbo].[Project] ([Id], [CustomerId], [Name], [Type], [Status], [InsDate], [UpsDate], [ProjectCode], [Address], [Area]) VALUES (N'310806e2-876b-48d6-a87a-e534e4ffa647', N'2fcb2c45-4dcc-4f3a-8fe4-1d2ee48fa931', N'Dự án báo giá 09-20-2024', N'ROUGH', N'Processing', CAST(N'2024-09-11T09:15:00.000' AS DateTime), CAST(N'2024-09-11T09:15:00.000' AS DateTime), N'F3421', N'120/5 Đường Nguyễn Xiển, Phường Long Thạnh Mỹ, TP.Thủ Đức', 125)
INSERT [dbo].[Project] ([Id], [CustomerId], [Name], [Type], [Status], [InsDate], [UpsDate], [ProjectCode], [Address], [Area]) VALUES (N'c64e1637-635b-4d03-a68e-ed5f81d3fd57', N'1e6fa320-6945-4b76-8426-ad6dfdb1ad74', N'Dự án báo giá 09-26-2024', N'ALL', N'Processing', CAST(N'2024-09-11T09:15:00.000' AS DateTime), CAST(N'2024-09-11T09:15:00.000' AS DateTime), N'E2245', N'56/4 Đường Nguyễn Duy Trinh, Phường Trường Thạnh, TP.Thủ Đức', 101)
INSERT [dbo].[Project] ([Id], [CustomerId], [Name], [Type], [Status], [InsDate], [UpsDate], [ProjectCode], [Address], [Area]) VALUES (N'eac91a0b-baae-4e00-a21f-f4d05153f447', N'1e6fa320-6945-4b76-8426-ad6dfdb1ad74', N'Báo giá sơ bộ - TP.Thủ Đức', N'ALL', N'Processing', CAST(N'2024-10-04T00:17:12.263' AS DateTime), CAST(N'2024-10-04T00:17:12.263' AS DateTime), N'SJRYA', N'382 D2, Phường Tân Phú, TP.Thủ Đức', 125)
GO
INSERT [dbo].[Promotion] ([Id], [Code], [Value], [AvailableTime], [InsDate], [Name]) VALUES (N'41b828b2-7b06-4389-9003-daac937158dd', NULL, 10, NULL, NULL, N'Giảm 10% cho khách hàng may mắn')
GO
INSERT [dbo].[QuotationUtilities] ([Id], [UtilitiesItemId], [FinalQuotationId], [InitialQuotationId], [Name], [Coefiicient], [Price], [Description], [InsDate], [UpsDate]) VALUES (N'7cb191cf-3578-40c2-9490-2364d9516622', N'2ec103aa-aa83-4d58-9e85-22a6247f4cd6', NULL, N'27303a9e-c1ff-4060-a145-bad032564a0a', N'', 0.05, 110617000, N'Sàn từ 30m2 ~ 40m2', CAST(N'2024-10-04T00:17:13.770' AS DateTime), CAST(N'2024-10-04T00:17:13.770' AS DateTime))
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
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'7d7a584c-1ad5-4a91-898b-0166748b0abb', N'Cty Việt-Nhật', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'20f32cbb-9415-4a0e-9404-2f21c770bda4', N'Bestmix', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'49cdf959-7ba7-43fa-bc63-53b312a3dcd0', N'Chinhanelectric', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'1b2d1a24-a9d0-44d0-969c-5a6732a519fa', N'Viet Han', NULL, NULL, NULL, NULL, NULL, 1, N'Dây Internet', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'58135989-3c74-4485-a1ab-5ddab11bb6f6', N'VLXD Vạn thành công', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'27528179-b5b6-4b5e-837b-62a202a37c10', N'Nhà máy sát thép', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'0c9f2fb1-845b-4ce0-b73c-6a2228955248', N'Cty Bình Minh', NULL, NULL, NULL, NULL, NULL, 1, N'', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'4674adf8-75fa-47e1-b27a-6ca23b102d88', N'Gạch Xinh', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'787502b1-30b2-49a1-b92f-75da8bbeaa3d', N'Quangminhhung', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'06eb8dc0-1ff9-410f-9d05-7f18d4a8f504', N'Nhà máy Tân Uyên - Bình dương', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'1ddabadf-7a17-49c0-9e4c-7fb3ffcffaa3', N'yamasu', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'b2cc8909-cc6e-45c0-b6d3-9debf98cc58b', N'VLXD Hiệp Hà', NULL, NULL, NULL, NULL, NULL, 1, N'Bê Tông thương phẩn đá đen', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'e962f51c-b52a-4ac5-b94c-af92a7f0128c', N'Cadivi', NULL, NULL, NULL, NULL, NULL, 1, N'Dây điện', NULL)
INSERT [dbo].[Supplier] ([Id], [Name], [Email], [ConstractPhone], [ImgUrl], [InsDate], [UpsDate], [Deflag], [ShortDescription], [Description]) VALUES (N'ddf513eb-c9e8-4430-ad57-f9cf7f9178b2', N'Siam City Cement', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL)
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
INSERT [dbo].[Utilities] ([Id], [Name], [Type], [Status], [InsDate], [UpsDate]) VALUES (N'04f30a66-9758-45db-88a7-6f098edc4837', N'Dịch vụ tiện ích thêm', NULL, N'Active', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime))
INSERT [dbo].[Utilities] ([Id], [Name], [Type], [Status], [InsDate], [UpsDate]) VALUES (N'2367dec1-e649-4549-b81b-701f2dbc1a7b', N'Nâng cao chất lượng phần thô', N'ROUGH', N'Active', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime))
INSERT [dbo].[Utilities] ([Id], [Name], [Type], [Status], [InsDate], [UpsDate]) VALUES (N'05430765-97fe-4186-900d-d5dc850e8cdb', N'Tiện ích công trình', NULL, N'Active', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime))
INSERT [dbo].[Utilities] ([Id], [Name], [Type], [Status], [InsDate], [UpsDate]) VALUES (N'002e459a-e010-493f-8585-d729d3cf357b', N'Điều kiện thi công không thuận lợi', N'ROUGH', N'Active', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime))
GO
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate]) VALUES (N'baba9651-91ee-40a2-a824-0b90f861b72e', N'8d94e702-1a40-4316-815c-1668ab01d7d6', N'Hẻm < 2m', 0.05, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime))
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate]) VALUES (N'8f17e0a9-6192-4155-a360-165073ce598d', N'8d94e702-1a40-4316-815c-1668ab01d7d6', N'Hẻm 2m - 3m', 0.04, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime))
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate]) VALUES (N'2ec103aa-aa83-4d58-9e85-22a6247f4cd6', N'dc3e0910-dae6-4faa-abfd-6e79d2a0a9aa', N'Sàn từ 30m2 ~ 40m2', 0.05, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime))
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate]) VALUES (N'0a76055d-3aa0-41bf-868f-637ef0c7b19b', N'dc3e0910-dae6-4faa-abfd-6e79d2a0a9aa', N'Sàn từ 40m2 ~ 50m2', 0.04, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime))
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate]) VALUES (N'3a071c11-b5f0-4b04-a7b2-7ae2fb655815', N'8d94e702-1a40-4316-815c-1668ab01d7d6', N'Hẻm 4m - 5m', 0.02, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime))
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate]) VALUES (N'422bb684-c541-47f5-ae3b-7f8f38e91e84', N'8d94e702-1a40-4316-815c-1668ab01d7d6', N'Hẻm 5m - 6m', 0.01, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime))
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate]) VALUES (N'69ac1a58-c762-4178-8a0b-ae1aa8d99edd', N'dc3e0910-dae6-4faa-abfd-6e79d2a0a9aa', N'Sàn từ 50m2 ~ 60m2', 0.03, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime))
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate]) VALUES (N'f9cacd68-a453-4ddf-ac9d-be97911d0e90', N'dc3e0910-dae6-4faa-abfd-6e79d2a0a9aa', N'Sàn từ 60m2 ~ 70m2', 0.01, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime))
INSERT [dbo].[UtilitiesItem] ([Id], [SectionId], [Name], [Coefficient], [InsDate], [UpsDate]) VALUES (N'8ebff368-da33-4437-b93e-c6fa1642e1b5', N'8d94e702-1a40-4316-815c-1668ab01d7d6', N'Hẻm 3m - 4m', 0.03, CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime))
GO
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Status], [InsDate], [UpsDate], [Description]) VALUES (N'8d94e702-1a40-4316-815c-1668ab01d7d6', N'002e459a-e010-493f-8585-d729d3cf357b', N'Chi phí thi công trình hẻm nhỏ', N'Active', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), NULL)
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Status], [InsDate], [UpsDate], [Description]) VALUES (N'dc3e0910-dae6-4faa-abfd-6e79d2a0a9aa', N'002e459a-e010-493f-8585-d729d3cf357b', N'Chi phí thi công sàn nhỏ hơn 70m2', N'Active', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), N'Với diện tích sàn < 70m2, vẫn phải tốn chi phí tương đương cho thao tác thi công, thời gian thi công...Vì vậy, đây là hạng mục được đưa vào chi phí bất lợi do điều kiện thi công')
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Status], [InsDate], [UpsDate], [Description]) VALUES (N'787cb231-0ed3-443d-b1da-ab05567284bb', N'002e459a-e010-493f-8585-d729d3cf357b', N'Hỗ trợ bãi tập kết, điều kiện thi công khó khăn', N'Active', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), NULL)
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Status], [InsDate], [UpsDate], [Description]) VALUES (N'1f0298c3-96fb-4997-8efa-af5b7e0eabf3', N'002e459a-e010-493f-8585-d729d3cf357b', N'Chi phí thi công nhà 2 mặt tiền trở lên', N'Active', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), NULL)
INSERT [dbo].[UtilitiesSection] ([Id], [UtilitiesId], [Name], [Status], [InsDate], [UpsDate], [Description]) VALUES (N'a3681867-6365-4a1d-9aa8-b2c68766f536', N'002e459a-e010-493f-8585-d729d3cf357b', N'Chi phí thi công công trình tỉnh', N'Active', CAST(N'2024-09-28T14:45:30.123' AS DateTime), CAST(N'2024-09-28T14:45:30.123' AS DateTime), NULL)
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
ALTER TABLE [dbo].[EquipmentItem]  WITH CHECK ADD  CONSTRAINT [FK_EquimentItem_FinalQuotation] FOREIGN KEY([FinalQuotationId])
REFERENCES [dbo].[FinalQuotation] ([Id])
GO
ALTER TABLE [dbo].[EquipmentItem] CHECK CONSTRAINT [FK_EquimentItem_FinalQuotation]
GO
ALTER TABLE [dbo].[FinalQuotation]  WITH CHECK ADD  CONSTRAINT [FK_FinalQuotation_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[FinalQuotation] CHECK CONSTRAINT [FK_FinalQuotation_Account]
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
ALTER TABLE [dbo].[FinalQuotationItem]  WITH CHECK ADD  CONSTRAINT [FK_FinalQuotationItem_FinalQuotation] FOREIGN KEY([FinalQuotationId])
REFERENCES [dbo].[FinalQuotation] ([Id])
GO
ALTER TABLE [dbo].[FinalQuotationItem] CHECK CONSTRAINT [FK_FinalQuotationItem_FinalQuotation]
GO
ALTER TABLE [dbo].[HouseDesignDrawing]  WITH CHECK ADD  CONSTRAINT [FK_HouseDesignDrawing_AssignTask] FOREIGN KEY([AssignTaskId])
REFERENCES [dbo].[AssignTask] ([Id])
GO
ALTER TABLE [dbo].[HouseDesignDrawing] CHECK CONSTRAINT [FK_HouseDesignDrawing_AssignTask]
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
ALTER TABLE [dbo].[InitialQuotation]  WITH CHECK ADD  CONSTRAINT [FK_InitialQuotation_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[InitialQuotation] CHECK CONSTRAINT [FK_InitialQuotation_Account]
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
REFERENCES [dbo].[Utilities] ([Id])
GO
ALTER TABLE [dbo].[UtilitiesSection] CHECK CONSTRAINT [FK_UltilitiesSection_Ultilities]
GO
USE [master]
GO
ALTER DATABASE [RHCQS] SET  READ_WRITE 
GO
