var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_Load').DataTable({
        "ajax": {
            "type": "GET",
            "url": "/Book/GetAll",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "author", "width": "20%" },
            { "data": "isbn", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="text-center">
                            <a href="/Book/Upsert?id=${data}" class="btn btn-success text-white" style="cursor: pointer; width: 70px">Edit</a>
                            &nbsp;
                        <button onclick="Delete('/Book/Delete?id=${data}')" class="btn btn-danger text-white" style="width: 70px">Delete</button>
                    </div>
                        `;
                },
                "width": "40%"
            }
        ],
        "language": {
            "emptyTable": "No data found."
        },
        "width": "100%"
    });

}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "Delete",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            })

        }
    })
}