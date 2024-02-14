IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Boards] (
    [Id] int NOT NULL IDENTITY,
    CONSTRAINT [PK_Boards] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240210025137_M1', N'7.0.15');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Boards] ADD [InternalColumns] nvarchar(max) NOT NULL DEFAULT N'';
GO

ALTER TABLE [Boards] ADD [InternalRows] nvarchar(max) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240210025231_M2', N'7.0.15');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Boards]') AND [c].[name] = N'InternalColumns');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Boards] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Boards] DROP COLUMN [InternalColumns];
GO

EXEC sp_rename N'[Boards].[InternalRows]', N'InternalArray', N'COLUMN';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240210040223_array', N'7.0.15');
GO

COMMIT;
GO

