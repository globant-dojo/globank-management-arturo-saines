USE [dojo.net.bp]
GO
/****** Object:  Table [dbo].[Clientes]    Script Date: 13/7/2022 18:34:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clientes](
	[ClienteId] [bigint] IDENTITY(1,1) NOT NULL,
	[PersonaId] [bigint] NOT NULL,
	[Contrasena] [varchar](50) NOT NULL,
	[Estado] [bit] NOT NULL,
 CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED 
(
	[ClienteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cuentas]    Script Date: 13/7/2022 18:34:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cuentas](
	[CuentaId] [bigint] IDENTITY(1,1) NOT NULL,
	[ClienteId] [bigint] NOT NULL,
	[NumeroCuenta] [varchar](20) NOT NULL,
	[TipoCuenta] [varchar](15) NOT NULL,
	[SaldoInicial] [money] NOT NULL,
	[SaldoDisponible] [money] NOT NULL,
	[Estado] [bit] NOT NULL,
 CONSTRAINT [PK_Cuentas] PRIMARY KEY CLUSTERED 
(
	[CuentaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UN_Cuentas_NumeroCuenta_TipoCta] UNIQUE NONCLUSTERED 
(
	[NumeroCuenta] ASC,
	[TipoCuenta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Movimientos]    Script Date: 13/7/2022 18:34:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Movimientos](
	[MovimientoId] [bigint] IDENTITY(1,1) NOT NULL,
	[CuentaId] [bigint] NOT NULL,
	[TipoMovimiento] [varchar](15) NOT NULL,
	[FechaMovimiento] [datetime] NOT NULL,
	[Valor] [money] NOT NULL,
	[Saldo] [money] NOT NULL,
 CONSTRAINT [PK_Movimientos] PRIMARY KEY CLUSTERED 
(
	[MovimientoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Personas]    Script Date: 13/7/2022 18:34:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Personas](
	[PersonaId] [bigint] IDENTITY(1,1) NOT NULL,
	[Identificacion] [varchar](13) NOT NULL,
	[NombresCompletos] [varchar](100) NOT NULL,
	[Genero] [varchar](20) NOT NULL,
	[FechaNacimiento] [date] NULL,
	[DireccionDomicilio] [varchar](200) NULL,
	[Telefono] [varchar](10) NULL,
 CONSTRAINT [PK_Personas] PRIMARY KEY CLUSTERED 
(
	[PersonaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UX_Persona_Identificacion] UNIQUE NONCLUSTERED 
(
	[Identificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Clientes]  WITH CHECK ADD  CONSTRAINT [FK_Personas_Clientes] FOREIGN KEY([PersonaId])
REFERENCES [dbo].[Personas] ([PersonaId])
GO
ALTER TABLE [dbo].[Clientes] CHECK CONSTRAINT [FK_Personas_Clientes]
GO
ALTER TABLE [dbo].[Cuentas]  WITH CHECK ADD  CONSTRAINT [FK_Clientes_Cuentas] FOREIGN KEY([ClienteId])
REFERENCES [dbo].[Clientes] ([ClienteId])
GO
ALTER TABLE [dbo].[Cuentas] CHECK CONSTRAINT [FK_Clientes_Cuentas]
GO
ALTER TABLE [dbo].[Movimientos]  WITH CHECK ADD  CONSTRAINT [FK_Cuentas_Movimientos] FOREIGN KEY([CuentaId])
REFERENCES [dbo].[Cuentas] ([CuentaId])
GO
ALTER TABLE [dbo].[Movimientos] CHECK CONSTRAINT [FK_Cuentas_Movimientos]
GO
