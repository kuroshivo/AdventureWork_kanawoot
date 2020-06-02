
const uri = "https://localhost:44352/AdventureWorkController1/"; 
var map;
var markercluster;
var search;

$(function () { 

    fnGetActivity();
    //fnInitMap(15.870032, 100.992541); 
    fnInitMap(39.909736, -99.702658);  

    $("#result").delegate("li", "click", function () {
        $("#txtPlace").val($(this).text());  
    });

});


function doSearch() {

    map.Search.search(search.value, {
        area: 10
    });
    // suggest.style.display = 'none'; 
}

function fnInitMap(plat, plng) {  

    // Map
    map = new longdo.Map({
        placeholder: document.getElementById('map') 
    }); 

    // Search
    search = document.getElementById("search");
    map.Search.placeholder(
        document.getElementById("result")
    );

    //หากมีการกด Enter ที่ #search
    search.onkeyup = function (event) {
        if ((event || window.event).keyCode != 13)
            return;
        doSearch();
    }
}

function fnSearch() {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("txtSearch");
    filter = input.value.toUpperCase();
    table = document.getElementById("tbAdventureList");
    tr = table.getElementsByTagName("tr");
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
} 

function fnSaveActivity(actionType) {

    if (actionType != "D") {
        if ($("#txtActivityName").val().length == 0) {
            alert("Please spectify activity.");
            return;
        }

        if ($("#txtDate").val().length == 0) {
            alert("Please spectify date.");
            return;
        }
    }

    const activityWork = {  
        KeyIndentity: $("#hidKeyIndentity").val(), 
        ActivityName: $("#txtActivityName").val(), 
        DatePic: $("#txtDate").val(), 
        Place: $("#txtPlace").val(), 
        Lat: "1", 
        Lng: "2",
        ActionType: actionType
    };    

    $.ajax({
        url: uri + "SaveActivity", 
        data: { "activityWork": activityWork },
        type: "POST",
        dataType: "json",
        async: true,
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Something went wrong!");
        },
        success: function (result) { 
            if (result.result.result == "S") {  
                fnRenderTableAdventure(result.result.listOfActivityWork.listOfActivityWork);
                fnClearValueElement();

                $('#modalActivity').modal('hide');  
            }
            else {
                alert(result.result.message);
            }
        }
    });  
}

function fnGetActivity() {  

    $.ajax({
        url: uri + "GetActivity", 
        type: "POST",
        dataType: "json",
        async: true,
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Something went wrong!");
        },
        success: function (result) { 
            //console.log(result.result.result);
            if (result.result.result == "S") { 
                fnRenderTableAdventure(result.result.listOfActivityWork.listOfActivityWork);
            }
            else {
                alert(result.result.result.message);
            }
        }
    });
}

function fnEditActivity(keyIndentity) {  
    $.ajax({
        url: uri + "GetActivityByKey",
        type: "POST",
        dataType: "json",
        data: { "keyIndentity": keyIndentity },
        async: true,
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Something went wrong!");
        },
        success: function (result) {
            //console.log(result.result.activityName); 
            if (result) {  
                $("#hidKeyIndentity").val(keyIndentity); 
                $("#txtActivityName").val(result.result.activityName);
                $("#txtDate").val(result.result.datePic);
                $("#txtPlace").val(result.result.place);

                $("#btnAddNew").attr("style", "display:none");
                $("#btnEdit").attr("style", "display:show");
                $("#btnDelete").attr("style", "display:show"); 

                $('#modalActivity').modal('show'); 
            }
        }
    });
}

function fnRenderTableAdventure(lst) {

    $("#tbAdventureList > tbody > tr").remove();
    var i;
    for (i = 0; i < lst.length; ++i) {
        var tr = $("#tbAdventureList > tbody:last-child");
        tr.append("<tr>"
            + "<td><button type='button' class='btn btn -default' onclick='fnEditActivity(" + lst[i].keyIndentity + ")'><h5>"
            + lst[i].activityName + "</h5></button><br>"
            + lst[i].datePic + "</td>"
            + "</tr>");

    }
}

   // + "<td><button type='button' class='btn btn -default' onclick='fnInitMap(15.870032 , 100.992541)'>View place</button></td>"}

function fnClearValueElement() {
    $("#hidKeyIndentity").val(0);
    $("#hidLat").val(0);
    $("#hidLng").val(0);

    $("#txtActivityName").val("");
    $("#txtDate").val("");
    $("#txtPlace").val("");  
}


function fnShowModalActivityNew() {
    fnClearValueElement(); 

    $("#btnAddNew").attr("style", "display:show");
    $("#btnEdit").attr("style", "display:none");
    $("#btnDelete").attr("style", "display:none"); 

    $('#modalActivity').modal('show');  
}


