--VIDEO 5--

SELECT Usuario.Id,Usuario.Documento,Usuario.NombreCompleto,Usuario.Correo,Usuario.Clave,Usuario.Estado,Rol.Id,Rol.Descripcion FROM Usuario
inner join rol on Rol.Id = Usuario.IdRol
go

Create procedure SP_RegistrarUsuario(
@Documento nvarchar(60),
@NombreCompleto nvarchar(60),
@Correo nvarchar(60),
@Clave nvarchar(60),
@IdRol int,
@Estado bit,
@IdUsuarioResultado int output,
@Mensaje nvarchar(500) output
)
as
begin
	set @IdUsuarioResultado = 0
	set @Mensaje = ''

	if not exists(select * from Usuario where Documento = @Documento)
	begin
		insert into Usuario(Documento,NombreCompleto,Correo,Clave,IdRol,Estado) values
		(@Documento, @NombreCompleto, @Correo, @Clave, @IdRol, @Estado)

		set @IdUsuarioResultado = SCOPE_IDENTITY()
	end
	else
		set @Mensaje = 'Documento ya existente'
end

go
--EJECUCION PROCEDURE--
declare @IdUsuarioGenerado int
declare @Mensaje nvarchar(500)

exec SP_RegistrarUsuario '123','pruebas','test@gmail.com','123',2,2,@IdUsuarioGenerado output, @Mensaje output

select @IdUsuarioGenerado

select @Mensaje

go

Create procedure SP_EditarUsuario(
@IdUsuario int,
@Documento nvarchar(60),
@NombreCompleto nvarchar(60),
@Correo nvarchar(60),
@Clave nvarchar(60),
@IdRol int,
@Estado bit,
@Respuesta bit output,
@Mensaje nvarchar(500) output
)
as
begin
	set @Respuesta = 0
	set @Mensaje = ''

	if not exists(select * from Usuario where Documento = @Documento and Id != @IdUsuario)
	begin
		update Usuario set
		Documento = @Documento,
		NombreCompleto = @NombreCompleto,
		Correo = @Correo,
		Clave = @Clave,
		IdRol = @IdRol,
		Estado = @Estado
		where Id = @IdUsuario

		set @Respuesta = 1
	end
	else
		set @Mensaje = 'Documento ya existente'
end

go

declare @Respuesta bit
declare @Mensaje nvarchar(500)

exec SP_EditarUsuario 3,'123','pruebas 2','test@gmail.com','123',2,2,@Respuesta output, @Mensaje output

select @Respuesta

select @Mensaje

go

--VIDEO 6--

Create proc SP_EliminarUsuario(
@IdUsuario int,
@Respuesta bit output,
@Mensaje nvarchar(500) output
)
as
begin
	set @Respuesta = 0
	set @Mensaje = ''
	declare @pasoreglas bit = 1

	if exists(select * from Compra
	inner join Usuario on Usuario.Id = Compra.IdUsuario
	where Usuario.Id = @IdUsuario
	)
	begin
		set @pasoreglas = 0
		set @Respuesta = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar. El usuario se encuentra relacionado a una compra\n'	
	end

	if exists(select * from Venta
	inner join Usuario on Usuario.Id = Venta.IdUsuario
	where Usuario.Id = @IdUsuario
	)
	begin
		set @pasoreglas = 0
		set @Respuesta = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar. El usuario se encuentra relacionado a una venta\n'	
	end

	if (@pasoreglas = 1)
	begin
		delete from Usuario where Id = @IdUsuario
		set @Respuesta = 1
	end
end

select * from Usuario
go

--VIDEO 8--

select * from Categoria
go

create procedure SP_RegistrarCategoria(
@Descripcion nvarchar(100),
@Estado bit,
@Resultado int output,
@Mensaje nvarchar(500) output
)
as
begin
	set @Resultado = 0
	set @Mensaje = ''

	if not exists (select * from Categoria where Descripcion = @Descripcion)
	begin
		insert into Categoria(Descripcion,Estado) values (@Descripcion,@Estado)
		set @Resultado = SCOPE_IDENTITY()
	end
	else
	begin
		set @Mensaje = 'Descripción ya existente'
	end
end
go

Create procedure SP_EditarCategoria(
@IdCategoria int,
@Descripcion nvarchar(100),
@Estado bit,
@Resultado bit output,
@Mensaje nvarchar(500) output
)
as
begin
	set @Resultado = 0
	set @Mensaje = ''

	if not exists(select * from Categoria where Descripcion = @Descripcion and Id != @IdCategoria)
	begin
		update Categoria set
		Descripcion = @Descripcion,
		Estado = @Estado
		where Id = @IdCategoria

		set @Resultado = 1
	end
	else
	begin
		set @Mensaje = 'Descripcion ya existente'
	end
end

go

create procedure SP_EliminarCategoria(
@IdCategoria int,
@Resultado bit output,
@Mensaje nvarchar(500) output
)
as
begin
	set @Resultado = 0
	set @Mensaje = ''

	if not exists(
		select * from Categoria
		inner join Producto on Producto.IdCategoria = Categoria.Id
		where Categoria.Id = @IdCategoria
	)
	begin
		delete top(1) from Categoria where Id = @IdCategoria
		set @Resultado = 1
	end
	else
	begin
		set @Mensaje = 'La categoria se encuentra relacionada a un producto'
	end
end

go

declare @Resultado bit
declare @Mensaje nvarchar(500)

exec SP_EliminarCategoria 6, @Resultado output, @Mensaje output

select @Resultado

select @Mensaje

go

--VIDEO 9--

select * from Producto
go

create procedure SP_RegistrarProducto(
	@Codigo nvarchar(60),
	@Nombre nvarchar(60),
	@Descripcion nvarchar(100),
	@Estado bit,
	@IdCategoria int,
	@Resultado bit output,
	@Mensaje nvarchar(500) output
)
as
begin
	set @Resultado = 0
	set @Mensaje = ''

	if not exists (select * from Producto where Codigo = @Codigo)
	begin
		insert into Producto(Codigo,Nombre,Descripcion,IdCategoria,Estado) values (@Codigo,@Nombre,@Descripcion,@IdCategoria,@Estado)
		set @Resultado = SCOPE_IDENTITY()
	end
	else
	begin
		set @Mensaje = 'Codigo ya existente'
	end
end
go

create procedure SP_EditarProducto(
	@IdProducto int,
	@Codigo nvarchar(60),
	@Nombre nvarchar(60),
	@Descripcion nvarchar(100),
	@Estado bit,
	@IdCategoria int,
	@Resultado bit output,
	@Mensaje nvarchar(500) output
)
as
begin
	set @Resultado = 0
	set @Mensaje = ''

	if not exists(select * from Producto where Codigo = @Codigo and Id != @IdProducto)
	begin
		update Producto set
		Codigo = @Codigo,
		Nombre = @Nombre,
		Descripcion = @Descripcion,
		Estado = @Estado,
		IdCategoria = @IdCategoria
		where Id = @IdProducto

		set @Resultado = 1
	end
	else
	begin
		set @Mensaje = 'Codigo ya existente'
	end
end
go

create procedure SP_EliminarProducto(
	@IdProducto int,
	@Resultado bit output,
	@Mensaje nvarchar(500) output
)
as
begin
	set @Resultado = 0
	set @Mensaje = ''
	declare @pasoreglas bit = 1

	if exists(
		select * from Detalle_Compra
		inner join Producto on Detalle_Compra.IdProducto = Producto.Id
		where Producto.Id = @IdProducto)
	begin
		set @pasoreglas = 0
		set @Resultado = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar, el producto esta relacionado a una compra'
	end
	
	if exists(
		select * from Detalle_Venta
		inner join Producto on Detalle_Venta.IdProducto = Producto.Id
		where Producto.Id = @IdProducto)
	begin
		set @pasoreglas = 0
		set @Resultado = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar, el producto esta relacionado a una compra'
	end

	if(@pasoreglas = 1)
	begin
		delete from Producto where Id = @IdProducto
		set @Resultado = 1
	end
end
go

--VIDEO 11--

create procedure SP_RegistrarCliente(
	@Documento nvarchar(60),
	@NombreCompleto nvarchar(60),
	@Correo nvarchar(60),			
	@Telefono nvarchar(60),			
	@Estado bit,
	@Resultado bit output,
	@Mensaje nvarchar(500) output
)
as
begin
	set @Resultado = 0
	set @Mensaje = ''

	if not exists (select * from Cliente where Documento = @Documento)
	begin
		insert into Cliente(Documento,NombreCompleto,Correo,Telefono,Estado) values (@Documento,@NombreCompleto,@Correo,@Telefono,@Estado)
		set @Resultado = SCOPE_IDENTITY()
	end
	else
	begin
		set @Mensaje = 'Documento ya existente'
	end
end
go

create procedure SP_EditarCliente(
	@IdCliente int,
	@Documento nvarchar(60),
	@NombreCompleto nvarchar(60),
	@Correo nvarchar(60),			
	@Telefono nvarchar(60),			
	@Estado bit,
	@Resultado bit output,
	@Mensaje nvarchar(500) output
)
as
begin
	set @Resultado = 0
	set @Mensaje = ''

	if not exists(select * from Cliente where Documento = @Documento and Id != @IdCliente)
	begin
		update Cliente set
		Documento = @Documento,
		NombreCompleto = @NombreCompleto,
		Correo = @Correo,
		Telefono = @Telefono,
		Estado = @Estado
		where Id = @IdCliente

		set @Resultado = 1
	end
	else
	begin
		set @Mensaje = 'Codigo ya existente'
	end
end
go

--VIDEO 12--

create procedure SP_RegistrarProveedor(
	@Documento nvarchar(60),
	@RazonSocial nvarchar(60),
	@Correo nvarchar(60),			
	@Telefono nvarchar(60),			
	@Estado bit,
	@Resultado bit output,
	@Mensaje nvarchar(500) output
)
as
begin
	set @Resultado = 0
	set @Mensaje = ''
	declare @IdPersona int

	if not exists (select * from Proveedor where Documento = @Documento)
	begin
		insert into Proveedor(Documento,RazonSocial,Correo,Telefono,Estado) values (@Documento,@RazonSocial,@Correo,@Telefono,@Estado)
		set @Resultado = SCOPE_IDENTITY()
	end
	else
	begin
		set @Mensaje = 'Documento ya existente'
	end
end
go

create procedure SP_EditarProveedor(
	@IdProveedor int,
	@Documento nvarchar(60),
	@RazonSocial nvarchar(60),
	@Correo nvarchar(60),			
	@Telefono nvarchar(60),			
	@Estado bit,
	@Resultado bit output,
	@Mensaje nvarchar(500) output
)
as
begin
	set @Resultado = 0
	set @Mensaje = ''

	if not exists(select * from Proveedor where Documento = @Documento and Id != @IdProveedor)
	begin
		update Proveedor set
		Documento = @Documento,
		RazonSocial = @RazonSocial,
		Correo = @Correo,
		Telefono = @Telefono,
		Estado = @Estado
		where Id = @IdProveedor

		set @Resultado = 1
	end
	else
	begin
		set @Mensaje = 'Codigo ya existente'
	end
end
go

create procedure SP_EliminarProveedor(
	@IdProveedor int,
	@Resultado bit output,
	@Mensaje nvarchar(500) output
)
as
begin
	set @Resultado = 0
	set @Mensaje = ''

	if exists(
		select * from Compra
		inner join Proveedor on Compra.IdProveedor = Proveedor.Id
		where Proveedor.Id = @IdProveedor)
	begin
		set @Mensaje = @Mensaje + 'No se puede eliminar, el proveedor esta relacionado a una compra'
	end
	else
	begin
		delete from Proveedor where Id = @IdProveedor
		set @Resultado = 1
	end
end
go

--VIDEO 15-- PROCESOS PARA REGISTRAR UNA COMPRA

--USO ESTO COMO UN PARAMETRO -- PARTE 15 MINTUO 44
create type [dbo].[Edetalle_Compra] as table(
	[IdProducto] int null,
	[PrecioCompra] decimal(10,2) null,
	[PrecioVenta] decimal(10,2) null,
	[Cantidad] int null,
	[MontoTotal] decimal(10,2) null
)
go

create procedure SP_RegistrarCompra(
@IdUsuario int,
@IdProveedor int,
@TipoDocumento nvarchar(60),
@NumeroDocumento nvarchar(60),
@MontoTotal decimal(10,2),
@DetalleCompra [EDetalle_Compra] readonly,
@Resultado bit output,
@Mensaje nvarchar(500) output
)
as
begin
	begin try
		declare @IdCompra int = 0
		set @Resultado = 1
		set @Mensaje = ''

		begin transaction registro
			
			insert into Compra(IdUsuario,IdProveedor,TipoDocumento,NroDocumento,MontoTotal)
			values (@IdUsuario,@IdProveedor,@TipoDocumento,@NumeroDocumento,@MontoTotal)

			set @IdCompra = SCOPE_IDENTITY()

			insert into Detalle_Compra(IdCompra,IdProducto,PrecioCompra,PrecioVenta,Cantidad,MontoTotal)
			select @IdCompra,IdProducto,PrecioCompra,PrecioVenta,Cantidad,MontoTotal from @DetalleCompra

			update Producto set Producto.Stock = Producto.Stock + dc.Cantidad,
			Producto.PrecioCompra = dc.PrecioCompra,
			Producto.PrecioVenta = dc.PrecioVenta
			from Producto
			inner join @DetalleCompra dc on dc.IdProducto = Producto.Id

		commit transaction registro
	end try
	begin catch
		set @Resultado = 0
		set @Mensaje = ERROR_MESSAGE()
		rollback transaction registro
	end catch
end
go

--VIDEO 19 -- PROCESOS PARA REGISTRAR UNA VENTA

--USO ESTO COMO UN PARAMETRO -- PARTE 19 MINTUO 2
create type [dbo].[Edetalle_Venta] as table(
	[IdProducto] int null,
	[PrecioVenta] decimal(10,2) null,
	[Cantidad] int null,
	[SubTotal] decimal(10,2) null
)
go

create procedure SP_RegistrarVenta(
@IdUsuario int,
@TipoDocumento nvarchar(60),
@NumeroDocumento nvarchar(60),
@DocumentoCliente nvarchar(60),
@NombreCliente nvarchar(60),
@MontoPago decimal(10,2),
@MontoCambio decimal(10,2),
@MontoTotal decimal(10,2),
@DetalleVenta [EDetalle_Venta] readonly,
@Resultado bit output,
@Mensaje nvarchar(500) output
)
as
begin
	begin try
		declare @IdVenta int = 0
		set @Resultado = 1
		set @Mensaje = ''

		begin transaction registro
			
			insert into Venta(IdUsuario,TipoDocumento,NroDocumento,DocumentoCliente,NombreCliente,MontoPago,MontoCambio,MontoTotal)
			values (@IdUsuario,@TipoDocumento,@NumeroDocumento,@DocumentoCliente,@NombreCliente,@MontoPago,@MontoCambio,@MontoTotal)

			set @IdVenta = SCOPE_IDENTITY()

			insert into Detalle_Venta(IdVenta,IdProducto,PrecioVenta,Cantidad,SubTotal)
			select @IdVenta,IdProducto,PrecioVenta,Cantidad,SubTotal from @DetalleVenta

		commit transaction registro
	end try
	begin catch
		set @Resultado = 0
		set @Mensaje = ERROR_MESSAGE()
		rollback transaction registro
	end catch
end
go

select * from Venta
select * from Detalle_Venta
go

select * from Producto