(function () {
    var requestNumber = 1;
    var a = document.getElementById('InputA');
    var b = document.getElementById('InputB');
    var calc = document.getElementById('calculate');
    var sum = document.getElementById('sum');
    var status = document.getElementById('status');
    sum.innerText = '?';
    var socket = new WebSocket('ws://localhost:' + window.location.port + '/Home/Socket');
    socket.onclose = function (e) { status.innerText = 'Socket closed'; };
    socket.onerror = function (e) { status.innerText = 'Socket error'; };
    socket.onmessage = function (e) {
        status.innerText = 'Socket message received: ' + e.data;
        var response = JSON.parse(e.data);
        if (response.id == requestNumber) // If it's a response to the last request we sent.
         {
            if (response.error) {
                status.innerText += '\nError: ' + response.error.message;
            }
            else if (response.result) {
                sum.innerText = response.result;
            }
        }
    };
    socket.onopen = function (e) { status.innerText = 'Socket opened'; };
    calc.onclick = function () {
        status.innerText = "sending request";
        var jsonRpcRequest = {
            jsonrpc: "2.0",
            id: ++requestNumber,
            method: "Add",
            params: [
                a.value,
                b.value
            ]
        };
        socket.send(JSON.stringify(jsonRpcRequest));
    };
})();
//# sourceMappingURL=site.js.map