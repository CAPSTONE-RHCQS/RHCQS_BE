USE [master]
GO
/****** Object:  Database [RHCQS]    Script Date: 9/9/2024 8:40:16 PM ******/
CREATE DATABASE [RHCQS]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RHCQS', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\RHCQS.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RHCQS_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\RHCQS_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
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
ALTER DATABASE [RHCQS] SET QUERY_STORE = OFF
GO
USE [RHCQS]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[Id] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NULL,
	[Email] [nvarchar](50) NULL,
	[Username] [nvarchar](50) NULL,
	[ImageUrl] [nvarchar](max) NULL,
	[PasswordHasd] [nvarchar](50) NULL,
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
/****** Object:  Table [dbo].[Contract]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contract](
	[Id] [uniqueidentifier] NOT NULL,
	[QuotationId] [uniqueidentifier] NULL,
	[Name] [nvarchar](50) NULL,
	[CustomerName] [nvarchar](100) NULL,
	[ContractCode] [nchar](10) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[ValidityPeriod] [int] NULL,
	[TaxCode] [nchar](10) NULL,
	[Deflag] [bit] NULL,
 CONSTRAINT [PK_Contract] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DesignHouse]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DesignHouse](
	[Id] [uniqueidentifier] NULL,
	[ProjectId] [uniqueidentifier] NULL,
	[Bedroom] [int] NULL,
	[Area] [float] NULL,
	[Floor] [int] NULL,
	[TotalPrice] [float] NULL,
	[InsDate] [datetime] NULL,
	[Deflag] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DesignTemplate]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DesignTemplate](
	[Id] [uniqueidentifier] NOT NULL,
	[PackageMaterialId] [uniqueidentifier] NULL,
	[ImgUrl] [nvarchar](max) NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_DesignTemplate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinishedConstruction]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinishedConstruction](
	[Id] [uniqueidentifier] NOT NULL,
	[ProjectId] [uniqueidentifier] NULL,
	[Paint] [nvarchar](100) NULL,
	[Window] [nvarchar](100) NULL,
	[Price] [float] NULL,
	[Unit] [nvarchar](50) NULL,
	[Area] [float] NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_FinishedConstruction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HouseDesignDrawing]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HouseDesignDrawing](
	[Id] [uniqueidentifier] NOT NULL,
	[ImageUrl] [nvarchar](max) NULL,
	[QuotationId] [uniqueidentifier] NULL,
	[IsCompany] [bit] NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_HouseDesignDrawing] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InstallmentPayment]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InstallmentPayment](
	[Id] [uniqueidentifier] NOT NULL,
	[ContractId] [uniqueidentifier] NULL,
	[Price] [float] NULL,
	[PaymentDate] [datetime] NULL,
	[PaymentPhase] [datetime] NULL,
 CONSTRAINT [PK_InstallmentPayment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Labor]    Script Date: 9/9/2024 8:40:16 PM ******/
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
 CONSTRAINT [PK_Labor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Material]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Material](
	[Id] [uniqueidentifier] NULL,
	[SupplierId] [uniqueidentifier] NULL,
	[MaterialTypeId] [uniqueidentifier] NULL,
	[Name] [nvarchar](50) NULL,
	[InventoryQuantity] [int] NULL,
	[InputPrice] [float] NULL,
	[ExportPrice] [float] NULL,
	[Unit] [nvarchar](50) NULL,
	[Size] [nvarchar](50) NULL,
	[Shape] [nvarchar](50) NULL,
	[ImgUrl] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaterialCategory]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MaterialCategory](
	[Id] [uniqueidentifier] NOT NULL,
	[QuotationMaterialId] [uniqueidentifier] NULL,
	[CategoryName] [nvarchar](100) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[Deflag] [bit] NULL,
 CONSTRAINT [PK_MaterialCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaterialHouse]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MaterialHouse](
	[Id] [uniqueidentifier] NULL,
	[MaterialId] [uniqueidentifier] NULL,
	[HouseDetaiIId] [uniqueidentifier] NULL,
	[Color] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaterialSection]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MaterialSection](
	[Id] [uniqueidentifier] NOT NULL,
	[MaterialTypeId] [uniqueidentifier] NULL,
	[Name] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_MaterialSection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaterialType]    Script Date: 9/9/2024 8:40:16 PM ******/
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
/****** Object:  Table [dbo].[Message]    Script Date: 9/9/2024 8:40:16 PM ******/
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
/****** Object:  Table [dbo].[Package]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Package](
	[Id] [uniqueidentifier] NOT NULL,
	[DesignHouseId] [uniqueidentifier] NULL,
	[PackageTypeId] [uniqueidentifier] NULL,
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
/****** Object:  Table [dbo].[PackageDetail]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageDetail](
	[Id] [uniqueidentifier] NOT NULL,
	[PackageId] [uniqueidentifier] NULL,
	[Action] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
 CONSTRAINT [PK_PackageDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PackageLabor]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageLabor](
	[Id] [uniqueidentifier] NOT NULL,
	[PackageDetailId] [uniqueidentifier] NULL,
	[LaborId] [uniqueidentifier] NULL,
	[Price] [float] NULL,
	[Quantity] [int] NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_PackageLabor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PackageMaterial]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageMaterial](
	[Id] [uniqueidentifier] NOT NULL,
	[PackageTypeId] [uniqueidentifier] NULL,
	[PackageName] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_PackageMaterial] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PackageMaterialSection]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageMaterialSection](
	[Id] [uniqueidentifier] NOT NULL,
	[MaterialSectionId] [uniqueidentifier] NULL,
	[PackageMaterialId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_PackageMaterialSection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PackageType]    Script Date: 9/9/2024 8:40:16 PM ******/
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
/****** Object:  Table [dbo].[Payment]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[Id] [uniqueidentifier] NOT NULL,
	[PaymentTypeId] [uniqueidentifier] NULL,
	[InstallmentpaymentId] [uniqueidentifier] NULL,
	[Status] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
 CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentType]    Script Date: 9/9/2024 8:40:16 PM ******/
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
/****** Object:  Table [dbo].[Project]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[Id] [uniqueidentifier] NOT NULL,
	[AccountId] [uniqueidentifier] NULL,
	[Name] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Promotion]    Script Date: 9/9/2024 8:40:16 PM ******/
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
/****** Object:  Table [dbo].[QuotationItem]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuotationItem](
	[Id] [uniqueidentifier] NOT NULL,
	[QuotationSectionId] [uniqueidentifier] NULL,
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
/****** Object:  Table [dbo].[QuotationLabor]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuotationLabor](
	[Id] [uniqueidentifier] NOT NULL,
	[QuotationItemId] [uniqueidentifier] NULL,
	[LaborId] [uniqueidentifier] NULL,
	[LaborPrice] [float] NULL,
	[Quantity] [int] NULL,
 CONSTRAINT [PK_QuotationLabor] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuotationMaterial]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuotationMaterial](
	[Id] [uniqueidentifier] NOT NULL,
	[QuotationItemId] [uniqueidentifier] NULL,
	[MaterialId] [uniqueidentifier] NULL,
	[MaterialCategoryId] [uniqueidentifier] NULL,
	[Unit] [nvarchar](50) NULL,
	[MaterialPrice] [float] NULL,
	[Quantity] [int] NULL,
 CONSTRAINT [PK_QuotationMaterial] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QuotationSection]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuotationSection](
	[Id] [uniqueidentifier] NOT NULL,
	[PackageId] [uniqueidentifier] NULL,
	[QuotationId] [uniqueidentifier] NULL,
	[Name] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL,
	[Status] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
 CONSTRAINT [PK_QuotationSection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 9/9/2024 8:40:16 PM ******/
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
/****** Object:  Table [dbo].[RoughCategory]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoughCategory](
	[Id] [uniqueidentifier] NOT NULL,
	[RoughConstructionId] [uniqueidentifier] NULL,
	[Name] [nvarchar](50) NULL,
	[Coefficient] [float] NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_RoughCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoughConstruction]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoughConstruction](
	[Id] [uniqueidentifier] NOT NULL,
	[ProjectId] [uniqueidentifier] NULL,
	[DesignHouseId] [uniqueidentifier] NULL,
	[RoughConstructionPriceId] [uniqueidentifier] NULL,
	[NumberOfFloors] [int] NULL,
	[Basement] [float] NULL,
	[Mezzanine] [float] NULL,
	[TopFloor] [float] NULL,
	[Rooft] [float] NULL,
	[SubRooft] [float] NULL,
	[ElevevatorRoom] [float] NULL,
	[PIT] [float] NULL,
	[UnitPrice] [float] NULL,
	[TotalPrice] [float] NULL,
	[Area] [float] NULL,
	[Cofficient] [float] NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_RoughConstruction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoughConstructionPrice]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoughConstructionPrice](
	[Id] [uniqueidentifier] NOT NULL,
	[RoughCategoryId] [uniqueidentifier] NULL,
	[Name] [nvarchar](50) NULL,
	[Price] [float] NULL,
	[Unit] [nvarchar](50) NULL,
	[Area] [float] NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_RoughConstructionPrice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Supplier]    Script Date: 9/9/2024 8:40:16 PM ******/
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
 CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ultilities]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ultilities](
	[Id] [uniqueidentifier] NOT NULL,
	[RoughId] [uniqueidentifier] NULL,
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
/****** Object:  Table [dbo].[UltilitiesDetails]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UltilitiesDetails](
	[Id] [uniqueidentifier] NOT NULL,
	[UltilitiesId] [uniqueidentifier] NULL,
	[QuotationItemId] [uniqueidentifier] NULL,
	[Name] [nvarchar](50) NULL,
	[Coefficient] [float] NULL,
	[Price] [float] NULL,
	[Description] [nvarchar](max) NULL,
	[InsDate] [datetime] NULL,
 CONSTRAINT [PK_UltilitiesDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UltilitiesItem]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UltilitiesItem](
	[Id] [uniqueidentifier] NOT NULL,
	[SectionId] [uniqueidentifier] NULL,
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
/****** Object:  Table [dbo].[UltilitiesSection]    Script Date: 9/9/2024 8:40:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UltilitiesSection](
	[Id] [uniqueidentifier] NULL,
	[UltilitiesId] [uniqueidentifier] NULL,
	[Name] [nvarchar](100) NULL,
	[Status] [nvarchar](50) NULL,
	[InsDate] [datetime] NULL,
	[UpsDate] [datetime] NULL
) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [RHCQS] SET  READ_WRITE 
GO
