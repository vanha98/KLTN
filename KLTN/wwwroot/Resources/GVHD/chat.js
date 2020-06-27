"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();


connection.on("ReceiveMessage", function (user, message)
{
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var a = document.createElement("a");
    a.textContent = encodedMsg;
    $("#messagesList").append(a);
});

connection.start()

document.getElementById("sendButton").addEventListener("click", function (event)
{
    var user = "Giảng viên";
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err)
    {
        return console.error(err.toString());
    });
    event.preventDefault();
});