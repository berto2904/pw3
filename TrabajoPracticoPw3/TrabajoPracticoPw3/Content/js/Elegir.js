$(document).ready(function () {
    

});

$('#btnConfirmarGustos').click(function () {

    var GustosEmpanadasCantidadArray = [];
    $('.cantidad').each(function () {
        var gustoCantidad = {
            IdGustoEmpanada: parseInt(this.id),
            Cantidad: parseInt($(this).val())
        }
        GustosEmpanadasCantidadArray.push(gustoCantidad);
    });
    var invitacion = {
        IdUsuario: parseInt($('#IdUsuario').val()),
        Token: $('#Token').val(),
        GustosEmpanadasCantidad: GustosEmpanadasCantidadArray
    }

    $.ajax({
        url: "/api/Pedido/ConfirmarGustos",
        method: "POST",
        dataType: 'json',
        data: JSON.stringify(invitacion),
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            var mensajeResult = JSON.parse(result);
            $('#headerResultado').html(mensajeResult.Resultado);
            $('#mensajeGustos').html(mensajeResult.Mensaje);
            $("#confirmacionGustosModal").modal();
            }
    });
});

