const token = document.querySelector("#FinnhubToken").value;
const socket = new WebSocket(`wss://ws.finnhub.io?token=${token}`);
var stockSymbol = document.getElementById("StockSymbol").value; 

socket.addEventListener('open', function (event) {
    socket.send(JSON.stringify({ 'type': 'subscribe', 'symbol': stockSymbol }))
});

socket.addEventListener('message', function (event) {
    if (event.data.type == "error") {
        $(".price").text(event.data.msg);
        return;
    }

    //data received from server
    //console.log('Message from server ', event.data);

    /* Sample response:
    {"data":[{"p":220.89,"s":"MSFT","t":1575526691134,"v":100}],"type":"trade"}
    type: message type
    data: [ list of trades ]
    s: symbol of the company
    p: Last price
    t: UNIX milliseconds timestamp
    v: volume (number of orders)
    c: trade conditions (if any)
    */

    var eventData = JSON.parse(event.data);
    if (eventData) {
        if (eventData.data) {
            var updatedPrice = JSON.parse(event.data).data[0].p;
            $(".price").text(updatedPrice.toFixed(2));
        }
    }
});

var unsubscribe = function (symbol) {
    socket.send(JSON.stringify({ 'type': 'unsubscribe', 'symbol': symbol }))
}

window.onunload = function () {
    unsubscribe(stockSsymbol);
};