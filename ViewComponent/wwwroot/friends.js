document.querySelector("#load-friends-button").addEventListener("click", async function () {
    var response = await fetch("friends-list");
    var friends = await response.text();

    document.querySelector("#friends-list-content").innerHTML = friends;
})