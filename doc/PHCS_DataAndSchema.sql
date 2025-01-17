USE [master]
GO
/****** Object:  Database [PetHealthCareSys]    Script Date: 10/07/2024 22:35:09 ******/
CREATE DATABASE [PetHealthCareSys]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PetHealthCareSys', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MQUANG\MSSQL\DATA\PetHealthCareSys.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PetHealthCareSys_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MQUANG\MSSQL\DATA\PetHealthCareSys_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [PetHealthCareSys] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PetHealthCareSys].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PetHealthCareSys] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PetHealthCareSys] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PetHealthCareSys] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PetHealthCareSys] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PetHealthCareSys] SET ARITHABORT OFF 
GO
ALTER DATABASE [PetHealthCareSys] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [PetHealthCareSys] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PetHealthCareSys] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PetHealthCareSys] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PetHealthCareSys] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PetHealthCareSys] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PetHealthCareSys] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PetHealthCareSys] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PetHealthCareSys] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PetHealthCareSys] SET  ENABLE_BROKER 
GO
ALTER DATABASE [PetHealthCareSys] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PetHealthCareSys] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PetHealthCareSys] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PetHealthCareSys] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PetHealthCareSys] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PetHealthCareSys] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [PetHealthCareSys] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PetHealthCareSys] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [PetHealthCareSys] SET  MULTI_USER 
GO
ALTER DATABASE [PetHealthCareSys] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PetHealthCareSys] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PetHealthCareSys] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PetHealthCareSys] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PetHealthCareSys] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [PetHealthCareSys] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [PetHealthCareSys] SET QUERY_STORE = ON
GO
ALTER DATABASE [PetHealthCareSys] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [PetHealthCareSys]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Appointment]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TimeTableId] [int] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
	[BookingType] [int] NOT NULL,
	[Rating] [smallint] NULL,
	[Feedback] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[LastUpdatedBy] [int] NULL,
	[DeletedBy] [int] NULL,
	[CreatedTime] [datetimeoffset](7) NOT NULL,
	[LastUpdatedTime] [datetimeoffset](7) NOT NULL,
	[DeletedTime] [datetimeoffset](7) NULL,
	[VetId] [int] NOT NULL,
	[AppointmentDate] [date] NOT NULL,
	[CustomerId] [int] NOT NULL,
 CONSTRAINT [PK_Appointment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppointmentPets]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppointmentPets](
	[AppointmentId] [int] NOT NULL,
	[PetId] [int] NOT NULL,
 CONSTRAINT [PK_AppointmentPets] PRIMARY KEY CLUSTERED 
(
	[AppointmentId] ASC,
	[PetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppointmentService]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppointmentService](
	[AppointmentsId] [int] NOT NULL,
	[ServicesId] [int] NOT NULL,
 CONSTRAINT [PK_AppointmentService] PRIMARY KEY CLUSTERED 
(
	[AppointmentsId] ASC,
	[ServicesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cage]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Capacity] [int] NOT NULL,
	[Material] [nvarchar](max) NULL,
	[Room] [int] NULL,
	[Address] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[LastUpdatedBy] [int] NULL,
	[DeletedBy] [int] NULL,
	[CreatedTime] [datetimeoffset](7) NOT NULL,
	[LastUpdatedTime] [datetimeoffset](7) NOT NULL,
	[DeletedTime] [datetimeoffset](7) NULL,
	[IsAvailable] [bit] NOT NULL,
 CONSTRAINT [PK_Cage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Configurations]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Configurations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ConfigKey] [nvarchar](max) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[CreatedBy] [int] NULL,
	[LastUpdatedBy] [int] NULL,
	[DeletedBy] [int] NULL,
	[CreatedTime] [datetimeoffset](7) NOT NULL,
	[LastUpdatedTime] [datetimeoffset](7) NOT NULL,
	[DeletedTime] [datetimeoffset](7) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Configurations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Hospitalization]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hospitalization](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MedicalRecordId] [int] NOT NULL,
	[CageId] [int] NOT NULL,
	[TimeTableId] [int] NULL,
	[Date] [date] NOT NULL,
	[HospitalizationDateStatus] [int] NOT NULL,
	[Reason] [nvarchar](max) NULL,
	[Diagnosis] [nvarchar](max) NULL,
	[Treatment] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[LastUpdatedBy] [int] NULL,
	[DeletedBy] [int] NULL,
	[CreatedTime] [datetimeoffset](7) NOT NULL,
	[LastUpdatedTime] [datetimeoffset](7) NOT NULL,
	[DeletedTime] [datetimeoffset](7) NULL,
	[VetId] [int] NOT NULL,
 CONSTRAINT [PK_Hospitalization] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedicalItem]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicalItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[Quantity] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[LastUpdatedBy] [int] NULL,
	[DeletedBy] [int] NULL,
	[CreatedTime] [datetimeoffset](7) NOT NULL,
	[LastUpdatedTime] [datetimeoffset](7) NOT NULL,
	[DeletedTime] [datetimeoffset](7) NULL,
	[MedicalItemType] [int] NOT NULL,
	[Note] [nvarchar](max) NULL,
 CONSTRAINT [PK_MedicalItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedicalItemMedicalRecord]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicalItemMedicalRecord](
	[MedicalItemsId] [int] NOT NULL,
	[MedicalRecordsId] [int] NOT NULL,
 CONSTRAINT [PK_MedicalItemMedicalRecord] PRIMARY KEY CLUSTERED 
(
	[MedicalItemsId] ASC,
	[MedicalRecordsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedicalRecord]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicalRecord](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PetId] [int] NOT NULL,
	[RecordDetails] [nvarchar](max) NULL,
	[Date] [datetimeoffset](7) NOT NULL,
	[Diagnosis] [nvarchar](max) NULL,
	[Treatment] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[NextAppointment] [datetimeoffset](7) NULL,
	[PetWeight] [decimal](5, 2) NOT NULL,
	[ServiceId] [int] NULL,
	[UserEntityId] [int] NULL,
	[CreatedBy] [int] NULL,
	[LastUpdatedBy] [int] NULL,
	[DeletedBy] [int] NULL,
	[CreatedTime] [datetimeoffset](7) NOT NULL,
	[LastUpdatedTime] [datetimeoffset](7) NOT NULL,
	[DeletedTime] [datetimeoffset](7) NULL,
	[AdmissionDate] [datetimeoffset](7) NULL,
	[DischargeDate] [datetimeoffset](7) NULL,
	[AppointmentId] [int] NOT NULL,
	[VetId] [int] NOT NULL,
 CONSTRAINT [PK_MedicalRecord] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pet]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pet](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Species] [nvarchar](max) NULL,
	[Breed] [nvarchar](max) NULL,
	[OwnerID] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[LastUpdatedBy] [int] NULL,
	[DeletedBy] [int] NULL,
	[CreatedTime] [datetimeoffset](7) NOT NULL,
	[LastUpdatedTime] [datetimeoffset](7) NOT NULL,
	[DeletedTime] [datetimeoffset](7) NULL,
	[DateOfBirth] [datetimeoffset](7) NOT NULL,
	[IsNeutered] [bit] NOT NULL,
	[Gender] [nvarchar](max) NULL,
 CONSTRAINT [PK_Pet] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RefreshTokens]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefreshTokens](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Token] [nvarchar](max) NOT NULL,
	[Expires] [datetimeoffset](7) NOT NULL,
	[CreatedBy] [int] NULL,
	[LastUpdatedBy] [int] NULL,
	[DeletedBy] [int] NULL,
	[CreatedTime] [datetimeoffset](7) NOT NULL,
	[LastUpdatedTime] [datetimeoffset](7) NOT NULL,
	[DeletedTime] [datetimeoffset](7) NULL,
 CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleClaims]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_RoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Service]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Service](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Duration] [int] NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[CreatedBy] [int] NULL,
	[LastUpdatedBy] [int] NULL,
	[DeletedBy] [int] NULL,
	[CreatedTime] [datetimeoffset](7) NOT NULL,
	[LastUpdatedTime] [datetimeoffset](7) NOT NULL,
	[DeletedTime] [datetimeoffset](7) NULL,
 CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TimeTable]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TimeTable](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [int] NULL,
	[LastUpdatedBy] [int] NULL,
	[DeletedBy] [int] NULL,
	[CreatedTime] [datetimeoffset](7) NOT NULL,
	[LastUpdatedTime] [datetimeoffset](7) NOT NULL,
	[DeletedTime] [datetimeoffset](7) NULL,
	[EndTime] [time](7) NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_TimeTable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[AppointmentId] [int] NULL,
	[MedicalRecordId] [int] NULL,
	[Total] [decimal](18, 0) NOT NULL,
	[PaymentDate] [datetimeoffset](7) NULL,
	[Status] [int] NOT NULL,
	[PaymentMethod] [int] NULL,
	[PaymentNote] [nvarchar](max) NULL,
	[PaymentId] [nvarchar](max) NULL,
	[PaymentStaffName] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[RefundPercentage] [decimal](5, 2) NULL,
	[RefundReason] [nvarchar](max) NULL,
	[RefundDate] [datetimeoffset](7) NULL,
	[CreatedBy] [int] NULL,
	[LastUpdatedBy] [int] NULL,
	[DeletedBy] [int] NULL,
	[CreatedTime] [datetimeoffset](7) NOT NULL,
	[LastUpdatedTime] [datetimeoffset](7) NOT NULL,
	[DeletedTime] [datetimeoffset](7) NULL,
	[PaymentStaffId] [int] NULL,
	[RefundPaymentId] [nvarchar](max) NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionDetails]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransactionId] [int] NOT NULL,
	[ServiceId] [int] NULL,
	[MedicalItemId] [int] NULL,
	[Quantity] [int] NOT NULL,
	[SubTotal] [decimal](18, 0) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Price] [decimal](18, 0) NOT NULL,
 CONSTRAINT [PK_TransactionDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserClaims]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_UserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserLogins]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_UserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[Discriminator] [nvarchar](21) NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Avatar] [nvarchar](max) NULL,
	[BirthDate] [datetimeoffset](7) NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[LastUpdatedBy] [nvarchar](max) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[CreatedTime] [datetimeoffset](7) NOT NULL,
	[LastUpdatedTime] [datetimeoffset](7) NOT NULL,
	[DeletedTime] [datetimeoffset](7) NULL,
	[Verified] [datetimeoffset](7) NULL,
	[OTPExpired] [datetimeoffset](7) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[OTP] [nvarchar](max) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserTokens]    Script Date: 10/07/2024 22:35:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserTokens](
	[UserId] [int] NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_UserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240607135420_InitialCreate', N'8.0.4')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240609043845_ChangeMRCageMIHosp', N'8.0.4')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240610033814_ChangeUserPetAppo', N'8.0.4')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240610183416_ChangePropPet', N'8.0.4')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240612043512_PetChange', N'8.0.4')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240612141714_ChangeDB', N'8.0.4')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240616023404_ChangeTimeTable', N'8.0.4')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240617202607_ChangeTransaction-Details-UserIden', N'8.0.4')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240620064151_ChangeTransaction-UpdateAdmin', N'8.0.4')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240703173716_ChangeMR-Apo-Config-AddUser', N'8.0.4')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240709150634_ChangeAppo-Timetable', N'8.0.4')
GO
SET IDENTITY_INSERT [dbo].[Appointment] ON 

INSERT [dbo].[Appointment] ([Id], [TimeTableId], [Note], [Status], [BookingType], [Rating], [Feedback], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [VetId], [AppointmentDate], [CustomerId]) VALUES (6, 1, N'string', 1, 2, NULL, NULL, 6, 6, NULL, CAST(N'2024-07-09T16:35:33.6945499+00:00' AS DateTimeOffset), CAST(N'2024-07-09T16:35:33.6945499+00:00' AS DateTimeOffset), NULL, 3, CAST(N'2024-07-10' AS Date), 6)
INSERT [dbo].[Appointment] ([Id], [TimeTableId], [Note], [Status], [BookingType], [Rating], [Feedback], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [VetId], [AppointmentDate], [CustomerId]) VALUES (7, 1, NULL, 1, 1, NULL, NULL, 2, 2, NULL, CAST(N'2024-07-10T14:42:38.9854182+00:00' AS DateTimeOffset), CAST(N'2024-07-10T14:42:38.9854182+00:00' AS DateTimeOffset), NULL, 3, CAST(N'2024-07-12' AS Date), 6)
SET IDENTITY_INSERT [dbo].[Appointment] OFF
GO
INSERT [dbo].[AppointmentPets] ([AppointmentId], [PetId]) VALUES (6, 1)
INSERT [dbo].[AppointmentPets] ([AppointmentId], [PetId]) VALUES (7, 1)
GO
INSERT [dbo].[AppointmentService] ([AppointmentsId], [ServicesId]) VALUES (6, 1)
INSERT [dbo].[AppointmentService] ([AppointmentsId], [ServicesId]) VALUES (7, 1)
INSERT [dbo].[AppointmentService] ([AppointmentsId], [ServicesId]) VALUES (6, 2)
INSERT [dbo].[AppointmentService] ([AppointmentsId], [ServicesId]) VALUES (7, 2)
GO
SET IDENTITY_INSERT [dbo].[Cage] ON 

INSERT [dbo].[Cage] ([Id], [Capacity], [Material], [Room], [Address], [Description], [Note], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [IsAvailable]) VALUES (1, 4, N'Metal', 101, N'123 Pet St', N'Large metal cage for dogs', NULL, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5123052+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5123052+07:00' AS DateTimeOffset), NULL, 1)
INSERT [dbo].[Cage] ([Id], [Capacity], [Material], [Room], [Address], [Description], [Note], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [IsAvailable]) VALUES (2, 2, N'Plastic', 102, N'123 Pet St', N'Small plastic cage for cats', NULL, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5123052+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5123052+07:00' AS DateTimeOffset), NULL, 1)
INSERT [dbo].[Cage] ([Id], [Capacity], [Material], [Room], [Address], [Description], [Note], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [IsAvailable]) VALUES (3, 3, N'Metal', 103, N'123 Pet St', N'Medium metal cage for rabbits', NULL, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5123052+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5123052+07:00' AS DateTimeOffset), NULL, 1)
INSERT [dbo].[Cage] ([Id], [Capacity], [Material], [Room], [Address], [Description], [Note], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [IsAvailable]) VALUES (4, 5, N'Metal', 104, N'123 Pet St', N'Extra large metal cage for large dogs', NULL, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5123052+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5123052+07:00' AS DateTimeOffset), NULL, 1)
INSERT [dbo].[Cage] ([Id], [Capacity], [Material], [Room], [Address], [Description], [Note], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [IsAvailable]) VALUES (5, 1, N'Plastic', 105, N'123 Pet St', N'Small plastic cage for hamsters', NULL, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5123052+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5123052+07:00' AS DateTimeOffset), NULL, 1)
INSERT [dbo].[Cage] ([Id], [Capacity], [Material], [Room], [Address], [Description], [Note], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [IsAvailable]) VALUES (6, 3, N'Metal', 106, N'123 Pet St', N'Medium metal cage for birds', NULL, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5123052+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5123052+07:00' AS DateTimeOffset), NULL, 1)
INSERT [dbo].[Cage] ([Id], [Capacity], [Material], [Room], [Address], [Description], [Note], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [IsAvailable]) VALUES (7, 4, N'Wood', 107, N'123 Pet St', N'Large wooden cage for small animals', NULL, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5123052+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5123052+07:00' AS DateTimeOffset), NULL, 1)
INSERT [dbo].[Cage] ([Id], [Capacity], [Material], [Room], [Address], [Description], [Note], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [IsAvailable]) VALUES (8, 2, N'Plastic', 108, N'123 Pet St', N'Small plastic cage for reptiles', NULL, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5123052+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5123052+07:00' AS DateTimeOffset), NULL, 1)
SET IDENTITY_INSERT [dbo].[Cage] OFF
GO
SET IDENTITY_INSERT [dbo].[Configurations] ON 

INSERT [dbo].[Configurations] ([Id], [ConfigKey], [Value], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [Description]) VALUES (1, N'HospitalizationPrice', N'100000', NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5613642+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5613642+07:00' AS DateTimeOffset), NULL, NULL)
INSERT [dbo].[Configurations] ([Id], [ConfigKey], [Value], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [Description]) VALUES (2, N'RefundPercentage', N'0.7', NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5613642+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5613642+07:00' AS DateTimeOffset), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Configurations] OFF
GO
SET IDENTITY_INSERT [dbo].[Hospitalization] ON 

INSERT [dbo].[Hospitalization] ([Id], [MedicalRecordId], [CageId], [TimeTableId], [Date], [HospitalizationDateStatus], [Reason], [Diagnosis], [Treatment], [Note], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [VetId]) VALUES (2, 2, 1, 1, CAST(N'2024-07-03' AS Date), 1, N'string', N'string', N'string', N'string', 2, NULL, NULL, CAST(N'2024-07-10T01:42:49.2178979+07:00' AS DateTimeOffset), CAST(N'2024-07-09T18:42:49.2178759+00:00' AS DateTimeOffset), NULL, 1)
SET IDENTITY_INSERT [dbo].[Hospitalization] OFF
GO
SET IDENTITY_INSERT [dbo].[MedicalItem] ON 

INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (1, N'Bravecto Chews', N'Điều trị ve và bọ chét cho chó', CAST(50 AS Decimal(18, 0)), 100, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 1, N'Dành cho chó trên 6 tháng tuổi')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (2, N'Heartgard Plus', N'Phòng ngừa giun tim cho chó', CAST(45 AS Decimal(18, 0)), 150, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 1, N'Dạng nhai hàng tháng')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (3, N'Simparica', N'Điều trị ve và bọ chét cho chó', CAST(55 AS Decimal(18, 0)), 120, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 1, N'Dạng nhai hàng tháng')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (4, N'Capstar', N'Điều trị bọ chét cho chó và mèo', CAST(35 AS Decimal(18, 0)), 200, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 1, N'Viên uống')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (5, N'NexGard', N'Điều trị ve và bọ chét cho chó', CAST(55 AS Decimal(18, 0)), 130, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 1, N'Dạng nhai hàng tháng')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (6, N'Frontline Plus', N'Phòng ngừa ve và bọ chét cho chó', CAST(50 AS Decimal(18, 0)), 140, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 1, N'Dung dịch bôi hàng tháng')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (7, N'Interceptor Plus', N'Phòng ngừa giun tim cho chó', CAST(45 AS Decimal(18, 0)), 110, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 1, N'Dạng nhai hàng tháng')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (8, N'Proin', N'Điều trị tiểu không tự chủ ở chó', CAST(40 AS Decimal(18, 0)), 85, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 1, N'Viên uống hàng ngày')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (9, N'Vetoryl', N'Điều trị hội chứng Cushing ở chó', CAST(75 AS Decimal(18, 0)), 60, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 1, N'Viên nang uống hàng ngày')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (10, N'Deramaxx', N'Điều trị viêm khớp và đau sau phẫu thuật cho chó', CAST(65 AS Decimal(18, 0)), 70, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 1, N'Viên nhai hàng ngày')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (11, N'Rabies Vaccine', N'Vắc-xin phòng bệnh dại cho chó và mèo', CAST(20 AS Decimal(18, 0)), 300, 2, 2, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 0, N'Vắc-xin hàng năm')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (12, N'Distemper Vaccine', N'Vắc-xin phòng bệnh sốt chó cho chó', CAST(25 AS Decimal(18, 0)), 250, 2, 2, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 0, N'Vắc-xin hàng năm')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (13, N'Parvovirus Vaccine', N'Vắc-xin phòng bệnh parvo cho chó', CAST(30 AS Decimal(18, 0)), 200, 2, 2, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 0, N'Vắc-xin hàng năm')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (14, N'Bordetella Vaccine', N'Vắc-xin phòng ho cũi chó cho chó', CAST(35 AS Decimal(18, 0)), 220, 2, 2, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 0, N'Vắc-xin hàng năm')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (15, N'Leptospirosis Vaccine', N'Vắc-xin phòng bệnh leptospirosis cho chó', CAST(40 AS Decimal(18, 0)), 180, 2, 2, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 0, N'Vắc-xin hàng năm')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (16, N'Lyme Disease Vaccine', N'Vắc-xin phòng bệnh Lyme cho chó', CAST(50 AS Decimal(18, 0)), 190, 2, 2, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 0, N'Vắc-xin hàng năm')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (17, N'Canine Influenza Vaccine', N'Vắc-xin phòng bệnh cúm chó cho chó', CAST(30 AS Decimal(18, 0)), 210, 2, 2, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 0, N'Vắc-xin hàng năm')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (18, N'Canine Hepatitis Vaccine', N'Vắc-xin phòng viêm gan ở chó', CAST(30 AS Decimal(18, 0)), 200, 2, 2, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 0, N'Vắc-xin hàng năm')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (19, N'Parainfluenza Vaccine', N'Vắc-xin phòng viêm phổi do virus Parainfluenza ở chó', CAST(25 AS Decimal(18, 0)), 230, 2, 2, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 0, N'Vắc-xin hàng năm')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (20, N'Coronavirus Vaccine', N'Vắc-xin phòng bệnh do virus Corona ở chó', CAST(35 AS Decimal(18, 0)), 240, 2, 2, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 0, N'Vắc-xin hàng năm')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (21, N'Revolution for Cats', N'Phòng ngừa ve, bọ chét và giun tim cho mèo', CAST(60 AS Decimal(18, 0)), 80, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 1, N'Dung dịch bôi ngoài')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (22, N'Capstar', N'Điều trị bọ chét cho chó và mèo', CAST(35 AS Decimal(18, 0)), 200, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 1, N'Viên uống')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (23, N'Advantage II', N'Phòng ngừa bọ chét cho mèo', CAST(40 AS Decimal(18, 0)), 90, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 1, N'Dung dịch bôi hàng tháng')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (24, N'Cheristin', N'Phòng ngừa bọ chét cho mèo', CAST(30 AS Decimal(18, 0)), 160, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 1, N'Dung dịch bôi ngoài')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (25, N'Comfortis', N'Điều trị bọ chét cho mèo', CAST(60 AS Decimal(18, 0)), 110, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 1, N'Viên uống hàng tháng')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (26, N'Drontal', N'Điều trị giun sán cho mèo', CAST(20 AS Decimal(18, 0)), 140, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 1, N'Viên uống')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (27, N'Cerenia', N'Điều trị nôn mửa ở mèo', CAST(25 AS Decimal(18, 0)), 100, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 1, N'Viên uống hàng ngày')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (28, N'Methimazole', N'Điều trị cường giáp ở mèo', CAST(35 AS Decimal(18, 0)), 70, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 1, N'Viên uống hàng ngày')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (29, N'Prednisolone', N'Điều trị viêm và dị ứng ở mèo', CAST(30 AS Decimal(18, 0)), 130, 1, 1, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 1, N'Viên uống hằng ngày')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (30, N'Rabies Vaccine', N'Vắc-xin phòng bệnh dại cho chó và mèo', CAST(20 AS Decimal(18, 0)), 300, 2, 2, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 0, N'Vắc-xin hàng năm')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (31, N'FVRCP Vaccine', N'Vắc-xin phòng bệnh viêm mũi khí quản, calicivirus và bệnh giảm bạch cầu cho mèo', CAST(25 AS Decimal(18, 0)), 270, 2, 2, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 0, N'Vắc-xin hàng năm')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (32, N'FeLV Vaccine', N'Vắc-xin phòng bệnh bạch cầu ở mèo', CAST(35 AS Decimal(18, 0)), 230, 2, 2, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 0, N'Vắc-xin hàng năm')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (33, N'FIP Vaccine', N'Vắc-xin phòng bệnh viêm phúc mạc truyền nhiễm ở mèo', CAST(45 AS Decimal(18, 0)), 150, 2, 2, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 0, N'Vắc-xin hàng năm')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (34, N'Bordetella Vaccine for Cats', N'Vắc-xin phòng bệnh ho cũi mèo', CAST(30 AS Decimal(18, 0)), 180, 2, 2, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 0, N'Vắc-xin hàng năm')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (35, N'Chlamydia Vaccine', N'Vắc-xin phòng bệnh chlamydia cho mèo', CAST(35 AS Decimal(18, 0)), 190, 2, 2, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 0, N'Vắc-xin hàng năm')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (36, N'Panleukopenia Vaccine', N'Vắc-xin phòng bệnh giảm bạch cầu ở mèo', CAST(25 AS Decimal(18, 0)), 240, 2, 2, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 0, N'Vắc-xin hàng năm')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (37, N'Calicivirus Vaccine', N'Vắc-xin phòng bệnh calicivirus ở mèo', CAST(30 AS Decimal(18, 0)), 210, 2, 2, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 0, N'Vắc-xin hàng năm')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (38, N'Rhinotracheitis Vaccine', N'Vắc-xin phòng bệnh viêm mũi khí quản ở mèo', CAST(30 AS Decimal(18, 0)), 220, 2, 2, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 0, N'Vắc-xin hàng năm')
INSERT [dbo].[MedicalItem] ([Id], [Name], [Description], [Price], [Quantity], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [MedicalItemType], [Note]) VALUES (39, N'Pneumonitis Vaccine', N'Vắc-xin phòng bệnh viêm phổi ở mèo', CAST(35 AS Decimal(18, 0)), 160, 2, 2, NULL, CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5476990+07:00' AS DateTimeOffset), NULL, 0, N'Vắc-xin hàng năm')
SET IDENTITY_INSERT [dbo].[MedicalItem] OFF
GO
INSERT [dbo].[MedicalItemMedicalRecord] ([MedicalItemsId], [MedicalRecordsId]) VALUES (1, 2)
GO
SET IDENTITY_INSERT [dbo].[MedicalRecord] ON 

INSERT [dbo].[MedicalRecord] ([Id], [PetId], [RecordDetails], [Date], [Diagnosis], [Treatment], [Note], [NextAppointment], [PetWeight], [ServiceId], [UserEntityId], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [AdmissionDate], [DischargeDate], [AppointmentId], [VetId]) VALUES (2, 1, N'string', CAST(N'2024-07-09T18:39:26.7395720+00:00' AS DateTimeOffset), N'string', N'string', N'string', NULL, CAST(1.00 AS Decimal(5, 2)), NULL, NULL, 3, 3, NULL, CAST(N'2024-07-09T18:39:26.7395720+00:00' AS DateTimeOffset), CAST(N'2024-07-09T18:39:26.7395720+00:00' AS DateTimeOffset), NULL, CAST(N'2024-07-10T11:08:07.9430000+00:00' AS DateTimeOffset), NULL, 6, 3)
SET IDENTITY_INSERT [dbo].[MedicalRecord] OFF
GO
SET IDENTITY_INSERT [dbo].[Pet] ON 

INSERT [dbo].[Pet] ([Id], [Name], [Species], [Breed], [OwnerID], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [DateOfBirth], [IsNeutered], [Gender]) VALUES (1, N'string', N'string', N'string', 6, NULL, NULL, NULL, CAST(N'2024-07-09T15:29:17.0176662+00:00' AS DateTimeOffset), CAST(N'2024-07-09T15:29:17.0176662+00:00' AS DateTimeOffset), NULL, CAST(N'2024-06-13T04:17:29.1030000+00:00' AS DateTimeOffset), 1, N'string')
INSERT [dbo].[Pet] ([Id], [Name], [Species], [Breed], [OwnerID], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [DateOfBirth], [IsNeutered], [Gender]) VALUES (2, N'22', N'Dog', N'11', 6, NULL, NULL, NULL, CAST(N'2024-07-10T15:02:17.4872860+00:00' AS DateTimeOffset), CAST(N'2024-07-10T15:02:17.4872860+00:00' AS DateTimeOffset), NULL, CAST(N'2005-01-01T00:00:00.0000000+07:00' AS DateTimeOffset), 1, N'Male')
SET IDENTITY_INSERT [dbo].[Pet] OFF
GO
SET IDENTITY_INSERT [dbo].[RefreshTokens] ON 

INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (1, 6, N'Vr3bJ+IEwcIRB7WXVpzz03qL1QHnPvsax1vPVsvQPyfKThTxB9lqr1vBlNGgmY6c++7qghTOtRzfPY5bgIX4Ew==', CAST(N'2024-07-11T15:36:47.0056625+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-09T15:36:47.0050444+00:00' AS DateTimeOffset), CAST(N'2024-07-09T15:36:47.0050444+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (2, 3, N'vTKUJXov8FtYat1dO08vpcJ9+0EyFNxn7gk8mNntAf3lDaUqyW9zJb3YvwPTpns5uph6rwjbjxUp4IxjG+6ZGg==', CAST(N'2024-07-11T18:08:48.2135249+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-09T18:08:48.2128757+00:00' AS DateTimeOffset), CAST(N'2024-07-09T18:08:48.2128757+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (3, 2, N'mR8keK5Je55Y5V6ApLVr31zecvDyjAxXPXwSfvGSNkmjUB1sBggSp2abLA13Uytn8wBxQJPogBl2DaD6epDE9g==', CAST(N'2024-07-11T18:59:36.8335467+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-09T18:59:36.8328525+00:00' AS DateTimeOffset), CAST(N'2024-07-09T18:59:36.8328525+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (4, 6, N'Pr8A164lCSXyFOTpUFeI7ohlWx1IqDa5WmtaT62nyl+XYui8gQjI7mKIEDTOTVZaZSz/DDeuvD6FGzFoja2g4g==', CAST(N'2024-07-11T20:27:34.3192526+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-09T20:27:34.3191201+00:00' AS DateTimeOffset), CAST(N'2024-07-09T20:27:34.3191201+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (5, 3, N'jkjdUqR3vip2E47leOp1DQQjcthDpjefa3Aan8rTXfblOQGhk/RGiHoBclaWCA8JJptdBYT3sMntU6LG19QEHw==', CAST(N'2024-07-11T20:40:56.7385309+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-09T20:40:56.7384160+00:00' AS DateTimeOffset), CAST(N'2024-07-09T20:40:56.7384160+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (6, 3, N'QMwK+kUsYPg8KLtULeQP3W+AHNItr5Vqgk6pBrMjYVYHL3HastoTYtzORRVJuDVqt+1dbe1h9nfpf8G1hEBKZA==', CAST(N'2024-07-11T20:41:54.7624231+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-09T20:41:54.7623134+00:00' AS DateTimeOffset), CAST(N'2024-07-09T20:41:54.7623134+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (7, 2, N'JBeklq5fbBjCiyXMl65oy8sR/GgYhcVRhpN55hGDZS1YFSfrUyG/cBckJBUN84U3sFObE308r6sc6nw2yhr4mQ==', CAST(N'2024-07-11T20:42:27.3188290+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-09T20:42:27.3188222+00:00' AS DateTimeOffset), CAST(N'2024-07-09T20:42:27.3188222+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (8, 2, N'J3v4fcbrMeCe6mdWjHjKk1YKXOc42n2ynJvLmIVluaHvqvjqJ8hnamzCkTt+O9z5dwM9wFm9YXR6ZFAyiCJrdA==', CAST(N'2024-07-11T20:45:45.1277698+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-09T20:45:45.1277651+00:00' AS DateTimeOffset), CAST(N'2024-07-09T20:45:45.1277651+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (9, 2, N'hbNtdNBRYtn3wND7wPDT442B9XhJb6MIVmEajGyIOqn478z+ISKMxiW/J4zAKBAYXRZdf3eVTyZF+F23tV3sXg==', CAST(N'2024-07-11T20:48:17.8157588+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-09T20:48:17.8156076+00:00' AS DateTimeOffset), CAST(N'2024-07-09T20:48:17.8156076+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (10, 2, N'zWgxScWCkdAQB5x7YS8o2vpPbIPQ9tnlqlASkhTFFToo8CgCApBjRdVdr7u+RGZ1t8GjLzOtbl9glTo3VcZrNA==', CAST(N'2024-07-12T06:18:08.0450672+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-10T06:18:08.0448956+00:00' AS DateTimeOffset), CAST(N'2024-07-10T06:18:08.0448956+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (11, 6, N'zujqMjTCkcMT9DudPfDRy9te5L0Rtd0JqpBYqnvyxG2BCkZkAFtEZJ1jOAiKPj0bSj/XJJF/Cv499jXAln8uoQ==', CAST(N'2024-07-12T06:18:44.5366442+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-10T06:18:44.5366409+00:00' AS DateTimeOffset), CAST(N'2024-07-10T06:18:44.5366409+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (12, 6, N'BuZDqSkzSsM0Qb+b5h86kw+8Ws7Pp++m9BG13FYLdkHp+l1uH+D6/z8b6crGHSaXgsQjlLKhUlKHaxIpoaiiDw==', CAST(N'2024-07-12T06:40:49.1126750+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-10T06:40:49.1126724+00:00' AS DateTimeOffset), CAST(N'2024-07-10T06:40:49.1126724+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (13, 6, N'7ztoNNa9Ji2gyctgrvK4ikdnJFxF3iRo5a+AOVlskgKz7x9YcqN4JsD4/q99vwBW71v0tcshpgBNHfNqSdEV+A==', CAST(N'2024-07-12T06:44:29.9742321+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-10T06:44:29.9740757+00:00' AS DateTimeOffset), CAST(N'2024-07-10T06:44:29.9740757+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (14, 6, N'rst9eztnTkM+Tyeca6f/ly2Ebd6NfdACM6HSBKlVQC0WiTAvISkLdE26khXvaVE3snUTsrdjtgI4/bWtP75BJQ==', CAST(N'2024-07-12T14:05:09.2631269+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-10T14:05:09.2630356+00:00' AS DateTimeOffset), CAST(N'2024-07-10T14:05:09.2630356+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (15, 6, N'ZmjIs6S6gYpRsVF0ulzgUkvgWlWiPif6fWcDCJeiPeepiu+PZ4H2aG34ltYhnjc8UCg+JeXBUT2wXZpWvtKO1g==', CAST(N'2024-07-12T14:08:47.9376496+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-10T14:08:47.9376464+00:00' AS DateTimeOffset), CAST(N'2024-07-10T14:08:47.9376464+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (16, 6, N'OTJGgUZllVSHR/CL/3Rhl9qYBVLedRGLD265TuQed1Wq3mnATo/Nyv4N04Rajg2ifQS5z8FIj9cYJeI1YA/wUQ==', CAST(N'2024-07-12T14:10:51.2616155+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-10T14:10:51.2615114+00:00' AS DateTimeOffset), CAST(N'2024-07-10T14:10:51.2615114+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (17, 1, N'/x5jPSY5yfQZNQoiwg3QFDAv3gzfEimPSsBKW7tFm0M8eukbPXSJDi7epB3FLJso1nu8tdv9jXxWD3p4WY3kXw==', CAST(N'2024-07-12T14:35:25.9929252+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-10T14:35:25.9926837+00:00' AS DateTimeOffset), CAST(N'2024-07-10T14:35:25.9926837+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (18, 2, N'DtnWB4k7CUpJ4awV6s4xMRyoUoOw9juZiUjJH8g40QBOr1i7iRFXm6q81LQ8sTtkCL65Yx0AkgBTK6S8jbNNSg==', CAST(N'2024-07-12T14:38:23.1711546+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-10T14:38:23.1711516+00:00' AS DateTimeOffset), CAST(N'2024-07-10T14:38:23.1711516+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (19, 6, N'62tlltKXlMMIGT1pj7/Lvo3l2jv9Boh6m0HHx2PsiuuL5icZ9gnWY7c2n4xkzbjvn7JPCqZrr8bYJSqxxWsnow==', CAST(N'2024-07-12T14:48:08.3323731+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-10T14:48:08.3323709+00:00' AS DateTimeOffset), CAST(N'2024-07-10T14:48:08.3323709+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (20, 2, N'K/6JrQnOXw/VD2j5JsCG0y8fYofp0HTXUyLh8JmsKsgPHN8uVNnuyOoLEvLHNzqmimxNMzmELshM28kBBktqgg==', CAST(N'2024-07-12T14:50:16.4719998+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-10T14:50:16.4719926+00:00' AS DateTimeOffset), CAST(N'2024-07-10T14:50:16.4719926+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (21, 6, N'MMo8VZJ0QWTcwYLJulG07AS7hbkcoc3ySJWe1YN7y7QzNkeffFdyoBUUlAllKiMuFftMuSgUACp9IFN5P+hfHw==', CAST(N'2024-07-12T14:50:49.5467180+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-10T14:50:49.5467168+00:00' AS DateTimeOffset), CAST(N'2024-07-10T14:50:49.5467168+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (22, 3, N'ad3SQu6vuFFsrM2ubvLaoz/xm7QS4PF+o8vwMPsuWlimJCDnp8xjYu19tpAdXuVRfbp0hcufSx7xQy121MuR2Q==', CAST(N'2024-07-12T14:55:43.5694528+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-10T14:55:43.5694511+00:00' AS DateTimeOffset), CAST(N'2024-07-10T14:55:43.5694511+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (23, 6, N'hyKcUS9+C8i8axqsTebhJRMhgbu01Nl0mSk3CExyH5QEzaIBpe83kQbeEUr1sE0LpKNz0IEgGnS4ITUBmRldbg==', CAST(N'2024-07-12T14:57:44.8828333+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-10T14:57:44.8828316+00:00' AS DateTimeOffset), CAST(N'2024-07-10T14:57:44.8828316+00:00' AS DateTimeOffset), NULL)
INSERT [dbo].[RefreshTokens] ([Id], [UserId], [Token], [Expires], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (24, 6, N'5vLERtixfQyEd9kencqt0izH2HA9QlC7gIDOLUvuYPkCPcAFu5+7fkscYlvtGBaOIIpLCrjRpY90fSFWskTBzA==', CAST(N'2024-07-12T15:13:34.0667856+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-10T15:13:34.0666344+00:00' AS DateTimeOffset), CAST(N'2024-07-10T15:13:34.0666344+00:00' AS DateTimeOffset), NULL)
SET IDENTITY_INSERT [dbo].[RefreshTokens] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (1, N'Admin', N'ADMIN', NULL)
INSERT [dbo].[Roles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (2, N'Staff', N'STAFF', NULL)
INSERT [dbo].[Roles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (3, N'Vet', N'VET', NULL)
INSERT [dbo].[Roles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (4, N'Customer', N'CUSTOMER', NULL)
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Service] ON 

INSERT [dbo].[Service] ([Id], [Name], [Description], [Duration], [Price], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (1, N'Kiểm soát bọ chét khi tắm (theo toa)', NULL, 30, CAST(200000 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Service] ([Id], [Name], [Description], [Duration], [Price], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (2, N'Tư vấn/Đào tạo Hành vi', NULL, 60, CAST(500000 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Service] ([Id], [Name], [Description], [Duration], [Price], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (3, N'Giấy chứng nhận sức khỏe (Bán hàng & Du lịch)', NULL, 45, CAST(150000 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Service] ([Id], [Name], [Description], [Duration], [Price], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (4, N'Nội trú và Chăm sóc ban ngày', NULL, 1440, CAST(3000000 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Service] ([Id], [Name], [Description], [Duration], [Price], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (5, N'Nhập viện', NULL, 1440, CAST(5000000 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Service] ([Id], [Name], [Description], [Duration], [Price], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (6, N'Hoàn thành các bài kiểm tra đánh giá y tế', NULL, 60, CAST(700000 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Service] ([Id], [Name], [Description], [Duration], [Price], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (7, N'Phòng thí nghiệm chẩn đoán trong nhà', NULL, 30, CAST(1000000 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Service] ([Id], [Name], [Description], [Duration], [Price], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (8, N'Vi mạch da liễu', NULL, 30, CAST(800000 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Service] ([Id], [Name], [Description], [Duration], [Price], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (9, N'Nhận biết', NULL, 15, CAST(50000 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Service] ([Id], [Name], [Description], [Duration], [Price], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (10, N'Chăm sóc nha khoa', NULL, 60, CAST(1200000 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Service] ([Id], [Name], [Description], [Duration], [Price], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (11, N'Tư vấn chế độ ăn uống', NULL, 30, CAST(200000 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Service] ([Id], [Name], [Description], [Duration], [Price], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (12, N'Tiệm thuốc', NULL, 15, CAST(100000 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Service] ([Id], [Name], [Description], [Duration], [Price], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (13, N'Siêu âm kỹ thuật số', NULL, 45, CAST(1500000 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Service] ([Id], [Name], [Description], [Duration], [Price], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (14, N'X-quang kỹ thuật số', NULL, 45, CAST(1200000 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Service] ([Id], [Name], [Description], [Duration], [Price], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (15, N'Vắc-xin', NULL, 15, CAST(300000 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Service] ([Id], [Name], [Description], [Duration], [Price], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (16, N'Nội soi sợi quang', NULL, 60, CAST(1800000 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), NULL)
INSERT [dbo].[Service] ([Id], [Name], [Description], [Duration], [Price], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime]) VALUES (17, N'Chương trình chăm sóc sức khỏe', NULL, 1440, CAST(10000000 AS Decimal(18, 0)), NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5055875+07:00' AS DateTimeOffset), NULL)
SET IDENTITY_INSERT [dbo].[Service] OFF
GO
SET IDENTITY_INSERT [dbo].[TimeTable] ON 

INSERT [dbo].[TimeTable] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [EndTime], [StartTime], [Type]) VALUES (1, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5271953+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5271953+07:00' AS DateTimeOffset), NULL, CAST(N'08:30:00' AS Time), CAST(N'08:00:00' AS Time), 1)
INSERT [dbo].[TimeTable] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [EndTime], [StartTime], [Type]) VALUES (2, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5271953+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5271953+07:00' AS DateTimeOffset), NULL, CAST(N'09:00:00' AS Time), CAST(N'08:30:00' AS Time), 1)
INSERT [dbo].[TimeTable] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [EndTime], [StartTime], [Type]) VALUES (3, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5271953+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5271953+07:00' AS DateTimeOffset), NULL, CAST(N'09:30:00' AS Time), CAST(N'09:00:00' AS Time), 1)
INSERT [dbo].[TimeTable] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [EndTime], [StartTime], [Type]) VALUES (4, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), NULL, CAST(N'10:00:00' AS Time), CAST(N'09:30:00' AS Time), 1)
INSERT [dbo].[TimeTable] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [EndTime], [StartTime], [Type]) VALUES (5, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), NULL, CAST(N'10:30:00' AS Time), CAST(N'10:00:00' AS Time), 1)
INSERT [dbo].[TimeTable] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [EndTime], [StartTime], [Type]) VALUES (6, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), NULL, CAST(N'11:00:00' AS Time), CAST(N'10:30:00' AS Time), 1)
INSERT [dbo].[TimeTable] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [EndTime], [StartTime], [Type]) VALUES (7, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), NULL, CAST(N'11:30:00' AS Time), CAST(N'11:00:00' AS Time), 1)
INSERT [dbo].[TimeTable] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [EndTime], [StartTime], [Type]) VALUES (8, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), NULL, CAST(N'12:00:00' AS Time), CAST(N'11:30:00' AS Time), 1)
INSERT [dbo].[TimeTable] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [EndTime], [StartTime], [Type]) VALUES (9, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), NULL, CAST(N'13:30:00' AS Time), CAST(N'13:00:00' AS Time), 1)
INSERT [dbo].[TimeTable] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [EndTime], [StartTime], [Type]) VALUES (10, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), NULL, CAST(N'14:00:00' AS Time), CAST(N'13:30:00' AS Time), 1)
INSERT [dbo].[TimeTable] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [EndTime], [StartTime], [Type]) VALUES (11, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), NULL, CAST(N'14:30:00' AS Time), CAST(N'14:00:00' AS Time), 1)
INSERT [dbo].[TimeTable] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [EndTime], [StartTime], [Type]) VALUES (12, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), NULL, CAST(N'15:00:00' AS Time), CAST(N'14:30:00' AS Time), 1)
INSERT [dbo].[TimeTable] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [EndTime], [StartTime], [Type]) VALUES (13, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), NULL, CAST(N'15:30:00' AS Time), CAST(N'15:00:00' AS Time), 1)
INSERT [dbo].[TimeTable] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [EndTime], [StartTime], [Type]) VALUES (14, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), NULL, CAST(N'16:00:00' AS Time), CAST(N'15:30:00' AS Time), 1)
INSERT [dbo].[TimeTable] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [EndTime], [StartTime], [Type]) VALUES (15, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5281943+07:00' AS DateTimeOffset), NULL, CAST(N'16:30:00' AS Time), CAST(N'16:00:00' AS Time), 1)
INSERT [dbo].[TimeTable] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [EndTime], [StartTime], [Type]) VALUES (16, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5291953+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5291953+07:00' AS DateTimeOffset), NULL, CAST(N'17:00:00' AS Time), CAST(N'16:30:00' AS Time), 1)
INSERT [dbo].[TimeTable] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [EndTime], [StartTime], [Type]) VALUES (17, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5332029+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5332029+07:00' AS DateTimeOffset), NULL, CAST(N'19:00:00' AS Time), CAST(N'18:00:00' AS Time), 2)
INSERT [dbo].[TimeTable] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [EndTime], [StartTime], [Type]) VALUES (18, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5336576+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5336576+07:00' AS DateTimeOffset), NULL, CAST(N'20:00:00' AS Time), CAST(N'19:00:00' AS Time), 2)
INSERT [dbo].[TimeTable] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [EndTime], [StartTime], [Type]) VALUES (19, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5336576+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5336576+07:00' AS DateTimeOffset), NULL, CAST(N'21:00:00' AS Time), CAST(N'20:00:00' AS Time), 2)
INSERT [dbo].[TimeTable] ([Id], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [EndTime], [StartTime], [Type]) VALUES (20, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5336576+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5336576+07:00' AS DateTimeOffset), NULL, CAST(N'22:00:00' AS Time), CAST(N'21:00:00' AS Time), 2)
SET IDENTITY_INSERT [dbo].[TimeTable] OFF
GO
SET IDENTITY_INSERT [dbo].[Transaction] ON 

INSERT [dbo].[Transaction] ([Id], [CustomerId], [AppointmentId], [MedicalRecordId], [Total], [PaymentDate], [Status], [PaymentMethod], [PaymentNote], [PaymentId], [PaymentStaffName], [Note], [RefundPercentage], [RefundReason], [RefundDate], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [PaymentStaffId], [RefundPaymentId]) VALUES (1, 1, NULL, NULL, CAST(650000 AS Decimal(18, 0)), NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5597833+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5597833+07:00' AS DateTimeOffset), NULL, NULL, NULL)
INSERT [dbo].[Transaction] ([Id], [CustomerId], [AppointmentId], [MedicalRecordId], [Total], [PaymentDate], [Status], [PaymentMethod], [PaymentNote], [PaymentId], [PaymentStaffName], [Note], [RefundPercentage], [RefundReason], [RefundDate], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [PaymentStaffId], [RefundPaymentId]) VALUES (2, 1, NULL, NULL, CAST(635000 AS Decimal(18, 0)), NULL, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2024-07-09T22:27:27.5597833+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:27:27.5597833+07:00' AS DateTimeOffset), NULL, NULL, NULL)
INSERT [dbo].[Transaction] ([Id], [CustomerId], [AppointmentId], [MedicalRecordId], [Total], [PaymentDate], [Status], [PaymentMethod], [PaymentNote], [PaymentId], [PaymentStaffName], [Note], [RefundPercentage], [RefundReason], [RefundDate], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [PaymentStaffId], [RefundPaymentId]) VALUES (3, 6, NULL, NULL, CAST(100 AS Decimal(18, 0)), NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 3, 3, NULL, CAST(N'2024-07-09T18:31:08.9557950+00:00' AS DateTimeOffset), CAST(N'2024-07-09T18:31:08.9557950+00:00' AS DateTimeOffset), NULL, NULL, NULL)
INSERT [dbo].[Transaction] ([Id], [CustomerId], [AppointmentId], [MedicalRecordId], [Total], [PaymentDate], [Status], [PaymentMethod], [PaymentNote], [PaymentId], [PaymentStaffName], [Note], [RefundPercentage], [RefundReason], [RefundDate], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [PaymentStaffId], [RefundPaymentId]) VALUES (4, 6, NULL, NULL, CAST(100 AS Decimal(18, 0)), NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 3, 3, NULL, CAST(N'2024-07-09T18:39:26.7396833+00:00' AS DateTimeOffset), CAST(N'2024-07-09T18:39:26.7396833+00:00' AS DateTimeOffset), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Transaction] OFF
GO
SET IDENTITY_INSERT [dbo].[TransactionDetails] ON 

INSERT [dbo].[TransactionDetails] ([Id], [TransactionId], [ServiceId], [MedicalItemId], [Quantity], [SubTotal], [Name], [Price]) VALUES (1, 1, 1, NULL, 2, CAST(400000 AS Decimal(18, 0)), N'Kiểm soát bọ chét khi tắm (theo toa)', CAST(200000 AS Decimal(18, 0)))
INSERT [dbo].[TransactionDetails] ([Id], [TransactionId], [ServiceId], [MedicalItemId], [Quantity], [SubTotal], [Name], [Price]) VALUES (2, 2, 2, NULL, 1, CAST(500000 AS Decimal(18, 0)), N'Tư vấn/Đào tạo Hành vi', CAST(500000 AS Decimal(18, 0)))
INSERT [dbo].[TransactionDetails] ([Id], [TransactionId], [ServiceId], [MedicalItemId], [Quantity], [SubTotal], [Name], [Price]) VALUES (3, 1, NULL, 1, 5, CAST(250000 AS Decimal(18, 0)), N'Bravecto Chews', CAST(50000 AS Decimal(18, 0)))
INSERT [dbo].[TransactionDetails] ([Id], [TransactionId], [ServiceId], [MedicalItemId], [Quantity], [SubTotal], [Name], [Price]) VALUES (4, 2, NULL, 2, 3, CAST(135000 AS Decimal(18, 0)), N'Heartgard Plus', CAST(45000 AS Decimal(18, 0)))
INSERT [dbo].[TransactionDetails] ([Id], [TransactionId], [ServiceId], [MedicalItemId], [Quantity], [SubTotal], [Name], [Price]) VALUES (5, 3, NULL, 1, 2, CAST(100 AS Decimal(18, 0)), N'Bravecto Chews', CAST(50 AS Decimal(18, 0)))
INSERT [dbo].[TransactionDetails] ([Id], [TransactionId], [ServiceId], [MedicalItemId], [Quantity], [SubTotal], [Name], [Price]) VALUES (6, 4, NULL, 1, 2, CAST(100 AS Decimal(18, 0)), N'Bravecto Chews', CAST(50 AS Decimal(18, 0)))
SET IDENTITY_INSERT [dbo].[TransactionDetails] OFF
GO
INSERT [dbo].[UserRoles] ([UserId], [RoleId], [Discriminator]) VALUES (1, 1, N'UserRoleEntity')
INSERT [dbo].[UserRoles] ([UserId], [RoleId], [Discriminator]) VALUES (2, 2, N'UserRoleEntity')
INSERT [dbo].[UserRoles] ([UserId], [RoleId], [Discriminator]) VALUES (3, 3, N'UserRoleEntity')
INSERT [dbo].[UserRoles] ([UserId], [RoleId], [Discriminator]) VALUES (4, 3, N'UserRoleEntity')
INSERT [dbo].[UserRoles] ([UserId], [RoleId], [Discriminator]) VALUES (5, 3, N'UserRoleEntity')
INSERT [dbo].[UserRoles] ([UserId], [RoleId], [Discriminator]) VALUES (6, 4, N'UserRoleEntity')
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [FullName], [Address], [Avatar], [BirthDate], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [Verified], [OTPExpired], [PhoneNumberConfirmed], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [ConcurrencyStamp], [PhoneNumber], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [OTP]) VALUES (1, N'Admin User', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2024-07-09T22:06:32.7587252+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:06:32.7587252+07:00' AS DateTimeOffset), NULL, NULL, NULL, 0, N'admin', N'ADMIN', N'admin@email.com', NULL, 0, N'$2a$11$ETqUuQQ3h409fMU77XErKO.n3Db3/opxDMuTC6gXr3x0o8x4oYUrC', N'7ec8a0c5-9eed-44e9-8c5f-7a5bf5ecaab3', NULL, 0, NULL, 0, 0, NULL)
INSERT [dbo].[Users] ([Id], [FullName], [Address], [Avatar], [BirthDate], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [Verified], [OTPExpired], [PhoneNumberConfirmed], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [ConcurrencyStamp], [PhoneNumber], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [OTP]) VALUES (2, N'Staff User', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2024-07-09T22:06:32.8961436+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:06:32.8961436+07:00' AS DateTimeOffset), NULL, NULL, NULL, 0, N'staff', N'staff', N'staff@email.com', NULL, 0, N'$2a$11$aeB6vHEZdKrWd65rAZgOv.V.Xl4ruBQQZraEmSRHl9vHEP6k0n.bC', N'2948b1c8-5da5-4b2a-98ae-70ed3109c8ac', NULL, 0, NULL, 0, 0, NULL)
INSERT [dbo].[Users] ([Id], [FullName], [Address], [Avatar], [BirthDate], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [Verified], [OTPExpired], [PhoneNumberConfirmed], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [ConcurrencyStamp], [PhoneNumber], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [OTP]) VALUES (3, N'John Doe', N'123 Main St', NULL, CAST(N'1985-06-15T00:00:00.0000000+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-09T22:06:33.0232155+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:06:33.0232155+07:00' AS DateTimeOffset), NULL, NULL, NULL, 0, N'vet1', N'JOHNDOE', N'johndoe@example.com', NULL, 0, N'$2a$11$QORHwkLgUyK7.JCHA3wQYuTQy0hT1xAy2rq8RDZobUdVUb69D9yoa', N'95a94c66-a888-4361-9714-f0128d10cc70', NULL, 0, NULL, 0, 0, NULL)
INSERT [dbo].[Users] ([Id], [FullName], [Address], [Avatar], [BirthDate], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [Verified], [OTPExpired], [PhoneNumberConfirmed], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [ConcurrencyStamp], [PhoneNumber], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [OTP]) VALUES (4, N'Jane Smith', N'456 Elm St', NULL, CAST(N'1990-09-20T00:00:00.0000000+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-09T22:06:33.1551884+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:06:33.1551884+07:00' AS DateTimeOffset), NULL, NULL, NULL, 0, N'vet2', N'JANESMITH', N'janesmith@example.com', NULL, 0, N'$2a$11$KE5zxdELvIEAu7pkWWBCae.Yb1kfx6gfrlphH2PGoqtZEvJuJDkQW', N'049b34b1-b416-47b6-9036-e5973d72a6e4', NULL, 0, NULL, 0, 0, NULL)
INSERT [dbo].[Users] ([Id], [FullName], [Address], [Avatar], [BirthDate], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [Verified], [OTPExpired], [PhoneNumberConfirmed], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [ConcurrencyStamp], [PhoneNumber], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [OTP]) VALUES (5, N'Alice Johnson', N'789 Pine St', NULL, CAST(N'1978-03-25T00:00:00.0000000+00:00' AS DateTimeOffset), NULL, NULL, NULL, CAST(N'2024-07-09T22:06:33.2756617+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:06:33.2756617+07:00' AS DateTimeOffset), NULL, NULL, NULL, 0, N'vet3', N'ALICEJOHNSON', N'alicejohnson@example.com', NULL, 0, N'$2a$11$.PlowQTFb1gkya5PNVLKK.nST844T77ulxj8ith0gGsuJA0T7EYnu', N'94b4c1fc-2e0b-40e9-aeb6-2829a0560192', NULL, 0, NULL, 0, 0, NULL)
INSERT [dbo].[Users] ([Id], [FullName], [Address], [Avatar], [BirthDate], [CreatedBy], [LastUpdatedBy], [DeletedBy], [CreatedTime], [LastUpdatedTime], [DeletedTime], [Verified], [OTPExpired], [PhoneNumberConfirmed], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [ConcurrencyStamp], [PhoneNumber], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [OTP]) VALUES (6, N'Tran Dinh Minh Quang', N'123 Main St', NULL, NULL, NULL, NULL, NULL, CAST(N'2024-07-09T22:06:33.4134160+07:00' AS DateTimeOffset), CAST(N'2024-07-09T22:06:33.4134160+07:00' AS DateTimeOffset), NULL, NULL, NULL, 0, N'cus1', N'CUS1', N'quangtdmse171391@example.com', NULL, 0, N'$2a$11$R1HO1LLbKkmiVz9eIGLTzO0reoq30D9rI1wqRStNe9Ue4BlqckElu', N'a01b8fdb-38b2-478f-95d0-894df3fab286', N'0123456789', 0, NULL, 0, 0, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  Index [Index_Id]    Script Date: 10/07/2024 22:35:09 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Index_Id] ON [dbo].[Appointment]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Appointment_TimeTableId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_Appointment_TimeTableId] ON [dbo].[Appointment]
(
	[TimeTableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AppointmentPets_PetId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_AppointmentPets_PetId] ON [dbo].[AppointmentPets]
(
	[PetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AppointmentService_ServicesId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_AppointmentService_ServicesId] ON [dbo].[AppointmentService]
(
	[ServicesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Index_Id]    Script Date: 10/07/2024 22:35:09 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Index_Id] ON [dbo].[Cage]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Index_Id]    Script Date: 10/07/2024 22:35:09 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Index_Id] ON [dbo].[Configurations]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Index_Id]    Script Date: 10/07/2024 22:35:09 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Index_Id] ON [dbo].[Hospitalization]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Hospitalization_CageId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_Hospitalization_CageId] ON [dbo].[Hospitalization]
(
	[CageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Hospitalization_MedicalRecordId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_Hospitalization_MedicalRecordId] ON [dbo].[Hospitalization]
(
	[MedicalRecordId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Hospitalization_TimeTableId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_Hospitalization_TimeTableId] ON [dbo].[Hospitalization]
(
	[TimeTableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Index_Id]    Script Date: 10/07/2024 22:35:09 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Index_Id] ON [dbo].[MedicalItem]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MedicalItemMedicalRecord_MedicalRecordsId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_MedicalItemMedicalRecord_MedicalRecordsId] ON [dbo].[MedicalItemMedicalRecord]
(
	[MedicalRecordsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Index_Id]    Script Date: 10/07/2024 22:35:09 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Index_Id] ON [dbo].[MedicalRecord]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MedicalRecord_AppointmentId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_MedicalRecord_AppointmentId] ON [dbo].[MedicalRecord]
(
	[AppointmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MedicalRecord_PetId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_MedicalRecord_PetId] ON [dbo].[MedicalRecord]
(
	[PetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MedicalRecord_ServiceId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_MedicalRecord_ServiceId] ON [dbo].[MedicalRecord]
(
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_MedicalRecord_UserEntityId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_MedicalRecord_UserEntityId] ON [dbo].[MedicalRecord]
(
	[UserEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Index_Id]    Script Date: 10/07/2024 22:35:09 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Index_Id] ON [dbo].[Pet]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Pet_OwnerID]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_Pet_OwnerID] ON [dbo].[Pet]
(
	[OwnerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Index_Id]    Script Date: 10/07/2024 22:35:09 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Index_Id] ON [dbo].[RefreshTokens]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RefreshTokens_UserId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_RefreshTokens_UserId] ON [dbo].[RefreshTokens]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RoleClaims_RoleId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_RoleClaims_RoleId] ON [dbo].[RoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 10/07/2024 22:35:09 ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[Roles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Index_Id]    Script Date: 10/07/2024 22:35:09 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Index_Id] ON [dbo].[Service]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Index_Id]    Script Date: 10/07/2024 22:35:09 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Index_Id] ON [dbo].[TimeTable]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Index_Id]    Script Date: 10/07/2024 22:35:09 ******/
CREATE UNIQUE NONCLUSTERED INDEX [Index_Id] ON [dbo].[Transaction]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Transaction_AppointmentId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_Transaction_AppointmentId] ON [dbo].[Transaction]
(
	[AppointmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Transaction_CustomerId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_Transaction_CustomerId] ON [dbo].[Transaction]
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Transaction_MedicalRecordId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_Transaction_MedicalRecordId] ON [dbo].[Transaction]
(
	[MedicalRecordId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TransactionDetails_MedicalItemId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_TransactionDetails_MedicalItemId] ON [dbo].[TransactionDetails]
(
	[MedicalItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TransactionDetails_ServiceId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_TransactionDetails_ServiceId] ON [dbo].[TransactionDetails]
(
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TransactionDetails_TransactionId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_TransactionDetails_TransactionId] ON [dbo].[TransactionDetails]
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserClaims_UserId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_UserClaims_UserId] ON [dbo].[UserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserLogins_UserId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_UserLogins_UserId] ON [dbo].[UserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserRoles_RoleId]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [IX_UserRoles_RoleId] ON [dbo].[UserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 10/07/2024 22:35:09 ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[Users]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 10/07/2024 22:35:09 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[Users]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Appointment] ADD  DEFAULT ((0)) FOR [VetId]
GO
ALTER TABLE [dbo].[Appointment] ADD  DEFAULT ('0001-01-01') FOR [AppointmentDate]
GO
ALTER TABLE [dbo].[Appointment] ADD  DEFAULT ((0)) FOR [CustomerId]
GO
ALTER TABLE [dbo].[Cage] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsAvailable]
GO
ALTER TABLE [dbo].[Hospitalization] ADD  DEFAULT ((0)) FOR [VetId]
GO
ALTER TABLE [dbo].[MedicalItem] ADD  DEFAULT ((0)) FOR [MedicalItemType]
GO
ALTER TABLE [dbo].[MedicalRecord] ADD  DEFAULT ((0)) FOR [AppointmentId]
GO
ALTER TABLE [dbo].[MedicalRecord] ADD  DEFAULT ((0)) FOR [VetId]
GO
ALTER TABLE [dbo].[Pet] ADD  DEFAULT ('0001-01-01T00:00:00.0000000+00:00') FOR [DateOfBirth]
GO
ALTER TABLE [dbo].[Pet] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsNeutered]
GO
ALTER TABLE [dbo].[TimeTable] ADD  DEFAULT ('00:00:00') FOR [EndTime]
GO
ALTER TABLE [dbo].[TimeTable] ADD  DEFAULT ('00:00:00') FOR [StartTime]
GO
ALTER TABLE [dbo].[TimeTable] ADD  DEFAULT ((0)) FOR [Type]
GO
ALTER TABLE [dbo].[TransactionDetails] ADD  DEFAULT ((0.0)) FOR [Price]
GO
ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_TimeTable_TimeTableId] FOREIGN KEY([TimeTableId])
REFERENCES [dbo].[TimeTable] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Appointment] CHECK CONSTRAINT [FK_Appointment_TimeTable_TimeTableId]
GO
ALTER TABLE [dbo].[AppointmentPets]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentPets_Appointment_AppointmentId] FOREIGN KEY([AppointmentId])
REFERENCES [dbo].[Appointment] ([Id])
GO
ALTER TABLE [dbo].[AppointmentPets] CHECK CONSTRAINT [FK_AppointmentPets_Appointment_AppointmentId]
GO
ALTER TABLE [dbo].[AppointmentPets]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentPets_Pet_PetId] FOREIGN KEY([PetId])
REFERENCES [dbo].[Pet] ([Id])
GO
ALTER TABLE [dbo].[AppointmentPets] CHECK CONSTRAINT [FK_AppointmentPets_Pet_PetId]
GO
ALTER TABLE [dbo].[AppointmentService]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentService_Appointment_AppointmentsId] FOREIGN KEY([AppointmentsId])
REFERENCES [dbo].[Appointment] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AppointmentService] CHECK CONSTRAINT [FK_AppointmentService_Appointment_AppointmentsId]
GO
ALTER TABLE [dbo].[AppointmentService]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentService_Service_ServicesId] FOREIGN KEY([ServicesId])
REFERENCES [dbo].[Service] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AppointmentService] CHECK CONSTRAINT [FK_AppointmentService_Service_ServicesId]
GO
ALTER TABLE [dbo].[Hospitalization]  WITH CHECK ADD  CONSTRAINT [FK_Hospitalization_Cage_CageId] FOREIGN KEY([CageId])
REFERENCES [dbo].[Cage] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hospitalization] CHECK CONSTRAINT [FK_Hospitalization_Cage_CageId]
GO
ALTER TABLE [dbo].[Hospitalization]  WITH CHECK ADD  CONSTRAINT [FK_Hospitalization_MedicalRecord_MedicalRecordId] FOREIGN KEY([MedicalRecordId])
REFERENCES [dbo].[MedicalRecord] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Hospitalization] CHECK CONSTRAINT [FK_Hospitalization_MedicalRecord_MedicalRecordId]
GO
ALTER TABLE [dbo].[Hospitalization]  WITH CHECK ADD  CONSTRAINT [FK_Hospitalization_TimeTable_TimeTableId] FOREIGN KEY([TimeTableId])
REFERENCES [dbo].[TimeTable] ([Id])
GO
ALTER TABLE [dbo].[Hospitalization] CHECK CONSTRAINT [FK_Hospitalization_TimeTable_TimeTableId]
GO
ALTER TABLE [dbo].[MedicalItemMedicalRecord]  WITH CHECK ADD  CONSTRAINT [FK_MedicalItemMedicalRecord_MedicalItem_MedicalItemsId] FOREIGN KEY([MedicalItemsId])
REFERENCES [dbo].[MedicalItem] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MedicalItemMedicalRecord] CHECK CONSTRAINT [FK_MedicalItemMedicalRecord_MedicalItem_MedicalItemsId]
GO
ALTER TABLE [dbo].[MedicalItemMedicalRecord]  WITH CHECK ADD  CONSTRAINT [FK_MedicalItemMedicalRecord_MedicalRecord_MedicalRecordsId] FOREIGN KEY([MedicalRecordsId])
REFERENCES [dbo].[MedicalRecord] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MedicalItemMedicalRecord] CHECK CONSTRAINT [FK_MedicalItemMedicalRecord_MedicalRecord_MedicalRecordsId]
GO
ALTER TABLE [dbo].[MedicalRecord]  WITH CHECK ADD  CONSTRAINT [FK_MedicalRecord_Appointment_AppointmentId] FOREIGN KEY([AppointmentId])
REFERENCES [dbo].[Appointment] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MedicalRecord] CHECK CONSTRAINT [FK_MedicalRecord_Appointment_AppointmentId]
GO
ALTER TABLE [dbo].[MedicalRecord]  WITH CHECK ADD  CONSTRAINT [FK_MedicalRecord_Pet_PetId] FOREIGN KEY([PetId])
REFERENCES [dbo].[Pet] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MedicalRecord] CHECK CONSTRAINT [FK_MedicalRecord_Pet_PetId]
GO
ALTER TABLE [dbo].[MedicalRecord]  WITH CHECK ADD  CONSTRAINT [FK_MedicalRecord_Service_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Service] ([Id])
GO
ALTER TABLE [dbo].[MedicalRecord] CHECK CONSTRAINT [FK_MedicalRecord_Service_ServiceId]
GO
ALTER TABLE [dbo].[MedicalRecord]  WITH CHECK ADD  CONSTRAINT [FK_MedicalRecord_Users_UserEntityId] FOREIGN KEY([UserEntityId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[MedicalRecord] CHECK CONSTRAINT [FK_MedicalRecord_Users_UserEntityId]
GO
ALTER TABLE [dbo].[Pet]  WITH CHECK ADD  CONSTRAINT [FK_Pet_Users_OwnerID] FOREIGN KEY([OwnerID])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Pet] CHECK CONSTRAINT [FK_Pet_Users_OwnerID]
GO
ALTER TABLE [dbo].[RefreshTokens]  WITH CHECK ADD  CONSTRAINT [FK_RefreshTokens_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RefreshTokens] CHECK CONSTRAINT [FK_RefreshTokens_Users_UserId]
GO
ALTER TABLE [dbo].[RoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_RoleClaims_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleClaims] CHECK CONSTRAINT [FK_RoleClaims_Roles_RoleId]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Appointment_AppointmentId] FOREIGN KEY([AppointmentId])
REFERENCES [dbo].[Appointment] ([Id])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Appointment_AppointmentId]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_MedicalRecord_MedicalRecordId] FOREIGN KEY([MedicalRecordId])
REFERENCES [dbo].[MedicalRecord] ([Id])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_MedicalRecord_MedicalRecordId]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Users_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Users_CustomerId]
GO
ALTER TABLE [dbo].[TransactionDetails]  WITH CHECK ADD  CONSTRAINT [FK_TransactionDetails_MedicalItem_MedicalItemId] FOREIGN KEY([MedicalItemId])
REFERENCES [dbo].[MedicalItem] ([Id])
GO
ALTER TABLE [dbo].[TransactionDetails] CHECK CONSTRAINT [FK_TransactionDetails_MedicalItem_MedicalItemId]
GO
ALTER TABLE [dbo].[TransactionDetails]  WITH CHECK ADD  CONSTRAINT [FK_TransactionDetails_Service_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Service] ([Id])
GO
ALTER TABLE [dbo].[TransactionDetails] CHECK CONSTRAINT [FK_TransactionDetails_Service_ServiceId]
GO
ALTER TABLE [dbo].[TransactionDetails]  WITH CHECK ADD  CONSTRAINT [FK_TransactionDetails_Transaction_TransactionId] FOREIGN KEY([TransactionId])
REFERENCES [dbo].[Transaction] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TransactionDetails] CHECK CONSTRAINT [FK_TransactionDetails_Transaction_TransactionId]
GO
ALTER TABLE [dbo].[UserClaims]  WITH CHECK ADD  CONSTRAINT [FK_UserClaims_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserClaims] CHECK CONSTRAINT [FK_UserClaims_Users_UserId]
GO
ALTER TABLE [dbo].[UserLogins]  WITH CHECK ADD  CONSTRAINT [FK_UserLogins_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserLogins] CHECK CONSTRAINT [FK_UserLogins_Users_UserId]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Roles_RoleId]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Users_UserId]
GO
ALTER TABLE [dbo].[UserTokens]  WITH CHECK ADD  CONSTRAINT [FK_UserTokens_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserTokens] CHECK CONSTRAINT [FK_UserTokens_Users_UserId]
GO
USE [master]
GO
ALTER DATABASE [PetHealthCareSys] SET  READ_WRITE 
GO
