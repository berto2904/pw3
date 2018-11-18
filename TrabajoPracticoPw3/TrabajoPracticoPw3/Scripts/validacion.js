function validacion() {
        nombre = document.getElementById("nombre").value;
        precioUnidad = document.getElementById("precioUnidad").value;
        precioDocena = document.getElementById("precioDocena").value;
        if (nombre == null || nombre.length == 0 || /^\s+$/.test(nombre)) {
        alert('[ERROR] El campo nombre es obligatorio');
    return false;
}
        else if (precioUnidad == null || precioUnidad.length == 0 || /^\s+$/.test(precioUnidad) || isNaN(precioUnidad)) {
        alert('[ERROR] El campo precioUnidad es obligatorio y debe ser un número');
    return false;
}
        else if (precioDocena == null || precioDocena.length == 0 || /^\s+$/.test(precioDocena) || isNaN(precioDocena)) {
        alert('[ERROR] El campo precioDocena es obligatorio y debe ser un número');
    return false;
}
return true;
}

function validacionLogin() {
    email = document.getElementById("email").value;
    pw = document.getElementById("password").value;
    if (email == null || email.length == 0 || /^\s+$/.test(email) || !(/\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)/.test(email))) {
        alert('[ERROR] El campo email es obligatorio');
        return false;
    }
    else if (pw == null || pw.length == 0 || /^\s+$/.test(pw)){
        alert('[ERROR] El campo password es obligatorio');
        return false;
    }
    return true;
}
