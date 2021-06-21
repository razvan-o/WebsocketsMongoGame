"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/gameHub").build();

//Disable vote button until connection is established
document.getElementById("voteButton").disabled = true;

connection.start().then(function () {
	document.getElementById("voteButton").disabled = false;
}).catch(function (err) {
	return console.error(err.toString());
});


connection.on("ReceivedMessage", function (user, message) {
	var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
	var encodedMsg = user + " says " + msg;
	var li = document.createElement("li");
	li.textContent = encodedMsg;
	document.getElementById("messagesList").appendChild(li);
});

connection.on("VoteStarted", function (voteObject, secondsUntilVoteEnd) {
	//var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
	var encodedMsg = "am inceput un vot: " + voteObject + " Timp ramas (secunde): " + secondsUntilVoteEnd;
	var li = document.createElement("li");
	li.textContent = encodedMsg;
	document.getElementById("messagesList").appendChild(li);
});

connection.on("VotesUpdated", function (voteObject) {
	//var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
	var encodedMsg = JSON.stringify(voteObject);
	var li = document.createElement("li");
	li.textContent = encodedMsg;
	document.getElementById("messagesList").appendChild(li);
});



document.getElementById("start").addEventListener("click", function (event) {
	var user = document.getElementById("userInput").value;
	var friends = document.getElementById("friendsInput").value;
	var gameId = document.getElementById("gameIdInput").value;
	connection.invoke("StartGame", user, friends, gameId).catch(function (err) {
		return console.error(err.toString());
	});
	event.preventDefault();
});


document.getElementById("join").addEventListener("click", function (event) {
	var user = document.getElementById("userInput").value;
	var gameId = document.getElementById("gameIdInput").value;
	connection.invoke("JoinGame", user, gameId).catch(function (err) {
		return console.error(err.toString());
	});
	event.preventDefault();
});

document.getElementById("endGame").addEventListener("click", function (event) {
	var user = document.getElementById("userInput").value;
	var gameId = document.getElementById("gameIdInput").value;
	connection.invoke("EndGame", user, gameId).catch(function (err) {
		return console.error(err.toString());
	});
	event.preventDefault();
});

document.getElementById("voteButton").addEventListener("click", function (event) {
	var user = document.getElementById("userInput").value;
	var gameId = document.getElementById("gameIdInput").value;
	var predictedNumber = document.getElementById("predictedNumber").value;

	connection.invoke("Vote", user, gameId, predictedNumber).catch(function (err) {
		return console.error(err.toString());
	});
	event.preventDefault();
});

connection.on("WinnersDetermined", function (gameId, winners) {
	// var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
	var encodedMsg = "Game" + gameId + " won by: " + winners;
	var li = document.createElement("li");
	li.textContent = encodedMsg;
	document.getElementById("messagesList").appendChild(li);
});


connection.on("ReceivedVote", function (playerId, vote) {
	// var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
	var encodedMsg = playerId + " voted " + vote;
	var li = document.createElement("li");
	li.textContent = encodedMsg;
	document.getElementById("messagesList").appendChild(li);
});

connection.on("PlayerJoined", function (playerId) {
	// var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
	var encodedMsg = playerId + " joined! ";
	var li = document.createElement("li");
	li.textContent = encodedMsg;
	document.getElementById("messagesList").appendChild(li);
});