$(document).ready(function(){
    $('.multipleSelect').fastselect();

});

$(document).on("click", ".botonEliminar", function (event) {
    var idPedido = $(this).data('id');
    $.ajax({
        url: "/api/Pedido",
        method: "GET",
        dataType: 'json',
        data: { id: idPedido },
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            var resultadoJson = JSON.parse(result);
            var cantidad = parseInt(resultadoJson.CantidadInvitados) > 1 ? "invitaciones confirmadas" : "invitacion confirmada";
            var mensaje = "Estas seguro de eliminar el pedido " + resultadoJson.NombreNegocio + "</br> Actualmente posee " + resultadoJson.CantidadInvitados + " " + cantidad;
            $('#mensajeEliminar').html(mensaje);
            var mensajePosEliminar = "Pedido" + resultadoJson.NombreNegocio+ "ha sido eliminado exitosamente"
            $(".modal-footer #btnEliminarPedido").attr("href", "/Pedidos/Eliminar/" + idPedido);    
            $('.modal-footer #btnEliminarPedido').click(function () {
                $("#confirmacionEliminarModal").modal();

            });
        }
    });

});
