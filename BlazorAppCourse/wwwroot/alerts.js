
function showSweetAlert() {
    Swal.fire({
        title: 'Confirmar',
        text: 'message 1',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, estoy seguro'
    }).then((result) => {
        if (result.isConfirmed) {
            return "st 1";
        } else {
            return "st 2";
        }
    })
};

