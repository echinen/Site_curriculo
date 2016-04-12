$(function ()
{
    //$('#treeDemo').jstree();
    $("#treeDemo").jstree({
        "checkbox": {
            "keep_selected_style": false
        },
        "plugins": ["checkbox"]
    });

    $("#userTree").jstree({
        "checkbox": {
            "keep_selected_style": false
        },
        "plugins": ["checkbox"]
    });

    $('#createForm').submit(function () {
        // GRUPOS
        var selectedElmsIds = [];
        var selectedElms = $('#treeDemo').jstree("get_checked", true);
        $.each(selectedElms, function () {
            selectedElmsIds.push(this.id);
        });
        
        selectedElmsIds.sort(function (a, b) {
           return Number(a.substring(3, 5)) > Number(b.substring(3, 5)) ? 1 : -1;
        });
        console.log(selectedElmsIds);
        var input = $("<input>")
               .attr("type", "hidden")
               .attr("name", "selectedElmsIds").val(JSON.stringify(selectedElmsIds));
        $('#createForm').append($(input));
        
        //USUÁRIOS
        var selectedUsersIds = [];
        var selectedUsers = $('#userTree').jstree("get_checked", true);
        $.each(selectedUsers, function () {
            selectedUsersIds.push(this.id);
        });

        console.log(selectedElmsIds);
        var inputUsers = $("<input>")
               .attr("type", "hidden")
               .attr("name", "selectedUsersIds").val(JSON.stringify(selectedUsersIds));
        $('#createForm').append($(inputUsers));
           
        console.log(selectedUsersIds);
        
        return true; // return false to cancel form action
    });

    $("#searchUser").click(function () {
        var selectedUsers = [];
        var Users = $('#userTree').jstree("get_checked", true);
        $.each(Users, function () {
            if (this.text != undefined) {
                var obj = { id: this.id, text: this.text, parent: "#", state: { checked: true } };
                selectedUsers.push(obj);
            }
        });
        $("#userTree").jstree("destroy");
        $.ajax({
            url: '/mpc_grupos/Users?userstring=' + $("#SearchString").val(),
            type: "GET",
            dataType: "json",
            success: function (data) {
                console.log(data);
                console.log(selectedUsers);
                $.each(data, function () {
                    var id = this.id;
                    var result = $.grep(selectedUsers, function (e) {
                        return e.id == id;
                    });
                    if (result.length == 0) {
                        // not found
                        var obj = { id: id, text: this.text, parent: "#" };
                        selectedUsers.push(obj);
                    } else  {
                        // found, do nothing
                    }
                                       
                });
                $("#userTree").jstree({
                    "checkbox": {
                        "tie_selection" : false
                    },
                    "plugins": ["checkbox"],
                    'core': {
                        'data': selectedUsers
                    }
                });

            }
        });
    });
});