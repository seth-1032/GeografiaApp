IF DB_ID('GeografiaDB') IS NULL
BEGIN
    CREATE DATABASE GeografiaDB;
END;
GO

USE GeografiaDB;
GO


CREATE TABLE dbo.Paises
(
    PaisId INT IDENTITY(1,1) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    CodigoIso2 CHAR(2) NULL,
    Activo BIT NOT NULL
        CONSTRAINT DF_Paises_Activo DEFAULT (1),
    FechaCreacion DATETIME2(0) NOT NULL
        CONSTRAINT DF_Paises_FechaCreacion
        DEFAULT (SYSDATETIME()),
    FechaModificacion DATETIME2(0) NULL,

    CONSTRAINT PK_Paises
        PRIMARY KEY (PaisId),

    CONSTRAINT UQ_Paises_Nombre
        UNIQUE (Nombre),

    CONSTRAINT UQ_Paises_CodigoIso2
        UNIQUE (CodigoIso2),

    CONSTRAINT CK_Paises_Nombre
        CHECK (LEN(TRIM(Nombre)) > 0),

    CONSTRAINT CK_Paises_CodigoIso2
        CHECK (
            CodigoIso2 IS NULL
            OR CodigoIso2 NOT LIKE '%[^A-Z]%'
        )
);
GO

CREATE TABLE dbo.Departamentos
(
    DepartamentoId INT IDENTITY(1,1) NOT NULL,
    PaisId INT NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Activo BIT NOT NULL
        CONSTRAINT DF_Departamentos_Activo DEFAULT (1),
    FechaCreacion DATETIME2(0) NOT NULL
        CONSTRAINT DF_Departamentos_FechaCreacion
        DEFAULT (SYSDATETIME()),
    FechaModificacion DATETIME2(0) NULL,

    CONSTRAINT PK_Departamentos
        PRIMARY KEY (DepartamentoId),

    CONSTRAINT FK_Departamentos_Paises
        FOREIGN KEY (PaisId)
        REFERENCES dbo.Paises(PaisId),

    CONSTRAINT UQ_Departamentos_Pais_Nombre
        UNIQUE (PaisId, Nombre),

    CONSTRAINT CK_Departamentos_Nombre
        CHECK (LEN(TRIM(Nombre)) > 0)
);
GO


CREATE TABLE dbo.Ciudades
(
    CiudadId INT IDENTITY(1,1) NOT NULL,
    DepartamentoId INT NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Activo BIT NOT NULL
        CONSTRAINT DF_Ciudades_Activo DEFAULT (1),
    FechaCreacion DATETIME2(0) NOT NULL
        CONSTRAINT DF_Ciudades_FechaCreacion
        DEFAULT (SYSDATETIME()),
    FechaModificacion DATETIME2(0) NULL,

    CONSTRAINT PK_Ciudades
        PRIMARY KEY (CiudadId),

    CONSTRAINT FK_Ciudades_Departamentos
        FOREIGN KEY (DepartamentoId)
        REFERENCES dbo.Departamentos(DepartamentoId),

    CONSTRAINT UQ_Ciudades_Departamento_Nombre
        UNIQUE (DepartamentoId, Nombre),

    CONSTRAINT CK_Ciudades_Nombre
        CHECK (LEN(TRIM(Nombre)) > 0)
);
GO


CREATE INDEX IX_Departamentos_PaisId
    ON dbo.Departamentos(PaisId);
GO

CREATE INDEX IX_Ciudades_DepartamentoId
    ON dbo.Ciudades(DepartamentoId);
GO


INSERT INTO dbo.Paises (Nombre, CodigoIso2)
VALUES
    (N'Colombia', 'CO'),
    (N'México', 'MX'),
    (N'Argentina', 'AR');
GO

INSERT INTO dbo.Departamentos (PaisId, Nombre)
SELECT p.PaisId, d.Nombre
FROM
(
    VALUES
        (N'Colombia', N'Cundinamarca'),
        (N'Colombia', N'Antioquia'),
        (N'Colombia', N'Valle del Cauca'),
        (N'México', N'Jalisco'),
        (N'México', N'Nuevo León'),
        (N'Argentina', N'Buenos Aires')
) AS d(Pais, Nombre)
INNER JOIN dbo.Paises AS p
    ON p.Nombre = d.Pais;
GO

INSERT INTO dbo.Ciudades (DepartamentoId, Nombre)
SELECT d.DepartamentoId, c.Nombre
FROM
(
    VALUES
        (N'Colombia', N'Cundinamarca', N'Bogotá'),
        (N'Colombia', N'Antioquia', N'Medellín'),
        (N'Colombia', N'Valle del Cauca', N'Cali'),
        (N'México', N'Jalisco', N'Guadalajara'),
        (N'México', N'Nuevo León', N'Monterrey'),
        (N'Argentina', N'Buenos Aires', N'La Plata')
) AS c(Pais, Departamento, Nombre)
INNER JOIN dbo.Paises AS p
    ON p.Nombre = c.Pais
INNER JOIN dbo.Departamentos AS d
    ON d.PaisId = p.PaisId
    AND d.Nombre = c.Departamento;
GO