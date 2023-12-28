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