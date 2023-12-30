--VIDEO 1-- CREACION DE TABLAS
create database FinalPrueba

use FinalPrueba
go
create table Rol(
Id int primary key identity,
Descripcion nvarchar(50),
FechaCreacion datetime default getdate()
)
go
create table Permiso(
Id int primary key identity,
IdRol int references Rol(Id),
NombreMenu nvarchar(100),
FechaCreacion datetime default getdate()
)
go
create table Proveedor(
Id int primary key identity,
Documento nvarchar(60),
RazonSocial nvarchar(60),
Correo nvarchar(60),
Telefono nvarchar(60),
Estado bit,
FechaCreacion datetime default getdate()
)
go
create table Cliente(
Id int primary key identity,
Documento nvarchar(60),
NombreCompleto nvarchar(60),
Correo nvarchar(60),
Telefono nvarchar(60),
Estado bit,
FechaCreacion datetime default getdate()
)
go
create table Usuario(
Id int primary key identity,
Documento nvarchar(60),
NombreCompleto nvarchar(60),
Correo nvarchar(60),
Clave nvarchar(60),
IdRol int references Rol(Id),
Estado bit,
FechaCreacion datetime default getdate()
)
go
create table Categoria(
Id int primary key identity,
Descripcion nvarchar(100),
Estado bit,
FechaCreacion datetime default getdate()
)
go
create table Producto(
Id int primary key identity,
Codigo nvarchar(60),
Nombre nvarchar(60),
Descripcion nvarchar(100),
Stock int not null default 0,
PrecioCompra decimal(10,2) default 0,
PrecioVenta decimal(10,2) default 0,
Estado bit,
FechaCreacion datetime default getdate(),
IdCategoria int references Categoria(Id)
)
go
create table Compra(
Id int primary key identity,
IdUsuario int references Usuario(Id),
IdProveedor int references Proveedor(Id),
TipoDocumento nvarchar(60),
NroDocumento nvarchar(60),
MontoTotal decimal(10,2),
FechaCreacion datetime default getdate()
)
go
create table Detalle_Compra(
Id int primary key identity,
IdCompra int references Compra(Id),
IdProducto int references Producto(Id),
PrecioCompra decimal(10,2) default 0,
PrecioVenta decimal(10,2) default 0,
Cantidad int,
MontoTotal decimal(10,2),
FechaCreacion datetime default getdate()
)
go
create table Venta(
Id int primary key identity,
IdUsuario int references Usuario(Id),
TipoDocumento nvarchar(60),
NroDocumento nvarchar(60),
DocumentoCliente nvarchar(60),
NombreCliente nvarchar(60),
MontoPago decimal(10,2),
MontoCambio decimal(10,2),
MontoTotal decimal(10,2),
FechaCreacion datetime default getdate()
)
go
create table Detalle_Venta(
Id int primary key identity,
IdVenta int references Venta(Id),
IdProducto int references Producto(Id),
PrecioVenta decimal(10,2),
Cantidad int,
SubTotal decimal(10,2),
FechaCreacion datetime default getdate()
)
go

--VIDEO 2-- INSERT DE ROL BASICO Y USUARIO
select * from usuario

select * from rol

insert into rol (Descripcion)
values
('ADMINISTRADOR')

insert into usuario (Documento,NombreCompleto,Correo,Clave,IdRol,Estado)
values
('101010','ADMIN','@GMAIL.COM','123',1,1)
go

--VIDEO 3-- INSERT PERMISOS DE MENU
select * from permiso
go
insert into permiso(IdRol,NombreMenu)
values
(1,'menuusuarios'),
(1,'menumantenedor'),
(1,'menuventas'),
(1,'menucompras'),
(1,'menuclientes'),
(1,'menuproveedores'),
(1,'menureportes'),
(1,'menuacercade')

insert into rol (Descripcion)
values
('EMPLEADO')

select * from Rol

insert into permiso(IdRol,NombreMenu)
values
(2,'menuventas'),
(2,'menucompras'),
(2,'menuclientes'),
(2,'menuproveedores'),
(2,'menuacercade')

select * from Permiso

select Permiso.IdRol,Permiso.NombreMenu from Permiso
inner join Rol on Rol.Id = Permiso.IdRol
inner join Usuario on Usuario.IdRol = Rol.Id
where Usuario.Id = 2

insert into usuario (Documento,NombreCompleto,Correo,Clave,IdRol,Estado)
values
('202020','EMPLEADO','@GMAIL.COM','123',2,1)

select * from Usuario
go

--VIDEO 4-- CORRECCION ERROR DE TIPE
UPDATE permiso
SET nombremenu = 'menuusuarios'
WHERE Id = 1
go

--VIDEO 8-- INSERT Y SELECT CATEGORIA

select id, descripcion, estado from categoria

insert into Categoria (descripcion,estado) values ('Postes',1),('Tejidos Romboidales',2),('Planchuelas',3)

select * from categoria
go

--VIDEO 9-- SELECT PRODUCTO PARA USAR EN EL CODIGO E INSERT

select Producto.Id,Codigo,Nombre,Producto.Descripcion,Stock,PrecioCompra,PrecioVenta,Producto.Estado,Categoria.Id,Categoria.Descripcion[DescripcionCategoria] from Producto
inner join Categoria on Categoria.Id = Producto.IdCategoria

insert into Producto(Codigo,Nombre,Descripcion,IdCategoria,Estado) values ('10','Tejido romboidal 1.80mts','Acero inoxidable',2,1)

select * from Producto
go

--VIDEO 11--

select Id,Documento,NombreCompleto,Correo,Telefono,Estado from Cliente

insert into Cliente(Documento,NombreCompleto,Correo,Telefono,Estado) values ('10','Francisco Bruno','franbruno@gmail.com','341341',1)
go

--VIDEO 12-- MODIFICO ERROR EN TABLA VENTAS (ESTABA EL ID PROVEEDOR QUE NO ES NECESARIO)
--NINGUNA DE LAS SIGUIENTES FORMAS FUNCIONO. LO HICE DESDE EL OBJECT EXPLORER BUSCANDO LA TABLA, CONSTRAINTS Y ELIMINANDOLA DESDE AHI
--TAMBIEN ELIMINE EL ATRIBUTO Y LA KEY DESDE EL OBJECT EXPLORER

select name
from sys.foreign_keys
where referenced_object_id = object_id('Venta')

alter table Venta
drop column IdProveedor

alter table Venta
drop constraint FK__Detalle_V__IdVen__4E88ABD4

SELECT CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS
WHERE CONSTRAINT_NAME = 'FK__Detalle_V__IdVen__4E88ABD4'

ALTER TABLE Venta
DROP CONSTRAINT FK__Detalle_V__IdVen__4E88ABD4

SELECT CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS
WHERE TABLE_NAME = 'Venta' AND CONSTRAINT_NAME = 'FK__Detalle_V__IdVen__4E88ABD4'
go

select * from Proveedor
go

--VIDEO 13-- TABLA NEGOCIO

create table Negocio(
Id int primary key,
Nombre nvarchar(60),
RUC nvarchar(60),
Direccion nvarchar(60),
Logo varbinary(max) NULL
)

select * from Negocio

insert into Negocio (Id,Nombre,RUC,Direccion) values (1,'Codigo Estudiante','101010','av. codigo 123')
go

--VIDEO 16-- SELECT TABLA COMPRA

select * from Compra where Id = 4
select * from Detalle_Compra where IdCompra = 00004
