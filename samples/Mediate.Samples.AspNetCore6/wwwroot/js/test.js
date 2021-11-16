$(document).ready(function () {

    var connection = new signalR.HubConnectionBuilder().withUrl("/hub/test").build();

    connection.start().then(function () {
    });

    connection.on("ReceiveMessage", function (message) {
        alert(message);
    });

});