--create database FinalPrueba

--use FinalPrueba

--create table Rol(
--Id int primary key identity,
--Descripcion nvarchar(50),
--FechaCreacion datetime default getdate()
--)

--create table Permiso(
--Id int primary key identity,
--IdRol int references Rol(Id),
--NombreMenu nvarchar(100),
--FechaCreacion datetime default getdate()
--)

--create table Proveedor(
--Id int primary key identity,
--Documento nvarchar(60),
--RazonSocial nvarchar(60),
--Correo nvarchar(60),
--Telefono nvarchar(60),
--Estado bit,
--FechaCreacion datetime default getdate()
--)

--go

--create table Cliente(
--Id int primary key identity,
--Documento nvarchar(60),
--NombreCompleto nvarchar(60),
--Correo nvarchar(60),
--Telefono nvarchar(60),
--Estado bit,
--FechaCreacion datetime default getdate()
--)

--create table Usuario(
--Id int primary key identity,
--Documento nvarchar(60),
--NombreCompleto nvarchar(60),
--Correo nvarchar(60),
--Clave nvarchar(60),
--IdRol int references Rol(Id),
--Estado bit,
--FechaCreacion datetime default getdate()
--)


--go

--create table Categoria(
--Id int primary key identity,
--Descripcion nvarchar(100),
--Estado bit,
--FechaCreacion datetime default getdate()
--)

--go

--create table Producto(
--Id int primary key identity,
--Codigo nvarchar(60),
--Nombre nvarchar(60),
--Descripcion nvarchar(100),
--Stock int not null default 0,
--PrecioCompra decimal(10,2) default 0,
--PrecioVenta decimal(10,2) default 0,
--Estado bit,
--FechaCreacion datetime default getdate(),
--IdCategoria int references Categoria(Id)
--)

--go

--create table Compra(
--Id int primary key identity,
--IdUsuario int references Usuario(Id),
--IdProveedor int references Proveedor(Id),
--TipoDocumento nvarchar(60),
--NroDocumento nvarchar(60),
--MontoTotal decimal(10,2),
--FechaCreacion datetime default getdate()
--)

--go

--create table Detalle_Compra(
--Id int primary key identity,
--IdCompra int references Compra(Id),
--IdProducto int references Producto(Id),
--PrecioCompra decimal(10,2) default 0,
--PrecioVenta decimal(10,2) default 0,
--Cantidad int,
--MontoTotal decimal(10,2),
--TipoDocumento nvarchar(60),
--NroDocumento nvarchar(60),
--FechaCreacion datetime default getdate()
--)

--go

--create table Venta(
--Id int primary key identity,
--IdUsuario int references Usuario(Id),
--IdProveedor int references Proveedor(Id),
--TipoDocumento nvarchar(60),
--NroDocumento nvarchar(60),
--DocumentoCliente nvarchar(60),
--NombreCliente nvarchar(60),
--MontoPago decimal(10,2),
--MontoCambio decimal(10,2),
--MontoTotal decimal(10,2),
--FechaCreacion datetime default getdate()
--)

--go

--create table Detalle_Venta(
--Id int primary key identity,
--IdVenta int references Venta(Id),
--IdProducto int references Producto(Id),
--PrecioVenta decimal(10,2),
--Cantidad int,
--SubTotal decimal(10,2),
--FechaCreacion datetime default getdate()
--)