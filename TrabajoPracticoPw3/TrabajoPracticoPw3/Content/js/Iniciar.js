$(document).ready(function(){
    $('.multipleSelect').fastselect();

});

$(document).on("click", ".botonEliminar", function (event) {
    var idPedido = $(this).data('id');
    $(".modal-footer #idPedido").attr("href", "/Pedidos/Eliminar/" + idPedido);    
});

$('.botonEliminar').click('', function (event) {
    //event.target.parentNode.parentNode.cells[1].textContent
            //event.target.id;
    //$.ajax({
    //    url: "Pedidos/",
    //    success: function (result) {
    //    $("#div1").html(result);
    //    }
    //});
        
        //$('#myModal').trigger('focus');
});

