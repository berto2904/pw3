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
            var mensajePosEliminar = "Pedido " + resultadoJson.NombreNegocio+ " ha sido eliminado exitosamente"
            var mensaje = "Estas seguro de eliminar el pedido " + resultadoJson.NombreNegocio + "</br> Actualmente posee " + resultadoJson.CantidadInvitados + " " + cantidad;
            $('#mensajeEliminar').html(mensaje);
            $('#mensaje').html(mensajePosEliminar);

            $('.modal-footer #btnEliminarPedido').click(function () {
                $("#EliminarModal").modal('toggle');
                eliminarFnc(idPedido);
            });

            //$(".modal-footer #btnEliminarPedido").attr("href", "/Pedidos/Eliminar/" + idPedido);    
        }
    });

});

function eliminarFnc(idPedido) {
    $.ajax({
        url: "/Pedidos/Eliminar/",
        method: "GET",
        data: { id: idPedido },
        success: function (result) {
            $("#confirmacionEliminarModal").modal();
        }
    });
}

$('#graciasId').click(function () {
    location.reload();
});